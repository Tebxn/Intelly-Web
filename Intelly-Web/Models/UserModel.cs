using Intelly_Web.Entities;
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

        public int AddUser(UserEntity entity)//REGISTRO
        {
            string url = _urlApi + "/api/Authentication/RegisterAccount";
            JsonContent obj = JsonContent.Create(entity);
            var resp = _httpClient.PostAsync(url, obj).Result;
            return resp.Content.ReadFromJsonAsync<int>().Result;
        }
        public List<UserEntity> GetAllUsers()
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

        public void SendEmail(string email)
        {
            _emailService.SendEmail(email);
        }

    }
}
