using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaSolicitarCambioPassword : ContentPage
    {
        private readonly ApiService _apiService;

        public PaginaSolicitarCambioPassword()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
        }

        private async void BotonSiguiente_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.IsEnabled = false; // Prevent multiple clicks

                    // Validate email
                    string email = EmailEntry.Text?.Trim();
                    if (string.IsNullOrWhiteSpace(email))
                    {
                        await DisplayAlert("Error", "Por favor, ingresa un correo electrónico", "OK");
                        button.IsEnabled = true;
                        return;
                    }

                    if (!IsValidEmail(email))
                    {
                        await DisplayAlert("Error", "Por favor, ingresa un correo electrónico válido", "OK");
                        button.IsEnabled = true;
                        return;
                    }

                    var req = new ReqSolicitarCambioPassword
                    {
                        Email = email
                    };

                    var response = await _apiService.SolicitarCambioPassword(req);

                    if (response.resultado)
                    {
                        await Navigation.PushAsync(new PaginaConfirmarCambioPassword(email));
                    }
                    else
                    {
                        await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al solicitar el cambio de contraseña", "OK");
                    }

                    button.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Excepción en BotonSiguiente_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al procesar la solicitud", "OK");
                if (sender is Button button)
                {
                    button.IsEnabled = true;
                }
            }
        }

        private async void TapVolverLogin_Tapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new PaginaInicioDeSesion());
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Simple regex for email validation
                return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
            }
            catch
            {
                return false;
            }
        }
    }
}