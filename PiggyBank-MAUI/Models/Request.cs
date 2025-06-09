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

    public class ReqSolicitarCambioPassword
    {
        public string Email { get; set; }
    }

    public class ReqConfirmarCambioPassword
    {
        public string Email { get; set; }
        public string CodigoVerificacion { get; set; }
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

    public class ReqCrearGrupoFamiliar : ReqBase
    {
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int UsuarioID { get; set; }
    }

    public class ReqInvitarMiembroGrupo : ReqBase
    {
        public int GrupoID { get; set; }
        public string correoUsuario { get; set; }
        public string Rol { get; set; }
    }

    public class ReqRegistrarGastoCompartido : ReqBase
    {
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public int CategoriaID { get; set; }
        public string Descripcion { get; set; }
    }


    public class ReqObtenerBalanceGrupal : ReqBase
    {
        public int GrupoID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }


    public class ReqSalirGrupo : ReqBase
    {
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
    }


    public class ReqListarGrupos : ReqBase
    {
        public int UsuarioID { get; set; }
    }


    public class ReqObtenerDetallesGrupo : ReqBase
    {
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
    }


    public class ReqEliminarMiembro : ReqBase
    {
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
        public int AdminUsuarioID { get; set; }
    }


    public class ReqActualizarGrupo : ReqBase
    {
        public int GrupoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public int AdminUsuarioID { get; set; }
    }


    public class ReqListarGastos : ReqBase
    {
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
    }


    public class ReqEliminarGrupo : ReqBase
    {
        public int GrupoID { get; set; }
        public int AdminUsuarioID { get; set; }
    }

    public class ReqActualizarEstadoGasto : ReqBase
    {
        public int GastoID { get; set; }
        public int UsuarioID { get; set; }
        public string NuevoEstado { get; set; }
    }

    public class ReqCrearMeta : ReqBase
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public decimal MontoObjetivo { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaObjetivo { get; set; }
    }

    public class ReqActualizarProgresoMeta : ReqBase
    {
        public int MetaID { get; set; }
        public int UsuarioID { get; set; }
        public decimal MontoActual { get; set; }
    }

    public class ReqListarMetas : ReqBase
    {
        public int UsuarioID { get; set; }
    }

    public class ReqObtenerDetallesMeta : ReqBase
    {
        public int MetaID { get; set; }
        public int UsuarioID { get; set; }
    }

    public class ReqActualizarMeta : ReqBase
    {
        public int MetaID { get; set; }
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public decimal MontoObjetivo { get; set; }
        public DateTime? FechaObjetivo { get; set; }
    }

    public class ReqEliminarMeta : ReqBase
    {
        public int MetaID { get; set; }
        public int UsuarioID { get; set; }
    }

    public class ReqAsignarTransaccion : ReqBase
    {
        public int MetaID { get; set; }
        public int UsuarioID { get; set; }
        public int TransaccionID { get; set; }
        public decimal MontoAsignado { get; set; }
    }

}







