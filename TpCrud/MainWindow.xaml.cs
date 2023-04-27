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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace TpCrud
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sql;
        
        public MainWindow()
        {
            InitializeComponent();
            string conn = ConfigurationManager.ConnectionStrings["TpCrud.Properties.Settings.PracticasConnectionString"].ConnectionString;

            try
            {
                sql = new SqlConnection(conn);
                MostrarInformacion();
            }
            catch (Exception)
            {

                throw;
            }

        }

        private void MostrarInformacion()
        {            
            string query = "SELECT * FROM CONCESIONARIA";
            SqlDataAdapter miAdaptador = new SqlDataAdapter(query,sql);

            using (miAdaptador) 
            {
                DataTable data = new DataTable();

                miAdaptador.Fill(data);
                infoAutos.SelectedValuePath = "id"; 
                infoAutos.ItemsSource = data.DefaultView; 
            }
        }



        private void AbrirVentanaEditar()
        {
            if (infoAutos.SelectedValue != null)
            {
                int id = int.Parse(infoAutos.SelectedValue.ToString());
                Editar editar = new Editar(id);
                editar.ShowDialog();
                MostrarInformacion();
            }
            else
            {
                MessageBox.Show("DEBE SELECCIONAR UN AUTOMOVIL");
            }
           
        }

        private void btnEditar_Click(object sender, RoutedEventArgs e)
        {
            AbrirVentanaEditar();
        }

      

        private void btnEliminar_Click(object sender, RoutedEventArgs e)
        {

            if (infoAutos.SelectedValue != null)
            {
                BorrarCoche();
                MostrarInformacion();
            }
            else
            {
                MessageBox.Show("DEBE SELECCIONAR UN AUTOMOVIL");
            }   
                
        }

        

        private void btnGuardar_Click(object sender, RoutedEventArgs e)
        {
            GuardarCoche();
            MostrarInformacion();
        }

        private void GuardarCoche()
        {
            string query = "INSERT INTO CONCESIONARIA(MARCA,MODELO,PATENTE,PRECIO) VALUES(@marca,@modelo,@patente,@precio)";

            

            SqlCommand command = new SqlCommand(query, sql);

            sql.Open();

            command.Parameters.AddWithValue("@marca", txtMarca.Text);
            command.Parameters.AddWithValue("@modelo", txtModelo.Text);
            command.Parameters.AddWithValue("@patente", txtPatente.Text);
            command.Parameters.AddWithValue("@precio", txtPrecio.Text);

            command.ExecuteNonQuery();
            sql.Close();

            txtMarca.Clear();
            txtModelo.Clear();
            txtPatente.Clear();
            txtPrecio.Clear();
        }

        private void BorrarCoche()
        {
            string query = "DELETE FROM CONCESIONARIA WHERE ID=@Id";
            if (MessageBox.Show("Esta seguro que quiere Eliminar el registro?", "Eliminar", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                SqlCommand command = new SqlCommand(query, sql);

                sql.Open();
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Id", infoAutos.SelectedValue);
                command.ExecuteNonQuery();
                sql.Close();
            }

        }

        private void txtBuscar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string query = $"SELECT * FROM CONCESIONARIA WHERE MARCA LIKE '{txtBuscar.Text}%'";

            SqlDataAdapter miAdaptador = new SqlDataAdapter(query, sql);

            using (miAdaptador)
            {
                DataTable data = new DataTable();

                miAdaptador.Fill(data);
                infoAutos.ItemsSource = data.DefaultView;
                infoAutos.DisplayMemberPath = "CONCESIONARIA";

            }
        }
       
    }
}
