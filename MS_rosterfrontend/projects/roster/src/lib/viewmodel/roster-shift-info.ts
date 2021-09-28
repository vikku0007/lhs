
export interface ShiftInfo {
    description?: string;
    duration? : number;
    ratio? : string;
   
    locationId?: number;
  
    otherLocation?: string;

    statusId?: number;

    isPublished?: boolean;

    startDate?: string;

    endDate?: string;

    startTime?: string;

    endTime?: string;

    allowances?: string;

    mileage?: number;
    employeeId?: [];
    serviceTypeId?: [];
    clientId?: [];
    expense?: number;
    reminder?: boolean;
    
}
