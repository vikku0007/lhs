import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { MembershipService, NotificationService } from 'projects/core/src/projects';
import { ClientDetails } from 'projects/employee-checkout/src/lib/view-models/add-client-details';
import { Paging } from 'projects/viewmodels/paging';
import { ResponseModel } from 'projects/viewmodels/response-model';
import { OtherService } from '../../services/other.service';

@Component({
  selector: 'lib-accident-incident-client-details',
  templateUrl: './accident-incident-client-details.component.html',
  styleUrls: ['./accident-incident-client-details.component.scss']
})
export class AccidentIncidentClientDetailsComponent implements OnInit {
  clientShiftModel: ClientDetails[] = [];
  response: ResponseModel = {};
  displayedColumnsRequired: string[] = ['employeeName', 'action'];
  dataSourceRequired = new MatTableDataSource(this.clientShiftModel);
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  orderBy: number;
  orderColumn: number;
  employeeId: number;
  shiftId: number;
  paging: Paging = {};
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  totalCount: number;

  ReportedBy: any;
  ProviderName: any;
  RegistrationId: any;
  ProviderABN: any;
  OutletName: any;
  RegistrationGroup: any;
  State: any;
  ContactTitle: any;
  ContactFirstName: any;
  ContactMiddleName: any;
  ContactLastName: any;
  ContactProvider: any;
  ContactPhoneNo: any;
  ContactEmail: any;
  ContactMethod: any;
  IsIncidentAnticipatedInfo: string;
  PrimaryCategoryName: any;
  SecondaryCategoryName: any;
  locationName: any;
  otherlocationInfo: any;
  circumstanceIncidentinfo: any;
  locationtypeinfo: any;
  Incidentdateinfo: any;
  Reasoninfo: any;
  NdisProviderTimeinfo: any;
  NdisProviderDateinfo: any;
  DescribeIncidentinfo: any;
  ImpactedPrimaryDisbilityinfo: any;
  ImpactedSecondaryDisbilityinfo: any;
  ImpactedCommunicationinfo: any;
  ImpactedBehaviourinfo: any;
  ImpactedTitleinfo: any;
  ImpactedName: any;
  ImpactedNdisNoinfo: any;
  ImpactedGenderinfo: any;
  ImpactedDOBinfo: any;
  ImpactedPhoneNo: any;
  ImpactedEmail: any;
  OtherDetail: any;
  ImpactedSecondaryDisabilityinfo: any;
  DisablePrimaryDisability: any;
  DisableOtherDisability: any;
  DisableCommunication: any;
  DisableBehaviour: any;
  WorkerTitle: any;
  WorkerName: any;
  WorkerPosition: any;
  WorkerGender: any;
  WorkerDOB: any;
  WorkerPhoneNo: any;
  WorkerEmail: any;
  AllegationOtherDetail: any;
  IsSubjectAllegationInfo: string;
  DeclarationDate: any;
  DeclarationName: any;
  DeclarationPosition: any;
  TobeFinished: any;
  InProgressRisk: any;
  NoRiskAssesmentInfo: any;
  RiskDetails: any;
  RiskAssesmentDate: any;
  IsRiskAssesmentInfo: string;
  DescribeDisability: any;
  WorkerDescribe: any;
  DescribeImmediate: any;
  ChildContacted: any;
  Guardian: any;
  IsUnder18Info: string;
  DisableTitle: any;
  DisableGender: any;
  DisableNdisNo: any;
  DisableName: any;
  DisableDOBirth: any;
  DisablePhoneNo: any;
  DisableEmail: any;
  OtherName: any;
  OtherRelationShip: any;
  OtherGender: any;
  OtherDOB: any;
  OtherPhoneNo: any;
  OtherEmail: any;
  OtherTitle: any;
  IsPoliceInformed: string;
  OfficerName: any;
  PoliceStation: any;
  PoliceNumber: any;
  PoliceNotInform: any;
  IsFamilyAwareInfo: string;
  ReportText: string;
  selectedName: any;
  todayDatemax = new Date();
  clientId: number;
  Primarylist: any;
  Secondarylist: any;
  Primarydislist: any;
  otherdislist: any;
  ImpactCommunication: any;
  ImpactConcerndata: any;
  PrimaryAllegation: any;
  otherAllegation: any;
  AllegationCommunication: any;
  AllegationConcerndata: any;  



