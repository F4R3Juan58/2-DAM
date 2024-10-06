using Microsoft.Data.Sqlite;

namespace EjercicioPaginas;

public partial class modificarPerfil : ContentPage
{
    private string nombre;
    public modificarPerfil()
	{

		InitializeComponent();
        nombre = UserSession.NombreUsuario;
        loadData();
    }

    public void loadData()
    {
        string connectionString = "Data Source=dataBase.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            try
            {
                connection.Open();

                string query = @"
                    SELECT nombre, email, password FROM usuarios
                    WHERE usuarioNombre = @UsuarioNombre;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioNombre", nombre);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nombrePerfil.Text = reader["nombre"]?.ToString() ?? "N/A"; // Manejo de nulos
                            emailPerfil.Text = reader["email"]?.ToString() ?? "N/A"; // Manejo de 
                        }
                        else
                        {
                            DisplayAlert("Error", "No se encontró el perfil del usuario.", "OK");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayAlert("Error", $"Ocurrió un error al cargar los datos: {ex.Message}", "OK");
            }
        }
    }

    private async void guardarPerfil(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Perfil2());
    }

    private void modificarUsuario(string usuario)
    {
        string connectionString = "Data Source=dataBase.db";

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string updateQuery = @"
                UPDATE usuarios
                SET nombre = @Nombre, 
                    email = @Email, 
                    password = @Password
                WHERE usuarioNombre = @UsuarioNombre;"; // Asegúrate de tener un identificador único para el usuario

            using (var command = new SqliteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@Nombre", nombrePerfil.Text);
                command.Parameters.AddWithValue("@Email", emailPerfil.Text);
                command.Parameters.AddWithValue("@Password", passwordPerfil.Text);
                command.Parameters.AddWithValue("@UsuarioNombre", usuarioPerfil.Text);
                
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}