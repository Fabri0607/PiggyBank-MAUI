using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Views
{
    public partial class EditFinancialGoalModalPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly MetaDTO _meta;
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

        public EditFinancialGoalModalPage(MetaDTO meta)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _meta = meta ?? throw new ArgumentNullException(nameof(meta));
            BindingContext = this;

            // Pre-populate fields
            NameEntry.Text = _meta.Nombre;
            TargetAmountEntry.Text = _meta.MontoObjetivo.ToString();
            if (_meta.FechaObjetivo.HasValue)
            {
                EnableTargetDateCheckBox.IsChecked = true;
                IsTargetDateEnabled = true;
                TargetDatePicker.Date = _meta.FechaObjetivo.Value;
            }
            else
            {
                IsTargetDateEnabled = false;
            }
        }

        private async void UpdateButton_Clicked(object sender, EventArgs e)
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

                var req = new ReqActualizarMeta
                {
                    MetaID = _meta.MetaID,
                    UsuarioID = usuarioId,
                    Nombre = NameEntry.Text.Trim(),
                    MontoObjetivo = montoObjetivo,
                    FechaObjetivo = EnableTargetDateCheckBox.IsChecked ? TargetDatePicker.Date : null,
                    token = Preferences.Get("AuthToken", string.Empty)
                };

                Debug.WriteLine($"Actualizando meta: ID={req.MetaID}, Nombre={req.Nombre}, MontoObjetivo={req.MontoObjetivo}");

                var response = await _apiService.ActualizarMeta(req);
                if (response.resultado)
                {
                    _meta.Nombre = req.Nombre;
                    _meta.MontoObjetivo = req.MontoObjetivo;
                    _meta.FechaObjetivo = req.FechaObjetivo;
                    Debug.WriteLine("Meta actualizada exitosamente");
                    await DisplayAlert("Éxito", "Meta actualizada correctamente", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    Debug.WriteLine($"Error al actualizar meta: {response.error?.FirstOrDefault()?.Message}");
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al actualizar la meta", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en UpdateButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al actualizar la meta", "OK");
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