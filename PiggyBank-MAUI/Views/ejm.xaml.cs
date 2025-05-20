namespace PiggyBank_MAUI.Views;

public partial class ejm : FlyoutPage
{
	public ejm()
	{
		InitializeComponent();
	}

    private void ImageButton_Clicked(object sender, EventArgs e)
    {
        IsPresented = true; // Abrir el flyout
    }
}