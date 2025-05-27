using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaCambiarPassword : ContentPage
    {
        private readonly ApiService _apiService;

        public PaginaCambiarPassword()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            _apiService = new ApiService();
        }

        private async void BotonCambiarPassword_Clicked(object sender, EventArgs e)
        {
            if (NewPasswordEntry.Text != ConfirmNewPasswordEntry.Text)
            {
                await DisplayAlert("Error", "Las nuevas contraseñas no coinciden", "OK");
                return;
            }

            var usuarioId = await SecureStorage.GetAsync("UsuarioID");
            if (string.IsNullOrEmpty(usuarioId)) return;

            var req = new ReqCambiarPassword
            {
                UsuarioID = int.Parse(usuarioId),
                PasswordActual = CurrentPasswordEntry.Text,
                NuevoPassword = NewPasswordEntry.Text,
                token = await SecureStorage.GetAsync("Token")
            };

            var response = await _apiService.CambiarPassword(req);

            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Contraseña cambiada correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cambiar contraseña", "OK");
            }
        }
    }
}