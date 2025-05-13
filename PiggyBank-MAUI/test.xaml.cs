using PiggyBank_MAUI.Views;

namespace PiggyBank_MAUI;

public partial class test : ContentPage
{
	public test()
	{
		InitializeComponent();
	}

	private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
    {
        if (e.Direction == SwipeDirection.Left)
        {
            Navigation.PushAsync(new PaginaInicioDeSesion());
        }
        else if (e.Direction == SwipeDirection.Right)
        {
            Navigation.PopAsync();
        }
    }

    private void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        DisplayAlert("Seleccionada", "Card selecionada", "OK");
    }
}