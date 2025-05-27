using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaVerificarUsuario : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly string _email;

        public PaginaVerificarUsuario(string email)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
            _email = email;
            EmailEntry.Text = email;
            EmailEntry.IsEnabled = false; // Deshabilitar edición del correo
        }

        private async void BotonVerificar_Clicked(object sender, EventArgs e)
        {
            var req = new ReqVerificarUsuario
            {
                Email = _email,
                CodigoVerificacion = CodigoEntry.Text
            };

            var response = await _apiService.VerificarUsuario(req);

            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Cuenta verificada correctamente", "OK");
                await Navigation.PushAsync(new PaginaInicioDeSesion());
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al verificar", "OK");
            }
        }

        private async void TapReenviarCodigo_Tapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new PaginaReenviarCodigoVerificacion(_email));
        }
    }
}