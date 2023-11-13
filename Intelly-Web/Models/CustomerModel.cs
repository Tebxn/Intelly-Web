using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace Intelly_Web.Models
{
    public class CustomerModel : ICustomer
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;

        public CustomerModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;

        }
        public async Task<ApiResponse<List<CustomerEnt>>> GetAllCustomers()
        {
            ApiResponse<List<CustomerEnt>> response = new ApiResponse<List<CustomerEnt>>();
            try
            {
                string companyId = _HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId");

                string url = $"{_urlApi}/api/Customer/GetAllCustomers/{companyId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<CustomerEnt>>>(json);
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
