export interface EmployeeEducationModel {
    id?: number;
    employeeId?: number;
    institute?: string;
    degree?: string;
    fieldStudy?: string;
    completionDate?: Date;
    additionalNotes?: string;
    qualificationType?:number;
}
