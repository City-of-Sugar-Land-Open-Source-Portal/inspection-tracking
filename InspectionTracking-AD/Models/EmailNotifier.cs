using System.Text;
using System.Net.Mail;

namespace InspectionTracking_AD.Models
{
    public class EmailNotifier
    {
        public bool SendEmail(string time, IxDetail detail, IEnumerable<string> emails)
        {
            using (var smtp = new SmtpClient())
            {
                smtp.EnableSsl = false;
                smtp.Host = "{SMTP Server}";
                smtp.Port = 25;
                smtp.UseDefaultCredentials = false;

                StringBuilder body = new StringBuilder()
                    .AppendLine(String.Format("Your inspector is on their way and should arrive in approximately {0} minutes.", time))
                    .AppendLine()
                    .Append("Your inspector: ")
                    .AppendLine(detail.Header.UserName)
                    .Append("Permit number: ")
                    .AppendLine(detail.PermitNo)
                    .Append("Type of inspection: ")
                    .AppendLine(detail.InspectionType)
                    .Append("Address: ")
                    .AppendLine(detail.Header.AddressLine)
                    .AppendLine()
                    .AppendLine("----------")
                    .AppendLine("This is an automated message, please do not reply to this e-mail. Please e-mail any questions or concerns to permits@sugarlandtx.gov. Thank you.");
                try
                {
                    foreach (string email in emails)
                    {
                        MailMessage message = new MailMessage(
                            "apps@sugarlandtx.gov",
                            email,
                            "Your Inspector is on their way",
                            body.ToString());
                        smtp.Send(message);
                    }
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
    }
}
