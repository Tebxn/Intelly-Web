﻿using Intelly_Web.Entities;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;
using Intelly_Web.Interfaces;
using System.Net;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Data;
using Microsoft.AspNetCore.Http;
using Intelly_Web.Implementations;

namespace Intelly_Web.Models
{
    public class UserModel : IUserModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        private String _urlApi;
        private readonly ITools _tools;


        public UserModel(HttpClient httpClient, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, ITools tools)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _HttpContextAccessor = httpContextAccessor;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;
            _tools = tools;

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
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserEnt>>();
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
                entity.User_Company_Id = long.Parse(_HttpContextAccessor.HttpContext.Session.GetString("UserCompanyId"));

                string url = _urlApi + "/api/Authentication/RegisterAccount";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                //_httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserEnt>>();
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
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<UserEnt>>>(json);  
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

        public async Task<ApiResponse<UserEnt>> GetSpecificUser(long userId)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();
            try
            {
                // Encripta el userId antes de pasarlo en la URL
                string encryptedUserId = _tools.Encrypt(userId.ToString());

                string url = $"{_urlApi}/api/Users/GetSpecificUser/{encryptedUserId}";

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
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

        public async Task<ApiResponse<List<UserRoleEnt>>> GetAllUsersRoles()
        {
            ApiResponse<List<UserRoleEnt>> response = new ApiResponse<List<UserRoleEnt>>();
            try
            {
                string url = _urlApi + "/api/Users/GetAllUsersRoles";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                HttpResponseMessage httpResponse = await _httpClient.GetAsync(url);

                if (httpResponse.IsSuccessStatusCode)
                {
                    string json = await httpResponse.Content.ReadAsStringAsync();
                    response = JsonConvert.DeserializeObject<ApiResponse<List<UserRoleEnt>>>(json);
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

        public async Task<ApiResponse<string>> ActivateAccount(int userId)
        {
            ApiResponse<string> response = new ApiResponse<string>();

            try
            {
                string url = _urlApi + "/api/Authentication/ActivateAccount";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                JsonContent obj = JsonContent.Create(new { User_Id = userId });

                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response.Data = "Account activated";
                }
                else if (httpResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    response.ErrorMessage = "User not found";
                    response.Code = 404;
                }
                else
                {
                    response.ErrorMessage = "Error activating account: " + httpResponse.ReasonPhrase;

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

        public async Task<ApiResponse<UserEnt>> UpdateUserPassword(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/UpdateUserPassword";
                string token = _HttpContextAccessor.HttpContext.Session.GetString("UserToken");
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

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

        public async Task<ApiResponse<UserEnt>> ChangePassword(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/ChangePassword";
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
                    response.ErrorMessage = "Error al conectar con el servidor. Contacte con soporte.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al actualizar usuario: " + ex.Message;
                return response;
            }
        }
        //public async Task<ApiResponse<UserEnt>> DisableSpecificUser(UserEnt entity)
        //{
        //    ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

        //    try
        //    {
        //        string url = _urlApi + "/api/Authentication/DisableSpecificUser";
        //        JsonContent obj = JsonContent.Create(entity);
        //        var httpResponse = await _httpClient.PutAsync(url, obj);

        //        if (httpResponse.IsSuccessStatusCode)
        //        {
        //            response.Success = true;
        //            response.Data = await httpResponse.Content.ReadFromJsonAsync<UserEnt>();
        //            return response;
        //        }
        //        else
        //        {
        //            response.ErrorMessage = "Error al conectar con el servidor. Contacte con soporte.";
        //            return response;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        response.ErrorMessage = "Error inesperado al actualizar usuario: " + ex.Message;
        //        return response;
        //    }
        //}

        public async Task<ApiResponse<UserEnt>> UpdateUserState(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Users/UpdateUserState";
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
                    response.ErrorMessage = "Error al conectar con el servidor. Contacte con soporte.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al actualizar usuario: " + ex.Message;
                return response;
            }
        }

        public async Task<ApiResponse<UserEnt>> UpdateNewPassword(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/UpdateNewPassword";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PutAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al actualizar la contraseña.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al actualizar la contraseña: " + ex.Message;
                return response;
            }

        }

        public async Task<ApiResponse<UserEnt>> PwdRecovery(UserEnt entity)
        {
            ApiResponse<UserEnt> response = new ApiResponse<UserEnt>();

            try
            {
                string url = _urlApi + "/api/Authentication/PwdRecovery";
                JsonContent obj = JsonContent.Create(entity);
                var httpResponse = await _httpClient.PostAsync(url, obj);

                if (httpResponse.IsSuccessStatusCode)
                {
                    response.Success = true;
                    response = await httpResponse.Content.ReadFromJsonAsync<ApiResponse<UserEnt>>();
                    return response;
                }
                else
                {
                    response.ErrorMessage = "Error al restablecer contraseña.";
                    return response;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Error inesperado al restablecer contraseña: " + ex.Message;
                return response;
            }

        }

    }
}