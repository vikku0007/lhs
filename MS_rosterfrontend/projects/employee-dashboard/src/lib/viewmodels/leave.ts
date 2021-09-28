export interface LeaveInfo {
        id?: number;
        leaveId?: number;
        employeeId?: number;
        leaveType?: number;
        dateFrom?: string;
        dateTo?: string;
        reasonOfLeave?: string;
        isApproved?: boolean;
        isRejected?: boolean;
}