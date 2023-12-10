using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;

namespace Intelly_Web.Models
{
    public class ProductModel : IProductModel
    {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;
        private readonly ITools _tools;

        public ProductModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITools tools)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _tools = tools;

        }

        public async Task<ApiResponse<ProductEnt>> AddProduct(ProductEnt entity)
        {
            ApiResponse<ProductEnt> response = new ApiResponse<ProductEnt>();

            try
            {
                string url = _urlApi + "/api/Product/AddProduct";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<ProductEnt>();
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

        public async Task<ApiResponse<List<ProductEnt>>> GetAllProducts()
        {
            ApiResponse<List<ProductEnt>> response = new ApiResponse<List<ProductEnt>>();
            try
            {
                string url = _urlApi + "/api/Product/GetAllProducts";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<ProductEnt>>>(json);
                    return response;
                }

                response.ErrorMessage = "Error al obtener productos del API.";
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

        public async Task<ApiResponse<ProductEnt>> GetSpecificProduct(int ProductId)
        {
         
            ApiResponse<ProductEnt> response = new ApiResponse<ProductEnt>();
            try
            {
                string url = $"{_urlApi}/api/Product/GetSpecificProduct/{ProductId}";
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<ProductEnt>>(json);
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

        public async Task<ApiResponse<ProductEnt>> EditSpecificProduct(ProductEnt entity)
        {
            ApiResponse<ProductEnt> response = new ApiResponse<ProductEnt>();

            try
            {
                string url = $"{_urlApi}/api/Product/EditSpecificProduct";
                JsonContent obj = JsonContent.Create(entity);

                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<ProductEnt>();
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
