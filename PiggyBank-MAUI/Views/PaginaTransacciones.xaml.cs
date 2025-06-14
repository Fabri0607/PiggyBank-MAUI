using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace PiggyBank_MAUI.Views;

public partial class PaginaTransacciones : ContentPage, INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private ObservableCollection<TransaccionDTO> _transacciones;
    private bool _isLoading;
    public ICommand MostrarBottomSheetCommand { get; }

    // Bandera estática para indicar si se agregó una transacción
    public static bool TransaccionAgregada { get; set; }

    public ObservableCollection<TransaccionDTO> Transacciones
    {
        get => _transacciones;
        set
        {
            _transacciones = value;
            OnPropertyChanged(nameof(Transacciones));
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

    public PaginaTransacciones()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        Transacciones = new ObservableCollection<TransaccionDTO>();
        _apiService = new ApiService();
        MostrarBottomSheetCommand = new Command<TransaccionDTO>(async (transaccion) => await MostrarBottomSheet(transaccion));
        BindingContext = this;

        // Suscribirse a los mensajes de BottomSheetTransacciones
        MessagingCenter.Subscribe<BottomSheetTransacciones, TransaccionDTO>(this, "EditarTransaccion", async (sender, transaccion) =>
        {
            await Navigation.PushAsync(new ActualizarTransaccion(transaccion));
        });
        MessagingCenter.Subscribe<BottomSheetTransacciones>(this, "TransaccionEliminada", (sender) =>
        {
            CargarTransacciones();
        });

        CargarTransacciones();
    }

    private async void CargarTransacciones()
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

            // Crear solicitud para obtener transacciones
            var req = new ReqTransaccionesPorUsuario
            {
                UsuarioID = usuarioId,
                FechaInicio = null,
                FechaFin = null,
                TipoTransaccion = null,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            // Llamar al servicio
            var response = await _apiService.ListarTransaccionesPorUsuario(req);
            if (response.resultado)
            {
                Transacciones.Clear();
                foreach (var transaccion in response.transacciones)
                {
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
        finally
        {
            IsLoading = false; // Ocultar loader
        }
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Recargar si no hay transacciones o si se agregó una nueva transacción
        if (Transacciones == null || Transacciones.Count == 0 || TransaccionAgregada)
        {
            CargarTransacciones();
            TransaccionAgregada = false; // Resetear la bandera
        }
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        // Desuscribirse para evitar memory leaks
        //MessagingCenter.Unsubscribe<BottomSheetTransacciones, TransaccionDTO>(this, "EditarTransaccion");
        //MessagingCenter.Unsubscribe<BottomSheetTransacciones>(this, "TransaccionEliminada");
    }

    private async Task MostrarBottomSheet(TransaccionDTO transaccion)
    {
        var bottomSheet = new BottomSheetTransacciones(transaccion)
        {
            HasHandle = true,
            HasBackdrop = true,
            IsCancelable = true,
            HandleColor = Colors.Gray
        };
        await bottomSheet.ShowAsync(Window);
    }

}