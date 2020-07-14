using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using NodaTime;

namespace DatabaseCreaton
{
    // NO MSQL
    public partial class Form1 : Form
    {
        string connectionString = "";
        string time_format = "TIMESTAMPTZ";
        string system_tz = DateTimeZoneProviders.Tzdb.GetSystemDefault().ToString();

        bool postgres = true;

        public Form1()
        {
            do
            {
                connectionString = Interaction.InputBox("Enter connection string", "Connetion string prompt", connectionString);
                if (connectionString.Length <= 0)
                {
                    Close();
                    Application.Exit();
                    return;
                }

                try
                {
                    using (OdbcConnection conn = new OdbcConnection(connectionString))
                    {
                        conn.Open();
                    }
                    break;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка соединения с базой данных:\n\t" + e.Message);
                }
            } while (true);

            //if (connectionString.ToLower().Contains("postgre"))
            //{
            //    time_format = "TIMESTAMPTZ";
            //    postgres = true;
            //}

            InitializeComponent();
        }

        private void notifySuccess(string db, string table)
        {
            MessageBox.Show(String.Format("Таблица '{1}' успешно создана", db, table), "Успех");
        }

        private string change_db_connstr(string conn_str, string db_name)
        {
            string connStr = conn_str.ToLower();
            if (connStr.Contains("database"))
                connStr = Regex.Replace(connectionString, @"(?<=database=)[\w\d_-]+(?=;)", db_name);
            else
            {
                if (!connStr.EndsWith(";"))
                    connStr += ";";
                connStr += String.Format("database={0};", db_name);
            }
            return connStr;
        }

        private void createDbTable(string db_name, string create_table, bool set_tz=true)
        {
            //change databse in connection string
            using (OdbcConnection conn = new OdbcConnection(change_db_connstr(connectionString, db_name)))
            using (OdbcCommand cmd = new OdbcCommand(create_table, conn))
            {
                conn.Open(); cmd.ExecuteNonQuery();
                if (set_tz)
                {
                    string sqlquery = String.Format("ALTER DATABASE {1} SET timezone TO '{0}'", system_tz, db_name);
                    cmd.CommandText = sqlquery;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void w_ext_cars(string db_name = "w_ext")
        {
            string table_name = "cars";
            string create = "CREATE TABLE " + table_name + "(" +
                "id VARCHAR(12) PRIMARY KEY," +
                "weight INTEGER," +
                "corr INTEGER," +
                "gn VARCHAR(12) UNIQUE NOT NULL)";

            createDbTable(db_name, create);
            notifySuccess(db_name, table_name);
        }

        private void w_ext_drivers(string db_name = "w_ext")
        {
            string table_name = "drivers";
            string create = "CREATE TABLE " + table_name + "(" +
                 "id INTEGER PRIMARY KEY," +
                 "fio VARCHAR(60) NOT NULL)";
            createDbTable(db_name, create);
            notifySuccess(db_name, table_name);
        }

        private void w_ext_info(string db_name = "w_ext")
        {
            string table_name = "info";
            string sequence_name = "event_id";
            string create = "CREATE TABLE " + table_name + "(" +
                 String.Format("n {0} PRIMARY KEY ,", postgres ? "BIGSERIAL" : "BIGINT IDENTITY(1, 1)") +
                 "com VARCHAR(6) NOT NULL," +
                 "id VARCHAR(12) NOT NULL," +
                 "event_id INTEGER NOT NULL," +
                 "weight INTEGER NOT NULL," +
                 "inp_weight INTEGER NOT NULL," +
                 "ts " + time_format + " DEFAULT CURRENT_TIMESTAMP);" +
                 String.Format("CREATE SEQUENCE {0} MINVALUE 0 MAXVALUE 1000 CYCLE;", sequence_name); // create sequence also
            // n isn't serial
            // ts isn't compatible with postgres
            // do message box query whether db is postgres or mssql
            createDbTable(db_name, create);
            MessageBox.Show(String.Format("Таблицы '{0}' и '{1}' созданны успешно", table_name, sequence_name), "Успех");
        }

        private void w_ext_debug(string db_name = "w_ext")
        {
            string table_name = "debug";
            string create = "CREATE TABLE " + table_name + "(" +
                 String.Format("id {0} PRIMARY KEY,", postgres ? "SERIAL" : "INTEGER IDENTITY(1, 1)") +
                 "ts " + time_format + " DEFAULT CURRENT_TIMESTAMP," +
                 "code INTEGER," +
                 "message VARCHAR(1024));";

            createDbTable(db_name, create);
            notifySuccess(db_name, table_name);
        }

        private void w_base_info(string db_name = "w_base")
        {
            string table_name = "info";
            string create = "CREATE TABLE " + table_name + "(" +
                 "event_id INTEGER PRIMARY KEY," +
                 "com VARCHAR(6) NOT NULL," +
                 "barcode VARCHAR(1024)," +
                 "gn VARCHAR(12)," +
                 "fio VARCHAR(60)," +
                 "ts " + time_format + " DEFAULT CURRENT_TIMESTAMP);";

            createDbTable(db_name, create);
            notifySuccess(db_name, table_name);
        }

        private void clearDebug_Click(object sender, EventArgs _)
        {
            string cleared;
            try
            {
                string db_name = "w_ext";
                using (OdbcConnection conn = new OdbcConnection(change_db_connstr(connectionString, db_name)))
                using (OdbcCommand cmd = new OdbcCommand("DELETE FROM debug", conn))
                {
                    conn.Open();
                    cleared = cmd.ExecuteNonQuery().ToString();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            MessageBox.Show("Было удалено " + cleared + " записей", "Успех");
        }

        private void create_db(string db_name)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            using (OdbcCommand cmd = new OdbcCommand(String.Format("CREATE DATABASE {0}", db_name), conn))
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("База данных '" + db_name + "' была успешно создана");
        }

        private void w_base_butt_Click(object sender, EventArgs _)
        {
            try
            {
                create_db("w_base");
                w_base_info();
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void w_ext_butt_Click(object sender, EventArgs _)
        {
            try
            {
                create_db("w_ext");
                w_ext_cars();
                w_ext_drivers();
                w_ext_info();
                w_ext_debug();
            } catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
