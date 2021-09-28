import { EmployeeShiftListViewModel } from './employee-shiftInfo-viewModel';
import { ClientShiftListViewModel } from './client-shiftInfo-viewModel';



export interface ShiftInfoViewModel {
   
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
    
}
