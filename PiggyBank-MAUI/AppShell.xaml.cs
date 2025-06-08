using PiggyBank_MAUI.Views;
using System.Windows.Input;

namespace PiggyBank_MAUI
{
    public partial class AppShell : Shell
    {
        public ICommand NavigateToSettingsCommand { get; }

        public AppShell()
        {
            InitializeComponent();

            // Registrar rutas para navegación
            Routing.RegisterRoute(nameof(PaginaInicioDeSesion), typeof(PaginaInicioDeSesion));
            Routing.RegisterRoute(nameof(PaginaRegistrarse), typeof(PaginaRegistrarse));
            Routing.RegisterRoute(nameof(PaginaVerificarUsuario), typeof(PaginaVerificarUsuario));
            Routing.RegisterRoute(nameof(PaginaReenviarCodigoVerificacion), typeof(PaginaReenviarCodigoVerificacion));
            Routing.RegisterRoute(nameof(PaginaActualizarPerfil), typeof(PaginaActualizarPerfil));
            Routing.RegisterRoute(nameof(PaginaCambiarPassword), typeof(PaginaCambiarPassword));
            Routing.RegisterRoute(nameof(PaginaCerrarSesion), typeof(PaginaCerrarSesion));
            Routing.RegisterRoute(nameof(FamilyGroupsPage), typeof(FamilyGroupsPage));
            Routing.RegisterRoute(nameof(GroupDetailsPage), typeof(GroupDetailsPage));
            Routing.RegisterRoute(nameof(RegisterExpenseModalPage), typeof(RegisterExpenseModalPage));
            Routing.RegisterRoute(nameof(UpdateGroupModalPage), typeof(UpdateGroupModalPage));
            Routing.RegisterRoute(nameof(FinancialGoalsPage), typeof(FinancialGoalsPage));
            Routing.RegisterRoute(nameof(FinancialGoalDetailsPage), typeof(FinancialGoalDetailsPage));

            NavigateToSettingsCommand = new Command(async () => await NavigateToSettings());
            BindingContext = this;
        }

        private async Task NavigateToSettings()
        {
            Shell.Current.FlyoutIsPresented = false;

            // Crear una instancia de la página Configuraciones
            var settingsPage = new Views.Configuraciones();
            // Empujar la página a la pila de navegación
            await Shell.Current.Navigation.PushAsync(settingsPage);
        }
    }
}