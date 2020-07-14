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

namespace driversTable
{
    public partial class Form1 : Form
    {
        string connectionString = @"Driver={PostgreSQL UNICODE};Server=localhost;Port=5432;Database=w_ext;Uid=postgres;Pwd=root;";
        OdbcCommandBuilder cb;

        public Form1()
        {
            connectionString = Interaction.InputBox("Enter connection string", "Connetion string prompt", connectionString);
            if (connectionString.Length > 0)
            {
                InitializeComponent();
            }
            else
            {
                Close();
                Application.Exit();
            }
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
                if (!Validate())
                    return;

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

        private bool fio_valid(string value)
        {
            return Regex.IsMatch(value.Trim(), @"[А-Я][а-я]+ [А-Я][а-я]+ [А-Я][а-я]+");
        }

        private void dataGridView1_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            
            string header =
                dataGridView1.Columns[e.ColumnIndex].HeaderText;
            switch (header)
            {
                case "fio":
                    {
                        e.Cancel = !fio_valid(dataGridView1[e.ColumnIndex, e.RowIndex].EditedFormattedValue.ToString());
                        if (e.Cancel)
                        {
                            dataGridView1.Rows[e.RowIndex].ErrorText = "fio колонка должна содержать ФИО в полном формате: Фамилия Имя Отчество";
                            MessageBox.Show(dataGridView1.Rows[e.RowIndex].ErrorText);
                        }
                    }
                    break;
            }
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {

            if (dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
            {
                if (e.RowIndex == 0)
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value = String.Format("{0, 0:D8}", 1);
                else
                {
                    var str = dataGridView1.Rows[e.RowIndex - 1].Cells[0].Value.ToString();
                    var num = ulong.Parse(str);
                    dataGridView1.Rows[e.RowIndex].Cells[0].Value = String.Format("{0, 0:D8}", (num + 1));
                }
            }
        }
    }
}
