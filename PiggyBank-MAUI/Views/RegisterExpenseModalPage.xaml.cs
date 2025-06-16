using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Views
{
    public partial class RegisterExpenseModalPage : ContentPage
    {
        private readonly ApiService _apiService;
        private readonly GrupoDTO _grupo;
        private ObservableCollection<Categoria> _categorias;

        public RegisterExpenseModalPage(GrupoDTO grupo)
        {
            InitializeComponent();
            _apiService = new ApiService();
            _grupo = grupo;
            _categorias = new ObservableCollection<Categoria>();

            BindingContext = this;

            // Set up StatusPicker
            StatusPicker.ItemsSource = new List<string> { "Pendiente", "Pagado" }; // Adjust as needed
            StatusPicker.SelectedIndex = 0;

            // Load categories
            CargarCategorias();
        }

        private async void CargarCategorias()
        {
            try
            {
                var response = await _apiService.ListarCategorias();
                if (response.resultado && response.categorias != null)
                {
                    _categorias.Clear();
                    foreach (var categoria in response.categorias)
                    {
                        _categorias.Add(categoria);
                    }
                    CategoryPicker.ItemsSource = _categorias;
                    CategoryPicker.ItemDisplayBinding = new Binding("Nombre");
                    CategoryPicker.SelectedIndex = 0;
                }
                else
                {
                    await DisplayAlert("Error", "No se pudieron cargar las categorías", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cargar las categorías: {ex.Message}", "OK");
            }
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

            if (CategoryPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "La categoría es obligatoria", "OK");
                return;
            }

            var usuarioId = int.Parse(await SecureStorage.GetAsync("UsuarioID") ?? "0");
            var categoriaSeleccionada = (Categoria)CategoryPicker.SelectedItem;
            var req = new ReqRegistrarGastoCompartido
            {
                GrupoID = _grupo.GrupoID,
                UsuarioID = usuarioId,
                Monto = monto,
                Estado = StatusPicker.SelectedItem?.ToString() ?? "Pendiente",
                CategoriaID = categoriaSeleccionada.CategoriaID, // Use the selected category's ID
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