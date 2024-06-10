using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ViewCreator
{
    public partial class FieldSelectionForm : Form
    {
        private string connectionString;
        private string selectedDatabase;
        private List<string> selectedTables;

        public FieldSelectionForm(string connectionString, string selectedDatabase, List<string> selectedTables)
        {
            InitializeComponent();
            this.connectionString = connectionString;
            this.selectedDatabase = selectedDatabase;
            this.selectedTables = selectedTables;
            LoadFields();
            LoadFunctions();
        }

        private void LoadFields()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (string tableName in selectedTables)
                    {
                        string query = $"SELECT COLUMN_NAME FROM {selectedDatabase}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = @TableName";
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@TableName", tableName);

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable fields = new DataTable();
                        adapter.Fill(fields);

                        foreach (DataRow row in fields.Rows)
                        {
                            lstFields.Items.Add(row["COLUMN_NAME"].ToString());
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error SQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFunctions()
        {
            cmbFunctions.Items.AddRange(new string[] { "None", "SUM", "AVG", "COUNT", "MAX", "MIN", "CONCAT" });
        }

        private void btnAddField_Click(object sender, EventArgs e)
        {
            if (lstFields.SelectedItems.Count > 0)
            {
                foreach (var selectedItem in lstFields.SelectedItems)
                {
                    // Acceder al texto del campo seleccionado y agregarlo a lstSelectedFields
                    string fieldName = selectedItem.ToString();
                    lstSelectedFields.Items.Add(fieldName);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione al menos un campo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCreateView_Click(object sender, EventArgs e)
        {
            if (lstSelectedFields.Items.Count > 0)
            {
                // Generar el nombre de la tabla combinando los nombres de las tablas seleccionadas
                string tableName = GenerateTableName();

                // Crear una nueva base de datos
                string newDatabaseName = $"{selectedDatabase}_Copy";
                CreateDatabase(newDatabaseName);

                // Crear la tabla en la nueva base de datos
                CreateTable(newDatabaseName, tableName);

                // Crear una lista para almacenar los nombres de los campos seleccionados
                List<string> selectedFields = new List<string>();

                // Obtener los nombres de los campos seleccionados en lstSelectedFields
                foreach (ListViewItem selectedItem in lstSelectedFields.Items)
                {
                    selectedFields.Add(selectedItem.Text);
                }

                // Agregar las columnas a la tabla
                AddColumns(newDatabaseName, tableName, selectedFields);

                // Insertar datos en la nueva tabla desde las tablas originales
                InsertDataFromOriginalTables(newDatabaseName, tableName, selectedFields);

                MessageBox.Show("La tabla se ha creado correctamente y los datos se han conservado.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Por favor, seleccione al menos un campo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private string GenerateTableName()
        {
            // Combinar los nombres de las tablas seleccionadas
            string combinedTableName = string.Join("_", selectedTables.Select(t => t.Replace(".", "_")));

            // Construir el nombre de la tabla con el prefijo "VM_DIM_"
            return $"VM_DIM_{combinedTableName}";
        }

        private void CreateDatabase(string databaseName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Eliminar la base de datos si ya existe
                    DropDatabase(databaseName, connection);
                    SqlCommand command = new SqlCommand($"CREATE DATABASE {databaseName}", connection);
                    command.ExecuteNonQuery();
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error SQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DropDatabase(string databaseName, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand($"IF EXISTS (SELECT 1 FROM sys.databases WHERE name = '{databaseName}') DROP DATABASE {databaseName}", connection);
            command.ExecuteNonQuery();
        }

        private void CreateTable(string databaseName, string tableName)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = $"CREATE TABLE {databaseName}.dbo.{tableName} (ID INT IDENTITY(1,1) PRIMARY KEY)";
                    ExecuteNonQuery(query, connection);
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Error SQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddColumns(string databaseName, string tableName, List<string> columnNames)
        {
            foreach (string columnName in columnNames)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = $"ALTER TABLE {databaseName}.dbo.{tableName} ADD {columnName} NVARCHAR(255)";
                        ExecuteNonQuery(query, connection);
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show($"Error SQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void InsertDataFromOriginalTables(string newDatabaseName, string newTableName, List<string> selectedFields)
        {
            foreach (string originalTableName in selectedTables)
            {
                foreach (string field in selectedFields)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string query = $"INSERT INTO {newDatabaseName}.dbo.{newTableName} ({field}) SELECT {field} FROM {selectedDatabase}.dbo.{originalTableName}";
                            ExecuteNonQuery(query, connection);
                        }
                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show($"Error SQL: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void ExecuteNonQuery(string query, SqlConnection connection)
        {
            SqlCommand command = new SqlCommand(query, connection);
            command.ExecuteNonQuery();
        }
    }
}
