using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class RegisterExpenseModalPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly GrupoDTO _grupo;

        public RegisterExpenseModalPage(GrupoDTO grupo)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _grupo = grupo;

            // Aquí puedes cargar las categorías desde una API o una lista estática
            CategoryPicker.ItemsSource = new List<string> { "General", "Comida", "Transporte", "Entretenimiento" }; // Ejemplo estático
            CategoryPicker.SelectedIndex = 0;
            StatusPicker.SelectedIndex = 0;
        }

        private async void RegisterButton_Clicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(DescriptionEntry.Text) || string.IsNullOrWhiteSpace(AmountEntry.Text))
            {
                await DisplayAlert("Error", "La descripción y el monto son obligatorios", "OK");
                return;
            }

            if (!decimal.TryParse(AmountEntry.Text, out var monto))
            {
                await DisplayAlert("Error", "El monto debe ser un valor numérico válido", "OK");
                return;
            }

            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var req = new ReqRegistrarGastoCompartido
            {
                GrupoID = _grupo.GrupoID,
                UsuarioID = usuarioId,
                Monto = monto,
                Estado = StatusPicker.SelectedItem?.ToString() ?? "Pendiente",
                CategoriaID = CategoryPicker.SelectedIndex + 1, // Ajusta según tu lógica de categorías
                Descripcion = DescriptionEntry.Text,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.RegistrarGastoCompartido(req);
            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Gasto registrado correctamente", "OK");
                await Navigation.PopModalAsync();
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al registrar el gasto", "OK");
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}