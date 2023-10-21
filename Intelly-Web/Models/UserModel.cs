﻿using Intelly_Web.Entities;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Intelly_Web.Interfaces;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace Intelly_Web.Models
{
    public class UserModel : IUserModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
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


        public async Task<ApiResponse<UserEnt>> GetSpecificUser(int UserId)
        {
            // Implementa la lógica para obtener un usuario específico
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();
            try
            {
                string url = $"{_urlApi}/api/Users/GetSpecificUser/{UserId}";
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<UserEnt>>(json);
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

        public async Task<ApiResponse<UserEnt>> EditSpecificUser(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = $"{_urlApi}/api/Users/EditSpecificUser";
                JsonContent obj = JsonContent.Create(entity);

                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<UserEnt>();
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


        public async Task<ApiResponse<List<UserTypeEnt>>> GetAllUsersRoles()
        {
            ApiResponse<List<UserTypeEnt>> response = new ApiResponse<List<UserTypeEnt>>();
            try
            {
                string url = _urlApi + "/api/Users/GetAllUsers";
                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<UserTypeEnt>>>(json);
                    List<UserTypeEnt> listUsers = new List<UserTypeEnt>();
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

        public async Task<ApiResponse<UserEnt>> PwdRecovery(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/RecoverAccount";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<UserEnt>();
                }
                else
                {
                    response.ErrorMessage = "Error al recuperar contraseña. Verifica su email.";
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al recuperar contraseña: " + ex.Message;
            }

            return response;
        }

        public async Task<ApiResponse<UserEnt>> UpdateUserPassword(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/UpdateUserPassword";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = await httpResponse.Content.ReadFromJsonAsync<UserEnt>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al Actualizar Usuario. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al actualizar usuario: " + ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<UserEnt>> GetUser(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/User/GetUser";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<UserEnt>>(json);
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al Consultar Usuario. Verifique los datos.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al consultar usuario: " + ex.Message;
                return response;
            }

        }
    }
}