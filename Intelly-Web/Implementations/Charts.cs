using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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

                string url = $"{_urlApi}/api/Chart/ChartNewCustomersMonth{companyId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        public async Task<ApiResponse<long>> ChartActivesMarketingCampaigns()
        {
            ApiResponse<long> response = new ApiResponse<long>();
            try
            {
                string companyId = _HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId");

                string url = $"{_urlApi}/api/Chart/ChartActivesMarketingCampaigns{companyId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        public async Task<ApiResponse<long>> ChartEmailsSendedMonth()
        {
            ApiResponse<long> response = new ApiResponse<long>();
            try
            {
                string companyId = _HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId");

                string url = $"{_urlApi}/api/Chart/ChartEmailsSendedMonth{companyId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        public async Task<ApiResponse<long>> ChartSellsWithCampaignMonth()
        {
            ApiResponse<long> response = new ApiResponse<long>();
            try
            {
                string companyId = _HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId");

                string url = $"{_urlApi}/api/Chart/ChartSellsWithCampaignMonth{companyId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

        public async Task<ApiResponse<List<ChartEnt>>> ChartSumTotalByMonthActualYear()
        {
            ApiResponse<List<ChartEnt>> response = new ApiResponse<List<ChartEnt>>();
            try
            {
                string companyId = _HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId");

                string url = $"{_urlApi}/api/Chart/ChartSumTotalByMonthActualYear{companyId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<ChartEnt>>>(json);
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

        public async Task<ApiResponse<List<ChartEnt>>> ChartTopCampaignsByTotal()
        {
            ApiResponse<List<ChartEnt>> response = new ApiResponse<List<ChartEnt>>();
            try
            {
                string companyId = _HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId");

                string url = $"{_urlApi}/api/Chart/ChartTopCampaignsByTotal{companyId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<ChartEnt>>>(json);
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
