using Microsoft.Data.Sqlite;
using System.Collections.ObjectModel;

namespace Biblioteca;

public partial class RegistroSalidas : ContentPage
{
    private readonly string connectionString = "Data Source=dataBase.db";

    public RegistroSalidas()
    {
        InitializeComponent();
        CargarLibrosPrestados();
    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarLibrosPrestados();
    }
    private void CargarLibrosPrestados()
    {
        var librosPrestados = new List<LibroPrestado>();

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string query = @"
                    SELECT biblioteca2.titulo, registroPrestamos.nombre_usuario
                    FROM registroPrestamos
                    JOIN biblioteca2 ON registroPrestamos.id_libro = biblioteca2.id_libro
                    WHERE registroPrestamos.devuelto = 0;";

            using (var command = new SqliteCommand(query, connection))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        librosPrestados.Add(new LibroPrestado
                        {
                            Titulo = reader["titulo"].ToString(),
                            NombreUsuario = reader["nombre_usuario"].ToString()
                        });
                    }
                }
            }

            connection.Close();
        }

        LibrosPrestadosCollectionView.ItemsSource = librosPrestados;
    }

    public class LibroPrestado
    {
        public string Titulo { get; set; }
        public string NombreUsuario { get; set; }
    }

    private void devolver(object sender, EventArgs e)
    {
        var libroSeleccionado = (LibroPrestado)((Button)sender).BindingContext;

        if (libroSeleccionado == null)
        {
            DisplayAlert("Error", "No se ha seleccionado un libro para devolver.", "OK");
            return;
        }

        string titulo = libroSeleccionado.Titulo;
        string usuario = libroSeleccionado.NombreUsuario;

        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string deleteQuery = @"
                DELETE FROM registroPrestamos
                WHERE id_libro = (SELECT id_libro FROM biblioteca2 WHERE titulo = @titulo)
                AND nombre_usuario = @usuario
                AND devuelto = 0;";

            using (var command = new SqliteCommand(deleteQuery, connection))
            {
                command.Parameters.AddWithValue("@titulo", titulo);
                command.Parameters.AddWithValue("@usuario", usuario);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    DisplayAlert("Éxito", "El libro ha sido devuelto correctamente.", "OK");
                }
                else
                {
                    DisplayAlert("Error", "No se pudo devolver el libro. Verifique si ya ha sido devuelto.", "OK");
                }
            }

            connection.Close();
        }

        CargarLibrosPrestados();
    }
}


