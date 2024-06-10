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
                // Concatenar los nombres de las tablas seleccionadas para la consulta SQL
                string tables = string.Join(",", selectedTables);

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string tabless = string.Join(",", selectedTables);
                    string tableNames = string.Join(",", selectedTables.Select(tableName => $"'{tableName}'"));

                    string query = $"SELECT COLUMN_NAME FROM {selectedDatabase}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME IN ({tableNames})";
                    SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                    DataTable fields = new DataTable();
                    adapter.Fill(fields);

                    // Agregar los campos al ListBox
                    foreach (DataRow row in fields.Rows)
                    {
                        lstFields.Items.Add(row["COLUMN_NAME"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar los campos: {ex.GetType().Name}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFunctions()
        {
            // Agregar las funciones disponibles al ComboBox
            cmbFunctions.Items.Add("SUM");
            cmbFunctions.Items.Add("AVG");
            cmbFunctions.Items.Add("COUNT");
            cmbFunctions.Items.Add("MAX");
            cmbFunctions.Items.Add("MIN");
            cmbFunctions.Items.Add("CONCAT");
        }

        private void btnAddField_Click(object sender, EventArgs e)
        {
            // Verificar si se seleccionó un campo
            if (lstFields.SelectedItem != null)
            {
                // Agregar el campo seleccionado a los campos seleccionados
                lstSelectedFields.Items.Add(lstFields.SelectedItem.ToString());
            }
            else
            {
                MessageBox.Show("Por favor, seleccione un campo.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCreateView_Click(object sender, EventArgs e)
        {
            if (lstSelectedFields.Items.Count > 0 && cmbFunctions.SelectedItem != null)
            {
                try
                {
                    string selectedFunction = cmbFunctions.SelectedItem.ToString();
                    List<string> selectedFields = lstSelectedFields.Items.Cast<string>().ToList();

                    // Crear la vista con los campos seleccionados y la función seleccionada
                    CreateView(selectedFields, selectedFunction);

                    // Mostrar el formulario de confirmación
                    //ViewConfirmationForm confirmationForm = new ViewConfirmationForm(selectedFields, selectedFunction);
                    //confirmationForm.ShowDialog(); // Mostrar el formulario como cuadro de diálogo

                    // Opcional: cerrar este formulario si deseas que se cierre automáticamente después de crear la vista
                    // this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error al crear la vista: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Por favor, seleccione al menos un campo y una función.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        private void CreateView(List<string> selectedFields, string selectedFunction)
        {
            try
            {
                // Construir el nombre de la vista
                string viewName = "VW_DIM_" + selectedTables.First() + "_" + selectedFunction.ToUpper();

                // Construir la lista de campos para la consulta SELECT
                string fieldsList = string.Join(", ", selectedFields.Select(field => $"[{field}]"));

                // Consultar la estructura de las tablas seleccionadas
                Dictionary<string, List<string>> tableStructures = new Dictionary<string, List<string>>();
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    foreach (string tableName in selectedTables)
                    {
                        // Consultar las columnas de la tabla
                        string queryColumns = $"SELECT COLUMN_NAME FROM {selectedDatabase}.INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
                        SqlCommand commandColumns = new SqlCommand(queryColumns, connection);
                        using (SqlDataReader readerColumns = commandColumns.ExecuteReader())
                        {
                            List<string> columns = new List<string>();
                            while (readerColumns.Read())
                            {
                                columns.Add(readerColumns.GetString(0));
                            }
                            tableStructures.Add(tableName, columns);
                        }
                    }
                }

                // Crear las tablas si no existen
                foreach (var pair in tableStructures)
                {
                    string tableName = pair.Key;
                    List<string> columns = pair.Value;

                    if (!TableExists(viewName, columns))
                    {
                        CreateTable(tableName, columns);
                    }
                }

                // Construir la consulta SQL para crear la vista
                string query = $"CREATE VIEW {viewName} AS " +
                               $"SELECT {selectedFunction}({fieldsList}) AS [{selectedFunction}] FROM {selectedTables.First()}";

                // Ejecutar la consulta en la base de datos
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }

                MessageBox.Show("La vista se ha creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la vista: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private bool TableExists(string tableName, List<string> columns)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Consultar si la tabla ya existe en la base de datos
                    string query = $"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
                    SqlCommand command = new SqlCommand(query, connection);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al verificar la existencia de la tabla: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void CreateTable(string tableName, List<string> columns)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    // Construir la consulta SQL para crear la tabla
                    string columnsDefinition = string.Join(", ", columns.Select(column => $"{column} VARCHAR(255)"));
                    string query = $"CREATE TABLE {tableName} ({columnsDefinition})";
                    SqlCommand command = new SqlCommand(query, connection);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show($"La tabla {tableName} se ha creado correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al crear la tabla {tableName}: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
    }

