using Intelly_Web.Entities;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Intelly_Web.Interfaces;
using System.Net;
using System.Collections.Generic;

namespace Intelly_Web.Models
{
    public class UserModel : IUserModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private String _urlApi;


        public UserModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;

        }

        public async Task<ApiResponse<UserEnt>> Login(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/Login";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<UserEnt>();
                }
                else
                {
                    response.ErrorMessage = "Error al iniciar sesión. Verifica tus credenciales.";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al iniciar sesión: " + ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse<UserEnt>> AddEmployee(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/RegisterAccount";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<UserEnt>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error Registrar Usuario. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al registrar usuario: " + ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<List<UserEnt>>> GetAllUsers()
        {
            ApiResponse<List<UserEnt>> response = new ApiResponse<List<UserEnt>>();
            try
            {
                string url = _urlApi + "/api/Users/GetAllUsers";
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<UserEnt>>>(json);
                    List<UserEnt> listUsers = new List<UserEnt>();
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

    }
}