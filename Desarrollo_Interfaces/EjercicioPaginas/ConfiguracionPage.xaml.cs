namespace EjercicioPaginas;

public partial class ConfiguracionPage : ContentPage
{
	public ConfiguracionPage()
	{
		InitializeComponent();
	}

    private void OnSwitchToggled(object sender, ToggledEventArgs e)
    {
        if (e.Value) // Si el switch está encendido, activar modo noche
        {
            Application.Current.Resources["BackgroundColor"] = Application.Current.Resources["BackgroundColorNight"];
            Application.Current.Resources["TextColor"] = Application.Current.Resources["TextColorNight"];
        }
        else // Si está apagado, activar modo día
        {
            Application.Current.Resources["BackgroundColor"] = Application.Current.Resources["BackgroundColorDay"];
            Application.Current.Resources["TextColor"] = Application.Current.Resources["TextColorDay"];
        }
    }
}