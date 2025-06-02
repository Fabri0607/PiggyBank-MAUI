using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Models
{
    public class ReqBase
    {
        public Sesion sesion { get; set; }
        public string token { get; set; }
    }

    public class ResBase
    {
        public bool resultado { get; set; }
        public List<Error> error { get; set; }
    }

    public class Error
    {
        public int ErrorCode { get; set; }
        public string Message { get; set; }
    }

    public class Sesion
    {
        public int SesionID { get; set; }
        public int UsuarioID { get; set; }
        public string Guid { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaExpiracion { get; set; }
        public bool EsActivo { get; set; }
        public string MotivoRevocacion { get; set; }
    }

    public class Usuario
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string CodigoVerificacion { get; set; }
        public DateTime? FechaExpiracionCodigo { get; set; }
        public bool EmailVerificado { get; set; }
        public string LlaveUnica { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public string ConfiguracionNotificaciones { get; set; }
    }

    public class UsuarioDTO
    {
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public string Email { get; set; }
        public DateTime FechaRegistro { get; set; }
        public DateTime? UltimoAcceso { get; set; }
        public string ConfiguracionNotificaciones { get; set; }
    }


    public class GrupoFamiliar
    {
        public int GrupoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CreadoPorUsuarioID { get; set; }
        public string Estado { get; set; } // 'Activo', 'Eliminado'
        public DateTime? FechaActualizacion { get; set; }
    }


    public class MiembroGrupo
    {
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
        public string Rol { get; set; } // 'Administrador', 'Miembro', 'Consulta'
        public DateTime FechaUnion { get; set; }
    }

    public class GastoCompartido
    {
        public int GastoID { get; set; }
        public int TransaccionID { get; set; }
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; } // 'Pendiente', 'Pagado', 'Rechazado'
        public DateTime Fecha { get; set; }
    }


    public class BalanceMiembro
    {
        public int BalanceID { get; set; }
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public decimal TotalGastos { get; set; }
        public decimal TotalPagado { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaCalculo { get; set; }
    }


    public class GrupoDTO
    {
        public int GrupoID { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public int CreadoPorUsuarioID { get; set; }
        public string Estado { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }


    public class MiembroDTO
    {
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public string Rol { get; set; }
        public DateTime FechaUnion { get; set; }
        public bool IsAdmin => Rol == "Administrador"; // Propiedad derivada para facilitar la lógica en la UI
    }


    public class GastoCompartidoDTO
    {
        public int GastoID { get; set; }
        public int TransaccionID { get; set; }
        public int GrupoID { get; set; }
        public int UsuarioID { get; set; }
        public string NombreUsuario { get; set; }
        public decimal Monto { get; set; }
        public string Estado { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }

        public bool IsPendiente => Estado?.Equals("Pendiente", StringComparison.OrdinalIgnoreCase) ?? false;
    }
    public class MetaDTO
    {
        public int MetaID { get; set; }
        public int UsuarioID { get; set; }
        public string Nombre { get; set; }
        public decimal MontoObjetivo { get; set; }
        public decimal MontoActual { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime? FechaObjetivo { get; set; }
        public bool Completada { get; set; }
        public decimal Progreso { get; set; } // Percentage
        public decimal AhorroMensualSugerido { get; set; } // Suggested monthly savings
    }

    public class MetaTransaccionDTO
    {
        public int TransaccionID { get; set; }
        public decimal MontoAsignado { get; set; }
        public DateTime FechaAsignacion { get; set; }
        public string Descripcion { get; set; }
        public decimal MontoTransaccion { get; set; }
    }

}

    

