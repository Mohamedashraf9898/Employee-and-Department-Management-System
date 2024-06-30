using Demo.DAL.Models;
using System.Net;
using System.Net.Mail;

namespace Demo.PL.Helper
{
    public class EmailSettings
    {
        public static void  SendEmail(Email email)
        {
            var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential("mohamedashraf171998@gmail.com", "rtpligpcuwsfnnzg");
            client.Send("mohamedashraf171998@gmail.com", email.Recipients, email.Subject, email.Body);
        }
    }
}
