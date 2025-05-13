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


    private async void OnShowModalClicked(object sender, EventArgs e)
    {
        modalView.IsVisible = true;
        modalView.Opacity = 0;
        await modalView.FadeTo(1, 250); // Animación de entrada
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await modalView.FadeTo(0, 250); // Animación de salida
        modalView.IsVisible = false;
    }
}