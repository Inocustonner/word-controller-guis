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

namespace driversTable
{
    public partial class Form1 : Form
    {
        string connectionString = @"Driver={PostgreSQL UNICODE};Server=localhost;Port=5432;Database=drivers;Uid=postgres;Pwd=root;";
        OdbcCommandBuilder cb;

        public Form1()
        {
            connectionString = Interaction.InputBox("Enter connection string", "Connetion string prompt", connectionString);
            InitializeComponent();
        }

        void populateTable()
        {
            try
            {
                this.driversTableAdapter.Fill(this.driversDataSet.drivers);
                cb = new OdbcCommandBuilder(driversTableAdapter.Adapter);
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка при подключении к датабазе:\n\t" + e.Message);
                Close();
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            driversTableAdapter.Connection.ConnectionString = connectionString;
            populateTable();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Validate();
                driversBindingSource.EndEdit();
                // postgres don't use [ ] in it's syntax
                if (cb.DataAdapter.InsertCommand != null)
                    cb.DataAdapter.InsertCommand.CommandText = cb.DataAdapter.InsertCommand.CommandText.Replace("[", "").Replace("]", "");
                if (cb.DataAdapter.UpdateCommand != null)
                    cb.DataAdapter.UpdateCommand.CommandText = cb.DataAdapter.UpdateCommand.CommandText.Replace("[", "").Replace("]", "");
                if (cb.DataAdapter.DeleteCommand != null)
                    cb.DataAdapter.DeleteCommand.CommandText = cb.DataAdapter.DeleteCommand.CommandText.Replace("[", "").Replace("]", "");

                driversTableAdapter.Update(driversDataSet);
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка при выполнении операций обновления:\n\t" + err.Message);
            }
        }
    }
}
