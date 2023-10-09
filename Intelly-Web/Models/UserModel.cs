using Intelly_Web.Entities;
using System.Net.Http.Json;
using Newtonsoft.Json;
using System.Text.Json.Serialization;


namespace Intelly_Web.Models
{
    public class UserModel : IUserModel
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private String _urlApi;

        public UserModel(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _urlApi = _configuration.GetSection("Llaves:urlApi").Value;

        }

        int IUserModel.AddUser(UserEntity entity)//REGISTRO
        {
            string url = _urlApi + "/api/Authentication/RegisterAccount";
            JsonContent obj = JsonContent.Create(entity);
            var resp = _httpClient.PostAsync(url, obj).Result;
            return resp.Content.ReadFromJsonAsync<int>().Result;
        }

        /* public List<UserEntity> GetAllUsers()
       {
           string url = "/api/Usuario/GetAllUsers";
           var resp = _httpClient.GetAsync(_urlApi + url).Result;

           if (resp.IsSuccessStatusCode)
           {
               var userEntAnswer = resp.Content.ReadFromJsonAsync<UserEntAnswer>().Result;

               if (userEntAnswer != null && userEntAnswer.Objects != null)
               {
                   return userEntAnswer.Objects;
               }
           }

           return new List<UserEntity>(); // Return an empty list in case of an error
       }

          }*/


        public async Task<List<UserEntity>> GetAllUsers()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("GetAllUsers");

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<UserEntity> users = JsonConvert.DeserializeObject<List<UserEntity>>(json);
                return users;
            }

            throw new Exception("Error al obtener usuarios del API.");
        }

        UserEntity? IUserModel.Login(UserEntity entity)
        {
            string url = _urlApi + "/api/Authentication/Login";
            JsonContent obj = JsonContent.Create(entity);
            var resp = _httpClient.PostAsync(url, obj).Result;

            if (resp.IsSuccessStatusCode)
                return resp.Content.ReadFromJsonAsync<UserEntity>().Result;
            else
                return null;
        }
    }
}
