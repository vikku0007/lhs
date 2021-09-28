export interface EmployeeTrainingModel {
    id?: number;
    employeeId?: number;
    mandatoryTraining?: string;
    trainingType?: string;
    startDate?: Date;
    endDate?: Date;
    remarks?: string;
    courseType?: number;
    isAlert?: Boolean;
    otherTraining?: string;
    
}


