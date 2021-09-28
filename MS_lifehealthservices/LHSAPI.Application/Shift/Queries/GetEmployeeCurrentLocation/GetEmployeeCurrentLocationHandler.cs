using LHSAPI.Common.ApiResponse;
using LHSAPI.Persistence.DbContext;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace LHSAPI.Application.Shift.Queries.GetEmployeeCurrentLocation
{
    public class GetEmployeeCurrentLocationHandler : IRequestHandler<GetEmployeeCurrentLocationQuery, ApiResponse>
    {
        private readonly LHSDbContext _dbContext;
        public GetEmployeeCurrentLocationHandler(LHSDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<ApiResponse> Handle(GetEmployeeCurrentLocationQuery request, CancellationToken cancellationToken)
        {
            ApiResponse response = new ApiResponse();
            try
            {
                var locationList = (from shiftData in _dbContext.ShiftInfo
                                    join location in _dbContext.Location on shiftData.LocationId equals location.LocationId
                                    where shiftData.IsDeleted == false && shiftData.IsActive == true && shiftData.Id == request.ShiftId
                                    && shiftData.LocationId != null
                                    select new
                                    {
                                        Latitude = location.Latitude,
                                        Longitude = location.Longitude,
                                    }).FirstOrDefault();
                var distanceCalculated = distance(request.Latitude, request.Longitude, locationList.Latitude, locationList.Longitude, 'K');
                GeoLocation geoLoc = new GeoLocation();
                geoLoc.DistanceInMeteres = Math.Round(distanceCalculated, 2);
                geoLoc.IsAllowed = geoLoc.DistanceInMeteres <= 100 ? true : false;
                response.ResponseData = geoLoc;
                response.Success(geoLoc);
            }
            catch (Exception ex)
            {
                response.Failed(ex.Message);
            }
            return response;
        }

        private double distance(double lat1, double lon1, double lat2, double lon2, char unit)
        {
            if ((lat1 == lat2) && (lon1 == lon2))
            {
                return 0;
            }
            else
            {
                double theta = lon1 - lon2;
                double dist = Math.Sin(deg2rad(lat1)) * Math.Sin(deg2rad(lat2)) + Math.Cos(deg2rad(lat1)) * Math.Cos(deg2rad(lat2)) * Math.Cos(deg2rad(theta));
                dist = Math.Acos(dist);
                dist = rad2deg(dist);
                dist = dist * 60 * 1.1515;
                if (unit == 'K')
                {
                    dist = (dist * 1.609344) * 1000;
                }
                else if (unit == 'N')
                {
                    dist = dist * 0.8684;
                }
                return (dist);
            }
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts decimal degrees to radians             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double deg2rad(double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        //::  This function converts radians to decimal degrees             :::
        //:::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
        private double rad2deg(double rad)
        {
            return (rad / Math.PI * 180.0);
        }

    }

    public class GeoLocation
    {
        public double DistanceInMeteres { get; set; }
        public bool IsAllowed { get; set; }
    }

}
