using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Diagnostics;

namespace PiggyBank_MAUI.Views;

public partial class NuevaTransaccion : ContentPage
{
    private readonly ApiService _apiService;
    public NuevaTransaccion()
	{
		InitializeComponent();

        _apiService = new ApiService();

        BindingContext = this;

        TipoTransaccionPicker.ItemsSource = new List<string>
        {
            "Ingreso",
            "Gasto"
        };

        CategoriaPicker.ItemsSource = new List<string>
        {
            "Categoria 1",
            "Categoria 2",
            "Categoria 3"
        };
    }

    private void LimpiarCampos()
    {
        monto_entry.Text = string.Empty;
        titulo_entry.Text = string.Empty;
        descripcion_entry.Text = string.Empty;
        TipoTransaccionPicker.SelectedItem = null;
        CategoriaPicker.SelectedItem = null;
        picker_fecha.Date = DateTime.Now;
    }

    private async void Boton_GuardarTransaccion_Clicked(object sender, EventArgs e)
    {
        try
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(TipoTransaccionPicker.SelectedItem?.ToString()))
            {
                await DisplayAlert("Error", "El tipo de transacción es obligatorio", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(CategoriaPicker.SelectedItem?.ToString()))
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

            // Mapear categoría a CategoriaID (esto es un ejemplo, ajustar según tu lógica)
            int categoriaId = 7; // Aquí deberías obtener el ID real de la categoría seleccionada

            // Crear el objeto de solicitud
            var req = new ReqIngresarTransaccion
            {
                Transaccion = new Transaccion
                {
                    TransaccionID = null,
                    UsuarioID = usuarioId,
                    Tipo = TipoTransaccionPicker.SelectedItem.ToString(),
                    Monto = monto,
                    CategoriaID = categoriaId,
                    Fecha = picker_fecha.Date,
                    Titulo = titulo_entry.Text.Trim(),
                    Descripcion = string.IsNullOrWhiteSpace(descripcion_entry.Text) ? null : descripcion_entry.Text.Trim(),
                    EsCompartido = false,
                    GrupoID = null
                },
                token = Preferences.Get("AuthToken", string.Empty)
            };

            Debug.WriteLine($"Creando transacción: Tipo={req.Transaccion.Tipo}, Monto={req.Transaccion.Monto}, CategoriaID={req.Transaccion.CategoriaID}, Titulo={req.Transaccion.Titulo}");

            // Llamar al servicio
            var response = await _apiService.IngresarTransaccion(req);
            if (response.resultado)
            {
                Debug.WriteLine("Transacción creada correctamente");
                await DisplayAlert("Éxito", "Transacción creada correctamente", "OK");
                LimpiarCampos();
            }
            else
            {
                Debug.WriteLine($"Error al crear transacción: {response.error?.FirstOrDefault()?.Message}");
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al crear la transacción", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción en Boton_GuardarTransaccion_Clicked: {ex.Message}");
            await DisplayAlert("Error", "Error inesperado al crear la transacción", "OK");
        }
    }
}