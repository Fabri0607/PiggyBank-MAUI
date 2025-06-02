using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Views
{
    public partial class LinkTransactionModalPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly MetaDTO _meta;

        public LinkTransactionModalPage(MetaDTO meta)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _meta = meta ?? throw new ArgumentNullException(nameof(meta));
        }

        private async void AssignButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TransactionIdEntry.Text) || string.IsNullOrWhiteSpace(AssignedAmountEntry.Text))
                {
                    await DisplayAlert("Error", "El ID de la transacción y el monto asignado son obligatorios", "OK");
                    return;
                }

                if (!int.TryParse(TransactionIdEntry.Text, out var transaccionId))
                {
                    await DisplayAlert("Error", "El ID de la transacción debe ser un número válido", "OK");
                    return;
                }

                if (!decimal.TryParse(AssignedAmountEntry.Text, out var montoAsignado) || montoAsignado <= 0)
                {
                    await DisplayAlert("Error", "El monto asignado debe ser un valor numérico positivo", "OK");
                    return;
                }

                var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                if (!int.TryParse(usuarioIdString, out int usuarioId))
                {
                    await DisplayAlert("Error", "Usuario no autenticado", "OK");
                    return;
                }

                var req = new ReqAsignarTransaccion
                {
                    MetaID = _meta.MetaID,
                    UsuarioID = usuarioId,
                    TransaccionID = transaccionId,
                    MontoAsignado = montoAsignado,
                    token = Preferences.Get("AuthToken", string.Empty)
                };

                Debug.WriteLine($"Asignando transacción: MetaID={req.MetaID}, TransaccionID={req.TransaccionID}, MontoAsignado={req.MontoAsignado}");

                var response = await _apiService.AsignarTransaccion(req);
                if (response.resultado)
                {
                    Debug.WriteLine("Transacción asignada exitosamente");
                    await DisplayAlert("Éxito", "Transacción asignada correctamente", "OK");
                    await Navigation.PopModalAsync();
                }
                else
                {
                    Debug.WriteLine($"Error al asignar transacción: {response.error?.FirstOrDefault()?.Message}");
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al asignar la transacción", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en AssignButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al asignar la transacción", "OK");
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}