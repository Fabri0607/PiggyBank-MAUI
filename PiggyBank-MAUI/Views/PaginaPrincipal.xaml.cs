using Microsoft.Maui.Controls;

namespace PiggyBank_MAUI.Views;

public partial class PaginaPrincipal : ContentPage
{
    public PaginaPrincipal()
    {
        InitializeComponent();
        NavigationPage.SetHasNavigationBar(this, false);
    }
}