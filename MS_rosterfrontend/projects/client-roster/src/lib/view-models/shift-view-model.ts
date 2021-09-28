export interface ShiftViewModel {
    id?: number;
    startDate?: Date;
    endDate?: Date;
    description?: string;
    startTime?: string;
    endTime?: string;
    location?: string;
    startTimeString?: string;
    endTimeString?: string;
    statusName?: string;
}


export interface TempShiftViewModel {
    id?: number;
    start?: Date;
    end?: Date;
    text?: string;
    startTime?: string;
    endTime?: string;
    location?: string;
    statusName?: string;
}