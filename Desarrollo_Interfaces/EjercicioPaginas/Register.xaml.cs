using Microsoft.Data.Sqlite;

namespace EjercicioPaginas
{
    public partial class Register : ContentPage
    {
        public Register()
        {
            InitializeComponent();
            // Crea la tabla si no existe cuando se inicializa la página de registro
            CrearTablaUsuarios();
        }

        private async void LoginButton(object sender, EventArgs e)
        {
            // Verifica si las contraseñas coinciden
            if (password.Text == confirmpassword.Text)
            {
                // Crea un nuevo objeto Usuario
                Usuario nuevoUsuario = new Usuario
                {
                    Nombre = name.Text,
                    Email = email.Text,
                    UsuarioNombre = user.Text,
                    Password = HashPassword(password.Text) // Asegúrate de hashear la contraseña
                };

                // Inserta el usuario en la base de datos
                InsertarUsuario(nuevoUsuario);

                // Limpia los campos de entrada
                name.Text = "";
                email.Text = "";
                user.Text = "";
                password.Text = "";
                confirmpassword.Text = "";

                // Navega a la página de login
                await Navigation.PushAsync(new Login());
            }
            else
            {
                // Muestra un mensaje de error si las contraseñas no coinciden
                await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            }
        }

        // Función para crear la tabla de usuarios si no existe
        private void CrearTablaUsuarios()
        {
            string connectionString = "Data Source=dataBase.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS usuarios (
                        id_usuario INTEGER PRIMARY KEY AUTOINCREMENT,
                        nombre TEXT NOT NULL,
                        email TEXT NOT NULL UNIQUE,
                        usuarioNombre TEXT NOT NULL,
                        password TEXT NOT NULL
                    );";

                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        // Función para insertar un usuario en la base de datos
        private void InsertarUsuario(Usuario usuario)
        {
            string connectionString = "Data Source=dataBase.db";

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = @"
                    INSERT INTO usuarios (nombre, email, usuarioNombre, password)
                    VALUES (@Nombre, @Email, @UsuarioNombre, @Password);";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@Email", usuario.Email);
                    command.Parameters.AddWithValue("@UsuarioNombre", usuario.UsuarioNombre);
                    command.Parameters.AddWithValue("@Password", usuario.Password);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }

        // Función para hashear la contraseña (simple SHA256 en este ejemplo)
        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        // Clase Usuario
        public class Usuario
        {
            public string Nombre { get; set; }
            public string Email { get; set; }
            public string UsuarioNombre { get; set; }
            public string Password { get; set; }
        }
    }
}