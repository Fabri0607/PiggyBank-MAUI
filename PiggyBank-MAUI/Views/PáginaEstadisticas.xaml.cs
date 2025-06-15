using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;

namespace PiggyBank_MAUI.Views;

public partial class PáginaEstadisticas : ContentPage, INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private ObservableCollection<TransaccionDTO> _transacciones;
    private bool _isLoading;
    private decimal _ingresosTotales;
    private decimal _gastosTotales;
    private decimal _balanceTotal;
    private ObservableCollection<CategoriaGasto> _categoriasGastos;
    private ObservableCollection<TransaccionDTO> _cincoMayoresGastos;

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

    public ObservableCollection<CategoriaGasto> CategoriasGastos
    {
        get => _categoriasGastos;
        set
        {
            _categoriasGastos = value;
            OnPropertyChanged(nameof(CategoriasGastos));
        }
    }

    public ObservableCollection<TransaccionDTO> CincoMayoresGastos
    {
        get => _cincoMayoresGastos;
        set
        {
            _cincoMayoresGastos = value;
            OnPropertyChanged(nameof(CincoMayoresGastos));
        }
    }

    public PáginaEstadisticas()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        _apiService = new ApiService();
        Transacciones = new ObservableCollection<TransaccionDTO>();
        CategoriasGastos = new ObservableCollection<CategoriaGasto>();
        CincoMayoresGastos = new ObservableCollection<TransaccionDTO>();
        BindingContext = this;
        CargarDatosEstadisticos();

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

                // Calcular categorías de gastos
                var gastos = Transacciones.Where(t => t.Tipo == "Gasto").ToList();
                var totalGastos = GastosTotales;
                CategoriasGastos.Clear();
                var categorias = gastos.GroupBy(t => t.Categoria)
                                      .Select(g => new CategoriaGasto
                                      {
                                          Nombre = g.Key,
                                          Total = g.Sum(t => t.Monto),
                                          Porcentaje = totalGastos > 0 ? (g.Sum(t => t.Monto) / totalGastos) * 100m : 0m,
                                          TotalConPorcentaje = $"{g.Sum(t => t.Monto):₡#,##0.00} ({(totalGastos > 0 ? (g.Sum(t => t.Monto) / totalGastos) * 100m : 0m):F1}%)"
                                      })
                                      .OrderByDescending(c => c.Porcentaje);
                foreach (var categoria in categorias)
                {
                    CategoriasGastos.Add(categoria);
                }

                // Calcular los 5 mayores gastos
                CincoMayoresGastos.Clear();
                var mayoresGastos = Transacciones
                    .Where(t => t.Tipo == "Gasto")
                    .OrderByDescending(t => t.Monto)
                    .Take(5)
                    .ToList();
                foreach (var gasto in mayoresGastos)
                {
                    CincoMayoresGastos.Add(gasto);
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
        // Recargar si no hay transacciones o si se agregó una nueva transacción
        CargarDatosEstadisticos();
    }

    // Clase para representar una categoría de gasto
    public class CategoriaGasto : INotifyPropertyChanged
    {
        private string _nombre;
        private decimal _total;
        private decimal _porcentaje;
        private string _totalConPorcentaje;

        public string Nombre
        {
            get => _nombre;
            set
            {
                _nombre = value;
                OnPropertyChanged(nameof(Nombre));
            }
        }

        public decimal Total
        {
            get => _total;
            set
            {
                _total = value;
                OnPropertyChanged(nameof(Total));
            }
        }

        public decimal Porcentaje
        {
            get => _porcentaje;
            set
            {
                _porcentaje = value;
                OnPropertyChanged(nameof(Porcentaje));
            }
        }

        public string TotalConPorcentaje
        {
            get => _totalConPorcentaje;
            set
            {
                _totalConPorcentaje = value;
                OnPropertyChanged(nameof(TotalConPorcentaje));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}