using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Views
{
    public partial class FinancialGoalDetailsPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly MetaDTO _meta;
        private ObservableCollection<MetaTransaccionDTO> _transactions;

        public string GoalName => _meta.Nombre;
        public MetaDTO Goal => _meta;
        public ObservableCollection<MetaTransaccionDTO> Transactions
        {
            get => _transactions;
            set
            {
                _transactions = value;
                OnPropertyChanged(nameof(Transactions));
            }
        }

        public FinancialGoalDetailsPage(MetaDTO meta)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
            _meta = meta ?? throw new ArgumentNullException(nameof(meta));
            _transactions = new ObservableCollection<MetaTransaccionDTO>();
            BindingContext = this;

            // Removed: EditGoalButton.Clicked += EditGoalButton_Clicked;
            // Removed: LinkTransactionButton.Clicked += LinkTransactionButton_Clicked;
            DeleteGoalButton.Clicked += DeleteGoalButton_Clicked;

            LoadGoalDetails();
        }

        private async void LoadGoalDetails()
        {
            try
            {
                var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                if (!int.TryParse(usuarioIdString, out int usuarioId))
                {
                    await DisplayAlert("Error", "Usuario no autenticado", "OK");
                    return;
                }

                var req = new ReqObtenerDetallesMeta
                {
                    MetaID = _meta.MetaID,
                    UsuarioID = usuarioId,
                    token = Preferences.Get("AuthToken", string.Empty)
                };

                var response = await _apiService.ObtenerDetallesMeta(req);
                if (response.resultado)
                {
                    if (response.Meta != null)
                    {
                        _meta.Nombre = response.Meta.Nombre;
                        _meta.MontoObjetivo = response.Meta.MontoObjetivo;
                        _meta.MontoActual = response.Meta.MontoActual;
                        _meta.FechaInicio = response.Meta.FechaInicio;
                        _meta.FechaObjetivo = response.Meta.FechaObjetivo;
                        _meta.Completada = response.Meta.Completada;
                        _meta.Progreso = response.Meta.Progreso;
                        _meta.AhorroMensualSugerido = response.Meta.AhorroMensualSugerido;
                        OnPropertyChanged(nameof(GoalName));
                        OnPropertyChanged(nameof(Goal));
                    }

                    Transactions.Clear();
                    foreach (var transaccion in response.Transacciones ?? Enumerable.Empty<MetaTransaccionDTO>())
                    {
                        Transactions.Add(transaccion);
                        Debug.WriteLine($"Transacción: ID={transaccion.TransaccionID}, Descripcion={transaccion.Descripcion}, MontoAsignado={transaccion.MontoAsignado}");
                    }
                }
                else
                {
                    Debug.WriteLine($"Error en LoadGoalDetails: {response.error?.FirstOrDefault()?.Message}");
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar los detalles de la meta", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en LoadGoalDetails: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al cargar los detalles de la meta", "OK");
            }
        }

        private async void EditGoalButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.IsEnabled = false; // Prevent rapid clicks
                    await Navigation.PushModalAsync(new EditFinancialGoalModalPage(_meta));
                    LoadGoalDetails();
                    OnPropertyChanged(nameof(GoalName));
                    OnPropertyChanged(nameof(Goal));
                    button.IsEnabled = true; // Re-enable button
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en EditGoalButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error al abrir la página de edición de meta", "OK");
                if (sender is Button button)
                {
                    button.IsEnabled = true; // Ensure button is re-enabled on error
                }
            }
        }

        private async void LinkTransactionButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.IsEnabled = false; // Prevent rapid clicks
                    await Navigation.PushModalAsync(new LinkTransactionModalPage(_meta));
                    LoadGoalDetails();
                    button.IsEnabled = true; // Re-enable button
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en LinkTransactionButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error al abrir la página de asignación de transacción", "OK");
                if (sender is Button button)
                {
                    button.IsEnabled = true; // Ensure button is re-enabled on error
                }
            }
        }

        private async void DeleteGoalButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                if (!int.TryParse(usuarioIdString, out int usuarioId))
                {
                    await DisplayAlert("Error", "Usuario no autenticado", "OK");
                    return;
                }

                var req = new ReqEliminarMeta
                {
                    MetaID = _meta.MetaID,
                    UsuarioID = usuarioId,
                    token = Preferences.Get("AuthToken", string.Empty)
                };

                var response = await _apiService.EliminarMeta(req);
                if (response.resultado)
                {
                    await DisplayAlert("Éxito", "Meta eliminada correctamente", "OK");
                    await Navigation.PopAsync();
                }
                else
                {
                    Debug.WriteLine($"Error en DeleteGoalButton_Clicked: {response.error?.FirstOrDefault()?.Message}");
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al eliminar la meta", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en DeleteGoalButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al eliminar la meta", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadGoalDetails();
        }
    }
}