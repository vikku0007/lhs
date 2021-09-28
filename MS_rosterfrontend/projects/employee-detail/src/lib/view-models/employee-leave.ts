export interface EmployeeLeave {
        id?: number;
        employeeId?: number;
        leaveType?: number;
        dateFrom?: string;
        dateTo?: string;
        reasonOfLeave?: string;
}