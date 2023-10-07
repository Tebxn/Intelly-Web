using Intelly_Web.Entities;

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

        public int AddUser(UserEntity entity)//REGISTRO
        {
            string url = _urlApi + "/api/Authentication/RegisterAccount";
            JsonContent obj = JsonContent.Create(entity);
            var resp = _httpClient.PostAsync(url, obj).Result;
            return resp.Content.ReadFromJsonAsync<int>().Result;
        }
        public UserEntAnswer? GetAllUsers()

        {
            string url = "/api/Usuario/GetAllUsers";
            var resp = _httpClient.PostAsync(url, null).Result;
            return resp.Content.ReadFromJsonAsync<UserEntAnswer>().Result;
        }



        /*

        public void EditUser(UserEntity entity)

        {

        }

        public void DeleteUser(UserEntity entity)

        {

        }*/
        public UserEntity? Login(UserEntity entity)
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
