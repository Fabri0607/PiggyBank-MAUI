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
        DisplayAlert("Redirecci�n", "Redirecci�n a cambiar contrase�a", "OK");
    }

    private void BotonRegistrarse_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PaginaRegistrarse());
    }

    private void Boton_IniciarSesion_Clicked(object sender, EventArgs e)
    {
        Application.Current.MainPage = new AppShell();
    }
}