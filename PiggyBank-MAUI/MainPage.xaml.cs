using PiggyBank_MAUI.Views;

namespace PiggyBank_MAUI
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnCounterClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
                CounterBtn.Text = $"Clicked {count} time";
            else
                CounterBtn.Text = $"Clicked {count} times";

            SemanticScreenReader.Announce(CounterBtn.Text);
        }

        private void LoginPage_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PaginaInicioDeSesion());
        }

        private void test_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new test());
        }

        private void Registrarse_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PaginaRegistrarse());
        }

        private void BotonBienvenida_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PaginaBienvenida());
        }

        private void BotonApp_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AppShell());
        }

        private void Tabbed_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new PaginaTap());
        }
    }

}
