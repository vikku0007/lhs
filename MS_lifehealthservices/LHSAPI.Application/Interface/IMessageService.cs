using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Interface
{
    public interface IMessageService
    {
        void SendingEmails( string toAddress, string subject, string message);
        void SendingEmailsWithCC(string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress);
        void SendingEmailsAsynWithOutCC(string toAddress, string subject, string message);
        void SendingEmailsAsynWithCC(string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress);
        void SendingConfirmationEmail(string toAddress, string subject, string message, string link, string carbonCopyAddress, string[] blindCarbonCopyAddress);
        string GetEmailTemplate();
        string GetResetPasswordTemplate();
        string GetForgotPasswordTemplate();
        string GetConfirmEmailTemplate();
        string GetShiftTemplate();
        string GetCommunicationTemplate();
        string GetCheckInEmailTemplate();
        string GetEmployeeDocumentTemplate();
    }
}
