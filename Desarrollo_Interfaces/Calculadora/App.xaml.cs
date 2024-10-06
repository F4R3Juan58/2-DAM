using Microsoft.UI;
using Microsoft.UI.Windowing;
using Windows.Graphics;

namespace Calculadora
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();
        }
        protected override Window CreateWindow(IActivationState activationState)
        {
            var windows = base.CreateWindow(activationState);

            windows.Height = 660;
            windows.Width = 400;
            windows.MaximumHeight = 660;
            windows.MaximumWidth = 400;
            windows.MinimumHeight = 660;
            windows.MinimumWidth = 400;

            return windows;
        }
    }
}
