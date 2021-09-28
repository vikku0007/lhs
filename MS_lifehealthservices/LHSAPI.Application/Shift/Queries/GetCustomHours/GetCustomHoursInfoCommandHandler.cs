using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LHSAPI.Application.Shift.Queries.GetCustomHours
{
    public class GetCustomHoursInfoCommandHandler : IRequestHandler<GetCustomHoursInfoCommand, ApiResponse>
    {
        public GetCustomHoursInfoCommandHandler()
        {

        }

        public async Task<ApiResponse> Handle(GetCustomHoursInfoCommand request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var dateList = Enumerable.Range(0, 1 + request.EndDate.Subtract(request.StartDate).Days).Select(offset => request.StartDate.AddDays(offset)).ToArray();
                dateList = dateList.Length > 1 ? dateList.SkipLast(1).ToArray() : dateList;
                double normalHours = 0;
                double activeHours = 0;
                double sleepHours = 0.0;
                int sleepover = 0;
                string responseString = null;
                foreach (var date in dateList)
                {
                    var startDate = date.Date;
                    var endDate = request.StartDate != request.EndDate ? date.Date.AddDays(1) : startDate;
                    var startDateTime = startDate.Add(TimeSpan.Parse(request.StartTime));
                    var endDateTime = (request.EndDate.Date == endDate) ? endDate.Add(TimeSpan.Parse(request.EndTime)) : endDate.Add(TimeSpan.Parse(request.StartTime));
                    var totalDuration = endDateTime.Subtract(startDateTime).TotalHours;
                    if (request.IsActiveNight)
                    {
                        var activeNightHours = CalculateActiveNightHours(startDateTime, endDateTime);
                        var normalHrs = totalDuration - activeNightHours;
                        normalHours += normalHrs;
                        activeHours += activeNightHours;
                    }
                    else
                    {
                        var sleepOverHours = CalculateSleepOverNightHours(startDateTime, endDateTime);
                        var normalHrs = totalDuration - sleepOverHours;
                        normalHours += normalHrs;
                        sleepHours += sleepOverHours;
                        if (sleepHours > 0 && sleepHours <= 8)
                        {
                            sleepover = 1;
                        }
                        else if (sleepHours > 8 && sleepHours <= 16)
                        {
                            sleepover = 2;
                        }
                        else if (sleepHours > 16 && sleepHours <= 24)
                        {
                            sleepover = 3;
                        }
                        else
                        {
                            sleepover = 4;
                        }
                    }
                }
                if (request.IsActiveNight)
                {
                    responseString = "Normal hours : " + normalHours + " hrs and Active hours : " + activeHours + " hrs";
                    response.ResponseData = responseString;
                }
                else
                {
                    responseString = "Normal hours : " + normalHours + " hrs and Sleepover : " + sleepover;
                    response.ResponseData = responseString;
                }
                response.SuccessWithOutMessage(response.ResponseData);
            }
            catch (Exception ex)
            {

            }
            return response;
        }

        private double CalculateActiveNightHours(DateTime StartDateTime, DateTime EndDateTime)
        {
            double returnValue = 0;

            TimeSpan activeNightDuration = TimeSpan.Zero;

            if (EndDateTime > StartDateTime)
            {
                DateTime dt = StartDateTime.Date.AddHours(24);
                DateTime dt1 = EndDateTime.Date.AddHours(6);
                if (StartDateTime < dt && EndDateTime > dt1)
                {
                    TimeSpan timeSpan = new TimeSpan(6, 0, 0);
                    activeNightDuration = timeSpan;
                }
                else if (StartDateTime < dt && EndDateTime < dt1)
                {
                    var time = EndDateTime - dt;
                    activeNightDuration = time;

                }
                else if (StartDateTime > dt && EndDateTime < dt1)
                {
                    var time = EndDateTime - StartDateTime;
                    activeNightDuration = time;
                }
                else if (StartDateTime > dt && EndDateTime > dt1)
                {
                    var time = dt1 - StartDateTime;
                    activeNightDuration = time;
                }
                returnValue = activeNightDuration.TotalHours;
            }
            return returnValue;

        }

        private double CalculateSleepOverNightHours(DateTime StartDateTime, DateTime EndDateTime)
        {
            double returnValue = 0;
            TimeSpan sleepoverDuration = TimeSpan.Zero;

            if (EndDateTime > StartDateTime)
            {
                DateTime dt = StartDateTime.Date.AddHours(22);
                DateTime dt1 = EndDateTime.Date.AddHours(6);
                if (StartDateTime < dt && EndDateTime > dt1)
                {
                    TimeSpan timeSpan = new TimeSpan(8, 0, 0);
                    sleepoverDuration = timeSpan;
                }
                else if (StartDateTime < dt && EndDateTime < dt1)
                {
                    var time = EndDateTime - dt;
                    sleepoverDuration = time;

                }
                else if (StartDateTime > dt && EndDateTime < dt1)
                {
                    var time = EndDateTime - StartDateTime;
                    sleepoverDuration = time;
                }
                else if (StartDateTime > dt && EndDateTime > dt1)
                {
                    var time = dt1 - StartDateTime;
                    sleepoverDuration = time;
                }
                returnValue = sleepoverDuration.TotalHours;
            }
            return returnValue;
        }


    }
}


