using Microsoft.Data.Sqlite;

namespace Tienda;

public partial class Bienvenida : ContentPage
{
    string connectionString = "Data Source=tienda.db";

    public Bienvenida()
	{
		InitializeComponent();
        createDataBase();
        Device.StartTimer(TimeSpan.FromSeconds(1), () =>
        {
            fechaHoraLabel.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
            return true;
        });
    }

    private void createDataBase()
    {
        using (var connection = new SqliteConnection(connectionString))
        {
            connection.Open();

            string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS clientes  (
                        id INTEGER PRIMARY KEY,
                        nombre TEXT NOT NULL,
                        apellidos TEXT NOT NULL,
                        ciudad TEXT,
                        correo_electronico TEXT UNIQUE NOT NULL,
                        comentarios TEXT,
                        vip BOOLEAN NOT NULL DEFAULT 0
                    );";

            using (var command = new SqliteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }

            connection.Close();
        }
    }
}