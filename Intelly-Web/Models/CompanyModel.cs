using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.ComponentModel.Design;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Intelly_Web.Models
{
    public class CompanyModel : ICompanyModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;

        public CompanyModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;

        }
        public async Task<ApiResponse<List<CompanyEnt>>> GetAllCompanies()
        {
            ApiResponse<List<CompanyEnt>> response = new ApiResponse<List<CompanyEnt>>();
            try
            {
                string url = _urlApi + "/api/Companies/GetAllCompanies";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<CompanyEnt>>>(json);
                    List<UserEnt> listCompanies = new List<UserEnt>();
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
        public async Task<ApiResponse<CompanyEnt>> CreateCompany(CompanyEnt entity)
        {
            ApiResponse<CompanyEnt> response = new ApiResponse<CompanyEnt>();

            try
            {
                string url = _urlApi + "/api/Companies/CreateCompany";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<CompanyEnt>();
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
       public async Task<ApiResponse<CompanyEnt>> GetSpecificCompany(int CompanyId)
          {
        // Implementa la lógica para obtener una empresa en específico
           ApiResponse<CompanyEnt> response = new ApiResponse<CompanyEnt>();
          try
          {
           string url = $"{_urlApi}/api/Companies/GetSpecificCompany/{CompanyId}";
        HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);
        
          if (httpResponse.IsSuccessStatusCode)
          {
             string json = await httpResponse.Content.ReadAsStringAsync();
             response = JsonConvert.DeserializeObject<ApiResponse<CompanyEnt>>(json);
            return response;
          }

           response.ErrorMessage = "Error al obtener el usuario del API.";
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

        public async Task<ApiResponse<CompanyEnt>> EditSpecificCompany(CompanyEnt entity)
        {
            ApiResponse<CompanyEnt> response = new ApiResponse<CompanyEnt>();

            try
            {
                string url = $"{_urlApi}/api/Companies/EditSpecificCompany";
                JsonContent obj = JsonContent.Create(entity);

                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<CompanyEnt>();
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


    }
}
