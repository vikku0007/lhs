export interface MedicalHistory {
    clientId?: number;
    name?: string;
    mobileNo?: string;
    gender?: string;
    checkCondition?: string;
    checkSymptoms?: string;
    isTakingMedication?: boolean;
    isMedicationAllergy?: boolean;
    isTakingTobacco?: boolean;
    isTakingIllegalDrug?: boolean;
    takingAlcohol?: string;
    medicationDetail?: string;
    otherCondition?: string;
    otherSymptoms?: string;
    shiftId?: number;
    employeeId?: number;
}
