﻿using Intelly_Web.Entities;
using Intelly_Web.Interfaces;
using Newtonsoft.Json;
using System.Net.Http;

namespace Intelly_Web.Models
{
    public class CompanyModel : ICompanyModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private String _urlApi;

        public CompanyModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;

        }
        public async Task<ApiResponse<List<CompanyEnt>>> GetAllCompanies()
        {
            ApiResponse<List<CompanyEnt>> response = new ApiResponse<List<CompanyEnt>>();
            try
            {
                string url = _urlApi + "/api/Companies/GetAllCompanies";
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<CompanyEnt>>>(json);
                    List<CompanyEnt> listCompanies = new List<CompanyEnt>();
                    return response;
                }

                response.ErrorMessage = "Error al obtener compañías del API.";
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

    }
}
