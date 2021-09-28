import { ServiceTypeViewModel } from 'projects/roster/src/lib/viewmodel/servicetype-viewModel';

export interface Shiftinfo {
    id?: number;
    description?: string;
    duration?: number;
    ratio?: string;
    clientCount?: number;
    employeeCount?: number;
    locationId?: number;
    locationName?: string;
    statusName?: string;
    otherLocation?: string;
    locationType?: number;
    statusId?: number;
    isPublished?: boolean;
    startDate?: string;
    endDate?: string;
    startTime?: string;
    endTime?: string;
    startTimeString?: string;
    endTimeString?: string;
    employeeId?: EmployeeShiftInfoViewModel[];
    clientId?: [];
    shiftRepeat?: string;
    days?: number;
    reminder?: boolean;
    employeeShiftInfoViewModel?: EmployeeShiftInfoViewModel[];
    clientShiftInfoViewModel?: ClientShiftInfoViewModel[];
    serviceTypeViewModel?: ServiceTypeViewModel[];
    isActiveNight?: boolean;
    isSleepOver?: boolean;
    shiftRepeatType?: string;
    customDuration?: string;
    adminCheckoutRemark?: string;
    checkoutRemark?: string;
}

export interface EmployeeShiftInfoViewModel {
    id?: number;
    employeeId?: number;
    name?: string;
    isSleepOver?: boolean;
}

export interface ClientShiftInfoViewModel {
    id?: string;
    clientId?: number;
    name?: string;
}
