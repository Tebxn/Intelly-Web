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
namespace Intelly_Web.Models
{
    public class MarketingModel : IMarketing 
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;


        public MarketingModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;

            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;

        }

        public async Task<ApiResponse<EmailEnt>> EmailMarketingManual(EmailEnt entity)
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

    }
}
