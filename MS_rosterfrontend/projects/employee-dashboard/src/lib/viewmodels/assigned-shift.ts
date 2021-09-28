export interface AssignedShift {
    id?: number;
    employeeId?: number;
    description?: string;
    startDate?: Date;
    endDate?: Date;
    location?: string;
    startTimeString?: string;
    endTimeString?: string;
    isAccepted?: boolean;
    isRejected?: boolean;
    clientInfo: ClientInfo[];
}

export interface CurrentShift {
    id?: number;
    description?: string;
    startDate?: Date;
    endDate?: Date;
    location?: string;
    startTimeString?: string;
    endTimeString?: string;
    isLogin?: boolean
    clientInfo: ClientInfo[];
    isLoginButtonVisible?: boolean;
    isLogoutButtonVisible?: boolean;
}
export interface ClientInfo {
    clientId?: Number;
    shiftId?: Number;
    clientName?: string;

}

