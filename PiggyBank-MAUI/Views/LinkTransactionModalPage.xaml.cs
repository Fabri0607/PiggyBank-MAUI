using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Views
{
    public partial class LinkTransactionModalPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly MetaDTO _meta;
        private ObservableCollection<TransaccionDTO> _transacciones;

        public LinkTransactionModalPage(MetaDTO meta)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _meta = meta ?? throw new ArgumentNullException(nameof(meta));
            _transacciones = new ObservableCollection<TransaccionDTO>();
            TransactionPicker.ItemsSource = _transacciones;
            LoadTransactionsAsync();
        }

        private async void LoadTransactionsAsync()
        {
            try
            {
                var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                if (!int.TryParse(usuarioIdString, out int usuarioId))
                {
                    await DisplayAlert("Error", "Usuario no autenticado. Por favor, inicia sesión nuevamente.", "OK");
                    await Navigation.PushAsync(new PaginaInicioDeSesion());
                    return;
                }

                var token = Preferences.Get("AuthToken", string.Empty);
                if (string.IsNullOrEmpty(token))
                {
                    await DisplayAlert("Error", "Sesión expirada. Por favor, inicia sesión nuevamente.", "OK");
                    await Navigation.PushAsync(new PaginaInicioDeSesion());
                    return;
                }

                var req = new ReqTransaccionesPorUsuario
                {
                    UsuarioID = usuarioId,
                    token = token
                };

                Debug.WriteLine($"Cargando transacciones para UsuarioID={usuarioId}");
                var response = await _apiService.ListarTransaccionesPorUsuario(req);

                if (response.resultado && response.transacciones != null)
                {
                    var orderedTransacciones = response.transacciones
                        .OrderByDescending(t => t.Fecha)
                        .ToList();

                    foreach (var transaccion in orderedTransacciones)
                    {
                        transaccion.DisplayText = $"{transaccion.Titulo} - {transaccion.Fecha:dd/MM/yyyy} - {transaccion.Monto:C}";
                        _transacciones.Add(transaccion);
                    }

                    if (_transacciones.Count == 0)
                    {
                        await DisplayAlert("Información", "No hay transacciones disponibles", "OK");
                    }
                }
                else
                {
                    Debug.WriteLine($"Error al cargar transacciones: {response.error?.FirstOrDefault()?.Message}");
                    if (response.error?.FirstOrDefault()?.Message.Contains("token", StringComparison.OrdinalIgnoreCase) == true)
                    {
                        await DisplayAlert("Error", "Sesión no válida. Por favor, inicia sesión nuevamente.", "OK");
                        await Navigation.PushAsync(new PaginaInicioDeSesion());
                    }
                    else
                    {
                        await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar transacciones", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en LoadTransactionsAsync: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al cargar transacciones", "OK");
            }
        }

        private async void AssignButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.IsEnabled = false;

                    if (TransactionPicker.SelectedItem == null)
                    {
                        await DisplayAlert("Error", "Por favor, selecciona una transacción", "OK");
                        button.IsEnabled = true;
                        return;
                    }

                    var selectedTransaccion = TransactionPicker.SelectedItem as TransaccionDTO;

                    var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                    if (!int.TryParse(usuarioIdString, out int usuarioId))
                    {
                        await DisplayAlert("Error", "Usuario no autenticado. Por favor, inicia sesión nuevamente.", "OK");
                        await Navigation.PushAsync(new PaginaInicioDeSesion());
                        button.IsEnabled = true;
                        return;
                    }

                    var token = Preferences.Get("AuthToken", string.Empty);
                    if (string.IsNullOrEmpty(token))
                    {
                        await DisplayAlert("Error", "Sesión expirada. Por favor, inicia sesión nuevamente.", "OK");
                        await Navigation.PushAsync(new PaginaInicioDeSesion());
                        button.IsEnabled = true;
                        return;
                    }

                    var req = new ReqAsignarTransaccion
                    {
                        MetaID = _meta.MetaID,
                        UsuarioID = usuarioId,
                        TransaccionID = selectedTransaccion.TransaccionID,
                        MontoAsignado = selectedTransaccion.Monto,
                        token = token
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
                        if (response.error?.FirstOrDefault()?.Message.Contains("token", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            await DisplayAlert("Error", "Sesión no válida. Por favor, inicia sesión nuevamente.", "OK");
                            await Navigation.PushAsync(new PaginaInicioDeSesion());
                        }
                        else
                        {
                            await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al asignar la transacción", "OK");
                        }
                    }

                    button.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en AssignButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al asignar la transacción", "OK");
                if (sender is Button button)
                {
                    button.IsEnabled = true;
                }
            }
        }

        private async void CancelButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}