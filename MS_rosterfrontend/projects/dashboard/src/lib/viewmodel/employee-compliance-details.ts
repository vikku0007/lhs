export interface EmployeeComplianceDetails {
    id?: number
    employeeId?: number;
    documentName?: number;
    documentType?: number;
    documentTypeName?: string;
    issueDate?: Date;
    expiryDate?: Date;
    description?: string;
    hasExpiry?: boolean;
    alert?: boolean;
    isActive?: boolean;
    isDeleted?: boolean;
    deletedById?: string;
    deletedBy?: string;
    deletedDate?: Date;
    createdById?: string;
    createdBy?: string;
    createdDate?: Date;
    updateById?: number;
    updateBy?: string;
    updatedDate?: Date;
    fileName?: string;
}

export interface EmployeeOtherComplianceDetails {
    employeeId?: number;
    otherDocumentName?: number;
    otherDocumentType?: number;
    otherIssueDate?: Date;
    otherExpiryDate?: Date;
    otherDescription?: string;
    otherHasExpiry?: boolean;
    otherAlert?: boolean;
    isActive?: boolean;
    isDeleted?: boolean;
    deletedById?: string;
    deletedBy?: string;
    deletedDate?: Date;
    createdById?: string;
    createdBy?: string;
    createdDate?: Date;
    updateById?: number;
    updateBy?: string;
    updatedDate?: Date;
    otherFileName?: string;
}
