using Microsoft.Data.Sqlite;

namespace Biblioteca
{
    public partial class Principal : ContentPage
    {

        string connectionString = "Data Source=dataBase.db";

        public Principal()        {
            InitializeComponent();
            createDataBase();
        }

        private void createDataBase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS biblioteca2 (
                        id_libro INTEGER PRIMARY KEY AUTOINCREMENT,
                        titulo TEXT NOT NULL,
                        autor TEXT NOT NULL,
                        editorial TEXT NOT NULL,
                        imagenpath TEXT
                    );";

                string createRegistroPrestamosQuery = @"
                    CREATE TABLE IF NOT EXISTS registroPrestamos (
                        id_prestamo INTEGER PRIMARY KEY AUTOINCREMENT,
                        id_libro INTEGER NOT NULL,
                        nombre_usuario TEXT NOT NULL,
                        devuelto INTEGER NOT NULL DEFAULT 0,
                        FOREIGN KEY (id_libro) REFERENCES biblioteca2(id_libro)
                    );";


                using (var command = new SqliteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                using (var command = new SqliteCommand(createRegistroPrestamosQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
