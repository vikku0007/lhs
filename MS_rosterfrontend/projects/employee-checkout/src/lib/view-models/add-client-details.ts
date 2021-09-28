export interface ClientDetails {
    clientId?: number;
    salutation?: string;
    firstName?: string;
    middleName?: string;
    lastName?: string;
    dateOfBirth?: string;
    maritalStatus?: string;
    mobileNo?: string;
    gender?: number;
    emailId?: string;
    address?: string;
    locationId?: number;
    employeeId?: number;
    fullName?: string;
    status?: boolean;
    isProgressNotesSubmitted?: number;
    isToDoListSubmitted?: number;
    isCheckoutCompleted?: boolean;
}

export interface EmployeeShiftToDo {
    Id?: string | number;
    dateTime?: Date;
    shiftTime?: string;
    shiftTypeString?: string;
    shiftTimeString?: string;
}
