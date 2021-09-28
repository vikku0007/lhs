export interface ShiftHistoryViewModel {
    id?: string;
    checkInTime?: string;
    checkInDate?: Date;
    checkOutDate?: Date;
    checkOutTime?: string;
    checkOutTimestring?: string;
    checkInTimestring?: string;
    duration?: string;
    customDuration?: string;
    isShiftCompleted?: boolean;
}
export interface ToDoShiftViewModel {
    id?: number;
    shiftId?: number,
    dateTime?: Date,
    shiftType?: number,
    shiftTypeName?: string,
}
export interface ShiftTodoListViewModel {
    id?: number;
    description?: string,
    isInitials?: boolean,
    initials?: string,
    todoItemId?: number
}