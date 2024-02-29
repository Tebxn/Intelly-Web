using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Intelly_Web.Models
{
    public class LocalModel : ILocalModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;
        private readonly ITools _tools;


        public LocalModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITools tools)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _tools = tools;

        }

        public async Task<ApiResponse<List<LocalEnt>>> GetAllLocals()
        {
            ApiResponse<List<LocalEnt>> response = new ApiResponse<List<LocalEnt>>();
            try
            {
                string LocalId = _HttpContextAccessor.HttpContext.Session.GetString("LocalId");

                string url = $"{_urlApi}/api/Local/GetAllLocals/{LocalId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<LocalEnt>>>(json);
                    return response;
                }

                response.ErrorMessage = "Error al obtener usuarios del API.";
                response.Code = (int)httpResponse.StatusCode;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Unexpected Error: " + ex.Message;
                response.Code = 500;
                return response;
            }
        }

        public async Task<ApiResponse<LocalEnt>> CreateLocal(LocalEnt entity)
        {
            ApiResponse<LocalEnt> response = new ApiResponse<LocalEnt>();

            try
            {
                string url = _urlApi + "/api/Local/CreateLocal";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<LocalEnt>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error Agregar Compañía. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al agregar compañía: " + ex.Message;
                return response;
            }
        }
        public async Task<ApiResponse<LocalEnt>> GetSpecificLocal(int LocalId)
        {
            ApiResponse<LocalEnt> response = new ApiResponse<LocalEnt>();
            try
            {
                string url = $"{_urlApi}/api/Local/GetSpecificLocal/{LocalId}";
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<LocalEnt>>(json);
                    return response;
                }

                response.ErrorMessage = "Error al obtener el local del API.";
                response.Code = (int)httpResponse.StatusCode;
                return response;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Unexpected Error: " + ex.Message;
                response.Code = 500;
                return response;
            }
        }

        public async Task<ApiResponse<LocalEnt>> EditSpecificLocal(LocalEnt entity)
        {
            ApiResponse<LocalEnt> response = new ApiResponse<LocalEnt>();

            try
            {
                string url = $"{_urlApi}/api/Local/EditSpecificLocal";
                JsonContent obj = JsonContent.Create(entity);

                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<LocalEnt>();
                }
                else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    response.ErrorMessage = "User not found";
                    response.Code = 404;
                }
                else
                {
                    response.ErrorMessage = "Unexpected Error: " + httpResponse.ReasonPhrase;
                    response.Code = (int)httpResponse.StatusCode;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Unexpected Error: " + ex.Message;
                response.Code = 500;
            }

            return response;
        }
        public async Task<ApiResponse<LocalEnt>> UpdateLocalState(LocalEnt entity)
        {
            ApiResponse<LocalEnt> response = new ApiResponse<LocalEnt>();

            try
            {
                string url = _urlApi  +"/api/Local/UpdateLocalState";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<LocalEnt>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al conectar con el servidor. Contacte con soporte.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Unexpected Error: " + ex.Message;
                response.Code = 500;
            }

            return response;
        }
    }
}
