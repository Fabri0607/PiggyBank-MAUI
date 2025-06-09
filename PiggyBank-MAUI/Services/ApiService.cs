using PiggyBank_MAUI.Models;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PiggyBank_MAUI.Services
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://34.68.201.182:44315/api/"; // Reemplazarla cada vez que se inicie el servidor
    
public ApiService()
{
    _httpClient = new HttpClient
    {
        BaseAddress = new Uri(BaseUrl)
    };

    // Configurar el token existente si hay uno guardado
    var existingToken = Preferences.Get("AuthToken", string.Empty);
    if (!string.IsNullOrEmpty(existingToken))
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", existingToken);
    }
}

private void UpdateAuthenticationHeader()
{
    var token = Preferences.Get("AuthToken", string.Empty);
    if (!string.IsNullOrEmpty(token))
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
    else
    {
        _httpClient.DefaultRequestHeaders.Authorization = null;
    }
}

public void SetToken(string token)
{
    Preferences.Set("AuthToken", token);
    _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
}

public void ClearToken()
{
    Preferences.Remove("AuthToken");
    _httpClient.DefaultRequestHeaders.Authorization = null;
}

public async Task<ResRegistrarUsuario> RegistrarUsuario(ReqRegistrarUsuario req)
{
    try
    {
        var json = JsonSerializer.Serialize(req);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("usuarios/registrar", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResRegistrarUsuario>(responseContent);
    }
    catch (Exception ex)
    {
        return new ResRegistrarUsuario { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
    }
}

public async Task<ResVerificarUsuario> VerificarUsuario(ReqVerificarUsuario req)
{
    try
    {
        var json = JsonSerializer.Serialize(req);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("usuarios/verificar", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResVerificarUsuario>(responseContent);
    }
    catch (Exception ex)
    {
        return new ResVerificarUsuario { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
    }
}

public async Task<ResIniciarSesion> IniciarSesion(ReqIniciarSesion req)
{
    try
    {
        var json = JsonSerializer.Serialize(req);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("usuarios/iniciar-sesion", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResIniciarSesion>(responseContent);

        if (result.resultado && !string.IsNullOrEmpty(result.Token))
        {
            SetToken(result.Token);
        }

        return result;
    }
    catch (Exception ex)
    {
        return new ResIniciarSesion { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
    }
}

public async Task<ResCerrarSesion> CerrarSesion(ReqCerrarSesion req)
{
    try
    {
        // Asegurar que el token esté configurado antes de hacer la petición
        UpdateAuthenticationHeader();

        var json = JsonSerializer.Serialize(req);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("usuarios/cerrar-sesion", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ResCerrarSesion>(responseContent);

        if (result.resultado)
        {
            ClearToken();
        }

        return result;
    }
    catch (Exception ex)
    {
        return new ResCerrarSesion { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
    }
}

public async Task<ResActualizarPerfil> ActualizarPerfil(ReqActualizarPerfil req)
{
    try
    {
        // Asegurar que el token esté configurado
        UpdateAuthenticationHeader();

        var json = JsonSerializer.Serialize(req);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("usuarios/actualizar-perfil", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResActualizarPerfil>(responseContent);
    }
    catch (Exception ex)
    {
        return new ResActualizarPerfil { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
    }
}

public async Task<ResCambiarPassword> CambiarPassword(ReqCambiarPassword req)
{
    try
    {
        // Asegurar que el token esté configurado
        UpdateAuthenticationHeader();

        var json = JsonSerializer.Serialize(req);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("usuarios/cambiar-password", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResCambiarPassword>(responseContent);
    }
    catch (Exception ex)
    {
        return new ResCambiarPassword { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
    }
}

public async Task<ResReenviarCodigoVerificacion> ReenviarCodigoVerificacion(ReqReenviarCodigoVerificacion req)
{
    try
    {
        var json = JsonSerializer.Serialize(req);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("usuarios/reenviar-codigo", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResReenviarCodigoVerificacion>(responseContent);
    }
    catch (Exception ex)
    {
        return new ResReenviarCodigoVerificacion { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
    }
}

public async Task<ResObtenerUsuario> ObtenerUsuario(ReqObtenerUsuario req)
{
    try
    {
        // Asegurar que el token esté configurado
        UpdateAuthenticationHeader();

        var json = JsonSerializer.Serialize(req);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var response = await _httpClient.PostAsync("usuarios/obtener", content);
        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ResObtenerUsuario>(responseContent);
    }
    catch (Exception ex)
    {
        return new ResObtenerUsuario { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
    }
}

        // Nuevos métodos para el módulo de grupos familiares
        public async Task<ResCrearGrupoFamiliar> CrearGrupoFamiliar(ReqCrearGrupoFamiliar req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("grupos", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResCrearGrupoFamiliar>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResCrearGrupoFamiliar { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResInvitarMiembroGrupo> InvitarMiembroGrupo(ReqInvitarMiembroGrupo req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"grupos/{req.GrupoID}/miembros", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResInvitarMiembroGrupo>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResInvitarMiembroGrupo { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResRegistrarGastoCompartido> RegistrarGastoCompartido(ReqRegistrarGastoCompartido req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"grupos/{req.GrupoID}/gastos", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResRegistrarGastoCompartido>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResRegistrarGastoCompartido { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResObtenerBalanceGrupal> ObtenerBalanceGrupal(ReqObtenerBalanceGrupal req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var query = $"grupos/{req.GrupoID}/balances";
                if (req.FechaInicio.HasValue && req.FechaFin.HasValue)
                {
                    query += $"?fechaInicio={req.FechaInicio.Value:yyyy-MM-dd}&fechaFin={req.FechaFin.Value:yyyy-MM-dd}";
                }
                var response = await _httpClient.GetAsync(query);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResObtenerBalanceGrupal>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResObtenerBalanceGrupal { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResSalirGrupo> SalirGrupo(ReqSalirGrupo req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"grupos/{req.GrupoID}/salir", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResSalirGrupo>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResSalirGrupo { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResListarGrupos> ListarGrupos(ReqListarGrupos req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var response = await _httpClient.GetAsync($"grupos?usuarioId={req.UsuarioID}");
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResListarGrupos>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResListarGrupos { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResObtenerDetallesGrupo> ObtenerDetallesGrupo(ReqObtenerDetallesGrupo req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var response = await _httpClient.GetAsync($"grupos/{req.GrupoID}?usuarioId={req.UsuarioID}");
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResObtenerDetallesGrupo>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResObtenerDetallesGrupo { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResEliminarMiembro> EliminarMiembro(ReqEliminarMiembro req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var response = await _httpClient.DeleteAsync($"grupos/{req.GrupoID}/miembros/{req.UsuarioID}?adminUsuarioId={req.AdminUsuarioID}");
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResEliminarMiembro>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResEliminarMiembro { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResActualizarGrupo> ActualizarGrupo(ReqActualizarGrupo req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"grupos/{req.GrupoID}", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResActualizarGrupo>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResActualizarGrupo { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResListarGastos> ListarGastos(ReqListarGastos req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var query = $"grupos/{req.GrupoID}/gastos?usuarioId={req.UsuarioID}";
                if (req.FechaInicio.HasValue && req.FechaFin.HasValue)
                {
                    query += $"&fechaInicio={req.FechaInicio.Value:yyyy-MM-dd}&fechaFin={req.FechaFin.Value:yyyy-MM-dd}";
                }
                var response = await _httpClient.GetAsync(query);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResListarGastos>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResListarGastos { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResEliminarGrupo> EliminarGrupo(ReqEliminarGrupo req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var response = await _httpClient.DeleteAsync($"grupos/{req.GrupoID}?adminUsuarioId={req.AdminUsuarioID}");
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResEliminarGrupo>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResEliminarGrupo { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResActualizarEstadoGasto> ActualizarEstadoGasto(ReqActualizarEstadoGasto req)
        {
            try
            {
                UpdateAuthenticationHeader();
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"gastos/{req.GastoID}/estado", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ResActualizarEstadoGasto>(responseContent);
            }
            catch (Exception ex)
            {
                return new ResActualizarEstadoGasto { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResCrearMeta> CrearMeta(ReqCrearMeta req)
        {
            try
            {
                UpdateAuthenticationHeader();
                Debug.WriteLine($"Enviando POST a metas con Nombre={req.Nombre}, MontoObjetivo={req.MontoObjetivo}");
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("metas", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResCrearMeta>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en CrearMeta: {ex.Message}");
                return new ResCrearMeta { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResActualizarProgresoMeta> ActualizarProgresoMeta(ReqActualizarProgresoMeta req)
        {
            try
            {
                UpdateAuthenticationHeader();
                Debug.WriteLine($"Enviando POST a metas/{req.MetaID}/progreso con MontoActual={req.MontoActual}");
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"metas/{req.MetaID}/progreso", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResActualizarProgresoMeta>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en ActualizarProgresoMeta: {ex.Message}");
                return new ResActualizarProgresoMeta { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResListarMetas> ListarMetas(ReqListarMetas req)
        {
            try
            {
                UpdateAuthenticationHeader();
                Debug.WriteLine($"Enviando GET a metas?usuarioId={req.UsuarioID}");
                var response = await _httpClient.GetAsync($"metas?usuarioId={req.UsuarioID}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResListarMetas>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en ListarMetas: {ex.Message}");
                return new ResListarMetas { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResObtenerDetallesMeta> ObtenerDetallesMeta(ReqObtenerDetallesMeta req)
        {
            try
            {
                UpdateAuthenticationHeader();
                Debug.WriteLine($"Enviando GET a metas/{req.MetaID}?usuarioId={req.UsuarioID}");
                var response = await _httpClient.GetAsync($"metas/{req.MetaID}?usuarioId={req.UsuarioID}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResObtenerDetallesMeta>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en ObtenerDetallesMeta: {ex.Message}");
                return new ResObtenerDetallesMeta { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResActualizarMeta> ActualizarMeta(ReqActualizarMeta req)
        {
            try
            {
                UpdateAuthenticationHeader();
                Debug.WriteLine($"Enviando PUT a metas/{req.MetaID} con Nombre={req.Nombre}, MontoObjetivo={req.MontoObjetivo}");
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"metas/{req.MetaID}", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResActualizarMeta>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en ActualizarMeta: {ex.Message}");
                return new ResActualizarMeta { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResEliminarMeta> EliminarMeta(ReqEliminarMeta req)
        {
            try
            {
                UpdateAuthenticationHeader();
                Debug.WriteLine($"Enviando DELETE a metas/{req.MetaID}?usuarioId={req.UsuarioID}");
                var response = await _httpClient.DeleteAsync($"metas/{req.MetaID}?usuarioId={req.UsuarioID}");
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResEliminarMeta>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en EliminarMeta: {ex.Message}");
                return new ResEliminarMeta { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResAsignarTransaccion> AsignarTransaccion(ReqAsignarTransaccion req)
        {
            try
            {
                UpdateAuthenticationHeader();
                Debug.WriteLine($"Enviando POST a metas/{req.MetaID}/transacciones con TransaccionID={req.TransaccionID}, MontoAsignado={req.MontoAsignado}");
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"metas/{req.MetaID}/transacciones", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResAsignarTransaccion>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en AsignarTransaccion: {ex.Message}");
                return new ResAsignarTransaccion { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResSolicitarCambioPassword> SolicitarCambioPassword(ReqSolicitarCambioPassword req)
        {
            try
            {
                Debug.WriteLine($"Enviando POST a usuarios/solicitar-cambio-password con Email={req.Email}");
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("usuarios/solicitar-cambio-password", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResSolicitarCambioPassword>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en SolicitarCambioPassword: {ex.Message}");
                return new ResSolicitarCambioPassword { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResConfirmarCambioPassword> ConfirmarCambioPassword(ReqConfirmarCambioPassword req)
        {
            try
            {
                Debug.WriteLine($"Enviando POST a usuarios/confirmar-cambio-password con Email={req.Email}");
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("usuarios/confirmar-cambio-password", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResConfirmarCambioPassword>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en ConfirmarCambioPassword: {ex.Message}");
                return new ResConfirmarCambioPassword { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        public async Task<ResIngresarTransaccion> IngresarTransaccion(ReqIngresarTransaccion req)
        {
            try
            {
                UpdateAuthenticationHeader();
                Debug.WriteLine($"Enviando POST a transacciones con titulo={req.Transaccion.Titulo}");
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("transaccion/ingresar", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResIngresarTransaccion>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en CrearMeta: {ex.Message}");
                return new ResIngresarTransaccion { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }

        //public async Task<ResTransaccionesPorUsuario> ListarTransaccionesPorUsuario(ReqTransaccionesPorUsuario req)
        //{
        //    try
        //    {
        //        UpdateAuthenticationHeader();
        //        var json = JsonSerializer.Serialize(req);
        //        var content = new StringContent(json, Encoding.UTF8, "application/json");
        //        var response = await _httpClient.GetAsync("transaccion/transaccionesPorUsuario", content);
        //        var responseContent = await response.Content.ReadAsStringAsync();
        //        Debug.WriteLine($"Respuesta del servidor: {responseContent}");
        //        return JsonSerializer.Deserialize<ResTransaccionesPorUsuario>(responseContent);
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine($"Excepción en Listar Transacciones: {ex.Message}");
        //        return new ResTransaccionesPorUsuario { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
        //    }
        //}

        public async Task<ResActualizarTransaccion> ActualizarTransaccion(ReqActualizarTransaccion req)
        {
            try
            {
                UpdateAuthenticationHeader();
                Debug.WriteLine($"Enviando PUT a transacciones con titulo={req.Titulo}");
                var json = JsonSerializer.Serialize(req);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("transaccion/actualizar", content);
                var responseContent = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Respuesta del servidor: {responseContent}");
                return JsonSerializer.Deserialize<ResActualizarTransaccion>(responseContent);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Excepción en CrearMeta: {ex.Message}");
                return new ResActualizarTransaccion { resultado = false, error = new List<Error> { new Error { Message = ex.Message } } };
            }
        }
    }
}

