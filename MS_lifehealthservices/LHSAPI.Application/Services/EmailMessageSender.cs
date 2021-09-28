using LHSAPI.Application.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading;

namespace LHSAPI.Application.Services
{
    public class EmailMessageSender : IMessageSender
    {
        private readonly IConfiguration _config;
        private SmtpClient smtpClient;
        public EmailMessageSender(IConfiguration config)
        {
            _config = config;
            GetSmtpCredentails();
        }

        /// <summary>
        /// get Exoandly smtp credentails from config file
        /// </summary>
        public void GetSmtpCredentails()
        {
            smtpClient = new SmtpClient();
            smtpClient.Host = _config.GetSection("SMTP:MailServer").Value;
            smtpClient.Port = Convert.ToInt32(_config.GetSection("SMTP:MailServerPort").Value);
            smtpClient.Credentials = new System.Net.NetworkCredential(_config.GetSection("SMTP:FromAddress").Value, _config.GetSection("SMTP:FromAddresspassword").Value);
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = Convert.ToBoolean(_config.GetSection("SMTP:EnableSsl").Value);
        }

        /// <summary>
        /// method for sending emails without cc or bcc
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void Send(string fromAddress, string toAddress, string subject, string message)
        {
            Send(fromAddress, toAddress, subject, message, null, null);
        }

        /// <summary>
        /// method for sending emails with cc or bcc
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="carbonCopyAddress"></param>
        /// <param name="blindCarbonCopyAddress"></param>
        public void Send(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress)
        {
            var emailMessage = BuildEmailMessage(fromAddress, toAddress, subject, message, carbonCopyAddress, blindCarbonCopyAddress);
            smtpClient.Send(emailMessage);
        }

        /// <summary>
        /// method for sending emails asynchronously without cc or bcc
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        public void SendAsync(string fromAddress, string toAddress, string subject, string message)
        {
            SendAsync(fromAddress, toAddress, subject, message, null, null);
        }

        /// <summary>
        /// method for sending emails asynchronously with cc or bcc
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="carbonCopyAddress"></param>
        /// <param name="blindCarbonCopyAddress"></param>
        public void SendAsync(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress)
        {
            var emailMessage = BuildEmailMessage(fromAddress, toAddress, subject, message, carbonCopyAddress, blindCarbonCopyAddress);

            SendAsyncEmail(emailMessage);
        }
        /// <summary>
        /// method for sending Confirmation emails asynchronously with cc or bcc
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="link"></param>
        /// <param name="carbonCopyAddress"></param>
        /// <param name="blindCarbonCopyAddress"></param>
        public void SendAsync(string fromAddress, string toAddress, string subject, string message, string link, string carbonCopyAddress, string[] blindCarbonCopyAddress)
        {
            var emailMessage = BuildEmailMessage(fromAddress, toAddress, subject, message, carbonCopyAddress, blindCarbonCopyAddress);

            SendAsyncEmail(emailMessage);
        }

        /// <summary>
        /// core method to build email message
        /// </summary>
        /// <param name="fromAddress"></param>
        /// <param name="toAddress"></param>
        /// <param name="subject"></param>
        /// <param name="message"></param>
        /// <param name="carbonCopyAddress"></param>
        /// <param name="blindCarbonCopyAddress"></param>
        /// <returns></returns>
        private static MailMessage BuildEmailMessage(string fromAddress, string toAddress, string subject, string message, 
            string carbonCopyAddress , string[] blindCarbonCopyAddress)
        {
            var emailMessage = new MailMessage(fromAddress, toAddress);
            if (!string.IsNullOrWhiteSpace(carbonCopyAddress) && !string.IsNullOrEmpty(carbonCopyAddress))
               emailMessage.Bcc.Add(carbonCopyAddress);
            if (blindCarbonCopyAddress != null)
            {
                foreach (var copy in blindCarbonCopyAddress)
                {
                    emailMessage.CC.Add(copy);
                }
            }
             ////if (!string.IsNullOrWhiteSpace(blindCarbonCopyAddress) && !string.IsNullOrEmpty(blindCarbonCopyAddress))
            ////    emailMessage.Bcc.Add(blindCarbonCopyAddress);
            emailMessage.Subject = subject;
            emailMessage.Body = message;
            emailMessage.IsBodyHtml = true;
            return emailMessage;
        }

        private class AsyncArgs
        {
            public MailMessage MailMessage { get; set; }
            public SmtpClient SmtpClient { get; set; }
        }

        /// <summary>
        /// Sends an email asynchronously using the System.Threading.Threadpool mechanism.
        /// The thread that executes the email send is a child of the w3wp process,
        ///     so if the process recycles / dies in the middle of a thread's execution, 
        ///     the thread will abort suddenly (possibly without sending the email).
        /// Since email sends only take a few seconds, this low risk is acceptable.
        ///     the alternative would be to write our own thread pool, which will likely be very time consuming.
        /// </summary>
        private void SendAsyncEmail(MailMessage mailMessage)
        {
            var asyncArgs = new AsyncArgs()
            {
                SmtpClient = smtpClient,
                MailMessage = mailMessage
            };
            ThreadPool.QueueUserWorkItem(SendAsyncEmailCallBack, asyncArgs);
        }

        private static void SendAsyncEmailCallBack(object state)
        {
            AsyncArgs asyncArgs = state as AsyncArgs;
            
            asyncArgs.SmtpClient.Send(asyncArgs.MailMessage);
            
        }

        

    }
}
