using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.ViewModel
{
    public class DashboardCountViewModel
    {
        public int IssueCorrective { get; set; }
        public int IssueDefect { get; set; }
        public int IssueWarning { get; set; }
        public int TrainingDue { get; set; }
        public int TrainingExpired { get; set; }
        public int OperatorTotal { get; set; }
        public int OperatorActive { get; set; }
        public int MachineTotal { get; set; }
        public int MachineOutOfService { get; set; }
        public int MachineOutOfSeason { get; set; }
        public int MachineActive { get; set; }
        public int MachineIdle { get; set; }
    }
    public class PreCheckCountViewModel
    {
        public int Due { get; set; }
        public int Late { get; set; }
        public int Completed { get; set; }
    }
        public class MachineWithOperatorResponseViewModel
    {
        public Guid MachineId { get; set; }
        public string MachineImage { get; set; }
        public string MachineImageUniqueName { get; set; }
        public string NickName { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public Guid? MachineTypeId { get; set; }
        public string MachineType { get; set; }
        public int MachineStatusId { get; set; }
        public string Status { get; set; }
        public string OperatorId { get; set; }
        public string OperatorName { get; set; }
        public string MachineCategoryName { get; set; }
        public Guid MachineCategoryId { get; set; }
        public string Location { get; set; }
        public bool InSeason { get; set; }

    }

    public class SearchMachineWithOperatorRequestViewModel
    {
        public string SearchKey { get; set; }
    }

    public class MachineWithOperatorCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<MachineWithOperatorResponseViewModel> List { get; set; }
    }

    public class OperatorUserResponseViewModel
    {
        public string ApplicationUserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfileImageName { get; set; }
        public string ProfileImageLink { get; set; }
        public int? OperatorStatusId { get; set; }
        public string OperatorStatusName { get; set; }
        public string Location { get; set; }
    }

    public class SearchOperatorsRequestViewModel
    {
        public string SearchKey { get; set; }
    }

    public class OperatorUserDetailsViewModel
    {
        public List<MachineNickNameViewModel> Operating { get; set; }
        public OperatorUserResponseViewModel UserDetails { get; set; }
    }

    public class OperatorCountRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<OperatorUserDetailsViewModel> List { get; set; }
    }
}
