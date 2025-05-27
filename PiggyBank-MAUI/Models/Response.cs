using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Models
{
    public class ResRegistrarUsuario : ResBase
    {
        public int UsuarioID { get; set; }
    }

    public class ResVerificarUsuario : ResBase
    {
    }

    public class ResIniciarSesion : ResBase
    {
        public string Token { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }

    public class ResCerrarSesion : ResBase
    {
    }

    public class ResActualizarPerfil : ResBase
    {
    }

    public class ResCambiarPassword : ResBase
    {
    }

    public class ResReenviarCodigoVerificacion : ResBase
    {
    }

    public class ResObtenerUsuario : ResBase
    {
        public UsuarioDTO Usuario { get; set; }
    }
}
