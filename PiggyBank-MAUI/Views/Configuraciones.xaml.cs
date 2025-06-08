namespace PiggyBank_MAUI.Views;

public partial class Configuraciones : ContentPage
{
	public Configuraciones()
	{
		InitializeComponent();
	}

    private void btn_actualizar_perfil_Tapped(object sender, TappedEventArgs e)
    {
		Navigation.PushAsync(new PaginaActualizarPerfil());
    }

    private void btn_cambiar_password_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new PaginaCambiarPassword());
    }

    private void btn_logout_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new PaginaCerrarSesion());
    }
}