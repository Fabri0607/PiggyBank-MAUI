namespace PiggyBank_MAUI.Views;

public partial class PaginaInicioDeSesion : ContentPage
{
	public PaginaInicioDeSesion()
	{
		InitializeComponent();
		NavigationPage.SetHasNavigationBar(this, false);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        DisplayAlert("Redirección", "Redirección a cambiar contraseña", "OK");
    }
}