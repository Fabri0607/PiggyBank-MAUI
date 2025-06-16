using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace PiggyBank_MAUI.Views;

public partial class PagosProgramados : ContentPage, INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private ObservableCollection<PagoDTO> _pagos;
    private bool _isLoading;
    private decimal _totalPendiente;
    public ICommand MostrarBottomSheetCommand { get; }

    // Bandera estática para indicar si se agregó un pago
    public static bool PagoAgregado { get; set; }

    public ObservableCollection<PagoDTO> Pagos
    {
        get => _pagos;
        set
        {
            _pagos = value;
            OnPropertyChanged(nameof(Pagos));
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged(nameof(IsLoading));
        }
    }

    public decimal TotalPendiente
    {
        get => _totalPendiente;
        set
        {
            _totalPendiente = value;
            OnPropertyChanged(nameof(TotalPendiente));
        }
    }

    public PagosProgramados()
	{
		InitializeComponent();

        Pagos = new ObservableCollection<PagoDTO>();
        _apiService = new ApiService();
        MostrarBottomSheetCommand = new Command<PagoDTO>(async (pago) => await MostrarBottomSheet(pago));
        BindingContext = this;

        // Suscribirse a los mensajes de BottomSheetPagos
        MessagingCenter.Subscribe<BottomSheetPagos, PagoDTO>(this, "EditarPago", async (sender, pago) =>
        {
            await Navigation.PushAsync(new ActualizarPago(pago));
        });
        MessagingCenter.Subscribe<BottomSheetPagos>(this, "PagoEliminado", (sender) =>
        {
            CargarPagos();
        });

        CargarPagos();
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new NuevoPago());
    }

    private async void CargarPagos()
    {
        try
        {
            IsLoading = true; // Mostrar loader
            // Obtener UsuarioID
            var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
            if (!int.TryParse(usuarioIdString, out int usuarioId))
            {
                await DisplayAlert("Error", "Usuario no autenticado", "OK");
                return;
            }

            // Crear solicitud para obtener pagos
            var req = new ReqPagosPorUsuario
            {
                UsuarioID = usuarioId,
                FechaInicio = null,
                FechaFin = null,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            // Llamar al servicio
            var response = await _apiService.ListarPagosPorUsuario(req);
            if (response.resultado && response.Pagos != null)
            {
                Pagos.Clear();
                foreach (var pago in response.Pagos)
                {
                    Pagos.Add(pago);
                }
                // Calcular la suma de los montos de pagos pendientes
                TotalPendiente = Pagos.Where(p => p.Estado == "Pendiente").Sum(p => p.Monto);
                Debug.WriteLine($"Pagos cargados: {Pagos.Count}, Total Pendiente: {TotalPendiente}");
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar los pagos", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción en CargarPagos: {ex.Message}");
            await DisplayAlert("Error", "Error inesperado al cargar los pagos", "OK");
        }
        finally
        {
            IsLoading = false; // Ocultar loader
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Recargar si no hay pagos o si se agregó un nuevo pago
        if (Pagos == null || Pagos.Count == 0 || PagoAgregado)
        {
            CargarPagos();
            PagoAgregado = false; // Resetear la bandera
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Desuscribirse para evitar memory leaks
        //MessagingCenter.Unsubscribe<BottomSheetPagos, PagoDTO>(this, "EditarPago");
        //MessagingCenter.Unsubscribe<BottomSheetPagos>(this, "PagoEliminado");
    }

    private async Task MostrarBottomSheet(PagoDTO pago)
    {
        var bottomSheet = new BottomSheetPagos(pago)
        {
            HasHandle = true,
            HasBackdrop = true,
            IsCancelable = true,
            HandleColor = Colors.Gray
        };
        await bottomSheet.ShowAsync(Window);
    }

    
}