import { EmployeeKinInfo } from '../viewmodel/employee-kin-info';
import { EmployeeMiscInfo } from '../viewmodel/employee-miscinfo';
import { EmployeeAwardInfo } from '../viewmodel/employee-award-info';
import { EmployeeDlModel } from '../viewmodel/employee-dl-model';
import { EmployeeEducationModel } from '../viewmodel/employee-education-model';
import { EmployeeJobProfile } from '../viewmodel/employee-jobprofile';
import { EmployeeWorkModel } from '../viewmodel/employee-work-model';
import { EmployeePayRates } from '../viewmodel/employee-payrates';
import { EmployeeDetails } from '../viewmodel/employee-details';


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