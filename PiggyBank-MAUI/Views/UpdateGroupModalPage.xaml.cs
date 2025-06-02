using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Views
{
    public partial class UpdateGroupModalPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly GrupoDTO _grupo;

        public UpdateGroupModalPage(GrupoDTO grupo)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _grupo = grupo ?? throw new ArgumentNullException(nameof(grupo));

            // Pre-populate the Entry fields with current group data
            NameEntry.Text = _grupo.Nombre;
            DescriptionEntry.Text = _grupo.Descripcion;
        }

        private async void UpdateButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameEntry.Text))
                {
                    await DisplayAlert("Error", "El nombre del grupo es obligatorio", "OK");
                    return;
                }

                var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                if (!int.TryParse(usuarioIdString, out int usuarioId))
                {
                    Debug.WriteLine("Error: UsuarioID no válido");
                    await DisplayAlert("Error", "Usuario no autenticado", "OK");
                    return;
                }

                var req = new ReqActualizarGrupo
                {
                    GrupoID = _grupo.GrupoID,
                    Nombre = NameEntry.Text.Trim(),
                    Descripcion = DescriptionEntry.Text?.Trim(),
                    AdminUsuarioID = usuarioId,
                    token = Preferences.Get("AuthToken", string.Empty)
                };

                Debug.WriteLine($"Actualizando grupo: ID={req.GrupoID}, Nombre={req.Nombre}, Descripcion={req.Descripcion}");

                var response = await _apiService.ActualizarGrupo(req);
                if (response.resultado)
                {
                    // Update the grupo object to reflect changes
                    _grupo.Nombre = req.Nombre;
                    _grupo.Descripcion = req.Descripcion;

                    Debug.WriteLine("Grupo actualizado exitosamente");
                    await DisplayAlert("Éxito", "Grupo actualizado correctamente", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    Debug.WriteLine($"Error al actualizar grupo: {response.error?.FirstOrDefault()?.Message}");
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al actualizar el grupo", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en UpdateButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al actualizar el grupo", "OK");
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}