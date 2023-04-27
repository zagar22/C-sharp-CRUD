using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TpCrud
{
    /// <summary>
    /// Lógica de interacción para Editar.xaml
    /// </summary>
    public partial class Editar : Window
    {
        SqlConnection sql;
        
        int id;
        public Editar(int id )
        {
            
            InitializeComponent();
            this.id = id;

            string conn = ConfigurationManager.ConnectionStrings["TpCrud.Properties.Settings.PracticasConnectionString"].ConnectionString;

            try
            {
                sql = new SqlConnection(conn);
                MostarAutoSeleccionado(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void btnCancelar_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnGuardarEditado_Click(object sender, RoutedEventArgs e)
        {
            
            GuardarEditado(id);
            this.Close();
        }
        private void GuardarEditado(int id)
        {
            string query = "UPDATE CONCESIONARIA SET MARCA = @MARCA, MODELO = @MODELO, PATENTE = @PATENTE, PRECIO = @PRECIO  WHERE id = @id";

            SqlCommand command = new SqlCommand(query, sql);

            sql.Open();

            command.Parameters.AddWithValue("@id", id);
            command.Parameters.AddWithValue("@MARCA", txtMarca.Text);
            command.Parameters.AddWithValue("@MODELO", txtModelo.Text);
            command.Parameters.AddWithValue("@PATENTE", txtPatente.Text);
            command.Parameters.AddWithValue("@PRECIO", txtPrecio.Text); 

            command.ExecuteNonQuery();
            sql.Close();
        }

        private void MostarAutoSeleccionado(int id)
        {
            string query = $"SELECT * FROM CONCESIONARIA WHERE ID={id}";

            SqlDataAdapter miAdaptador = new SqlDataAdapter(query, sql);

            using (miAdaptador)
            {
                DataTable data = new DataTable();

                miAdaptador.Fill(data);
                txtMarca.Text = data.Rows[0]["marca"].ToString();
                txtModelo.Text = data.Rows[0]["modelo"].ToString();
                txtPatente.Text = data.Rows[0]["patente"].ToString();
                txtPrecio.Text = data.Rows[0]["precio"].ToString();
                

            }
        }
       


    }
}