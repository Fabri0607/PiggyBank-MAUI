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

}
