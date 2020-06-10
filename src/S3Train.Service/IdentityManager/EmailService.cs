using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace S3Train.IdentityManager
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            if (message == null)
                return;

            await Task.Factory.StartNew(() => { SendMail(message); });
        }

        void SendMail(IdentityMessage message)
        {
            try
            {
                var senderEmail = new MailAddress("traxuanson456@gmail.com", "Hệ Thống Lưu Trữ");
                var receiverEmail = new MailAddress(message.Destination, "Receiver");
                var password = "Shenlong@1234";

                var smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(senderEmail.Address, password)
                };
                using (var mess = new MailMessage(senderEmail, receiverEmail)
                {
                    Subject = message.Subject,
                    Body = message.Body,
                    IsBodyHtml = true
                })
                {
                    smtp.Send(mess);
                }
            }
            catch (Exception e)
            {
                throw new NotImplementedException(e.Message);
            }
        }
    }
}
