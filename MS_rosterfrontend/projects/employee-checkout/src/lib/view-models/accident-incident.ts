export interface AccidentIncidents {
    clientId?: number;
    primaryIncidentId?: [];
    secondaryIncidentId?:[]

}

export interface IncidentImpactedPerson { 
    id?: number;
    CommunicationId?: [];
    ConcerBehaviourId?:[]
    PrimaryDisability?:[]
    OtherDisability?:[]
    
}
export interface IncidentAllegation { 
    id?: number;
    CommunicationId?: [];
    ConcerBehaviourId?:[]
    PrimaryDisability?:[]
    OtherDisability?:[]
    
}
export interface IncidentAttachment { 
    id?: number;
    clientId?: number;
    documentName?: string;
    fileName?:string;
}
