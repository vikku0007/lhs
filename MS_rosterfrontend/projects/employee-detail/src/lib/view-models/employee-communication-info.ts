export interface EmployeeCommunicationInfo { 
    id?: number;
    employeeId?: number;
    subject?: string
    assignedTo?: [];
    message?: string;
    createdDate?: Date;
    assignedToName?:string;
    
}
export interface CommunicationRecepientmodel { 
    id?: number;
    employeeId?: number;
    CommunicationId?: number
    assignedToName?:string;
    
}