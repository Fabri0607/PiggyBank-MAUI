using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Models
{
    public class ReqRegistrarUsuario
    {
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ReqVerificarUsuario
    {
        public string Email { get; set; }
        public string CodigoVerificacion { get; set; }
    }

    public class ReqIniciarSesion
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class ReqCerrarSesion : ReqBase
    {
        public int SesionID { get; set; }
        public string MotivoRevocacion { get; set; }
    }

    public class ReqActualizarPerfil : ReqBase
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string ConfiguracionNotificaciones { get; set; }
    }

    public class ReqCambiarPassword : ReqBase
    {
        public int UsuarioID { get; set; }
        public string PasswordActual { get; set; }
        public string NuevoPassword { get; set; }
    }

    public class ReqReenviarCodigoVerificacion
    {
        public string Email { get; set; }
    }

    public class ReqObtenerUsuario : ReqBase
    {
        public int UsuarioID { get; set; }
    }
}
