using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.ViewModel
{

    public class SearchEquipmentRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
        public int? EquipmentStatusId { get; set; }
        public bool? HasIssues { get; set; }
        public bool? isOUtOfSeason { get; set; }
    }
    public class EquipmentResponseViewModel
    {
        public Guid EquipmentId { get; set; }
        public string EquipmentImage { get; set; }
        public string EquipmentImageUniqueName { get; set; }
        public string NickName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int EquipmentStatusId { get; set; }
        public string Status { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
    }

    public class EquipmentListViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<EquipmentDetailsViewModel> List { get; set; }
    }

    public class EquipmentDetailsViewModel
    {
        public EquipmentResponseViewModel EquipmentDetails { get; set; }
        public LastEventViewModel LastEventDetails { get; set; }
        public DateTime? LastCheck { get; set; }
        public string LastOperatorId { get; set; }
        public string LastOperatorName { get; set; }
        public int Issues { get; set; }
    }


    public class SearchEquipmentHistoryRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? EventTypeId { get; set; }


        public class SearchEquipmentPreCheckHistoryRequestViewModel
        {
            //public int PageNumber { get; set; }
            //public int PageSize { get; set; }
            public string SearchKey { get; set; }
            public Guid? MachineId { get; set; }
            public int? ResultId { get; set; }
        }
    }
}
