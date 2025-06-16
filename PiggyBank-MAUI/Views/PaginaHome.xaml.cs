using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace PiggyBank_MAUI.Views;

public partial class PaginaHome : ContentPage, INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private ObservableCollection<TransaccionDTO> _transacciones;
    private bool _isLoading;
    private decimal _ingresosTotales;
    private decimal _gastosTotales;
    private decimal _balanceTotal;
    private ObservableCollection<TransaccionDTO> _ultimasTransacciones;

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

    public decimal IngresosTotales
    {
        get => _ingresosTotales;
        set
        {
            _ingresosTotales = value;
            OnPropertyChanged(nameof(IngresosTotales));
        }
    }

    public decimal GastosTotales
    {
        get => _gastosTotales;
        set
        {
            _gastosTotales = value;
            OnPropertyChanged(nameof(GastosTotales));
        }
    }

    public decimal BalanceTotal
    {
        get => _balanceTotal;
        set
        {
            _balanceTotal = value;
            OnPropertyChanged(nameof(BalanceTotal));
        }
    }

    public ObservableCollection<TransaccionDTO> UltimasTransacciones
    {
        get => _ultimasTransacciones;
        set
        {
            _ultimasTransacciones = value;
            OnPropertyChanged(nameof(UltimasTransacciones));
        }
    }

    public PaginaHome()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);

        _apiService = new ApiService();
        Transacciones = new ObservableCollection<TransaccionDTO>();
        UltimasTransacciones = new ObservableCollection<TransaccionDTO>();
        BindingContext = this;
        CargarDatosEstadisticos();
    }

    private void boton_nueva_transaccion_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new NuevaTransaccion());
    }

    private void btn_Grupos_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new FamilyGroupsPage());
    }

    private void btn_metas_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new FinancialGoalsPage());
    }

    private void btn_mas_opciones_Tapped(object sender, TappedEventArgs e)
    {
        var page = new MiBottomSheet();

        page.HasHandle = true;
        page.HasBackdrop = true;
        page.IsCancelable = true;
        page.HandleColor = Colors.Gray;
        

        page.ShowAsync(Window);
    }

    private async void CargarDatosEstadisticos()
    {
        try
        {
            IsLoading = true;

            var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
            if (!int.TryParse(usuarioIdString, out int usuarioId))
            {
                await DisplayAlert("Error", "Usuario no autenticado", "OK");
                return;
            }

            var req = new ReqTransaccionesPorUsuario
            {
                UsuarioID = usuarioId,
                FechaInicio = null,
                FechaFin = null,
                TipoTransaccion = null,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            var response = await _apiService.ListarTransaccionesPorUsuario(req);
            if (response.resultado)
            {
                Transacciones.Clear();
                foreach (var transaccion in response.transacciones)
                {
                    transaccion.ColorHex = transaccion.Tipo == "Ingreso" ? "#5EBF7E" : "#FF0000";
                    Transacciones.Add(transaccion);
                }

                // Calcular totales de ingresos, gastos y balance
                IngresosTotales = Transacciones.Where(t => t.Tipo == "Ingreso").Sum(t => t.Monto);
                GastosTotales = Transacciones.Where(t => t.Tipo == "Gasto").Sum(t => t.Monto);
                BalanceTotal = IngresosTotales - GastosTotales;

                // Cargar las últimas 5 transacciones
                UltimasTransacciones.Clear();
                var ultimas = Transacciones.OrderByDescending(t => t.Fecha).Take(5).ToList();
                foreach (var transaccion in ultimas)
                {
                    UltimasTransacciones.Add(transaccion);
                }
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar transacciones", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción en CargarDatosEstadisticos: {ex.Message}");
            await DisplayAlert("Error", "Error inesperado al cargar los datos", "OK");
        }
        finally
        {
            IsLoading = false;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CargarDatosEstadisticos();
    }
}