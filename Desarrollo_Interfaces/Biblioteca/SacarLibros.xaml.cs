using Microsoft.Data.Sqlite;
namespace Biblioteca
{
    public partial class SacarLibros : ContentPage
    {
        private string connectionString = "Data Source=dataBase.db";
        private string titulo;
        private string autorEditorSeleccionado;

        public SacarLibros()
        {
            InitializeComponent();
        }

        private void limpiarListViews()
        {
            AutorEditorialListView.ItemsSource = null;
        }

        private void cargarAutores()
        {
            var autores = new HashSet<string>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT autor FROM biblioteca2;";

                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        autores.Add(reader.GetString(0));
                    }
                }
                connection.Close();
            }
            AutorEditorialListView.ItemsSource = autores;
        }

        private void cargarEditoriales()
        {
            var editoriales = new HashSet<string>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT editorial FROM biblioteca2;";
                using (var command = new SqliteCommand(selectQuery, connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        editoriales.Add(reader.GetString(0));
                    }
                }

                connection.Close();
            }
            AutorEditorialListView.ItemsSource = editoriales;
        }

        private void cargarTitulosAutor(string autor)
        {
            LibrosListView.ItemsSource = null;
            var titulos = new HashSet<string>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT titulo FROM biblioteca2 WHERE autor=@autor;";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@autor", autor);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            titulos.Add(reader.GetString(0));
                        }
                    }
                }

                connection.Close();
            }

            LibrosListView.ItemsSource = titulos;
        }

        private void cargarTituloEditorial(string editorial)
        {
            LibrosListView.ItemsSource = null;
            var titulos = new HashSet<string>();

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT titulo FROM biblioteca2 WHERE editorial=@editorial;";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@editorial", editorial);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            titulos.Add(reader.GetString(0));
                        }
                    }
                }

                connection.Close();
            }

            LibrosListView.ItemsSource = titulos;
        }

        private void OnConsultaChanged(object sender, CheckedChangedEventArgs e)
        {
            if (consultaRadioAutor.IsChecked == true)
            {
                limpiarListViews();
                cargarAutores();
            }
            else if (consultaRadioEditorial.IsChecked == true)
            {
                limpiarListViews();
                cargarEditoriales();
            }
        }

        private void cargarLibro(string titulo)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT imagenPath FROM biblioteca2 WHERE titulo=@titulo COLLATE NOCASE;";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@titulo", titulo.Trim());

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            PortadaImage.Source = reader["imagenPath"].ToString();
                        }
                        else
                        {
                            DisplayAlert("Error", "No se encontró el libro especificado.", "OK");
                        }
                    }
                }

                connection.Close();
            }
        }

        private void OnAutorEditorialSelected(object sender, SelectedItemChangedEventArgs e)
        {
            autorEditorSeleccionado = e.SelectedItem as string;

            if (consultaRadioAutor.IsChecked == true)
            {
                cargarTitulosAutor(autorEditorSeleccionado);
            }
            else if (consultaRadioEditorial.IsChecked == true)
            {
                cargarTituloEditorial(autorEditorSeleccionado);
            }
        }

        private void OnLibroTapped(object sender, ItemTappedEventArgs e)
        {
            titulo = e.Item as string;

            if (titulo != null)
            {
                cargarLibro(titulo);
            }
        }

        private void SacarLibro(object sender, EventArgs e)
        {
            if (titulo == null)
            {
                DisplayAlert("Error", "Debe seleccionar un libro antes de proceder.", "OK");
                return;
            }

            string autor = string.Empty;
            string editorial = string.Empty;
            string img = string.Empty;

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string selectQuery = "SELECT autor, editorial, imagenpath FROM biblioteca2 WHERE titulo = @titulo COLLATE NOCASE;";
                using (var command = new SqliteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@titulo", titulo);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            autor = reader["autor"].ToString();
                            editorial = reader["editorial"].ToString();
                            img = reader["imagenpath"].ToString();
                        }
                        else
                        {
                            DisplayAlert("Error", "No se encontraron detalles del libro seleccionado.", "OK");
                            return;
                        }
                    }
                }

                connection.Close();
            }
            sacarLibroInfo.IsVisible = true;
            textoSacarLibro.Text = $" está sacando el libro con título '{titulo}' en el que el autor es '{autor}' y es de la editorial '{editorial}' con la portada: ";
            sacarLibroImg.Source =img;
        }



        private void confirmarSalida(object sender, EventArgs e)
        {
            string usuario = entryUsuarioSalida.Text;

            if (string.IsNullOrEmpty(usuario))
            {
                DisplayAlert("Error", "El nombre del usuario es obligatorio.", "OK");
                return;
            }

            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string checkQuery = @"
            SELECT COUNT(*) FROM registroPrestamos
            WHERE id_libro = (SELECT id_libro FROM biblioteca2 WHERE titulo = @titulo)
            AND devuelto = 0"; 

                using (var command = new SqliteCommand(checkQuery, connection))
                {
                    command.Parameters.AddWithValue("@titulo", titulo);

                    var result = command.ExecuteScalar();
                    if (Convert.ToInt32(result) > 0)
                    {
                        DisplayAlert("Error", "Este libro ya ha sido sacado y no ha sido devuelto.", "OK");
                        return; 
                    }
                }

                string insertQuery = @"
            INSERT INTO registroPrestamos (id_libro, nombre_usuario, devuelto)
            VALUES ((SELECT id_libro FROM biblioteca2 WHERE titulo = @titulo), @usuario, 0)";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@titulo", titulo);
                    command.Parameters.AddWithValue("@usuario", usuario);

                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            DisplayAlert("Confirmación", "La salida del libro ha sido registrada.", "Aceptar");
        }

    }
}
