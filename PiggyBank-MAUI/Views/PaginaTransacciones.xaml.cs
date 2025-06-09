using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PiggyBank_MAUI.Views;

public partial class PaginaTransacciones : ContentPage
{
    private readonly ApiService _apiService;
    private ObservableCollection<TransaccionDTO> _transacciones;

    public ObservableCollection<TransaccionDTO> Transacciones
    {
        get => _transacciones;
        set
        {
            _transacciones = value;
            OnPropertyChanged(nameof(Transacciones));
        }
    }

    public PaginaTransacciones()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Transacciones = new ObservableCollection<TransaccionDTO>();
        _apiService = new ApiService();
        BindingContext = this;
        CargarTransacciones();
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ActualizarTransaccion());
    }

    private async void CargarTransacciones()
    {
        try
        {
            // Obtener UsuarioID
            var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
            if (!int.TryParse(usuarioIdString, out int usuarioId))
            {
                await DisplayAlert("Error", "Usuario no autenticado", "OK");
                return;
            }

            // Crear solicitud para obtener transacciones
            var req = new ReqTransaccionesPorUsuario
            {
                UsuarioID = usuarioId,
                FechaInicio = null, // Puedes ajustar para filtrar por fechas
                FechaFin = null,
                TipoTransaccion = null, // Puedes ajustar para filtrar por tipo
                token = Preferences.Get("AuthToken", string.Empty)
            };

            // Llamar al servicio
            var response = await _apiService.ListarTransaccionesPorUsuario(req);
            if (response.resultado)
            {
                Transacciones.Clear();
                foreach (var transaccion in response.transacciones)
                {
                    // Asegurar que el color sea correcto según el tipo
                    transaccion.ColorHex = transaccion.Tipo == "Ingreso" ? "#5EBF7E" : "#FF0000";
                    Transacciones.Add(transaccion);
                }
                Debug.WriteLine($"Transacciones cargadas: {Transacciones.Count}");
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar transacciones", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción en CargarTransacciones: {ex.Message}");
            await DisplayAlert("Error", "Error inesperado al cargar las transacciones", "OK");
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarTransacciones(); // Recargar transacciones al aparecer la página
    }

    //public async Task ActualizarTransacciones()
    //{
    //    await CargarTransacciones();
    //}
}