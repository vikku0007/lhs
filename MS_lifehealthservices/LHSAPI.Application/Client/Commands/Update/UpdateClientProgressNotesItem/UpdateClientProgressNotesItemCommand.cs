using LHSAPI.Common.ApiResponse;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace LHSAPI.Application.Client.Commands.Update.UpdateClientProgressNotesItem
{
    public class UpdateClientProgressNotesItemCommand : IRequest<ApiResponse>
    {

        public int Id { get; set; }

        public int ClientId { get; set; }

        public int ClientProgressNoteId { get; set; }

        public DateTime Date { get; set; }

        public string Note9AMTo11AM { get; set; }

        public string Note11AMTo1PM { get; set; }

        public string Note1PMTo15PM { get; set; }

        public string Note15PMTo17PM { get; set; }

        public string Note17PMTo19PM { get; set; }

        public string Note19PMTo21PM { get; set; }

        public string Note21PMTo23PM { get; set; }

        public string Note23PMTo1AM { get; set; }

        public string Note1AMTo3AM { get; set; }

        public string Note3AMTo5AM { get; set; }

        public string Note5AMTo7AM { get; set; }

        public string Note7AMTo9AM { get; set; }
        public string Summary { get; set; }
        public string OtherInfo { get; set; }
    }
}
