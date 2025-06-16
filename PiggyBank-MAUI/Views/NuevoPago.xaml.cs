using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PiggyBank_MAUI.Views;

public partial class NuevoPago : ContentPage
{
    private readonly ApiService _apiService;
    private ObservableCollection<Categoria> _categorias;

    public NuevoPago()
	{
		InitializeComponent();
        _apiService = new ApiService();
        _categorias = new ObservableCollection<Categoria>();
        BindingContext = this;
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
                CategoriaPicker.ItemsSource = _categorias;
                CategoriaPicker.ItemDisplayBinding = new Binding("Nombre");
            }
            else
            {
                await DisplayAlert("Error", "No se pudieron cargar las categorías", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción al cargar categorías: {ex.Message}");
            await DisplayAlert("Error", "Error al cargar las categorías", "OK");
        }
    }

    private void LimpiarCampos()
    {
        TituloEntry.Text = string.Empty;
        MontoEntry.Text = string.Empty;
        CategoriaPicker.SelectedItem = null;
        FechaVencimientoPicker.Date = DateTime.Now;
    }

    private async void Boton_GuardarPago_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(TituloEntry.Text))
            {
                await DisplayAlert("Error", "El título es obligatorio", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(MontoEntry.Text))
            {
                await DisplayAlert("Error", "El monto es obligatorio", "OK");
                return;
            }

            if (!decimal.TryParse(MontoEntry.Text, out var monto) || monto <= 0)
            {
                await DisplayAlert("Error", "El monto debe ser un valor numérico positivo", "OK");
                return;
            }

            if (CategoriaPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "La categoría es obligatoria", "OK");
                return;
            }

            // Obtener UsuarioID
            var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
            if (!int.TryParse(usuarioIdString, out int usuarioId))
            {
                await DisplayAlert("Error", "Usuario no autenticado", "OK");
                return;
            }

            // Obtener el CategoriaID de la categoría seleccionada
            var categoriaSeleccionada = (Categoria)CategoriaPicker.SelectedItem;
            int categoriaId = categoriaSeleccionada.CategoriaID;

            // Crear el objeto de solicitud
            var req = new ReqIngresarPagoProgramado
            {
                UsuarioID = usuarioId,
                Titulo = TituloEntry.Text.Trim(),
                Monto = monto,
                Fecha_Vencimiento = FechaVencimientoPicker.Date,
                CategoriaID = categoriaId,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            Debug.WriteLine($"Creando pago programado: Titulo={req.Titulo}, Monto={req.Monto}, CategoriaID={req.CategoriaID}, Fecha_Vencimiento={req.Fecha_Vencimiento}");

            // Llamar al servicio
            var response = await _apiService.IngresarPagoProgramado(req);
            if (response.resultado)
            {
                PagosProgramados.PagoAgregado = true;
                Debug.WriteLine("Pago programado creado correctamente");
                await DisplayAlert("Éxito", "Pago programado creado correctamente", "OK");
                LimpiarCampos();
            }
            else
            {
                Debug.WriteLine($"Error al crear pago programado: {response.error?.FirstOrDefault()?.Message}");
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al crear el pago programado", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción en Boton_GuardarPago_Clicked: {ex.Message}");
            await DisplayAlert("Error", "Error inesperado al crear el pago programado", "OK");
        }
    }
}