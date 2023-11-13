using Intelly_Web.Entities;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Intelly_Web.Interfaces;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Data;
using Intelly_Web.Implementations;

namespace Intelly_Web.Models
{
    public class MarketingModel : IMarketing 
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;
        private readonly ITools _tools;


        public MarketingModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITools tools)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _tools = tools;

        }

        public async Task<ApiResponse<List<MarketingCampaignEnt>>> GetAllMarketingCampaigns()
        {
            ApiResponse<List<MarketingCampaignEnt>> response = new ApiResponse<List<MarketingCampaignEnt>>();
            try
            {
                string MarketingCampaign_CompanyId = _HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId");
                string url = $"{_urlApi}/api/EmailMarketing/GetAllMarketingCampaigns/{MarketingCampaign_CompanyId}";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<MarketingCampaignEnt>>>(json);
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

        public async Task<ApiResponse<EmailEnt>> EmailMarketingManual(EmailEnt entity)//revisar
        {
            ApiResponse<EmailEnt> response = new ApiResponse<EmailEnt>();

            try
            {
                string url = _urlApi + "/api/EmailMarketing/EmailMarketingManual";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<EmailEnt>();
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

        public async Task<ApiResponse<MarketingCampaignEnt>> CreateMarketingCampaign(MarketingCampaignEnt entity)
        {
            ApiResponse<MarketingCampaignEnt> response = new ApiResponse<MarketingCampaignEnt>();

            try
            {
                entity.MarketingCampaign_CompanyId = long.Parse(_HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId"));

                string url = _urlApi + "/api/EmailMarketing/CreateCampaign";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<MarketingCampaignEnt>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al crear campaña publicitaria. Verifique los datos.";
                    return response;
                }
            }


            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al crear campaña: " + ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<List<MembershipEnt>>> GetAllMembershipLevels()
        {
            ApiResponse<List<MembershipEnt>> response = new ApiResponse<List<MembershipEnt>>();
            try
            {
                string url = $"{_urlApi}/api/EmailMarketing/GetAllMembershipLevels/";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<MembershipEnt>>>(json);
                    return response;
                }

                response.ErrorMessage = "Error al obtener niveles de membresia del servidor.";
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

        public async Task<ApiResponse<MarketingCampaignEnt>> CreateCampaignEmail(MarketingCampaignEnt entity)
        {
            ApiResponse<MarketingCampaignEnt> response = new ApiResponse<MarketingCampaignEnt>();

            try
            {
                entity.MarketingCampaign_CompanyId = long.Parse(_HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId"));

                string url = _urlApi + "/api/EmailMarketing/CreateCampaignEmail";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<MarketingCampaignEnt>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al enviar los emails publicitarios. Verifique los datos.";
                    return response;
                }
            }


            catch (Exception ex)
            {
                response.ErrorMessage = "Error con el servidor: " + ex.Message;
                return response;
            }
        }
    }
}
