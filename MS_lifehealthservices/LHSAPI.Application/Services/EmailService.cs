using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using LHSAPI.Application.Interface;
using LHSAPI.Common.ApiResponse;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace LHSAPI.Application.Services
{
    public class EmailService : IMessageService
    {
        private readonly ISendGridMessageSender _MessageService;
        private IHostingEnvironment _hostingEnvironment;
        private readonly IConfiguration _configuration;
        public EmailService(ISendGridMessageSender MessageService, IHostingEnvironment hostingEnvironment, IConfiguration configuration)
        {

            _MessageService = MessageService;
            _hostingEnvironment = hostingEnvironment;
            _configuration = configuration;
        }

        public void SendingEmails(string toAddress, string subject, string message)
        {

            try
            {
                _MessageService.Send(_configuration.GetSection("SMTP:FromAddress").Value, toAddress, subject, message);

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void SendingEmailsWithCC(string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress)
        {
            try
            {
                _MessageService.Send(_configuration.GetSection("SMTP:FromAddress").Value, toAddress, subject, message, carbonCopyAddress, blindCarbonCopyAddress);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void SendingEmailsAsynWithOutCC(string toAddress, string subject, string message)
        {
            try
            {
                _MessageService.SendAsync(_configuration.GetSection("SMTP:FromAddress").Value, toAddress, subject, message);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void SendingEmailsAsynWithCC(string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress)
        {
            try
            {
                _MessageService.SendAsync(_configuration.GetSection("SMTP:FromAddress").Value, toAddress, subject, message, carbonCopyAddress, blindCarbonCopyAddress);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public void SendingConfirmationEmail(string toAddress, string subject, string message, string link, string carbonCopyAddress, string[] blindCarbonCopyAddress)
        {
            try
            {
                _MessageService.SendAsync(_configuration.GetSection("SMTP:FromAddress").Value, toAddress, subject, message, link, carbonCopyAddress, blindCarbonCopyAddress);
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public string GetEmailTemplate()
        {

            string emailTemplatePath = System.IO.Path.Combine(Environment.CurrentDirectory + "\\wwwroot\\EmailTemplate\\Notification.html");
            string emailBody = File.ReadAllText(emailTemplatePath);
            return emailBody;

        }
        public string GetResetPasswordTemplate()
        {

            string emailTemplatePath = System.IO.Path.Combine(Environment.CurrentDirectory + "\\wwwroot\\EmailTemplate\\ResetPasswordTemplate.html");
            string emailBody = File.ReadAllText(emailTemplatePath);
            return emailBody;

        }
        public string GetForgotPasswordTemplate()
        {

            string emailTemplatePath = System.IO.Path.Combine(Environment.CurrentDirectory + "\\wwwroot\\EmailTemplate\\ForgotPasswordTemplate.html");
            string emailBody = File.ReadAllText(emailTemplatePath);
            return emailBody;

        }
        public string GetConfirmEmailTemplate()
        {

            string emailTemplatePath = System.IO.Path.Combine(Environment.CurrentDirectory + "\\wwwroot\\EmailTemplate\\ConfirmEmailTemplate.html");
            string emailBody = File.ReadAllText(emailTemplatePath);
            return emailBody;

        }
        public string GetShiftTemplate()
        {

            string emailTemplatePath = System.IO.Path.Combine(Environment.CurrentDirectory + "\\wwwroot\\EmailTemplate\\ShiftTemplate.html");
            string emailBody = File.ReadAllText(emailTemplatePath);
            return emailBody;

        }
        public string GetCommunicationTemplate()
        {

            string emailTemplatePath = System.IO.Path.Combine(Environment.CurrentDirectory + "\\wwwroot\\EmailTemplate\\CommunicationTemplate.html");
            string emailBody = File.ReadAllText(emailTemplatePath);
            return emailBody;
        }
        public string GetCheckInEmailTemplate()
        {
            string emailTemplatePath = System.IO.Path.Combine(Environment.CurrentDirectory + "\\wwwroot\\EmailTemplate\\CheckInEmailTemplate.html");
            string emailBody = File.ReadAllText(emailTemplatePath);
            return emailBody;
        }
        public string GetEmployeeDocumentTemplate()
        {
            string emailTemplatePath = System.IO.Path.Combine(Environment.CurrentDirectory + "\\wwwroot\\EmailTemplate\\EmployeeDocumentEmailTemplate.html");
            string emailBody = File.ReadAllText(emailTemplatePath);
            return emailBody;
        }
    }
}
