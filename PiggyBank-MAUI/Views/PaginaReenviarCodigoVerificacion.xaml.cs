using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaReenviarCodigoVerificacion : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly string _email;

        public PaginaReenviarCodigoVerificacion(string email)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
            _email = email;
            EmailEntry.Text = email;
            EmailEntry.IsEnabled = false;
        }

        private async void BotonReenviar_Clicked(object sender, EventArgs e)
        {
            var req = new ReqReenviarCodigoVerificacion { Email = _email };
            var response = await _apiService.ReenviarCodigoVerificacion(req);

            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Código reenviado correctamente", "OK");
                await Navigation.PushAsync(new PaginaVerificarUsuario(_email));
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al reenviar código", "OK");
            }
        }

        private async void TapVolverVerificar_Tapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new PaginaVerificarUsuario(_email));
        }
    }
}