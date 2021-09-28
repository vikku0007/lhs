using EasyNetQ;
using LHSAPI.Application.Interface;
using LHSAPI.Application.SchedulerService;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Services
{
    public class NativeInjectorServices
    {
        public static void RegisterServices(IServiceCollection services)
        {
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPreProcessorBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestPerformanceBehaviour<,>));
            services.AddTransient<IShiftService, ShiftService>();
            services.AddTransient<ISessionService, SessionService>();
            services.AddTransient<IMessageService, EmailService>();
            services.AddTransient<IMessageSender, EmailMessageSender>();
            services.AddTransient<ISendGridMessageSender, SendgridEmailMessageSender>();
            services.AddTransient<INotification, NotificationService>();
            services.AddHostedService<CheckInReminderService>();
            services.AddHostedService<CheckOutReminderService>();
            services.AddHostedService<EmployeeDocumentReminderService>();

        }
    }
}




