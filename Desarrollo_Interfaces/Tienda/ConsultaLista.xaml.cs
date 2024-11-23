using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace Tienda
{
    public class Clientes
    {
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string CorreoElectronico { get; set; }
        public string Comentarios { get; set; }
        public bool EsVIP { get; set; }
    }

    public partial class ConsultaLista : ContentPage
    {
        string connectionString = "Data Source=tienda.db";

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CargarDatos();
            CargarCiudades();
        }

        public ConsultaLista()
        {
            InitializeComponent();
            CargarCiudades();
            CargarDatos();
        }

        private void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            CargarDatos();
        }

        private void CargarCiudades()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var ciudades = new List<string>();

                using (var command = connection.CreateCommand())
                {
                    // Consulta para obtener ciudades únicas
                    command.CommandText = "SELECT DISTINCT ciudad FROM clientes";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ciudades.Add(reader.GetString(0));
                        }
                    }
                }

                ciudades.Insert(0, "Todas");
                miPicker.ItemsSource = ciudades;
            }
        }

        private void OnVipCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            CargarDatos();
        }

        public void CargarDatos()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                var clientes = new List<Clientes>();

                using (var command = connection.CreateCommand())
                {
                    bool filtrarPorVIP = vipCheckBox.IsChecked;
                    string ciudadSeleccionada = miPicker.SelectedItem?.ToString();

                    
                    command.CommandText = "SELECT apellidos, nombre, ciudad, correo_electronico, comentarios, vip FROM Clientes WHERE 1=1";

                    if (filtrarPorVIP)
                    {
                        command.CommandText += " AND vip = 1";
                    }

                    if (!string.IsNullOrEmpty(ciudadSeleccionada) && ciudadSeleccionada != "Todas")
                    {
                        command.CommandText += " AND ciudad = @ciudad";
                        command.Parameters.AddWithValue("@ciudad", ciudadSeleccionada);
                    }

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var cliente = new Clientes
                            {
                                Apellido = reader.GetString(0),
                                Nombre = reader.GetString(1),
                                Ciudad = reader.GetString(2),
                                CorreoElectronico = reader.GetString(3),
                                Comentarios = reader.GetString(4),
                                EsVIP = reader.GetBoolean(5)
                            };
                            clientes.Add(cliente);
                        }
                    }
                }
                AgregarFilasAlGrid(clientes);
            }
        }



            private void AgregarFilasAlGrid(List<Clientes> clientes)
        {
            clientesGrid.Children.Clear();
            clientesGrid.RowDefinitions.Clear();

            clientesGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

            var encabezadoApellido = new Label { Text = "Apellido", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, Margin = 5 };
            var encabezadoNombre = new Label { Text = "Nombre", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, Margin = 5 };
            var encabezadoCiudad = new Label { Text = "Ciudad", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, Margin = 5 };
            var encabezadoCorreo = new Label { Text = "Correo Electrónico", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, Margin = 5 };
            var encabezadoComentario = new Label { Text = "Comentarios", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, Margin = 5 };
            var encabezadoVIP = new Label { Text = "VIP", HorizontalTextAlignment = TextAlignment.Center, FontAttributes = FontAttributes.Bold, Margin = 5 };

            Grid.SetRow(encabezadoApellido, 0);
            Grid.SetColumn(encabezadoApellido, 0);
            clientesGrid.Children.Add(encabezadoApellido);

            Grid.SetRow(encabezadoNombre, 0);
            Grid.SetColumn(encabezadoNombre, 1);
            clientesGrid.Children.Add(encabezadoNombre);

            Grid.SetRow(encabezadoCiudad, 0);
            Grid.SetColumn(encabezadoCiudad, 2);
            clientesGrid.Children.Add(encabezadoCiudad);

            Grid.SetRow(encabezadoCorreo, 0);
            Grid.SetColumn(encabezadoCorreo, 3);
            clientesGrid.Children.Add(encabezadoCorreo);

            Grid.SetRow(encabezadoComentario, 0);
            Grid.SetColumn(encabezadoComentario, 4);
            clientesGrid.Children.Add(encabezadoComentario);

            Grid.SetRow(encabezadoVIP, 0);
            Grid.SetColumn(encabezadoVIP, 5);
            clientesGrid.Children.Add(encabezadoVIP);

            int row = 1;

            foreach (var cliente in clientes)
            {
                clientesGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });

                var apellidoLabel = new Label { Text = cliente.Apellido, HorizontalTextAlignment = TextAlignment.Center, Margin = 5 };
                var nombreLabel = new Label { Text = cliente.Nombre, HorizontalTextAlignment = TextAlignment.Center, Margin = 5 };
                var ciudadLabel = new Label { Text = cliente.Ciudad, HorizontalTextAlignment = TextAlignment.Center, Margin = 5 };
                var correoLabel = new Label { Text = cliente.CorreoElectronico, HorizontalTextAlignment = TextAlignment.Center, Margin = 5 };
                var comentarioLabel = new Label { Text = cliente.Comentarios, HorizontalTextAlignment = TextAlignment.Center, Margin = 5 };
                var vipLabel = new Label { Text = cliente.EsVIP ? "True" : "False", HorizontalTextAlignment = TextAlignment.Center, Margin = 5 };

                Grid.SetRow(apellidoLabel, row);
                Grid.SetColumn(apellidoLabel, 0);
                clientesGrid.Children.Add(apellidoLabel);

                Grid.SetRow(nombreLabel, row);
                Grid.SetColumn(nombreLabel, 1);
                clientesGrid.Children.Add(nombreLabel);

                Grid.SetRow(ciudadLabel, row);
                Grid.SetColumn(ciudadLabel, 2);
                clientesGrid.Children.Add(ciudadLabel);

                Grid.SetRow(correoLabel, row);
                Grid.SetColumn(correoLabel, 3);
                clientesGrid.Children.Add(correoLabel);

                Grid.SetRow(comentarioLabel, row);
                Grid.SetColumn(comentarioLabel, 4);
                clientesGrid.Children.Add(comentarioLabel);

                Grid.SetRow(vipLabel, row);
                Grid.SetColumn(vipLabel, 5);
                clientesGrid.Children.Add(vipLabel);

                row++;
            }
        }
    }
}