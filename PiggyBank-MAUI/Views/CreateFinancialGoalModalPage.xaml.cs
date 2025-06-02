using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Views
{
    public partial class CreateFinancialGoalModalPage : ContentPage
    {
        private readonly ApiService _apiService;
        private bool _isTargetDateEnabled;

        public bool IsTargetDateEnabled
        {
            get => _isTargetDateEnabled;
            set
            {
                _isTargetDateEnabled = value;
                OnPropertyChanged(nameof(IsTargetDateEnabled));
            }
        }

        public CreateFinancialGoalModalPage()
        {
            InitializeComponent();
            _apiService = new ApiService();
            IsTargetDateEnabled = false;
            BindingContext = this;
        }

        private async void CreateButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NameEntry.Text) || string.IsNullOrWhiteSpace(TargetAmountEntry.Text))
                {
                    await DisplayAlert("Error", "El nombre y el monto objetivo son obligatorios", "OK");
                    return;
                }

                if (!decimal.TryParse(TargetAmountEntry.Text, out var montoObjetivo) || montoObjetivo <= 0)
                {
                    await DisplayAlert("Error", "El monto objetivo debe ser un valor numérico positivo", "OK");
                    return;
                }

                var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                if (!int.TryParse(usuarioIdString, out int usuarioId))
                {
                    await DisplayAlert("Error", "Usuario no autenticado", "OK");
                    return;
                }

                var req = new ReqCrearMeta
                {
                    UsuarioID = usuarioId,
                    Nombre = NameEntry.Text.Trim(),
                    MontoObjetivo = montoObjetivo,
                    FechaInicio = StartDatePicker.Date,
                    FechaObjetivo = EnableTargetDateCheckBox.IsChecked ? TargetDatePicker.Date : null,
                    token = Preferences.Get("AuthToken", string.Empty)
                };

                Debug.WriteLine($"Creando meta: Nombre={req.Nombre}, MontoObjetivo={req.MontoObjetivo}");

                var response = await _apiService.CrearMeta(req);
                if (response.resultado)
                {
                    Debug.WriteLine($"Meta creada: MetaID={response.MetaID}");
                    await DisplayAlert("Éxito", "Meta creada correctamente", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    Debug.WriteLine($"Error al crear meta: {response.error?.FirstOrDefault()?.Message}");
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al crear la meta", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en CreateButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al crear la meta", "OK");
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private void EnableTargetDateCheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            IsTargetDateEnabled = e.Value;
            TargetDatePicker.IsEnabled = e.Value;
        }
    }
}