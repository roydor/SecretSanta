using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;

namespace SecretSanta
{
    public class SecretSantaMailer
    {
        private const string RESULTS_DIRECTORY = "results";

        private SecretSantaMailSettings _settings;
        private SmtpClient _smtpClient;

        public SecretSantaMailer(string settingsPath)
        {
            if (!File.Exists(settingsPath))
            {
                throw new FileNotFoundException(settingsPath);
            }

            string settingsJson = File.ReadAllText(settingsPath);
            _settings = JsonConvert.DeserializeObject<SecretSantaMailSettings>(settingsJson);

            _smtpClient = new SmtpClient()
            {
                EnableSsl = true,
                Port = 587,
                Host = "smtp.gmail.com",
                Timeout = 10000,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_settings.SantasEmail, _settings.SantasPassword)
            };
        }

        /// <summary>
        /// Writes to a file the same thing that would be mailed out.
        /// </summary>
        public void WriteFile(SecretSantaParticipant gifter, SecretSantaParticipant recipient)
        {
            string path = Path.Combine(RESULTS_DIRECTORY, string.Format("{0}.txt", gifter.Name));
            string content = string.Format(_settings.SantasMessage, gifter.Name, recipient.Name);

            // Replace </br> with new lines, and strip other html.
            content = Regex.Replace(content, "</br>", "\r\n");
            string noHTML = Regex.Replace(content, @"<[^>]+>|&nbsp;", "").Trim();

            File.WriteAllText(path, noHTML);
        }

        /// <summary>
        /// Sends an email to the gifter who their recipient is.
        /// </summary>
        public void SendMail(SecretSantaParticipant gifter, SecretSantaParticipant recipient)
        {
            using (var mailMessage = new MailMessage())
            {
                mailMessage.From = new MailAddress(_settings.SantasEmail);
                mailMessage.To.Add(new MailAddress(gifter.Email));

                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = Encoding.UTF8;

                mailMessage.Subject = string.Format(_settings.SantasSubject, gifter.Name, recipient.Name);
                mailMessage.Body = string.Format(_settings.SantasMessage, gifter.Name, recipient.Name);

                _smtpClient.Send(mailMessage);
            }
        }
    }
}
