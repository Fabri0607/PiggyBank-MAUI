namespace PiggyBank_MAUI.Views;

public partial class PaginaRegistrarse : ContentPage
{
	public PaginaRegistrarse()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new PaginaInicioDeSesion());
    }
}