export interface ShiftInfoViewModel {
    id?: number;
    clientName?: string;
    location?: string;
    startDate?: Date;
    endDate?: Date;
    startTimeString?: string;
    endTimeString?: string;
    employeeName?: string;
    description?: String;
    employeeCount?: number;
    clientCount?: number;
}

export interface ShiftDetailViewModel {
    id?: number;
    description?: string;
    duration? : number;
    ratio? : string;
    clientCount? : number;
    employeeCount? : number;
    locationId?: number;
    locationName?: string;
    statusName ?: string;
    otherLocation?: string;
    locationType? : number;
    statusId?: number; 
    isPublished?: boolean;
    startDate?: string;
    endDate?: string;
    startTime?: string;
    endTime?: string;
    startTimeString?: string;
    endTimeString?: string;
    employeeId?: EmployeeShiftListViewModel [];
    clientId?: [];
    serviceTypeId?: [];
    shiftRepeat?: string;
    days ?: number;
    reminder?: boolean;
    employeeShiftInfoViewModel? : EmployeeShiftListViewModel [];
    clientShiftInfoViewModel? : ClientShiftListViewModel []; 
    customDuration?:string;   
}

export interface ClientShiftListViewModel {
    id?: string;
    clientId? : number;
    name? : string;
}

export interface EmployeeShiftListViewModel {
    id?: number;
    employeeId? : number;
    name? : string;
    isSleepOver?: boolean;
}
