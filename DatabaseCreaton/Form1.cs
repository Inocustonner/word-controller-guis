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

namespace DatabaseCreaton
{
    public partial class Form1 : Form
    {
        string connectionString = "";
        string time_format = "DATETIME";
        string serial_suffix = "IDENTITY(1,1)";

        bool postgres = false;

        public Form1()
        {
            do
            {
                connectionString = Interaction.InputBox("Enter connection string", "Connetion string prompt", connectionString);
                if (connectionString == "")
                    Close();

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

            if (connectionString.ToLower().Contains("postgre"))
            {
                time_format = "TIMESTAMP";
                postgres = true;
            }

            InitializeComponent();
        }

        private void notifySuccess(string db, string table)
        {
            MessageBox.Show(String.Format("База данных '{0}' и дочерняя таблица '{1}' созданны успешно", db, table), "Успех");
        }

        private void createDbTable(string db_name, string create_db, string create_table)
        {
            using (OdbcConnection conn = new OdbcConnection(connectionString))
            {
                conn.Open();

                using (OdbcCommand cmd_db = new OdbcCommand(create_db, conn))
                {
                    cmd_db.ExecuteNonQuery();
                }
            }
            using (OdbcConnection conn = new OdbcConnection(connectionString + "Database=" + db_name))
            using (OdbcCommand cmd = new OdbcCommand(create_table, conn))
            {
                conn.Open(); cmd.ExecuteNonQuery();
            }
        }

        private void cars_butt_Click(object sender, EventArgs _)
        {
            string table_name = "cars_table";
            string db_name = "carsdb";
            string create_db = "CREATE DATABASE " + db_name + ";";
            string create = "CREATE TABLE " + table_name + "(" +
                "id VARCHAR(12) PRIMARY KEY," +
                "weight INTEGER," +
                "corr INTEGER," +
                "gn VARCHAR(12))";
            try
            {
                createDbTable(db_name, create_db, create);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            notifySuccess(db_name, table_name);
        }

        private void drivers_butt_Click(object sender, EventArgs _)
        {
            string table_name = "drivers";
            string db_name = "driversdb";
            string create_db = "CREATE DATABASE " + db_name + ";";
            string create = "CREATE TABLE " + table_name + "(" +
                 "id INTEGER PRIMARY KEY," +
                 "fio VARCHAR(12))";
            try
            {
                createDbTable(db_name, create_db, create);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            notifySuccess(db_name, table_name);
        }

        private void store_butt_Click(object sender, EventArgs _)
        {
            string table_name = "info";
            string db_name = "storedb";
            string sequence_name = "event_id";
            string create_db = "CREATE DATABASE " + db_name + ";";
            string create = "CREATE TABLE " + table_name + "(" +
                 String.Format("n {0} PRIMARY KEY ,", postgres ? "BIGSERIAL" : "INTEGER IDENTITY(1, 1)") +
                 "com VARCHAR(6)," +
                 "id VARCHAR(12)," +
                 "event_id INTEGER," +
                 "weight INTEGER," +
                 "inp_weight INTEGER," +
                 "ts " + time_format + " DEFAULT CURRENT_TIMESTAMP);" +
                 String.Format("CREATE SEQUENCE {0} MINVALUE 0 MAXVALUE 1000 CYCLE;", sequence_name); // create sequence also
            // n isn't serial
            // ts isn't compatible with postgres
            // do message box query whether db is postgres or mssql
            try
            {
                createDbTable(db_name, create_db, create);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            MessageBox.Show(String.Format("База данных '{0}' и дочерние таблицы '{1}' и '{2}' созданны успешно", db_name, table_name, sequence_name), "Успех");
        }

        private void store_info_butt_Click(object sender, EventArgs _)
        {
            string table_name = "info";
            string db_name = "store_infodb";
            string create_db = "CREATE DATABASE " + db_name + ";";
            string create = "CREATE TABLE " + table_name + "(" +
                 "event_id INTEGER PRIMARY KEY," +
                 "com VARCHAR(6)," +
                 "barcode VARCHAR(1024)," +
                 "gn VARCHAR(12)," + 
                 "fio VARCHAR(60)," + 
                 "ts " + time_format + " DEFAULT CURRENT_TIMESTAMP);";
            try
            {
                createDbTable(db_name, create_db, create);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            notifySuccess(db_name, table_name);
        }

        private void debug_Click(object sender, EventArgs _)
        {
            string table_name = "debug";
            string db_name = "debugdb";
            string create_db = "CREATE DATABASE " + db_name + ";";
            string create = "CREATE TABLE " + table_name + "(" +
                 String.Format("id {0} PRIMARY KEY,", postgres ? "SERIAL" : "INTEGER IDENTITY(1, 1)") +
                 "ts " + time_format + " DEFAULT CURRENT_TIMESTAMP," +
                 "code INTEGER," +
                 "message VARCHAR(1024));";
            try
            {
                createDbTable(db_name, create_db, create);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
                return;
            }
            notifySuccess(db_name, table_name);
        }
    }
}
