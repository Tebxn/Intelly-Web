using Intelly_Web.Services.Interfaces;
using System.Net;
using System.Net.Mail;

namespace Intelly_Web.Services
{
    public class EmailService : IEmailService
    {
        public EmailService() {

        }

        public void SendEmail(string email)
        {
            var smtpClient =  new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("prueba4542@gmail.com", "prueba123"),
                EnableSsl = true,
            };

            smtpClient.Send("prueba4542@gmail.com", email, "Recuperar Contrasena", "Su nueva contrasena temporal es: prueba321");
        }
    }
}
