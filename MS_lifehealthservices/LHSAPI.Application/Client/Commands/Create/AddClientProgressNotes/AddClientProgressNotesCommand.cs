using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Create.AddClientProgressNotes
{
  public class AddClientProgressNotesCommand : IRequest<ApiResponse>
  {
    public int ClientId { get; set; }

    public string PatientName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public string MedicalRecordNo { get; set; }

    public string ScheduleFor { get; set; }

    public string AppointmentTo { get; set; }

    public string ForDescharge { get; set; }

    public string Other { get; set; }

    public DateTime ReviewDate { get; set; }

    public DateTime? SignedDate { get; set; }
  }
}
