using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewCreator
{
    public partial class ConnectionForm : Form
    {
        public string ConnectionString { get; private set; }

        public ConnectionForm()
        {
            InitializeComponent();
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                string server = txtServer.Text;
                string user = txtUser.Text;
                string password = txtPassword.Text;

                // Construir la cadena de conexión
                ConnectionString = $"Server={server};User Id={user};Password={password};";

                // Probar la conexión
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();
                    MessageBox.Show("Conexión exitosa", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Abrir el formulario de selección de base de datos y pasar la cadena de conexión
                DatabaseSelectionForm databaseSelectionForm = new DatabaseSelectionForm(ConnectionString);
                databaseSelectionForm.Show();

                // Ocultar el formulario de conexión en lugar de cerrarlo
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al conectar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
