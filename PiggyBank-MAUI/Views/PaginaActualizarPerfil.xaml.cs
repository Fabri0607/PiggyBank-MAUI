using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class PaginaActualizarPerfil : ContentPage
    {
        private readonly ApiService _apiService;

        public PaginaActualizarPerfil()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, true);
            _apiService = new ApiService();
            LoadUserData();
        }

        private async void LoadUserData()
        {
            var usuarioId = await SecureStorage.GetAsync("UsuarioID");
            if (string.IsNullOrEmpty(usuarioId)) return;

            var req = new ReqObtenerUsuario { UsuarioID = int.Parse(usuarioId) };
            var response = await _apiService.ObtenerUsuario(req);

            if (response.resultado && response.Usuario != null)
            {
                NameEntry.Text = response.Usuario.Nombre;
                NotificacionesEntry.Text = response.Usuario.ConfiguracionNotificaciones;
            }
        }

        private async void BotonActualizar_Clicked(object sender, EventArgs e)
        {
            var usuarioId = await SecureStorage.GetAsync("UsuarioID");
            if (string.IsNullOrEmpty(usuarioId)) return;

            var req = new ReqActualizarPerfil
            {
                UsuarioID = int.Parse(usuarioId),
                Nombre = NameEntry.Text,
                ConfiguracionNotificaciones = NotificacionesEntry.Text,
                token = await SecureStorage.GetAsync("Token")
            };

            var response = await _apiService.ActualizarPerfil(req);

            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Perfil actualizado correctamente", "OK");
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al actualizar perfil", "OK");
            }
        }
    }
}