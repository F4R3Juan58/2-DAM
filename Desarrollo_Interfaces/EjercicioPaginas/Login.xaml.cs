using Microsoft.Data.Sqlite;

namespace EjercicioPaginas
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void RegisterButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Register());
        }

        private async void LoginButton(object sender, EventArgs e)
        {
            // Obtener los valores del usuario y la contraseña de los campos de entrada
            string usuarioNombre = user.Text;
            string passwordIngresada = password.Text;

            // Verificar las credenciales del usuario
            if (VerificarCredenciales(usuarioNombre, passwordIngresada))
            {
                // Si las credenciales son correctas, navegar a la página de inicio
                await Navigation.PushAsync(new InicioPage(usuarioNombre));
            }
            else
            {
                // Mostrar un mensaje de error si las credenciales son incorrectas
                await DisplayAlert("Error", "Nombre de usuario o contraseña incorrectos", "OK");
            }
        }

        // Función para verificar las credenciales del usuario
        private bool VerificarCredenciales(string usuarioNombre, string password)
        {
            string connectionString = "Data Source=dataBase.db";
            bool credencialesValidas = false;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // Consulta SQL para obtener el hash de la contraseña del usuario
                string query = @"
                    SELECT password FROM usuarios
                    WHERE usuarioNombre = @UsuarioNombre;";

                using (var command = new SqliteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UsuarioNombre", usuarioNombre);

                    // Ejecutar la consulta y obtener el hash de la contraseña
                    var result = command.ExecuteScalar();

                    if (result != null)
                    {
                        string passwordAlmacenada = result.ToString();

                        // Comparar la contraseña ingresada con la almacenada (usando hash)
                        if (passwordAlmacenada == HashPassword(password))
                        {
                            credencialesValidas = true;
                        }
                    }
                }

                connection.Close();
            }

            return credencialesValidas;
        }

        // Función para hashear la contraseña (usando SHA256)
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }
    }
}