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

    private void btn_Grupos_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new FamilyGroupsPage());
    }

    private void btn_metas_Tapped(object sender, TappedEventArgs e)
    {
        Navigation.PushAsync(new FinancialGoalsPage());
    }

    private void btn_mas_opciones_Tapped(object sender, TappedEventArgs e)
    {
        var page = new MiBottomSheet();

        page.HasHandle = true;
        page.HasBackdrop = true;
        page.IsCancelable = true;
        page.HandleColor = Colors.Gray;
        

        page.ShowAsync(Window);
    }
}