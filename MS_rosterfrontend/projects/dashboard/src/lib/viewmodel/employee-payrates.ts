export interface EmployeePayRates {
    employeeId?: number;
    level?: number;
    monToFri6To12AM?: number;
    sat6To12AM?: number;
    sun6To12AM?: number;
    holiday6To12AM?: number;
    monToFri12To6AM?: number;
    sat12To6AM?: number;
    sun12To6AM?: number;
    holiday12To6AM?: number;
    activeNightsAndSleep?: number;
    houseCleaning?: number;
    transportPetrol?: number;
    levelName?: string;
}