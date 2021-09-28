use  [lifehealthservices]
alter table [dbo].[AspNetUsers] add EmployeeId int Not null CONSTRAINT [AspNetUsers_EmployeeId] DEFAULT 0

alter table [user] drop COLUMN  EmployeeId

--Leave changes
alter table EmployeeLeaveInfo add IsApproved bit 
alter table EmployeeLeaveInfo add IsRejected bit 
alter table EmployeeLeaveInfo add ApprovedById int 
alter table EmployeeLeaveInfo add RejectedById int 
alter table EmployeeLeaveInfo add ApprovedDate datetime 
alter table EmployeeLeaveInfo add RejectedDate datetime
alter table EmployeeLeaveInfo add RejectRemark varchar(500) 