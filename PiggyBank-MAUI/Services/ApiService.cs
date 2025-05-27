using PiggyBank_MAUI.Models;
using System;
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
        private const string BaseUrl = "https://592e-152-231-161-126.ngrok-free.app/api/"; // Reemplaza con la URL de tu backend
        private string _token;

        public ApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public void SetToken(string token)
        {
            _token = token;
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        public void ClearToken()
        {
            _token = null;
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
    }
}