using System;
using System.Data;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace ViewCreator
{
    public partial class DatabaseSelectionForm : Form
    {
        private string connectionString;

        public DatabaseSelectionForm(string connectionString)
        {
            InitializeComponent();
            this.connectionString = $"{connectionString};TrustServerCertificate=True";
            LoadDatabases();
        }
        private void LoadDatabases()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    DataTable databases = connection.GetSchema("Databases");

                    foreach (DataRow row in databases.Rows)
                    {
                        cmbDatabases.Items.Add(row["database_name"]);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar bases de datos: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void btnLoadTables_Click(object sender, EventArgs e)
        {
            if (cmbDatabases.SelectedItem != null)
            {
                string selectedDatabase = cmbDatabases.SelectedItem.ToString();
                TableSelectionForm tableSelectionForm = new TableSelectionForm(connectionString, selectedDatabase);
                tableSelectionForm.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Por favor, seleccione una base de datos.", "Advertencia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DatabaseSelectionForm_Load(object sender, EventArgs e)
        {

        }
    }
}
