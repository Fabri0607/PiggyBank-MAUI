using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace PiggyBank_MAUI.Views;

public partial class ActualizarPago : ContentPage, INotifyPropertyChanged
{
    private readonly ApiService _apiService;
    private ObservableCollection<Categoria> _categorias;
    private ObservableCollection<string> _estados;
    private Categoria _selectedCategoria;
    private PagoDTO _pago;

    public PagoDTO Pago
    {
        get => _pago;
        set
        {
            _pago = value;
            OnPropertyChanged(nameof(Pago));
        }
    }

    public ObservableCollection<Categoria> Categorias
    {
        get => _categorias;
        set
        {
            _categorias = value;
            OnPropertyChanged(nameof(Categorias));
        }
    }

    public ObservableCollection<string> Estados
    {
        get => _estados;
        set
        {
            _estados = value;
            OnPropertyChanged(nameof(Estados));
        }
    }

    public Categoria SelectedCategoria
    {
        get => _selectedCategoria;
        set
        {
            _selectedCategoria = value;
            OnPropertyChanged(nameof(SelectedCategoria));
        }
    }

    public ICommand GuardarCommand { get; }

    public ActualizarPago(PagoDTO pago)
    {
        InitializeComponent();
        _apiService = new ApiService();
        Pago = pago ?? new PagoDTO();
        Categorias = new ObservableCollection<Categoria>();
        Estados = new ObservableCollection<string> { "Pendiente", "Pagado" };
        GuardarCommand = new Command(async () => await GuardarPago());
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
                Categorias.Clear();
                foreach (var categoria in response.categorias)
                {
                    Categorias.Add(categoria);
                }
                // Seleccionar la categoría correspondiente
                SelectedCategoria = Categorias.FirstOrDefault(c => c.CategoriaID == Pago.CategoriaID);
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

    private async Task GuardarPago()
    {
        try
        {
            // Validar campos obligatorios
            if (string.IsNullOrWhiteSpace(Pago.Titulo))
            {
                await DisplayAlert("Error", "El título es obligatorio", "OK");
                return;
            }

            if (Pago.Monto <= 0)
            {
                await DisplayAlert("Error", "El monto debe ser un valor numérico positivo", "OK");
                return;
            }

            if (SelectedCategoria == null)
            {
                await DisplayAlert("Error", "La categoría es obligatoria", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(Pago.Estado))
            {
                await DisplayAlert("Error", "El estado es obligatorio", "OK");
                return;
            }

            // Obtener UsuarioID
            var usuarioIdString = await SecureStorage.GetAsync("UsuarioID");
            if (!int.TryParse(usuarioIdString, out int usuarioId))
            {
                await DisplayAlert("Error", "Usuario no autenticado", "OK");
                return;
            }

            // Crear el objeto de solicitud
            var req = new ReqActualizarPago
            {
                PagoID = Pago.PagoID,
                UsuarioID = usuarioId,
                Titulo = Pago.Titulo.Trim(),
                Monto = Pago.Monto,
                Fecha_Vencimiento = Pago.Fecha_Vencimiento,
                CategoriaID = SelectedCategoria.CategoriaID,
                Estado = Pago.Estado,
                token = Preferences.Get("AuthToken", string.Empty)
            };

            Debug.WriteLine($"Actualizando pago programado: PagoID={req.PagoID}, Titulo={req.Titulo}, Monto={req.Monto}, CategoriaID={req.CategoriaID}, Fecha_Vencimiento={req.Fecha_Vencimiento}, Estado={req.Estado}");

            // Llamar al servicio
            var response = await _apiService.ActualizarPago(req);
            if (response.resultado)
            {
                PagosProgramados.PagoAgregado = true; // Indicar que se actualizó un pago
                Debug.WriteLine("Pago programado actualizado correctamente");
                await DisplayAlert("Éxito", "Pago programado actualizado correctamente", "OK");
                await Navigation.PopAsync();
            }
            else
            {
                Debug.WriteLine($"Error al actualizar pago programado: {response.error?.FirstOrDefault()?.Message}");
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al actualizar el pago programado", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Excepción en GuardarPago: {ex.Message}");
            await DisplayAlert("Error", "Error inesperado al actualizar el pago programado", "OK");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}