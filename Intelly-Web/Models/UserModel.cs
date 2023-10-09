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

        int IUserModel.AddUser(UserEnt entity)//REGISTRO
        {
            string url = _urlApi + "/api/Authentication/RegisterAccount";
            JsonContent obj = JsonContent.Create(entity);
            var resp = _httpClient.PostAsync(url, obj).Result;
            return resp.Content.ReadFromJsonAsync<int>().Result;
        }

        public async Task<List<UserEnt>> GetAllUsers()
        {
            string url = _urlApi + "/api/Users/GetAllUsers";
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                List<UserEnt> users = JsonConvert.DeserializeObject<List<UserEnt>>(json);
                return users;
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
            catch (Exception ex)
            {
                // Manejo de excepciones, puedes agregar el código necesario aquí
                return null;
            }
        }


    }
}
