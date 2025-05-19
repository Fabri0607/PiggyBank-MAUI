namespace PiggyBank_MAUI.Views;

public partial class PaginaBienvenida : ContentPage
{
	public PaginaBienvenida()
	{
		InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }

    private void BotonIniciarSesion_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PaginaInicioDeSesion());
    }

    private void BotonRegistrarse_Clicked(object sender, EventArgs e)
    {
        Navigation.PushAsync(new PaginaRegistrarse());
    }
}