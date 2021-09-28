export interface EmployeeDlModel {
    id?: number;
    employeeId?: number;
    drivingLicense?: boolean;
    carInsurance?: boolean;
    carRegExpiryDate?: Date;
    carRegNo?: string;
    licenseType?: string;
    licenseState?: string;
    licenseNo?: string;
    licenseExpiryDate?: Date;
    insuranceExpiryDate?: string;
    licenseTypeName?: string;
    licenseStateName?: string;
    licenseOrigin?: string;
}
