using Experimental.System.Messaging;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Linq;

namespace CommonLayer.Models
{
    public class MSMQmodel
    {
        MessageQueue message = new MessageQueue();
        public void sendData2Queue(string Token)
        {
            message.Path = @".\private$\Token";
            if (!MessageQueue.Exists(message.Path))
            {
                MessageQueue.Create(message.Path);
                //Exists
            }

            message.Formatter = new XmlMessageFormatter(new Type[] { typeof(string) });
            message.ReceiveCompleted += Message_ReceiveCompleted;

            message.Send(Token);
            message.BeginReceive();
            message.Close();
        }



        private void Message_ReceiveCompleted(object sender, ReceiveCompletedEventArgs e)
        {

            try
            {
                var msg = message.EndReceive(e.AsyncResult);
                string Token = msg.Body.ToString();
                string subject = "forget password token";
                string Body = Token;
                string Jwt = JwtToken(Token);
                var SMTPClient = new SmtpClient("smtp.gmail.com")

                {
                    Port = 587,
                    Credentials = new NetworkCredential("vetribrlabz@gmail.com", "trpvpgzsjcvnxglj"),
                    EnableSsl = true,
                };
                SMTPClient.Send("vetribrlabz@gmail.com", Jwt, subject, Body);
                // Process the logic be sending the message
                //Restart the asynchronous receive operation.
                message.BeginReceive();
            }
            catch (MessageQueueException qexception)
            {
                throw;
            }
        }
        public string JwtToken(string Token)
        {
            var decodedToken = Token;
            var handler = new JwtSecurityTokenHandler();
            var JsonToken = handler.ReadJwtToken((decodedToken));
            var Result = JsonToken.Claims.FirstOrDefault().Value;
            return Result;
        }
    }
}
