using PiggyBank_MAUI.Views;

namespace PiggyBank_MAUI
{
    public partial class AppShell : Shell
    {
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
        }
    }
}