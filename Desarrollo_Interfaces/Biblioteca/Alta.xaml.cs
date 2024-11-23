using Microsoft.Data.Sqlite;
using System.IO;

namespace Biblioteca;

public partial class Alta : ContentPage
{
    private string imagenPath;
    string connectionString = "Data Source=dataBase.db";

    public Alta()
    {
        InitializeComponent();
    }

    private async void onClickGuardar(object sender, EventArgs e)
    {
        try
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string insertQuery = @"
        INSERT INTO biblioteca2 (titulo, autor, editorial, imagenpath) 
        VALUES (@titulo, @autor, @editorial, @imagenPath);";

                using (var command = new SqliteCommand(insertQuery, connection))
                {
                    command.Parameters.AddWithValue("@titulo", tituloEntry.Text);
                    command.Parameters.AddWithValue("@autor", autorEntry.Text);
                    command.Parameters.AddWithValue("@editorial", editorialEntry.Text);
                    command.Parameters.AddWithValue("@imagenPath", imagenPath);

                    command.ExecuteNonQuery();
                }
            }
            await DisplayAlert("Éxito", "Los datos se han guardado correctamente.", "OK");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Ocurrió un error: {ex.Message}", "OK");
        }
    }



        private void onClickLimpiar(object sender, EventArgs e)
    {
        tituloEntry.Text = string.Empty;
        autorEntry.Text = string.Empty;
        editorialEntry.Text = string.Empty;
        SelectedImage.Source = null;
        imagenPath = null;
    }

    private async void onClickSeleccionarImagen(object sender, EventArgs e)
    {
        try
        {
            var result = await FilePicker.Default.PickAsync(new PickOptions
            {
                PickerTitle = "Selecciona una imagen",
                FileTypes = FilePickerFileType.Images
            });

            if (result != null)
            {
                    SelectedImage.Source = ImageSource.FromFile(result.FullPath);
                    imagenPath= result.FullPath;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo seleccionar la imagen: {ex.Message}", "OK");
        }
    }

}
