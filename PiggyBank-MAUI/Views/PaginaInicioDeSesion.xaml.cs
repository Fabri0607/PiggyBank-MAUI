using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaInicioDeSesion : ContentPage
    {
        private readonly ApiService _apiService;

        public PaginaInicioDeSesion()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
        }

        private async void Boton_IniciarSesion_Clicked(object sender, EventArgs e)
        {
            try
            {
                var req = new ReqIniciarSesion
                {
                    Email = EmailEntry.Text,
                    Password = PasswordEntry.Text
                };

                var response = await _apiService.IniciarSesion(req);

                if (response.resultado)
                {
                    await SecureStorage.SetAsync("Token", response.Token);
                    await SecureStorage.SetAsync("UsuarioID", response.Usuario.UsuarioID.ToString());

                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        Application.Current.MainPage = new AppShell();
                    });
                }
                else
                {
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al iniciar sesión", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error en login: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al iniciar sesión", "OK");
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new PaginaSolicitarCambioPassword());
        }

        private async void BotonRegistrarse_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new PaginaRegistrarse());
        }
    }
}