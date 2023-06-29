using System;
using System.Net;
using System.Net.Mail;

namespace Yet_Another_Posting_System
{
    internal class MailUtils
    {
        MailAddress fromAddress;
        string fromPassword;
        SmtpClient smtp;

        public MailUtils(string fromAddress, string fromName, string password)
        {
            this.fromAddress = new MailAddress(fromAddress, fromName);
            this.fromPassword = password;

            smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(this.fromAddress.Address, fromPassword)
            };
        }

        public void SendMail(string address, string subject, string body)
        {
            var toAddress = new MailAddress(address);
            try
            {
                using (var messsage = new MailMessage(fromAddress, toAddress) { Subject = subject, Body = body })
                {
                    smtp.Send(messsage);
                }
            }
            catch (Exception e)
            {
                throw new Exception("An error occured sending email: " + e.Message);
            }
        }
    }
}
