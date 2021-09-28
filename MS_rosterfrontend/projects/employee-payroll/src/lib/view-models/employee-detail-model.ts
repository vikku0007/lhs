import { Time } from '@angular/common';

export interface EmployeeDetailModel {
    description?: string;
    startDate?: Date;
    endDate?: Date;
    startTime?: Time;
    endTime?: Time;
    startTimeString?: string;
    endTimeString?: string;
    checkInDate?: Date;
    checkOutDate?: Date;
    checkOutTime?: Time;
    checkInTime?: Time;
    checkOutTimeString?: string;
    checkInTimeString?: string;
    isLogin?: boolean;
    employeeId?: string;
    shiftId?: string;
    employeeShiftInfoViewModel?: any;
    remark?: string;
}
