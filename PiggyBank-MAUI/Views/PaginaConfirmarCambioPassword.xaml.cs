using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaConfirmarCambioPassword : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly string _email;

        public PaginaConfirmarCambioPassword(string email)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
            _email = email;
            EmailEntry.Text = email;
        }

        private async void BotonConfirmar_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.IsEnabled = false; // Prevent multiple clicks

                    // Validate inputs
                    string codigo = CodigoEntry.Text?.Trim();
                    string password = PasswordEntry.Text?.Trim();
                    string confirmPassword = ConfirmPasswordEntry.Text?.Trim();

                    if (string.IsNullOrWhiteSpace(codigo))
                    {
                        await DisplayAlert("Error", "Por favor, ingresa el código de verificación", "OK");
                        button.IsEnabled = true;
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
                    {
                        await DisplayAlert("Error", "Por favor, ingresa y confirma la nueva contraseña", "OK");
                        button.IsEnabled = true;
                        return;
                    }

                    if (password != confirmPassword)
                    {
                        await DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
                        button.IsEnabled = true;
                        return;
                    }

                    var req = new ReqConfirmarCambioPassword
                    {
                        Email = _email,
                        CodigoVerificacion = codigo,
                        NuevoPassword = password
                    };

                    var response = await _apiService.ConfirmarCambioPassword(req);

                    if (response.resultado)
                    {
                        await DisplayAlert("Éxito", "Contraseña cambiada correctamente", "OK");
                        await Navigation.PushAsync(new PaginaInicioDeSesion());
                    }
                    else
                    {
                        await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cambiar la contraseña", "OK");
                    }

                    button.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Excepción en BotonConfirmar_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al procesar el cambio de contraseña", "OK");
                if (sender is Button button)
                {
                    button.IsEnabled = true;
                }
            }
        }

        private async void TapReenviarCodigo_Tapped(object sender, TappedEventArgs e)
        {
            try
            {
                var req = new ReqSolicitarCambioPassword
                {
                    Email = _email
                };

                var response = await _apiService.SolicitarCambioPassword(req);

                if (response.resultado)
                {
                    await DisplayAlert("Éxito", "Código de verificación reenviado", "OK");
                }
                else
                {
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al reenviar el código", "OK");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Excepción en TapReenviarCodigo_Tapped: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al reenviar el código", "OK");
            }
        }

        private async void TapVolverLogin_Tapped(object sender, TappedEventArgs e)
        {
            await Navigation.PushAsync(new PaginaInicioDeSesion());
        }
    }
}