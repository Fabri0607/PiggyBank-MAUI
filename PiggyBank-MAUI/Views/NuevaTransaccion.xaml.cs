namespace PiggyBank_MAUI.Views;

public partial class NuevaTransaccion : ContentPage
{
	public NuevaTransaccion()
	{
		InitializeComponent();

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
}