using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ViewCreator
{
    public partial class TableSelectionForm : Form
    {
        private string connectionString;
        private string selectedDatabase;

        public TableSelectionForm(string connectionString, string selectedDatabase)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.selectedDatabase = selectedDatabase;
            LoadTables();
        }
        private void LoadTables()
        {
            
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Consulta para obtener las tablas de la base de datos seleccionada
                    string query = $"SELECT TABLE_NAME FROM {selectedDatabase}.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";

                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable tables = new DataTable();
                    adapter.Fill(tables);

                    // Agregar las tablas al ListBox
                    foreach (DataRow row in tables.Rows)
                    {
                        clbTables.Items.Add(row["TABLE_NAME"]);
                    }
                }
            
           
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // Obtener las tablas seleccionadas
            List<string> selectedTables = new List<string>();
            foreach (var item in clbTables.CheckedItems)
            {
                selectedTables.Add(item.ToString());
            }

            // Pasar las tablas seleccionadas al formulario FieldSelectionForm
            FieldSelectionForm fieldSelectionForm = new FieldSelectionForm(connectionString, selectedDatabase, selectedTables);
            fieldSelectionForm.Show();

            // Cerrar este formulario
            this.Hide();
        }
    }
    }

