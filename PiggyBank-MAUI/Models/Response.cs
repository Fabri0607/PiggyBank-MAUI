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

    public class ResSolicitarCambioPassword : ResBase
    {
    }

    public class ResConfirmarCambioPassword : ResBase
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

    public class ResCrearMeta : ResBase
    {
        public int MetaID { get; set; }
    }

    public class ResActualizarProgresoMeta : ResBase
    {
    }

    public class ResListarMetas : ResBase
    {
        public List<MetaDTO> Metas { get; set; }
    }

    public class ResObtenerDetallesMeta : ResBase
    {
        public MetaDTO Meta { get; set; }
        public List<MetaTransaccionDTO> Transacciones { get; set; }
    }

    public class ResActualizarMeta : ResBase
    {
    }

    public class ResEliminarMeta : ResBase
    {
    }

    public class ResAsignarTransaccion : ResBase
    {
    }

    public class ResIngresarTransaccion : ResBase
    {

    }

    public class ResTransaccionesPorUsuario : ResBase
    {
        public List<TransaccionDTO> transacciones { get; set; }
    }

    public class ResObtenerDetalleTransaccion : ResBase
    {
        public int TransaccionID { get; set; }
        public int UsuarioID { get; set; }
        public string Tipo { get; set; } // 'Ingreso' o 'Gasto'
        public decimal Monto { get; set; }
        public string Categoria { get; set; }
        public DateTime Fecha { get; set; }
        public string Titulo { get; set; }
        public string Descripcion { get; set; }
        public bool EsCompartido { get; set; }
        public int? GrupoID { get; set; }
    }

    public class ResActualizarTransaccion : ResBase
    {

    }

    public class ResEliminarTransaccion : ResBase
    {

    }

    public class ResObtenerTodosContexto : ResBase
    {
        public List<ContextoDTO> Contextos { get; set; }
    }
    public class ResObtenerAnalisisUsuario : ResBase
    {
        public List<AnalisisDTO> AnalisisIA { get; set; }
    }
    public class ResObtenerMensajes : ResBase
    {
        public List<MensajeDTO> MensajesChat { get; set; }
    }
    public class ResCrearAnalisis : ResBase
    {
        public int AnalisisID { get; set; }
    }
    public class ResInsertarMensaje : ResBase
    {
        public int MensajeConsultaID { get; set; }
        public int MensajeRespuestaID { get; set; }
    }

    public class ResCrearCategoria : ResBase
    {

    }

    public class ResActualizarCategoria : ResBase
    {

    }

    public class ResEliminarCategoria : ResBase
    {

    }

    public class ResObtenerCategorias : ResBase
    {
        public List<Categoria> categorias { get; set; }
    }

    public class ResIngresarPagoProgramado : ResBase
    {

    }

    public class ResPagosPorUsuario : ResBase
    {
        public List<PagoDTO> Pagos { get; set; }
    }

    public class ResObtenerDetallePago : ResBase
    {
        public int PagoID { get; set; }
        public int UsuarioID { get; set; }
        public string Titulo { get; set; }
        public decimal Monto { get; set; }
        public string NombreCategoria { get; set; }
        public DateTime Fecha_Vencimiento { get; set; }
        public string Estado { get; set; }
        public int CategoriaID { get; set; }
    }

    public class ResActualizarPago : ResBase
    {

    }

    public class ResEliminarPago : ResBase
    {

    }
}
