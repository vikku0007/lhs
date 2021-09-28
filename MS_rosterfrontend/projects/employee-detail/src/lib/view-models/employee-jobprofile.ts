export interface EmployeeJobProfile {
    id?: number;
    employeeId?: number;
    jobDesc?: string;
    departmentId?: number;
    departmentName?: string;
    locationId?: number;
    locationName?: string;
    reportingToId?: number;
    reportingToName?: string;
    dateOfJoining?: string;
    source?: number;
    sourceName?: string;
    workingHoursWeekly?: number;
    distanceTravel?: number;
    locationType?: number;
    otherLocation?:string;
    locationTypeName?:string;

}