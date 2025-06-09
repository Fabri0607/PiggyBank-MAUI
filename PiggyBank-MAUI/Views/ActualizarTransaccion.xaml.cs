using PiggyBank_MAUI.Services;

namespace PiggyBank_MAUI.Views;

public partial class ActualizarTransaccion : ContentPage
{
    private readonly ApiService _apiService;
	public ActualizarTransaccion()
	{
		InitializeComponent();

        _apiService = new ApiService();

        BindingContext = this;

        TipoTransaccionPickerActualizar.ItemsSource = new List<string>
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

    private void Boton_ActualizarTransaccion_Clicked(object sender, EventArgs e)
    {

    }
}