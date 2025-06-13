using PiggyBank_MAUI.Models;
using PiggyBank_MAUI.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace PiggyBank_MAUI.Views;

public partial class Categorias : ContentPage
{
	private readonly ApiService _apiService;
    private ObservableCollection<Categoria> _categorias;

    public ObservableCollection<Categoria> CategoriasList
    {
        get => _categorias;
        set
        {
            _categorias = value;
            OnPropertyChanged(nameof(CategoriasList));
        }
    }

    public Categorias()
	{
		InitializeComponent();
        _apiService = new ApiService();
        CategoriasList = new ObservableCollection<Categoria>();
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
                CategoriasList.Clear();
                foreach (var categoria in response.categorias)
                {
                    CategoriasList.Add(categoria);
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

    private void OnCategoriaTapped(object sender, EventArgs e)
    {
        // Aquí puedes agregar la lógica para manejar el tap en una categoría
        if (sender is Border border && border.BindingContext is Categoria categoria)
        {
            Debug.WriteLine($"Categoría seleccionada: {categoria.Nombre} (ID: {categoria.CategoriaID})");
            // Ejemplo: await DisplayAlert("Categoría", $"Seleccionaste: {categoria.Nombre}", "OK");
        }
    }
}