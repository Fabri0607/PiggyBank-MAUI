namespace PiggyBank_MAUI.Views;

public partial class PaginaTransacciones : ContentPage
{
	public PaginaTransacciones()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new ActualizarTransaccion());
    }
}