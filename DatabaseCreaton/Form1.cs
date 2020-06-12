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
                }
                catch (Exception e)
                {
                    MessageBox.Show("Ошибка соединения с базой данных:\n\t" + e.Message);
                    continue;
                }
            } while (false);

            if (connectionString.ToLower().Contains("postgre"))
            {
                time_format = "TIMESTAMP";
                postgres = true;
            }

            InitializeComponent();
        }

        private void cars_butt_Click(object sender, EventArgs _)
        {
            string create = "CREATE TABLE cars(" +
                "id VARCHAR(12) PRIMARY KEY," +
                "weight INTEGER," +
                "corr INTEGER," +
                "gn VARCHAR(12))";
            try
            {
                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(create, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void drivers_butt_Click(object sender, EventArgs _)
        {
            string create = "CREATE TABLE drivers(" +
                 "id INTEGER PRIMARY KEY," +
                 "fio VARCHAR(12))";
            try
            {
                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(create, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void store_butt_Click(object sender, EventArgs _)
        {
            string create = "CREATE TABLE store(" +
                 String.Format("n {0} PRIMARY KEY ,", postgres ? "BIGSERIAL" : "INTEGER IDENTITY(1, 1)") +
                 "com VARCHAR(6)," +
                 "id VARCHAR(12)," +
                 "event_id INTEGER," +
                 "weight INTEGER," +
                 "inp_weight INTEGER," +
                 "ts " + time_format + " DEFAULT CURRENT_TIMESTAMP);";
            // n isn't serial
            // ts isn't compatible with postgres
            // do message box query whether db is postgres or mssql
            try
            {
                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(create, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void store_info_butt_Click(object sender, EventArgs _)
        {
            string create = "CREATE TABLE store_info(" +
                 "event_id INTEGER PRIMARY KEY," +
                 "com VARCHAR(6)," +
                 "barcode VARCHAR(1024)," +
                 "fio VARCHAR(60)," + 
                 "ts " + time_format + " DEFAULT CURRENT_TIMESTAMP);";
            try
            {
                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(create, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private void debug_Click(object sender, EventArgs _)
        {
            string create = "CREATE TABLE debug(" +
                 String.Format("id {0} PRIMARY KEY,", postgres ? "SERIAL" : "INTEGER IDENTITY(1, 1)") +
                 "ts " + time_format + " DEFAULT CURRENT_TIMESTAMP," +
                 "code INTEGER," +
                 "message VARCHAR(1024));";
            try
            {
                using (OdbcConnection conn = new OdbcConnection(connectionString))
                {
                    conn.Open();
                    using (OdbcCommand cmd = new OdbcCommand(create, conn))
                    {
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
    }
}
