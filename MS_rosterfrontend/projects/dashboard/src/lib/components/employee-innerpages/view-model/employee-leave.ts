export interface EmployeeLeave {
        id?: number;
        employeeId?: number;
        leaveType?: number;
        dateFrom?: string;
        dateTo?: string;
        reasonOfLeave?: string;
        isApproved?: boolean;
        isRejected?: boolean;
        Id?: number;
}