using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace Yet_Another_Posting_System.Utils
{
    internal class MailUtils
    {
        MailAddress fromAddress;
        string fromPassword;
        SmtpClient smtp;

        public MailUtils(string fromAddress, string fromName, string password)
        {
            this.fromAddress = new MailAddress(fromAddress, fromName);
            fromPassword = password;

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

        public MailUtils(string credsFile)
        {
            List<string> credentials = new List<string>();
            using (StreamReader stream = new StreamReader(credsFile))
            {
                string? line;
                while ((line = stream.ReadLine()) != null)
                {
                    credentials.Add(line.Trim());
                }
            }

            fromAddress = new MailAddress(credentials[0], credentials[1]);
            fromPassword = credentials[2];

            smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
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
