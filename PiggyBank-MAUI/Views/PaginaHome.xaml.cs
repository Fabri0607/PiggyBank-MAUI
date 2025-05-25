namespace PiggyBank_MAUI.Views;

public partial class PaginaHome : ContentPage
{
	public PaginaHome()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void boton_nueva_transaccion_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new NuevaTransaccion());
    }
}