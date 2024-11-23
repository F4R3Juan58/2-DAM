using System.Data;
using Microsoft.Data.Sqlite;

namespace Biblioteca
{
    public partial class Consulta : ContentPage
    {
        string connectionString = "Data Source=dataBase.db";


        public Consulta()
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
                    ;

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
            var autorEditorSeleccionado = e.SelectedItem as string;

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
            var titulo = e.Item as string;

            if (titulo != null)
            {
                cargarLibro(titulo);
            }
        }
    }
}