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

namespace carsTable
{
    public partial class CarsTableForm : Form
    {
        string connectionString = @"Driver={PostgreSQL UNICODE};Server=localhost;Port=5432;Database=cars;Uid=postgres;Pwd=root;";
        OdbcCommandBuilder cb;
        public CarsTableForm()
        {
            connectionString = Interaction.InputBox("Enter connection string", "Connetion string prompt", connectionString);
            InitializeComponent();
        }

        void populateTable()
        {
            try
            {
                this.cars_tableTableAdapter.Fill(this.carsdbDataSet.cars_table);
                cb = new OdbcCommandBuilder(cars_tableTableAdapter.Adapter);
            }
            catch (Exception e)
            {
                MessageBox.Show("Произошла ошибка при подключении к датабазе:\n\t" + e.Message);
                Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.cars_tableTableAdapter.Connection.ConnectionString = connectionString;
            populateTable();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                Validate();
                carstableBindingSource.EndEdit();

                if (cb.DataAdapter.InsertCommand != null)
                    cb.DataAdapter.InsertCommand.CommandText = cb.DataAdapter.InsertCommand.CommandText.Replace("[", "").Replace("]", "");
                if (cb.DataAdapter.UpdateCommand != null)
                    cb.DataAdapter.UpdateCommand.CommandText = cb.DataAdapter.UpdateCommand.CommandText.Replace("[", "").Replace("]", "");
                if (cb.DataAdapter.DeleteCommand != null)
                    cb.DataAdapter.DeleteCommand.CommandText = cb.DataAdapter.DeleteCommand.CommandText.Replace("[", "").Replace("]", "");

                cars_tableTableAdapter.Update(carsdbDataSet);
            }
            catch (Exception err)
            {
                MessageBox.Show("Ошибка при выполнении операций обновления:\n\t" + err.Message);
            }
        }
    }
}
