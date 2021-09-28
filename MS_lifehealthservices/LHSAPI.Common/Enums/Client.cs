using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Common.Enums
{
    public class Client
    {
        public enum ClientOrderBy
        {
            Name,
            MobileNo,
            Email,
            Address,
            CreatedDate
        }
        public enum ClientAccidentOrderBy
        {
            Name,
            ReportedTo,
            Department,
            EventType,
            Location,
            AccidentDate

        }
        public enum ClientAccidentInfoOrderBy
        {
            Name,
            EventType,
            Location,
            Department,
            ReportedTo,
            AccidentDate

        }
        public enum ClientMedicalOrderBy
        {
            Name,
            Gender,
            MobileNo

        }
        public enum ClientProgressOrderBy
        {
            Name,
            ProgressNotes,
            Date

        }
        public enum ClientProgressInfoOrderBy
        {

            ProgressNotes,
            Date

        }
        public enum ClientFundingOrderBy
        {

            ServiceType,
            NoDays,
            Ammount,
            TotalAmount

        }
        public enum ClientAgreementOrderBy
        {

            ClientName,
            FundingSource,
            StartDate,
            EndDate,
            Expiry,
            Balance

        }
        public enum ClientContactOrderBy
        {

            Name,
            Relationship,
            Email,
            ContactNo,
            PhoneNo

        }
        public enum ClientComplianceOrderBy
        {
            documentType,
            documentName,
            description,
            dateofissue,
            hasExpiry,
            dateOfExpiry,
            alert

        }
        public enum ClientDocumentListOrderBy
        {
            clientName,
            documentType,
            documentName,
            description,
            dateofissue,
            hasExpiry,
            dateOfExpiry,
            alert

        }
        public enum ClientShiftlistOrderBy
        {
            Name,
            StartDate,
            EndDate,
            StartTime,
            EndTime,
            Duration

        }
    }
    public enum ClientSortOrder
    {
        Asc,
        Desc,
    }

}
