using PiggyBank_MAUI.Services;
using Microsoft.Maui.Storage; // Asegúrate de incluir este namespace
using System;
using System.Threading.Tasks;
using PiggyBank_MAUI.Models;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaCerrarSesion : ContentPage
    {
        private readonly ApiService _apiService;

        public PaginaCerrarSesion()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            _apiService = new ApiService();
        }

        private async void BotonCerrarSesion_Clicked(object sender, EventArgs e)
        {
            var req = new ReqCerrarSesion
            {
                MotivoRevocacion = "Cierre de sesión por usuario",
                token = await SecureStorage.GetAsync("Token")
            };

            var response = await _apiService.CerrarSesion(req);

            if (response.resultado)
            {
                SecureStorage.Remove("Token"); // Cambiado de RemoveAsync a Remove
                SecureStorage.Remove("UsuarioID"); // Cambiado de RemoveAsync a Remove
                Application.Current.MainPage = new NavigationPage(new PaginaInicioDeSesion());
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cerrar sesión", "OK");
            }
        }

        private async void BotonCancelar_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }
}