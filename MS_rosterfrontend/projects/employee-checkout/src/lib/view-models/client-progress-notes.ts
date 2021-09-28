export interface ClientProgressNotes {
    id?: number;
    clientId?: number;
    patientName?: string;
    dateOfBirth?: string;
    medicalRecordNo?: string;
    scheduleFor?: string;
    appointmentTo?: string;
    forDescharge?: string;
    other?: string;
    reviewDate?: string;
    signedDate?: string;
}

export interface ClientProgressNotesFields {
    date?: Date;
    note9AMTo11AM?: string;
    note11AMTo1PM?: string;
    note1PMTo15PM?: string;
    note15PMTo17PM?: string;
    note17PMTo19PM?: string;
    note19PMTo21PM?: string;
    note21PMTo23PM?: string;
    note23PMTo1AM?: string;
    note1AMTo3AM?: string;
    note3AMTo5AM?: string;
    note5AMTo7AM?: string;
    note7AMTo9AM?: string;
    summary?: string;
    communityAccess?: string;
    culturalNeeds?: string;
    behaviourConcern?: string;
    medicationgiven?: string;
    anyFalls?: string;
    mobilitySafety?: string;
    nutritionalDetail?: string;
    exerciseDone?: string;
    otherInfo?: string;
    id?: number;
    clientId?: number;
    clientProgressNoteId?: number;
}
