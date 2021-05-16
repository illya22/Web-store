using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Store.Lib.Abstract;
using Store.Lib.Entities;

namespace Store.Lib.Concrete
{
    public class EmailSettings
    {
        public string MailToAddress = "ilgyha228@gmail.com";
        public string MailFromAddress = "store@example.com";
        public bool UseSsl = true;
        public string Username = "MySmtpUsername";
        public string Password = "MySmtpPassword";
        public string ServerName = "smtp.example.com";
        public int ServerPort = 587;
        public bool WriteAsFile = true;
        public string FileLocation = @"D:\email";
    }


   public  class EmailOrder : IOrder
    {
        private EmailSettings emailSettings;

        public EmailOrder(EmailSettings settings)
        {
            emailSettings = settings;
        }

        public void ProcessOrder(ShoppingBasket shoppingBasket, ShippingDetails shippingDetails)
        {
            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = emailSettings.UseSsl;
                smtpClient.Host = emailSettings.ServerName;
                smtpClient.Port = emailSettings.ServerPort;
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials =
                    new NetworkCredential(emailSettings.Username, emailSettings.Password);

                if(emailSettings.WriteAsFile)
                {
                    smtpClient.DeliveryMethod = SmtpDeliveryMethod
                        .SpecifiedPickupDirectory;
                    smtpClient.PickupDirectoryLocation =
                        emailSettings.FileLocation;
                    smtpClient.EnableSsl = false;
                }

                StringBuilder body = new StringBuilder()
                    .AppendLine("New order checked")
                    .AppendLine("-----")
                    .AppendLine("List of items");

                foreach(var line in shoppingBasket.Lines)
                {
                    var subtotal = line.Part.Price * line.Quantity;
                    body.AppendFormat("{0} x {1} (total: {2:c}", line.Quantity,
                        line.Part.Name, subtotal);
                }

                body.AppendFormat("Total cost: {0:c}", shoppingBasket.Total_Value())
                    .AppendLine("-----")
                    .AppendLine("Delivery")
                    .AppendLine(shippingDetails.Name)
                    .AppendLine(shippingDetails.Line1)
                    .AppendLine(shippingDetails.Line2 ?? "")
                     .AppendLine(shippingDetails.Line3 ?? "")
                     .AppendLine(shippingDetails.City)
                     .AppendLine(shippingDetails.Country)
                     .AppendLine("-----")
                     .AppendFormat("C.O.D.: {0}", shippingDetails.Сash_on_delivery ? "yes" : "no")
                     .AppendFormat("Payment: {0}", shippingDetails.Payment ? "yes" : "no");

                MailMessage mailMessage = new MailMessage(
                    emailSettings.MailFromAddress,
                    emailSettings.MailToAddress,
                    "Order shipped",
                    body.ToString());

                if(emailSettings.WriteAsFile)
                {
                    mailMessage.BodyEncoding = Encoding.UTF8;
                }
                smtpClient.Send(mailMessage);
            }
        }
    }
}
