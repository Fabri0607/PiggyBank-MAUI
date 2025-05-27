using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaRegistrarse : ContentPage
    {
        private readonly ApiService _apiService;

        public PaginaRegistrarse()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
        }

        private async void Boton_CrearCuenta_Clicked(object sender, EventArgs e)
        {
            // Usa los nombres directamente
            if (PasswordEntry.Text != ConfirmPasswordEntry.Text)
            {
                await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
                return;
            }

            var req = new ReqRegistrarUsuario
            {
                Nombre = NameEntry.Text,
                Email = EmailEntry.Text,
                Password = PasswordEntry.Text
            };

            var response = await _apiService.RegistrarUsuario(req);

            if (response.resultado)
            {
                await Navigation.PushAsync(new PaginaVerificarUsuario(req.Email));
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al registrar", "OK");
            }
        }

        private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new PaginaInicioDeSesion());
        }
    }
}