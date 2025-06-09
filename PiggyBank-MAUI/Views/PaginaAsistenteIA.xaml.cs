using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace PiggyBank_MAUI.Views;

public partial class PaginaAsistenteIA : ContentPage
{
    public readonly ApiService _apiService;
    private ObservableCollection<AnalisisDTO> _analisisList;
    public ObservableCollection<AnalisisDTO> AnalisisList
    {
        get => _analisisList;
        set
        {
            _analisisList = value;
            OnPropertyChanged(nameof(AnalisisList));
        }
    }

    //private ObservableCollection<ContextoDTO> _contextosList;
    //public ObservableCollection<ContextoDTO> ContextosList
    //{
    //    get => _contextosList;
    //    set
    //    {
    //        _contextosList = value;
    //        OnPropertyChanged(nameof(ContextosList));
    //    }
    //}

    public ICommand ViewDetailsCommand { get; }
    public ICommand SendCommand { get; }
    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set
        {
            _isBusy = value;
            OnPropertyChanged(nameof(IsBusy));
        }
    }
    private bool _isEmptyState = true;
    public bool IsEmptyState
    {
        get => _isEmptyState;
        set
        {
            _isEmptyState = value;
            OnPropertyChanged(nameof(IsEmptyState));
            OnPropertyChanged(nameof(HasAnalisis));
        }
    }

    public bool HasAnalisis
    {
        get => !IsEmptyState;
    }

    public PaginaAsistenteIA()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
        _apiService = new ApiService();
        _analisisList = new ObservableCollection<AnalisisDTO>();
        //_contextosList = new ObservableCollection<ContextoDTO>();
        ViewDetailsCommand = new Command<AnalisisDTO>(async (analisis) => await OnViewDetails(analisis));
        BindingContext = this;
        LoadAnalisis();
        //LoadContextos();
    }

    //private async void LoadContextos()
    //{
    //    Debug.WriteLine("Iniciando LoadContextos");
    //    try
    //    {
    //        var response = await _apiService.ObtenerTodosContexto();
    //        Debug.WriteLine($"Respuesta de ObtenerTodosContexto: {response.resultado}");

    //        if (response.resultado)
    //        {
    //            ContextosList.Clear();
    //            if (response.Contextos != null)
    //            {
    //                foreach (var contexto in response.Contextos)
    //                {
    //                    ContextosList.Add(contexto);
    //                }
    //            }
    //            ContextoPicker.ItemsSource = ContextosList;
    //        }
    //        else
    //        {
    //            await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar los contextos", "OK");
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Debug.WriteLine($"Exception in LoadContextos: {ex.Message}");
    //        await DisplayAlert("Error", "Ocurrió un error inesperado al cargar los contextos.", "OK");
    //    }
    //}

    private async Task OnViewDetails(AnalisisDTO analisis)
    {
        try
        {
            string mensaje = $"Fecha Generación: {analisis.FechaGeneracion:dd/MM/yyyy HH:mm}\n" +
                           $"Período: {analisis.FechaInicio:dd/MM/yyyy} - {analisis.FechaFin:dd/MM/yyyy}\n\n" +
                           $"Contexto: {analisis.Contexto}\n\n" +
                           $"Resumen: {analisis.Resumen}";

            await DisplayAlert("Detalles del Análisis", mensaje, "OK");
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error en OnViewDetails: {ex.Message}");
            await DisplayAlert("Error", "Ocurrió un error al mostrar los detalles del análisis", "OK");
        }
    }

    private async void LoadAnalisis()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            var token = Preferences.Get("AuthToken", string.Empty);
            var req = new ReqObtenerAnalisisUsuario { token = token };
            var response = await _apiService.ObtenerAnalisis(req);

            if (response.resultado)
            {
                AnalisisList.Clear();
                if (response.AnalisisIA != null)
                {
                    foreach (var analisis in response.AnalisisIA)
                    {
                        AnalisisList.Add(analisis);
                    }
                }
                IsEmptyState = !AnalisisList.Any();
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al cargar los análisis", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in LoadAnalisis: {ex.Message}");
            await DisplayAlert("Error", "Ocurrió un error inesperado al cargar los análisis.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private void OnSendClicked(object sender, EventArgs e)
    {
        ModalOverlay.IsVisible = true;
    }

    private void OnCancelModalClicked(object sender, EventArgs e)
    {
        ModalOverlay.IsVisible = false;
    }

    private async void OnConfirmModalClicked(object sender, EventArgs e)
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            //var selectedContexto = ContextoPicker.SelectedItem as ContextoDTO;
            //if (selectedContexto == null)
            //{
            //    await DisplayAlert("Error", "Debe seleccionar un contexto.", "OK");
            //    return;
            //}

            var request = new ReqCrearAnalisis
            {
                token = Preferences.Get("AuthToken", string.Empty),
                FechaInicio = FechaInicioPicker.Date,
                FechaFin = FechaFinPicker.Date,
                //ContextoID = selectedContexto.ContextoID,
                Consulta = MessageEntry.Text
            };

            var response = await _apiService.CrearAnalisis(request);

            if (response.resultado)
            {
                await DisplayAlert("Éxito", "Análisis creado correctamente.", "OK");
                ModalOverlay.IsVisible = false;
                MessageEntry.Text = string.Empty;
                LoadAnalisis(); // Recargar la lista de análisis
            }
            else
            {
                await DisplayAlert("Error", response.error?.FirstOrDefault()?.Message ?? "Error al crear el análisis.", "OK");
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Exception in OnConfirmModalClicked: {ex.Message}");
            await DisplayAlert("Error", "Ocurrió un error inesperado al crear el análisis.", "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
