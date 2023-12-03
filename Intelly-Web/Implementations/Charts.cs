using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Newtonsoft.Json;

namespace Intelly_Web.Implementations
{
    public class Charts : ICharts
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;

        public Charts(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
        }
        public async Task<ApiResponse<long>> ChartNewCustomersMonth()
        {
            ApiResponse<long> response = new ApiResponse<long>();
            try
            {
                string companyId = _HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId");

                string url = $"{_urlApi}/api/Chart/ChartNewCustomersMonth/{companyId}";
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<long>>(json);
                    return response;
                }

                response.ErrorMessage = "Error al cargar grafico.";
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