  constructor(private otherService: OtherService, private route: ActivatedRoute, private router: Router,
    private notificationService: NotificationService, private membershipService: MembershipService) {
    this.paging.pageNo = 1;
    this.paging.pageSize = 10;
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: any) => {
      this.shiftId = params.params.id;
    });
    this.employeeId = this.membershipService.getUserDetails('employeeId');
    this.getClientShift();
  }

  getClientShift() {
    const data = {
      Id: Number(this.shiftId)
    };
    this.otherService.getCheckOutClientList(data).subscribe(res => {
      this.response = res;
      this.totalCount = this.response.total;
      switch (this.response.status) {
        case 1:
          this.clientShiftModel = this.response.responseData;
          this.dataSourceRequired = new MatTableDataSource(this.clientShiftModel);
          break;
        default:
          break;
      }
    });
  }

  getAccidentIncidentDetails(clientId: number, shiftId: number) {
    const data = {
      Id: Number(clientId),
      ShiftId: Number(shiftId)
    };
    this.otherService.GetAllIncidentDetails(data).subscribe(res => {
      this.response = res;
      if (this.response.status > 0) {
        this.ReportedBy = (this.response.responseData.clientAccidentProviderInfo.reportCompletedBy);
        this.ProviderName = (this.response.responseData.clientAccidentProviderInfo.providerName);
        this.RegistrationId = (this.response.responseData.clientAccidentProviderInfo.providerregistrationId);
        this.ProviderABN = (this.response.responseData.clientAccidentProviderInfo.providerABN);
        this.OutletName = (this.response.responseData.clientAccidentProviderInfo.outletName);
        this.RegistrationGroup = (this.response.responseData.clientAccidentProviderInfo.registrationgroup);
        this.State = (this.response.responseData.clientAccidentProviderInfo.stateName);
        this.ContactTitle = (this.response.responseData.clientAccidentPrimaryContact.title);
        this.ContactFirstName = (this.response.responseData.clientAccidentPrimaryContact.fullName);
        this.ContactMiddleName = (this.response.responseData.clientAccidentPrimaryContact.middleName);
        this.ContactLastName = (this.response.responseData.clientAccidentPrimaryContact.lastName);
        this.ContactProvider = (this.response.responseData.clientAccidentPrimaryContact.providerPosition);
        this.ContactPhoneNo = (this.response.responseData.clientAccidentPrimaryContact.phoneNo);
        this.ContactEmail = (this.response.responseData.clientAccidentPrimaryContact.email);
        this.ContactMethod = (this.response.responseData.clientAccidentPrimaryContact.contactMetod);
        this.IsIncidentAnticipatedInfo = this.response.responseData.clientIncidentCategory.isIncidentAnticipated == 1 ? "Yes" : this.response.responseData.clientIncidentCategory.isIncidentAnticipated == 2 ? "No" : this.response.responseData.clientIncidentCategory.isIncidentAnticipated == 3 ? "Unknown" : "";
        this.Primarylist = this.response.responseData.clientPrimaryIncidentCategory;
        this.PrimaryCategoryName = this.Primarylist.map(x => x.primaryIncidentName);
        this.Secondarylist = this.response.responseData.clientSecondaryIncidentCategory;
        this.SecondaryCategoryName = (this.Secondarylist.map(x => x.secondaryIncidentName));
        this.locationtypeinfo = (this.response.responseData.clientIncidentDetails.locationTypeName);
        this.Incidentdateinfo = (this.response.responseData.clientIncidentDetails.dateTime);
        this.Reasoninfo = (this.response.responseData.clientIncidentDetails.unknowndateReason);
        this.NdisProviderTimeinfo = (this.response.responseData.clientIncidentDetails.startTimeString);
        this.NdisProviderDateinfo = (this.response.responseData.clientIncidentDetails.ndisProviderDate);
        this.DescribeIncidentinfo = (this.response.responseData.clientIncidentDetails.incidentAllegtion);
        this.circumstanceIncidentinfo = (this.response.responseData.clientIncidentDetails.allegtionCircumstances);
        if (this.response.responseData.clientIncidentDetails.address != "" && this.response.responseData.clientIncidentDetails.address != null && this.response.responseData.clientIncidentDetails.address != undefined) {
          this.otherlocationInfo = (this.response.responseData.clientIncidentDetails.address);

        }
        else if (this.response.responseData.clientIncidentDetails.locationId != null && this.response.responseData.clientIncidentDetails.locationId > 0) {

          this.locationName = (this.response.responseData.clientIncidentDetails.locationName);

        }
        this.Primarydislist = this.response.responseData.incidentPrimaryDisability;
        this.ImpactedPrimaryDisbilityinfo = (this.Primarydislist.map(x => x.primaryDisabilityName));
        this.otherdislist = this.response.responseData.incidentOtherDisability;
        this.ImpactedSecondaryDisabilityinfo = (this.otherdislist.map(x => x.secondaryDisabilityName));
        this.ImpactCommunication = this.response.responseData.clientIncidentCommunication;
        this.ImpactedCommunicationinfo = (this.ImpactCommunication.map(x => x.communicationName));
        this.ImpactConcerndata = this.response.responseData.incidentConcernBehaviour;
        this.ImpactedBehaviourinfo = (this.ImpactConcerndata.map(x => x.concernBehaviourName));
        this.ImpactedTitleinfo = (this.response.responseData.incidentImpactedPerson.title);
        this.ImpactedName = (this.response.responseData.incidentImpactedPerson.fullName);
        this.ImpactedNdisNoinfo = (this.response.responseData.incidentImpactedPerson.ndisParticipantNo);
        this.ImpactedGenderinfo = (this.response.responseData.incidentImpactedPerson.genderName);
        this.ImpactedDOBinfo = (this.response.responseData.incidentImpactedPerson.dateOfBirth);
        this.ImpactedPhoneNo = (this.response.responseData.incidentImpactedPerson.phoneNo);
        this.ImpactedEmail = (this.response.responseData.incidentImpactedPerson.email);
        this.OtherDetail = (this.response.responseData.incidentImpactedPerson.otherDetail);
        this.PrimaryAllegation = this.response.responseData.incidentAllegationPrimaryDisability;
        this.DisablePrimaryDisability = (this.PrimaryAllegation.map(x => x.primaryDisabilityName));
        this.otherAllegation = this.response.responseData.incidentAllegationOtherDisability;
        this.DisableOtherDisability = (this.otherAllegation.map(x => x.secondaryDisabilityName));
        this.AllegationCommunication = this.response.responseData.incidentAllegationCommunication;
        this.DisableCommunication = (this.AllegationCommunication.map(x => x.communicationName));
        this.AllegationConcerndata = this.response.responseData.incidentAllegationBehaviour;
        this.DisableBehaviour = (this.AllegationConcerndata.map(x => x.concernBehaviourName));
        this.WorkerTitle = (this.response.responseData.incidentWorkerAllegation.title);
        this.WorkerName = (this.response.responseData.incidentWorkerAllegation.subjectFullName);
        this.WorkerPosition = (this.response.responseData.incidentWorkerAllegation.position);
        this.WorkerGender = (this.response.responseData.incidentWorkerAllegation.genderName);
        this.WorkerDOB = (this.response.responseData.incidentWorkerAllegation.dateOfBirth);
        this.WorkerPhoneNo = (this.response.responseData.incidentWorkerAllegation.phoneNo);
        this.WorkerEmail = (this.response.responseData.incidentWorkerAllegation.email);
        this.AllegationOtherDetail = (this.response.responseData.incidentDisablePersonAllegation.otherDetail);
        this.IsSubjectAllegationInfo = this.response.responseData.incidentWorkerAllegation.isSubjectAllegation == 1 ? "Yes" : this.response.responseData.incidentWorkerAllegation.isSubjectAllegation == 2 ? "No" : this.response.responseData.incidentWorkerAllegation.isSubjectAllegation == 3 ? "Unknown" : "";
        this.DisableTitle = (this.response.responseData.incidentDisablePersonAllegation.title);
        this.DisableName = (this.response.responseData.incidentDisablePersonAllegation.disableFullName);
        this.DisableNdisNo = (this.response.responseData.incidentDisablePersonAllegation.ndisNumber);
        this.DisableGender = (this.response.responseData.incidentDisablePersonAllegation.genderName);
        this.DisableDOBirth = (this.response.responseData.incidentDisablePersonAllegation.dateOfBirth);
        this.DisablePhoneNo = (this.response.responseData.incidentDisablePersonAllegation.phoneNo);
        this.DisableEmail = (this.response.responseData.incidentDisablePersonAllegation.email);
        this.OtherName = (this.response.responseData.incidentOtherAllegation.otherFullName);
        this.OtherRelationShip = (this.response.responseData.incidentOtherAllegation.relationship);
        this.OtherGender = (this.response.responseData.incidentOtherAllegation.genderName);
        this.OtherDOB = (this.response.responseData.incidentOtherAllegation.dateOfBirth);
        this.OtherPhoneNo = (this.response.responseData.incidentOtherAllegation.phoneNo);
        this.OtherEmail = (this.response.responseData.incidentOtherAllegation.email);
        this.OtherTitle = (this.response.responseData.incidentOtherAllegation.title);
        this.IsPoliceInformed = (this.response.responseData.incidentImmediateAction.isPoliceInformed == true ? "Yes" : "No");
        this.OfficerName = (this.response.responseData.incidentImmediateAction.officerName);
        this.PoliceStation = (this.response.responseData.incidentImmediateAction.policeStation);
        this.PoliceNumber = (this.response.responseData.incidentImmediateAction.policeNo);
        this.PoliceNotInform = (this.response.responseData.incidentImmediateAction.providerPosition);
        this.IsFamilyAwareInfo = this.response.responseData.incidentImmediateAction.isFamilyAware == 1 ? "Yes" : this.response.responseData.incidentImmediateAction.isFamilyAware == 2 ? "No" : this.response.responseData.incidentImmediateAction.isFamilyAware == 3 ? "Unsure" : "";
        this.IsUnder18Info = this.response.responseData.incidentImmediateAction.isUnder18 == 1 ? "Yes" : this.response.responseData.incidentImmediateAction.isUnder18 == 2 ? "No" : this.response.responseData.incidentImmediateAction.isUnder18 == 3 ? "Unkown" : this.response.responseData.incidentImmediateAction.isUnder18 == 4 ? "NotApplicable" : "";
        this.Guardian = (this.response.responseData.incidentImmediateAction.contacttoFamily);
        this.ChildContacted = (this.response.responseData.incidentImmediateAction.contactChildProtection);
        this.DescribeImmediate = (this.response.responseData.incidentImmediateAction.disabilityPerson);
        this.WorkerDescribe = (this.response.responseData.incidentImmediateAction.subjectWorkerAllegation);
        this.DescribeDisability = (this.response.responseData.incidentImmediateAction.subjectDisabilityPerson);
        this.IsRiskAssesmentInfo = this.response.responseData.incidentRiskAssesment.isRiskAssesment == 1 ? "Yes" : this.response.responseData.incidentRiskAssesment.isRiskAssesment == 2 ? "No" : this.response.responseData.incidentRiskAssesment.isRiskAssesment == 3 ? "InProgress" : "";
        this.RiskAssesmentDate = (this.response.responseData.incidentRiskAssesment.riskAssesmentDate);
        this.RiskDetails = (this.response.responseData.incidentRiskAssesment.riskDetails);
        this.NoRiskAssesmentInfo = (this.response.responseData.incidentRiskAssesment.noRiskAssesmentInfo);
        this.InProgressRisk = (this.response.responseData.incidentRiskAssesment.inProgressRisk);
        this.TobeFinished = (this.response.responseData.incidentRiskAssesment.tobeFinished);
        this.DeclarationName = (this.response.responseData.incidentDeclaration.name);
        this.DeclarationPosition = (this.response.responseData.incidentDeclaration.positionAtOrganisation);
        this.DeclarationDate = (this.response.responseData.incidentDeclaration.date);

      }
    });
  }

  PageIndexEvent(event: PageEvent) {
    this.paging.pageNo = event.pageIndex + 1;
    this.paging.pageSize = event.pageSize;
    this.getClientShift();
  }

}
