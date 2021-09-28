import { ClientShiftInfoViewModel } from './client-shiftInfo-viewModel';
import { EmployeeShiftInfoViewModel } from './employee-shiftInfo-viewModel';
import { ServiceTypeViewModel } from './servicetype-viewModel';

export interface ShiftInfoViewModel {

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
    serviceTypeId?: [];
    shiftRepeat?: string;
    days?: number;
    reminder?: boolean;
    employeeShiftInfoViewModel?: EmployeeShiftInfoViewModel[];
    clientShiftInfoViewModel?: ClientShiftInfoViewModel[];
    serviceTypeViewModel?: ServiceTypeViewModel[];
    name?: string;
    shiftRepeatType?: string;
    isSleepOver?: boolean;
    isActiveNight?: boolean;
    isShiftCompleted?: boolean;
    remark?: string;
    customDuration?: string;
    adminCheckoutRemark?: string;
    checkoutRemark?: string;
}


export interface CalendarShiftInfoViewModel {
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
    employeeId?: number;
    clientId?: [];
    serviceTypeId?: [];
    shiftRepeat?: string;
    days?: number;
    reminder?: boolean;
    employeeShiftInfoViewModel?: EmployeeShiftInfoViewModel[];
    clientShiftInfoViewModel?: ClientShiftInfoViewModel[];
    serviceTypeViewModel?: ServiceTypeViewModel[];
    name?: string;
    isShiftCompleted?: boolean;
    customDuration?: string;
}