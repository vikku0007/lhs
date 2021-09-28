export interface AccidentIncidentDetails {
    Id?: string;
    eventType?: string;
    locationId?: number;
    raisedBy?: number;
    reportedTo?: number;
    accidentDate?: Date;
    briefDescription?: string;
    detailedDescription?: string;
    eventTypeName?: string;
    locationName?: string;
    reportedToName?: string;
    raisedByName?: string;
    createdDate?: Date;
    locationType?: string;
    otherLocation?: string;
    locationTypeName?: string;
    actionTaken?: string;
}

