using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Master.Models
{
    public class AccidentIncidentStaticModel
    {
        public List<State> StateList { get; set; }
        public List<PrimaryCategory> PrimaryCategoryList { get; set; }
        public List<SecondaryCategory> SecondaryCategoryList { get; set; }
        public List<LocationType> LocationTypeList { get; set; }

        public List<PrimaryDisability> PrimaryDisabilityList { get; set; }
        public List<SecondaryDisability> SecondaryDisabilityList { get; set; }
        public List<CommunicationType> CommunicationTypeList { get; set; }
        public List<ConcernBehaviour> ConcernBehaviourList { get; set; }
        public List<Gender> GenderList { get; set; }
        public List<Location> LocationList { get; set; }
    }

    public class MedicalHistoryStaticModel
    {
        public List<Gender> GenderList { get; set; }
        public List<ConditionType> ConditionType { get; set; }
        public List<SymptomType> SymptomType { get; set; }

    }

    public class State
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class PrimaryCategory
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class SecondaryCategory
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class LocationType
    {
        public int? Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class PrimaryDisability
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class SecondaryDisability
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class CommunicationType
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class ConcernBehaviour
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class Gender
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class ConditionType
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }

    public class SymptomType
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }
    }
    public class Location
    {
        public int Id { get; set; }
        public string CodeDescription { get; set; }        
        public string Address { get; set; }
    }
}
