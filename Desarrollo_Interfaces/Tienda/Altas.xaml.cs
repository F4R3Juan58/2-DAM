using Microsoft.Data.Sqlite;
using Microsoft.Maui;
using Microsoft.Maui.Controls;

namespace Tienda;

public partial class Altas : ContentPage
{
    int idCliente;
    string connectionString = "Data Source=tienda.db";
    ConsultaLista consultaLista = new ConsultaLista();
    public Altas()
	{
		InitializeComponent();
        cargarClientes();
	}

    private void cargarClientes()
    {
        ClienteListView.ItemsSource = null;

        var clientes = new HashSet<string>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "SELECT nombre, apellidos FROM clientes;";

            using (var command = new SqliteCommand(selectQuery, connection))
            using (var reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    clientes.Add(reader.GetString(0) + " " +reader.GetString(1));
                }
            }
            connection.Close();
        }

        ClienteListView.ItemsSource = clientes;
    }

    private void LimpiarClicked(object sender, EventArgs e)
    {
        nombreAltas.Text = "";
        apellidoAltas.Text = "";
        ciudadAltas.Text = "";
        emailAltas.Text = "";
        comentarioAltas.Text = "";
    }

    private void BorrarClicked(object sender, EventArgs e)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "DELETE FROM clientes where id=@idCliente;";

            using (var command = new SqliteCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@idCliente", idCliente);
                command.ExecuteReader();
            }
            connection.Close();
            idCliente = 0;
            nombreAltas.Text = "";
            apellidoAltas.Text = "";
            ciudadAltas.Text = "";
            emailAltas.Text = "";
            comentarioAltas.Text = "";
            vipCheckbox.IsChecked = false;
            cargarClientes();
            consultaLista.CargarDatos();
        }
    }

    private void ModificarClicked(object sender, EventArgs e)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string selectQuery = "UPDATE clientes SET nombre = @nombre , apellidos = @apellidos , ciudad = @ciudad , correo_electronico = @correo_electronico , comentarios = @comentarios, vip = @vip  where id=@idCliente;";

            using (var command = new SqliteCommand(selectQuery, connection))
            {
                command.Parameters.AddWithValue("@idCliente", idCliente);
                command.Parameters.AddWithValue("@nombre", nombreAltas.Text);
                command.Parameters.AddWithValue("@apellidos", apellidoAltas.Text);
                command.Parameters.AddWithValue("@ciudad", ciudadAltas.Text);
                command.Parameters.AddWithValue("@correo_electronico", emailAltas.Text);
                command.Parameters.AddWithValue("@comentarios", comentarioAltas.Text);
                command.Parameters.AddWithValue("@vip", vipCheckbox.IsChecked ? 1 : 0);

                command.ExecuteReader();
            }
            connection.Close();
            idCliente = 0;
            nombreAltas.Text = "";
            apellidoAltas.Text = "";
            ciudadAltas.Text = "";
            emailAltas.Text = "";
            comentarioAltas.Text = "";
            vipCheckbox.IsChecked = false;
            cargarClientes();
            consultaLista.CargarDatos();
        }
    }

    private void AñadirClicked(object sender, EventArgs e)
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string insertQuery = @"
            INSERT INTO clientes (nombre, apellidos, ciudad, correo_electronico, comentarios, vip) 
            VALUES (@nombre, @apellidos, @ciudad, @correo_electronico, @comentarios, @vip);";

            using (var command = new SqliteCommand(insertQuery, connection))
            {
                command.Parameters.AddWithValue("@nombre", nombreAltas.Text);
                command.Parameters.AddWithValue("@apellidos", apellidoAltas.Text);
                command.Parameters.AddWithValue("@ciudad", ciudadAltas.Text);
                command.Parameters.AddWithValue("@correo_electronico", emailAltas.Text);
                command.Parameters.AddWithValue("@comentarios", comentarioAltas.Text);
                command.Parameters.AddWithValue("@vip", vipCheckbox.IsChecked ? 1 : 0);  // 1 si está marcado, 0 si no

                command.ExecuteNonQuery();
            }
            nombreAltas.Text = "";
            apellidoAltas.Text = "";
            ciudadAltas.Text = "";
            emailAltas.Text = "";
            comentarioAltas.Text = "";
            vipCheckbox.IsChecked= false;
            cargarClientes();
            consultaLista.CargarDatos();
        }
    }
        private void cargarCliente(string cliente)
        {
            string[] partes = cliente.Split(' ');

            string nombre = partes[0];
            string apellido = partes[1];
        using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT * FROM clientes where nombre=@nombre and apellidos = @apellidos;";

                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@apellidos", apellido);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            idCliente = reader.GetInt32(0);
                            nombreAltas.Text = reader.GetString(1);
                            apellidoAltas.Text = reader.GetString(2);
                            ciudadAltas.Text = reader.GetString(3);
                            emailAltas.Text = reader.GetString(4);
                            comentarioAltas.Text = reader.GetString(5);
                            vipCheckbox.IsChecked=reader.GetBoolean(6);
                        }
                    }
                }
                connection.Close();
            }
        }

        private void OnClienteTapped(object sender, ItemTappedEventArgs e)
        {
            var cliente = e.Item as string;

            if (cliente != null)
            {
                cargarCliente(cliente);
            }
        }
}