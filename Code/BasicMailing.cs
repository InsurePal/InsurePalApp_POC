using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Configuration;
using System.Net.Mail;
using System.Web;

namespace SupportFriends.Code
{
    public static class BasicMailing
    {
        public static bool SendEmail(string emailTo, string emailCc, string subject, string body)
        {
            bool send = true;
            try
            {
                MailMessage mail = new MailMessage();

                string strFrom = "sebastjan.burnar@neolab.si";
                if (ConfigurationManager.AppSettings["MailFrom"] != null)
                {
                    strFrom = ConfigurationManager.AppSettings["MailFrom"];
                }


                mail.To.Clear();
                mail.CC.Clear();
                mail.Bcc.Clear();

                //mail to added friend
                mail.To.Add(emailTo);

                if (emailCc != String.Empty)
                {
                    mail.CC.Add(emailCc);
                }

                mail.From = new MailAddress(strFrom, strFrom);
                mail.Subject = subject;
                mail.IsBodyHtml = true;

                //mail body
                mail.IsBodyHtml = true;
                mail.Body = body;

                //---send
                SmtpClient smtp = new SmtpClient();

                //MailSettingsSectionGroup settings2 = (MailSettingsSectionGroup)ConfigurationManager.GetSection("mailSettings");

                //System.Configuration.Configuration config = WebConfigurationManager.OpenWebConfiguration(HttpContext.Current.Request.ApplicationPath);
                //MailSettingsSectionGroup settings = (MailSettingsSectionGroup)config.GetSectionGroup("system.net/mailSettings");

                SmtpSection settings = (SmtpSection)ConfigurationManager.GetSection("system.net/mailSettings/smtp");

                smtp.Host = settings.Network.Host;
                smtp.Port = settings.Network.Port;

                smtp.Send(mail);

                mail.To.Clear();
            }
            catch (Exception exc)
            {
                send = false;

            }

            return send;
        }
    }
}