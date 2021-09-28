
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LHSAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace LHSAPI.Persistence.DbContext
{
    public class LHSDbContext : IdentityDbContext<ApplicationUser>
    {
        public LHSDbContext(DbContextOptions<LHSDbContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }

        public DbSet<UserRegister> UserRegister { get; set; }
        public DbSet<EmployeePrimaryInfo> EmployeePrimaryInfo { get; set; }
        public DbSet<EmployeeMiscInfo> EmployeeMiscInfo { get; set; }
        public DbSet<EmployeeKinInfo> EmployeeKinInfo { get; set; }
        public DbSet<EmployeeAwardInfo> EmployeeAwardInfo { get; set; }
        public DbSet<EmployeePicInfo> EmployeePicInfo { get; set; }
        public DbSet<EmployeeDrivingLicenseInfo> EmployeeDrivingLicenseInfo { get; set; }

        public DbSet<EmployeeEducation> EmployeeEducation { get; set; }
        public DbSet<EmployeeJobProfile> EmployeeJobProfile { get; set; }
        public DbSet<EmployeeWorkExp> EmployeeWorkExp { get; set; }
        public DbSet<EmployeePayRate> EmployeePayRate { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Error> Error { get; set; }
        public DbSet<StoreDeviceToken> storeDeviceTokens { get; set; }
        public DbSet<EmployeeAccidentInfo> EmployeeAccidentInfo { get; set; }
        public DbSet<EmployeeLeaveInfo> EmployeeLeaveInfo { get; set; }
        public DbSet<EmployeeStaffWarning> EmployeeStaffWarning { get; set; }
        public DbSet<EmployeeAvailabilityDetails> EmployeeAvailabilityDetails { get; set; }
        public DbSet<EmployeeCommunicationInfo> EmployeeCommunicationInfo { get; set; }
        public DbSet<EmployeeCompliancesDetails> EmployeeCompliancesDetails { get; set; }
        public DbSet<EmployeeAppraisalDetails> EmployeeAppraisalDetails { get; set; }
        public DbSet<EmployeeAppraisalStandards> EmployeeAppraisalStandards { get; set; }
        public DbSet<EmployeeOtherComplianceDetails> EmployeeOtherComplianceDetails { get; set; }
        public DbSet<EmployeeShiftTracker> EmployeeShiftTracker { get; set; }

        public DbSet<ClientPrimaryInfo> ClientPrimaryInfo { get; set; }
        public DbSet<ClientPrimaryCareInfo> ClientPrimaryCareInfo { get; set; }
        public DbSet<ClientBoadingNotes> ClientBoadingNotes { get; set; }
        public DbSet<ClientAdditionalNotes> ClientAdditionalNotes { get; set; }
        public DbSet<ClientFunding> ClientFunding { get; set; }
        public DbSet<ClientFundingInfo> ClientFundingInfo { get; set; }
        public DbSet<ClientMedicalHistory> ClientMedicalHistory { get; set; }

        public DbSet<ClientProgressNotes> ClientProgressNotes { get; set; }
        public DbSet<ProgressNotesList> ProgressNotesList { get; set; }
        public DbSet<StandardCode> StandardCode { get; set; }
        public DbSet<GlobalPayRate> GlobalPayRate { get; set; }
        public DbSet<ClientPicInfo> ClientPicInfo { get; set; }
        public DbSet<ClientSupportCoordinatorInfo> ClientSupportCoordinatorInfo { get; set; }
        public DbSet<ClientCompliancesDetails> ClientCompliancesDetails { get; set; }
        public DbSet<ClientAccidentIncidentInfo> ClientAccidentIncidentInfo { get; set; }
        public DbSet<ShiftInfo> ShiftInfo { get; set; }
        public DbSet<EmployeeShiftInfo> EmployeeShiftInfo { get; set; }
        public DbSet<ClientShiftInfo> ClientShiftInfo { get; set; }
        public DbSet<ShiftToDo> ShiftToDo { get; set; }
        public DbSet<CommunicationRecipient> CommunicationRecipient { get; set; }
        public DbSet<EmployeeTraining> EmployeeTraining { get; set; }
        public DbSet<ClientGuardianInfo> ClientGuardianInfo { get; set; }
        public DbSet<ServiceDetails> ServiceDetails { get; set; }

        public DbSet<ShiftInfoTemplate> ShiftInfoTemplate { get; set; }
        public DbSet<EmployeeShiftInfoTemplate> EmployeeShiftInfoTemplate { get; set; }
        public DbSet<ClientShiftInfoTemplate> ClientShiftInfoTemplate { get; set; }

        public DbSet<ShiftTemplate> ShiftTemplate { get; set; }
        public DbSet<ClientLivingArrangement> ClientLivingArrangement { get; set; }
        public DbSet<ClientPersonalPreferences> ClientPersonalPreferences { get; set; }
        public DbSet<ClientCultureNeed> ClientCultureNeed { get; set; }
        public DbSet<ClientSocialConnections> ClientSocialConnections { get; set; }
        public DbSet<ClientOtherInformtion> ClientOtherInformtion { get; set; }
        public DbSet<ClientEatingNutrition> ClientEatingNutrition { get; set; }
        public DbSet<ClientSafetyBehavioursInfo> ClientSafetyBehavioursInfo { get; set; }
        public DbSet<ClientBehaviourofConcern> ClientBehaviourofConcern { get; set; }
        public DbSet<ClientProgressNotesTiming> ClientProgressNotesTiming { get; set; }
        public DbSet<ClientAccidentProviderInfo> ClientAccidentProviderInfo { get; set; }
        public DbSet<ClientAccidentPrimaryContact> ClientAccidentPrimaryContact { get; set; }
        public DbSet<ClientIncidentCategory> ClientIncidentCategory { get; set; }
        public DbSet<ClientPrimaryIncidentCategory> ClientPrimaryIncidentCategory { get; set; }
        public DbSet<ClientSecondaryIncidentCategory> ClientSecondaryIncidentCategory { get; set; }
        public DbSet<IncidentImpactedPerson> IncidentImpactedPerson { get; set; }
        public DbSet<IncidentPrimaryDisability> IncidentPrimaryDisability { get; set; }
        public DbSet<IncidentOtherDisability> IncidentOtherDisability { get; set; }
        public DbSet<ClientIncidentCommunication> ClientIncidentCommunication { get; set; }
        public DbSet<IncidentConcernBehaviour> IncidentConcernBehaviour { get; set; }
        public DbSet<IncidentImmediateAction> IncidentImmediateAction { get; set; }
        public DbSet<IncidentRiskAssesment> IncidentRiskAssesment { get; set; }
        public DbSet<IncidentDeclaration> IncidentDeclaration { get; set; }
        public DbSet<ClientIncidentDetails> ClientIncidentDetails { get; set; }
        public DbSet<IncidentWorkerAllegation> IncidentWorkerAllegation { get; set; }
        public DbSet<IncidentDisablePersonAllegation> IncidentDisablePersonAllegation { get; set; }
        public DbSet<IncidentAllegationBehaviour> IncidentAllegationBehaviour { get; set; }
        public DbSet<IncidentAllegationCommunication> IncidentAllegationCommunication { get; set; }
        public DbSet<IncidentAllegationOtherDisability> IncidentAllegationOtherDisability { get; set; }
        public DbSet<IncidentAllegationPrimaryDisability> IncidentAllegationPrimaryDisability { get; set; }
        public DbSet<IncidentOtherAllegation> IncidentOtherAllegation { get; set; }
        public DbSet<IncidentDocumentDetails> IncidentDocumentDetails { get; set; }
        public DbSet<ShiftToDoList> ShiftToDoList { get; set; }
        public DbSet<ToDoShift> ToDoShift { get; set; }

        public DbSet<EmployeeHobbies> EmployeeHobbies { get; set; }
        public DbSet<ServiceTypeShiftInfo> ServiceTypeShiftInfo { get; set; }
        public DbSet<PublicHoliday> PublicHoliday { get; set; }
        public DbSet<ToDoShiftItem> ToDoShiftItem { get; set; }
        public DbSet<ClientDocumentName> ClientDocumentName { get; set; }
        public DbSet<ServiceTypeShiftInfoTemplate> ServiceTypeShiftInfoTemplate { get; set; }

        public DbSet<ActivityLog> ActivityLog { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<ShiftHistoryInfo> ShiftHistoryInfo { get; set; }
        public DbSet<DayReportAppointments> DayReportAppointments { get; set; }
        public DbSet<DayReportCashHandOver> DayReportCashHandOver { get; set; }
        public DbSet<DayReportDailyHandOver> DayReportDailyHandOver { get; set; }
        public DbSet<DayReportDetail> DayReportDetail { get; set; }
        public DbSet<DayReportFoodIntake> DayReportFoodIntake { get; set; }
        public DbSet<DayReportSupportWorkers> DayReportSupportWorkers { get; set; }
        public DbSet<DayReportTelePhoneMsg> DayReportTelePhoneMsg { get; set; }
        public DbSet<DayReportVisitor> DayReportVisitor { get; set; }
        public DbSet<ShiftEmailNotification> ShiftEmailNotification { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }
        public override int SaveChanges()
        {


            var entities = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified || x.State == EntityState.Deleted));

            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Added)
                {

                    var userid = entity.CurrentValues.GetValue<int>("CreatedById");
                    return SaveActivity(entity.Entity.GetType().Name, Domain.EntityUtility.GetUserReadableEntityName(entity.Entity.GetType().Name) + " Added", userid);

                }
                if (entity.State == EntityState.Modified)
                {
                    var userid = entity.CurrentValues.GetValue<int>("CreatedById");
                    return SaveActivity(entity.Entity.GetType().Name, Domain.EntityUtility.GetUserReadableEntityName(entity.Entity.GetType().Name) + " Updated", userid);
                }
                if (entity.State == EntityState.Deleted)
                {
                    var userid = entity.CurrentValues.GetValue<int>("CreatedById");
                    return SaveActivity(entity.Entity.GetType().Name, Domain.EntityUtility.GetUserReadableEntityName(entity.Entity.GetType().Name) + " Deleted", userid);
                }
            }

            return base.SaveChanges();
        }

        private int SaveActivity(string entityName, string Description, int id)
        {
            if (entityName != "ActivityLog")
            {
                ActivityLog _ActivityLog = new ActivityLog();
                _ActivityLog.EntityName = entityName;
                _ActivityLog.Description = Description;
                _ActivityLog.CreatedById = id;
                _ActivityLog.CreatedDate = DateTime.Now;
                this.ActivityLog.Add(_ActivityLog);
            }
            return base.SaveChanges();
        }
    }


}
