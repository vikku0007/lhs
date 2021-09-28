using LHSAPI.Application.Interface;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using SendGrid;
using SendGrid.Helpers;
using System.Net.Http;
using System.Net.Mail;
using SendGrid.Helpers.Mail;
using System.Threading;

namespace LHSAPI.Application.Services
{
    public class SendgridEmailMessageSender : ISendGridMessageSender
    {
        private readonly IConfiguration _config;
        private SendGridClient client;
        public SendgridEmailMessageSender(IConfiguration config)
        {
            _config = config;
            GetSmtpCredentails();
        }

        /// <summary>
        /// get Exoandly smtp credentails from config file
        /// </summary>
        public void GetSmtpCredentails()
        {            
            var apiKey = _config.GetSection("SMTP:SendGridApiKey").Value;
            client = new SendGrid.SendGridClient(apiKey);
        }
        public void Send(string fromAddress, string toAddress, string subject, string message)
        {
            Send(fromAddress, toAddress, subject, message, null, null);
        }

        public void Send(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress)
        {
            //var emailMessage = BuildEmailMessage(fromAddress, toAddress, subject, message, carbonCopyAddress, blindCarbonCopyAddress);
            var from = new EmailAddress(fromAddress);
            var to = new EmailAddress(toAddress);
            var emailMessage = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            client.SendEmailAsync(emailMessage);
        }

        public void SendAsync(string fromAddress, string toAddress, string subject, string message)
        {
            SendAsync(fromAddress, toAddress, subject, message, null, null);
        }

        public void SendAsync(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress)
        {
            var from = new EmailAddress(fromAddress);
            var to = new EmailAddress(toAddress);
            var emailMessage = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            SendAsyncEmail(emailMessage);
        }

        public void SendAsync(string fromAddress, string toAddress, string subject, string message, string link, string carbonCopyAddress, string[] blindCarbonCopyAddress)
        {
            var from = new EmailAddress(fromAddress);
            var to = new EmailAddress(toAddress);
            var emailMessage = MailHelper.CreateSingleEmail(from, to, subject, message, message);
            SendAsyncEmail(emailMessage);
        }

        private class AsyncArgs
        {
            public SendGridMessage MailMessage { get; set; }
            public SendGridClient SendGridClient { get; set; }
        }

        /// <summary>
        /// Sends an email asynchronously using the System.Threading.Threadpool mechanism.
        /// The thread that executes the email send is a child of the w3wp process,
        ///     so if the process recycles / dies in the middle of a thread's execution, 
        ///     the thread will abort suddenly (possibly without sending the email).
        /// Since email sends only take a few seconds, this low risk is acceptable.
        ///     the alternative would be to write our own thread pool, which will likely be very time consuming.
        /// </summary>
        private void SendAsyncEmail(SendGridMessage mailMessage)
        {
            var asyncArgs = new AsyncArgs()
            {
                SendGridClient = client,
                MailMessage = mailMessage
            };
            ThreadPool.QueueUserWorkItem(SendAsyncEmailCallBack, asyncArgs);
        }

        private static void SendAsyncEmailCallBack(object state)
        {
            AsyncArgs asyncArgs = state as AsyncArgs;

            asyncArgs.SendGridClient.SendEmailAsync(asyncArgs.MailMessage);

        }


    }
}
