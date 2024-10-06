using Microsoft.Data.Sqlite;


namespace EjercicioPaginas;

public partial class Perfil2 : ContentPage
{
    private string nombre;

    public Perfil2()
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
                    SELECT nombre, email, usuarioNombre FROM usuarios
                    WHERE usuarioNombre = @UsuarioNombre;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioNombre", nombre);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            nombrePerfil.Text = reader["nombre"]?.ToString() ?? "N/A"; // Manejo de nulos
                            emailPerfil.Text = reader["email"]?.ToString() ?? "N/A"; // Manejo de nulos
                            usuarioPerfil.Text = reader["usuarioNombre"]?.ToString() ?? "N/A"; // Manejo de nulos
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

    private async void modificarPerfil(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new modificarPerfil());
    }
}