using Microsoft.AspNetCore.Mvc;
using SentMail.Models;
using System.Diagnostics;
using System.Net.Mail;

namespace SentMail.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult SendEmail(EmailEntity objEmailParameters,IFormFile PostedFile)
        {
            var myAppConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

            var Username = myAppConfig.GetValue<string>("EmailConfig:Username");
            var Password = myAppConfig.GetValue<string>("EmailConfig:Password");
            var Host = myAppConfig.GetValue<string>("EmailConfig:Host");
            var Port = myAppConfig.GetValue<int>("EmailConfig:Port");
            var FromEmail = myAppConfig.GetValue<string>("EmailConfig:FromEmail");

            MailMessage message = new MailMessage();
            message.From = new MailAddress(FromEmail);
            message.To.Add(objEmailParameters.ToEmailAddress.ToString());
            if (!string.IsNullOrEmpty(objEmailParameters.CCAddress))
            {
                message.CC.Add(objEmailParameters.CCAddress);
            }
            if (!string.IsNullOrEmpty(objEmailParameters.Subject))
            {
                message.Subject = objEmailParameters.Subject;
            }
            if (!string.IsNullOrEmpty(objEmailParameters.EmailBodyMessage))
            {
                message.Body = objEmailParameters.EmailBodyMessage;
            }
            message.IsBodyHtml = true;
            if (!string.IsNullOrEmpty(objEmailParameters.AttachmentURL))
            {
                message.Attachments.Add(new Attachment(PostedFile.OpenReadStream(),
                    PostedFile.FileName));
            }
            
            SmtpClient smtpClient = new SmtpClient(Host);

            try
            {
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new System.Net.NetworkCredential(Username, Password);
                smtpClient.Host = Host;
                smtpClient.Port = Port;
                smtpClient.EnableSsl = true;
                // Gửi email một lần nhưng gửi cho 10 người nhận cùng một lúc
                for (int i = 0; i < 10; i++)
                {
                    smtpClient.Send(message);
                    // Đặt một khoảng thời gian chờ giữa các lần gửi để tránh quá tải máy chủ email
                    System.Threading.Thread.Sleep(1000); // Chờ 1 giây trước khi gửi email tiếp theo
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                message.Dispose();
                smtpClient.Dispose();
            }
            return View("Index");
        }
    }
}