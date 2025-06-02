using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Views
{
    public partial class FinancialGoalsPage : ContentPage
    {
        private readonly ApiService _apiService;
        private ObservableCollection<MetaDTO> _goals;

        public ObservableCollection<MetaDTO> Goals
        {
            get => _goals;
            set
            {
                _goals = value;
                OnPropertyChanged(nameof(Goals));
            }
        }

        public ICommand ViewDetailsCommand { get; }

        public FinancialGoalsPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _apiService = new ApiService();
            _goals = new ObservableCollection<MetaDTO>();
            BindingContext = this;

            ViewDetailsCommand = new Command<MetaDTO>(async (meta) => await OnViewDetails(meta));

            // Removed: CreateGoalButton.Clicked += CreateGoalButton_Clicked;

            LoadGoals();
        }

        private async void LoadGoals()
        {
            try
            {
                var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
                if (!int.TryParse(usuarioIdString, out int usuarioId))
                {
                    await DisplayAlert("Error", "Usuario no autenticado", "OK");
                    return;
                }

                var req = new ReqListarMetas
                {
                    UsuarioID = usuarioId,
                    token = Preferences.Get("AuthToken", string.Empty)
                };

                var response = await _apiService.ListarMetas(req);
                if (response.resultado)
                {
                    Goals.Clear();
                    foreach (var meta in response.Metas ?? Enumerable.Empty<MetaDTO>())
                    {
                        Goals.Add(meta);
                        Debug.WriteLine($"Meta: ID={meta.MetaID}, Nombre={meta.Nombre}, Progreso={meta.Progreso:F2}%");
                    }
                }
                else
                {
                    Debug.WriteLine($"Error en LoadGoals: {response.error?.FirstOrDefault()?.Message}");
                    await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar las metas", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en LoadGoals: {ex.Message}");
                await DisplayAlert("Error", "Error inesperado al cargar las metas", "OK");
            }
        }

        private async void CreateGoalButton_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (sender is Button button)
                {
                    button.IsEnabled = false; // Prevent rapid clicks
                    await Navigation.PushModalAsync(new CreateFinancialGoalModalPage());
                    LoadGoals(); // Refresh goals after modal closes
                    button.IsEnabled = true; // Re-enable button
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en CreateGoalButton_Clicked: {ex.Message}");
                await DisplayAlert("Error", "Error al abrir la página de creación de meta", "OK");
                if (sender is Button button)
                {
                    button.IsEnabled = true; // Ensure button is re-enabled on error
                }
            }
        }

        private async Task OnViewDetails(MetaDTO meta)
        {
            try
            {
                if (meta == null)
                {
                    await DisplayAlert("Error", "Meta no válida", "OK");
                    return;
                }
                await Navigation.PushAsync(new FinancialGoalDetailsPage(meta));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en OnViewDetails: {ex.Message}");
                await DisplayAlert("Error", "Error al abrir los detalles de la meta", "OK");
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            LoadGoals();
        }
    }
}