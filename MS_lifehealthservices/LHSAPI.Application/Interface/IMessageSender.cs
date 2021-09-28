using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Interface
{
    public interface IMessageSender
    {
        void Send(string fromAddress, string toAddress, string subject, string message);
        void Send(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress);
        void SendAsync(string fromAddress, string toAddress, string subject, string message);
        void SendAsync(string fromAddress, string toAddress, string subject, string message, string carbonCopyAddress, string[] blindCarbonCopyAddress);
        void SendAsync(string fromAddress, string toAddress, string subject, string message, string link, string carbonCopyAddress, string[] blindCarbonCopyAddress);
        
    }
}
