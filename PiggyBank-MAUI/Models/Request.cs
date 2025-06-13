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

    public class ReqIngresarTransaccion : ReqBase
    {
        public Transaccion Transaccion { get; set; }
    }

    public class ReqTransaccionesPorUsuario : ReqBase
    {
        public int UsuarioID { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public string? TipoTransaccion { get; set; }
    }

    public class ReqObtenerDetalleTransaccion : ReqBase
    {
        public int TransaccionID { get; set; }
        public int UsuarioID { get; set; }
    }

    public class ReqActualizarTransaccion : ReqBase
    {
        public int TransaccionID { get; set; }
        public int UsuarioID { get; set; }
        public string Tipo { get; set; } // 'Ingreso' o 'Gasto'
        public decimal Monto { get; set; }
        public int CategoriaID { get; set; }
        public DateTime Fecha { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool EsCompartido { get; set; }
        public int? GrupoID { get; set; }
    }

    public class ReqEliminarTransaccion : ReqBase
    {
        public int TransaccionID { get; set; }
        public int UsuarioID { get; set; }
    }

    public class ReqObtenerTodosContexto : ReqBase
    {

    }

    public class ReqObtenerAnalisisUsuario : ReqBase
    {

    }
    public class ReqObtenerMensajes : ReqBase
    {
        public int AnalisisID { get; set; } // ID del análisis al que pertenecen los mensajes
    }
    public class ReqCrearAnalisis : ReqBase
    {
        public DateTime? FechaInicio { get; set; } // Fecha de inicio del análisis
        public DateTime? FechaFin { get; set; } // Fecha de fin del análisis
        public int ContextoID { get; set; } // ID del contexto del análisis
        public string Consulta { get; set; } // Mensaje del análisis


    }

    public class ReqInsertarMensajeChat : ReqBase
    {
        public int AnalisisID { get; set; } // ID del análisis al que pertenece el mensaje
        public string Role { get; set; } // Rol del remitente (user o assistant)
        public string Content { get; set; } // Contenido del mensaje
    }

    public class ReqCrearCategoria : ReqBase
    {
        public string Nombre { get; set; }
        public string? Icono { get; set; }
        public string? ColorHex { get; set; }
    }

    public class ReqActualizarCategoria : ReqBase
    {
        public string Nombre { get; set; }
        public string? Icono { get; set; }
        public string? ColorHex { get; set; }
    }


}









