using The49.Maui.BottomSheet;

namespace PiggyBank_MAUI.Views;

public partial class MiBottomSheet: BottomSheet
{
	public MiBottomSheet()
	{
		InitializeComponent();
	}

    private async void btn_categorias_Tapped(object sender, TappedEventArgs e)
    {
		var categoriasPage = new Categorias();

		await Shell.Current.Navigation.PushAsync(categoriasPage);
    }
}