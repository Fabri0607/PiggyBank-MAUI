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

    public class ResCrearGrupoFamiliar : ResBase
    {
        public int GrupoID { get; set; }
    }

    public class ResInvitarMiembroGrupo : ResBase
    {
    }


    public class ResRegistrarGastoCompartido : ResBase
    {
        public int GastoID { get; set; }
        public int TransaccionID { get; set; }
    }

    public class ResObtenerBalanceGrupal : ResBase
    {
        public List<BalanceMiembro> Balances { get; set; }
    }


    public class ResSalirGrupo : ResBase
    {
    }


    public class ResListarGrupos : ResBase
    {
        public List<GrupoDTO> Grupos { get; set; }
    }


    public class ResObtenerDetallesGrupo : ResBase
    {
        public GrupoDTO Grupo { get; set; }
        public List<MiembroDTO> Miembros { get; set; }
    }


    public class ResEliminarMiembro : ResBase
    {
    }


    public class ResActualizarGrupo : ResBase
    {
    }


    public class ResListarGastos : ResBase
    {
        public List<GastoCompartidoDTO> Gastos { get; set; }
    }


    public class ResEliminarGrupo : ResBase
    {
    }

    public class ResActualizarEstadoGasto : ResBase
    {
        public GastoCompartidoDTO Gasto { get; set; }
    }

}
















