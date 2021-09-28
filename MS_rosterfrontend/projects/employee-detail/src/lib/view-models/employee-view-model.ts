import { EmployeeDetails } from './employee-details';
import { EmployeeMiscInfo } from './employee-miscinfo';
import { EmployeeKinInfo } from './employee-kin-info';
import { EmployeeAwardInfo } from './employee-award-info';
import { EmployeeDlModel } from './employee-dl-model';
import { EmployeeEducationModel } from './employee-education-model';
import { EmployeeJobProfile } from './employee-jobprofile';
import { EmployeeWorkModel } from './employee-work-model';
import { EmployeePayRates } from './employee-payrates';


export interface EmployeeViewModel {
    employeePrimaryInfo?: EmployeeDetails;
    employeeMiscInfo?: EmployeeMiscInfo;
    employeeKinInfo?: EmployeeKinInfo;
    employeeAwardInfo?: EmployeeAwardInfo;
    employeePicInfo?: null;
    employeeDrivingLicenseInfo?: EmployeeDlModel;
    employeeEducation?: EmployeeEducationModel[];
    employeeJobProfile?: EmployeeJobProfile;
    employeeWorkExp?: EmployeeWorkModel[];
    employeePayRate?: EmployeePayRates;
}