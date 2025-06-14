using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PiggyBank_MAUI.Views;

public partial class ActualizarTransaccion : ContentPage
{
    private readonly ApiService _apiService;
    private readonly TransaccionDTO _transaccion;
    private ObservableCollection<Categoria> _categorias;

    public ActualizarTransaccion(TransaccionDTO transaccion = null)
    {
        InitializeComponent();
        _apiService = new ApiService();
        _transaccion = transaccion;
        _categorias = new ObservableCollection<Categoria>();
        BindingContext = this;

        // Configurar TipoTransaccionPicker
        TipoTransaccionPickerActualizar.ItemsSource = new List<string>
        {
            "Ingreso",
            "Gasto"
        };

        // Cargar categorías
        CargarCategorias();

        // Llenar campos si hay una transacción para editar
        if (_transaccion != null)
        {
            LlenarCampos();
        }
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

                // Seleccionar la categoría de la transacción si existe
                if (_transaccion != null && string.IsNullOrEmpty(_transaccion.Categoria))
                {
                    CategoriaPicker.SelectedItem = _categorias.FirstOrDefault(c => c.Nombre == _transaccion.Categoria);
                }
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

    private void LlenarCampos()
    {
        if (_transaccion != null)
        {
            picker_fecha.Date = _transaccion.Fecha;
            TipoTransaccionPickerActualizar.SelectedItem = _transaccion.Tipo;
            monto_entry.Text = _transaccion.Monto.ToString("N0");
            titulo_entry.Text = _transaccion.Titulo;
            descripcion_entry.Text = _transaccion.Descripcion;
            // La categoría se selecciona en CargarCategorias
        }
    }

    private void LimpiarCampos()
    {
        monto_entry.Text = string.Empty;
        titulo_entry.Text = string.Empty;
        descripcion_entry.Text = string.Empty;
        TipoTransaccionPickerActualizar.SelectedItem = null;
        CategoriaPicker.SelectedItem = null;
        picker_fecha.Date = DateTime.Now;
    }

    private async void Boton_ActualizarTransaccion_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(TipoTransaccionPickerActualizar.SelectedItem?.ToString()))
            {
                await DisplayAlert("Error", "El tipo de transacción es obligatorio", "OK");
                return;
            }

            if (CategoriaPicker.SelectedItem == null)
            {
                await DisplayAlert("Error", "La categoría es obligatoria", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(monto_entry.Text))
            {
                await DisplayAlert("Error", "El monto es obligatorio", "OK");
                return;
            }

            if (!decimal.TryParse(monto_entry.Text, out var monto) || monto <= 0)
            {
                await DisplayAlert("Error", "El monto debe ser un valor numérico positivo", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(titulo_entry.Text))
            {
                await DisplayAlert("Error", "El título es obligatorio", "OK");
                return;
            }

            // Validar fecha
            if (picker_fecha.Date > DateTime.Now.Date)
            {
                await DisplayAlert("Error", "La fecha no puede ser futura", "OK");
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
            var req = new ReqActualizarTransaccion
            {
                TransaccionID = _transaccion?.TransaccionID ?? 0, // Usar 0 si es una nueva transacción
                UsuarioID = usuarioId,
                Tipo = TipoTransaccionPickerActualizar.SelectedItem.ToString(),
                Monto = monto,
                CategoriaID = categoriaId,
                Fecha = picker_fecha.Date,
                Titulo = titulo_entry.Text.Trim(),
                Descripcion = string.IsNullOrWhiteSpace(descripcion_entry.Text) ? null : descripcion_entry.Text.Trim(),
                EsCompartido = false,
                GrupoID = null,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            Debug.WriteLine($"Actualizando transacción: TransaccionID={req.TransaccionID}, Tipo={req.Tipo}, Monto={req.Monto}, CategoriaID={req.CategoriaID}, Titulo={req.Titulo}");

            // Llamar al servicio
            var response = await _apiService.ActualizarTransaccion(req);
            if (response.resultado)
            {
                PaginaTransacciones.TransaccionAgregada = true; // Indicar que se actualizó una transacción
                Debug.WriteLine("Transacción actualizada correctamente");
                await DisplayAlert("Éxito", _transaccion == null ? "Transacción creada correctamente" : "Transacción actualizada correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                Debug.WriteLine($"Error al actualizar transacción: {response.error?.FirstOrDefault()?.Message}");
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al actualizar la transacción", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción en Boton_ActualizarTransaccion_Clicked: {ex.Message}");
            await DisplayAlert("Error", "Error inesperado al actualizar la transacción", "OK");
        }
    }
}