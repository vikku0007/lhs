export interface CommunicationInfo { 
    id?: number;
    employeeId?: number;
    subject?: string
    assignedTo?: [];
    message?: string;
    createdDate?: Date;
    assignedToName?:string;
    
}
export interface CommRecepientmodel { 
    id?: number;
    employeeId?: number;
    CommunicationId?: number
    assignedToName?:string;
    
}