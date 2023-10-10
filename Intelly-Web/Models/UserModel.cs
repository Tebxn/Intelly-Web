﻿using Intelly_Web.Entities;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Intelly_Web.Services.Interfaces;


namespace Intelly_Web.Models
{
    public class UserModel : IUserModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private String _urlApi;
        private IEmailService _emailService;

        public UserModel(HttpClient httpClient, IConfiguration configuration, IEmailService emailService)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _emailService = emailService;

        }

        string? IUserModel.AddEmployee(UserEnt entity)//REGISTRO
        {
            string url = _urlApi + "/api/Authentication/RegisterAccount";
            JsonContent obj = JsonContent.Create(entity);
            var resp = _httpClient.PostAsync(url, obj).Result;
            return resp.Content.ReadAsStringAsync().Result;
        }

        public async Task<List<UserEnt>?> GetAllUsers()
        {
            string url = _urlApi + "/api/Users/GetAllUsers";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<UserEnt>>(json);
            }

            throw new Exception("Error al obtener usuarios del API.");
        }

        public async Task<UserEnt?> Login(UserEnt entidad)
        {
            try
            {
                string url = _urlApi + "/api/Authentication/Login";
                JsonContent obj = JsonContent.Create(entidad);
                var response = await _httpClient.PostAsync(url, obj);

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<UserEnt>();
                else
                    return null;
            }
            catch (Exception)
            {
                // Manejo de excepciones, puedes agregar el código necesario aquí
                return null;
            }
        }

        public void SendEmail(string email)
        {
            _emailService.SendEmail(email);
        }
    }
}