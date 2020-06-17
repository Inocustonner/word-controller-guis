using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;
using System.Globalization;

namespace carsTable
{
    public partial class CarsTableForm : Form
    {
        enum ValidationError
        {
            NOT_UNIQUE,
            INVALID_FORMAT,

            NONE,
        }

        string connectionString = @"Driver={PostgreSQL UNICODE};Server=localhost;Port=5432;Database=carsdb;Uid=postgres;Pwd=root;";

        OdbcCommandBuilder cb;
        public CarsTableForm()
        {
            connectionString = Interaction.InputBox("Enter connection string", "Connetion string prompt", connectionString);
            InitializeComponent();
            bindingNavigator1.CausesValidation = false;
        }

        void gridRefresh()
        {
            this.cars_tableTableAdapter.Fill(this.carsdbDataSet.cars_table);
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

            // if not admin hide corr
            string password = "C0L7B7Dm3n";
            var pass = Interaction.InputBox("Enter admin password", "Admin pass");
            if (pass != password)
                dataGridCars.Columns[2].Visible = false;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Validate())
                    return;

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

        private bool id_valid_format(string value)
        {
            int _;
            return int.TryParse(value, NumberStyles.HexNumber, null, out _);
        }

        private bool is_col_exists(string col, string value, bool is_char = true)
        {
            using (var conn = new OdbcConnection(connectionString))
            {

                conn.Open();
                var cmd = new OdbcCommand(String.Format("SELECT COUNT({0}) FROM cars_table WHERE {0}={1}", col, is_char ? "'" + value + "'" : value ), conn) ;
                return cmd.ExecuteScalar().ToString() != "0";
            }
        }

        private bool gn_valid_format(string value)
        {
            return Regex.IsMatch(value.Trim(), @"^[АВЕКМНОРСТУХ]\d{3}[АВЕКМНОРСТУХ]{2}\d{1,3}$");
        }

        private ValidationError id_valid(string value)
        {
            if (!id_valid_format(value))
                return ValidationError.INVALID_FORMAT;
            else if (is_col_exists("id", value))
                return ValidationError.NOT_UNIQUE;
            else
                return ValidationError.NONE;
        }

        private ValidationError gn_valid(string value)
        {
            if (!gn_valid_format(value))
                return ValidationError.INVALID_FORMAT;
            else if (is_col_exists("gn", value))
                return ValidationError.NOT_UNIQUE;
            else
                return ValidationError.NONE;
        }


        private bool validate_col_gn_id(string col, string value, string prev_value)
        {
            bool is_id = col == "id";
            switch (is_id ? id_valid(value) : gn_valid(value))
            {
                case ValidationError.INVALID_FORMAT:
                    {
                        if (is_id)
                            MessageBox.Show(String.Format("{0} колонка должна содержать только численные 16тиричные значения", col));
                        else
                            MessageBox.Show(String.Format("{0} колонка должна содержать правильный формат гос. номера с только заглавными буквами", col));
                        return true;
                    }
                case ValidationError.NOT_UNIQUE:
                    {
                        if (prev_value != value)
                        {
                            MessageBox.Show(String.Format("{0} колонка должна быть уникальной", col));
                            return true;
                        }
                    }break;
            }
            return false;
        }

        private void dataGridCars_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string header =
                dataGridCars.Columns[e.ColumnIndex].HeaderText;
            if (header == "id" || header == "gn")
                e.Cancel = validate_col_gn_id(header, dataGridCars[e.ColumnIndex, e.RowIndex].EditedFormattedValue.ToString(), dataGridCars[e.ColumnIndex, e.RowIndex].Value.ToString());
        }

        private void dataGridCars_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridCars.Rows[e.RowIndex].ErrorText = String.Empty;
        }

        private void dataGridCars_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            //to hex12 + 1
            if (dataGridCars.Rows[e.RowIndex].Cells[0].Value.ToString() == "")
            {
                if (e.RowIndex == 0)
                    dataGridCars.Rows[e.RowIndex].Cells[0].Value = 1;
                else
                {
                    var str = dataGridCars.Rows[e.RowIndex - 1].Cells[0].Value.ToString();
                    var num = ulong.Parse(str, NumberStyles.HexNumber);
                    dataGridCars.Rows[e.RowIndex].Cells[0].Value = (num + 1).ToString("X12");
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            int corr;
            if (int.TryParse(Interaction.InputBox("Введите коррекцию которая будет выставленна всем", "Correction"), out corr))
            {
                using (var conn = new OdbcConnection(connectionString))
                using (var cmd = new OdbcCommand(String.Format("UPDATE cars_table SET corr={0}", corr), conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                gridRefresh();
            }
            else
            {
                MessageBox.Show("Принимаются только числовые значения");
            }
        }
    }

    class RowComparer : System.Collections.IComparer
    {
        private static int sortOrderModifier = 1;

        public RowComparer(SortOrder sortOrder)
        {
            if (sortOrder == SortOrder.Descending)
            {
                sortOrderModifier = -1;
            }
            else if (sortOrder == SortOrder.Ascending)
            {
                sortOrderModifier = 1;
            }
        }

        public int Compare(object x, object y)
        {
            DataGridViewRow DataGridViewRow1 = (DataGridViewRow)x;
            DataGridViewRow DataGridViewRow2 = (DataGridViewRow)y;

            bool cmp = ulong.Parse(DataGridViewRow1.Cells[0].Value.ToString(), NumberStyles.HexNumber) < ulong.Parse(DataGridViewRow2.Cells[0].Value.ToString(), NumberStyles.HexNumber);
            int CompareResult = cmp ? 1 : -1;
            return CompareResult * sortOrderModifier;
        }
    }
}
