//using CapstoneTraineeManagement.DTO;
//using CapstoneTraineeManagement.Interfaces;
//using CapstoneTraineeManagement.Models;
//using Microsoft.Extensions.Options;
//using System.Net;
//using System.Net.Mail;
//using System.Threading.Tasks;

//namespace CapstoneTraineeManagement.Services
//{
//    public class EmailService : IEmailService
//    {
//        private readonly SmtpSettings _smtpSettings;

//        public EmailService(IOptions<SmtpSettings> smtpSettings)
//        {
//            _smtpSettings = smtpSettings.Value;
//        }

//        public async Task SendEnrollmentConfirmationAsync(Trainee trainee, DTO.Program program)
//        {
//            string subject = $"You're Enrolled: Welcome to {program.Name}";

//            string body = $@"
//                Hello {trainee.FullName},<br><br>
//                Congratulations! Your enrollment in the course has been successfully confirmed. Please find the program details below: 
//                <ul>
//                    <li><strong>Program Name:</strong> {program.Name}</li>
//                    <li><strong>Program Category:</strong> {program.CategoryLookUp?.ValueCode}</li>
//                    <li><strong>Duration:</strong> {program.Duration}</li>
//                    <li><strong>Mode:</strong> {program.ModeLookUp?.ValueCode}</li>
//                </ul>
//                We’re excited to have you join us and look forward to supporting you on your learning journey.<br><br>
//                Additional details and access instructions will be shared with you shortly.<br><br>
//                If you have any questions in the meantime, feel free to reach out to us.<br><br>
//                Best regards,<br>
//                {_smtpSettings.FromName}";

//            var mailMessage = new MailMessage
//            {
//                From = new MailAddress(_smtpSettings.Username, _smtpSettings.FromName),
//                Subject = subject,
//                Body = body,
//                IsBodyHtml = true,
//            };
//            mailMessage.To.Add(new MailAddress(trainee.Email, trainee.FullName));

//            using (var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
//            {
//                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
//                smtpClient.EnableSsl = _smtpSettings.EnableSsl;
//                await smtpClient.SendMailAsync(mailMessage);
//            }
//        }
//    }
//}


using CapstoneTraineeManagement.DTO;
using CapstoneTraineeManagement.Interfaces;
using CapstoneTraineeManagement.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Reflection; // <-- Add this
using System.Resources;   // <-- Add this
using System.Threading.Tasks;

namespace CapstoneTraineeManagement.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;

        // The constructor NO LONGER asks for IStringLocalizer
        public EmailService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendEnrollmentConfirmationAsync(Trainee trainee, DTO.Program program)
        {
            // 1. We create a ResourceManager and tell it exactly where to find our file.
            var resourceManager = new ResourceManager("CapstoneTraineeManagement.Resources.EmailTemplates", Assembly.GetExecutingAssembly());

            // 2. We get the strings using the ResourceManager.
            string subjectTemplate = resourceManager.GetString("EnrollmentSubject");
            string bodyTemplate = resourceManager.GetString("EnrollmentBody");

            string subject = string.Format(subjectTemplate, program.Name);
            string body = string.Format(bodyTemplate,
                trainee.FullName,
                program.Name,
                program.CategoryLookUp?.ValueCode,
                program.Duration,
                program.ModeLookUp?.ValueCode);

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.Username, _smtpSettings.FromName),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(new MailAddress(trainee.Email, trainee.FullName));

            using (var smtpClient = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
            {
                smtpClient.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
                smtpClient.EnableSsl = _smtpSettings.EnableSsl;
                await smtpClient.SendMailAsync(mailMessage);
            }
        }
    }
}