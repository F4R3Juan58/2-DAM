namespace EjercicioPaginas
{
    public partial class InicioPage : ContentPage
    {
        public InicioPage(string nombreUsuario)
        {
            InitializeComponent();
            UserSession.NombreUsuario = nombreUsuario; // Almacena el nombre de usuario
        }

        private async void goDetalleButton(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new DetallesPage());
        }
    }

    public static class UserSession
    {
        public static string NombreUsuario { get; set; }
    }

}
