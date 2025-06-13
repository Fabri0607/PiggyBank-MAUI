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

    public class AnalisisDTO
    {
        public int AnalisisID { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Resumen { get; set; }
        public DateTime FechaGeneracion { get; set; }
        public int Contexto { get; set; }

    }

    public class MensajeDTO
    {
        public int MensajeID { get; set; }
        public int AnalisisID { get; set; }
        public string Role { get; set; } // 'user', 'assistant'
        public string Content { get; set; }
        public int Orden { get; set; } // Order of the message in the conversation
        public DateTime FechaEnvio { get; set; }
    }

    public class ContextoDTO
    {
        public int ContextoID { get; set; }
        public string Nombre { get; set; }
        public DateTime FechaCreacion { get; set; }
    }



    public class Transaccion
    {
        public int? TransaccionID { get; set; }
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

    public class Categoria
    {
        public int CategoriaID { get; set; }
        public string Nombre { get; set; }
        public string Icono { get; set; }
        public string ColorHex { get; set; }
    }

    public class TransaccionDTO
    {
        public int TransaccionID { get; set; }
        public string Tipo { get; set; } // 'Ingreso' o 'Gasto'
        public decimal Monto { get; set; }
        public string Categoria { get; set; }
        public DateTime Fecha { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public string Icono { get; set; }
        public string ColorHex { get; set; }

        public bool EsCompartido { get; set; }
        public int? GrupoID { get; set; }

        // Add the missing property to fix CS1061  
        public string DisplayText { get; set; }
    }

}

    

