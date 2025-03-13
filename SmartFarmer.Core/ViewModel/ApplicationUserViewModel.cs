using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace SmartFarmer.Core.ViewModel
{
    public class ApplicationUserViewModel
    {
        public string ApplicationUserId { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public string Email { get; set; }
        public int ApplicationUserStatusId { get; set; }
        public string Status { get; set; }
        public string ProfileImageName { get; set; }
        public string ProfileImageLink { get; set; }
        public string Mobile { get; set; }
        public string HouseNameNumber { get; set; }
        public int ApplicationUserTypeId { get; set; }
        public string Type { get; set; }
        public int? OperatorStatusId { get; set; }
        public string OperatorStatusName { get; set; }
        public Guid? UserGroupId { get; set; }
        public string UserGroupName { get; set; }
        public string Location { get; set; }
        public string Street { get; set; }
        public string Addressline2 { get; set; }
        public string Town { get; set; }
        public string County { get; set; }
        public string PostCode { get; set; }
        public Guid? FarmId { get; set; }
    }
    public class ApplicationUserDetailsViewModel
    {
        public LastEventViewModel LastEventDetails { get; set; }
        public List<MachineNickNameViewModel> Operating { get; set; }
        public List<TrainingOperatorViewModel> Training { get; set; }
        public ApplicationUserViewModel UserDetails { get; set; }
        public List<OperatorTrainingRecordViewModel> TrainingRecords { get; set; }
    }
    //public class AddFarmUserViewModel: AddUserViewModel
    //{
    //    [Required(ErrorMessage = "FarmId is required.")]
    //    public Guid FarmId { get; set; }
    //}
    public class AddUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password must have at least 8 characters including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }
        public IFormFile ProfileImage { get; set; }
        [MaxLength(500)]
        public string Mobile { get; set; }
        [MaxLength(500)]
        public string HouseNameNumber { get; set; }

        [MaxLength(100)]
        public string Street { get; set; }
        [MaxLength(100)]
        public string Addressline2 { get; set; }
        [MaxLength(100)]
        public string Town { get; set; }
        [MaxLength(100)]
        public string County { get; set; }
        [MaxLength(100)]
        public string PostCode { get; set; }
        public Guid? UserGroupId { get; set; }
        public int ApplicationUserTypeId { get; set; }
    }

    public class OperatorUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password must have at least 8 characters including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }
        public IFormFile ProfileImage { get; set; }
        [MaxLength(500)]
        public string MobileHouseNameNumber { get; set; }
        [MaxLength(100)]
        public string Street { get; set; }
        [MaxLength(100)]
        public string Addressline2 { get; set; }
        [MaxLength(100)]
        public string Town { get; set; }
        [MaxLength(100)]
        public string County { get; set; }
        [MaxLength(100)]
        public string PostCode { get; set; }
        public int?  OperatorStatusId { get; set; }
        public Guid? UserGroupId { get; set; }
    }

    public class SearchUserRequestViewModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SearchKey { get; set; }
    }

    public class UserRequestViewModel
    {
        public int TotalCount { get; set; }
        public IEnumerable<ApplicationUserViewModel> List { get; set; }
    }

    public class ApplicationUserRequestViewModel
    {
        public string ApplicationUserId { get; set; }
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        ErrorMessage = "Password must have at least 8 characters including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string NewPassword { get; set; }
        [MaxLength(100)]
        public string FirstName { get; set; }
        [MaxLength(100)]
        public string LastName { get; set; }
        public int ApplicationUserStatusId { get; set; }
        public string Mobile { get; set; }
        [MaxLength(500)]
        public string HouseNameNumber { get; set; }

        [MaxLength(100)]
        public string Street { get; set; }
        [MaxLength(100)]
        public string Addressline2 { get; set; }
        [MaxLength(100)]
        public string Town { get; set; }
        [MaxLength(100)]
        public string County { get; set; }
        [MaxLength(100)]
        public string PostCode { get; set; }
        public int ApplicationUserTypeId { get; set; }
        public Guid? UserGroupId { get; set; }
    }

    public class ApplicationUserNameListViewModel
    {
        public List<ApplicationUserNameViewModel> List { get; set; }
    }
    
    public class ApplicationUserNameViewModel
    {
        public string ApplicationUserId { get; set; }
        public string UserName { get; set; }
        public int ApplicationUserTypeId { get; set; }
        public string Type { get; set; }
        public bool IsAddedInTrainingRecord { get; set; }
    }
    
    public class ApplicationUserProfileImageViewModel
    {
        public string ApplicationUserId { get; set; }
        public string ProfileImageLink { get; set; }
        public string ProfileImageName { get; set; }
    }
    public class GetOperatorBothUserDetailsViewModel
    {
        public string ApplicationUserId { get; set; }
        public string ProfileImageLink { get; set; }
        public string ProfileImageName { get; set; }
        public string Name { get; set; }
        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; }
    }
    public class GetOperatorCheckInDetailsViewModel
    {
        public string UserId { get; set; }
        public string ProfileImageLink { get; set; }
        public string ProfileImageName { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int UserTypeId { get; set; }
        public string UserType { get; set; }
        public string ClockInLocaton { get; set; }
        public DateTime? ClockInTime { get; set; }   
        public string ClockOutLocaton { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public DateTime? TimeOut { get; set; }
        public string TimeLeft { get; set; }
        public string CheckInLocaton { get; set; }
        public DateTime? CheckInTime { get; set; }
        public Guid? GroupId { get; set; }
        public string GroupName { get; set; }
        public Guid? WelfareRoutineId { get; set; }
        public string WelfareRoutineName { get; set; }
        public int? CheckInMinutes { get; set; }
        public string UserLocaton { get; set; }
        public string MobileNumber { get; set; }
        public int? MobileActionTypeId { get; set; }
    }
    public class UserWelfareChcekViewModel
    {
        public List<WelfareChcekViewModel> List { get; set; }
    }

    public class WelfareChcekViewModel
    {
        public string UserId { get; set; }
        public int UserTypeId { get; set; }
        public string ClockInLocaton { get; set; }
        public DateTime? ClockInTime { get; set; }
        public string ClockOutLocaton { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string CheckInLocaton { get; set; }
        public DateTime? CheckInTime { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? WelfareRoutineId { get; set; }
        public int? WelfareTime { get; set; }
        public DateTime? WelfareTimeOut { get; set; }
    }





    public class UpdateUserResponseViewModel: UpdateUserRequestViewModel
    {
        public string UserId { get; set; }
        //public string UserStatus { get; set; }
    }
    public class UpdateUserRequestViewModel
    {
        [MaxLength(100)]
        [Required]
        public string FirstName { get; set; }
        [MaxLength(100)]
        [Required]
        public string LastName { get; set; }

        //[RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z0-9]).{8,}$",
        //ErrorMessage = "Password must have at least 8 characters including one uppercase letter, one lowercase letter, one digit, and one special character.")]
        //public string NewPassword { get; set; }
  
        //public int ApplicationUserStatusId { get; set; }
        public string Mobile { get; set; }

        [MaxLength(500)]
        public string HouseNameNumber { get; set; }

        [MaxLength(100)]
        public string Street { get; set; }

        [MaxLength(100)]
        public string Town { get; set; }

        [MaxLength(100)]
        public string County { get; set; }

        [MaxLength(100)]
        public string PostCode { get; set; }
    }

    public class WelfareAlarmViewModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Phone { get; set; }
        public string ProfileImageLink { get; set; }
        public int UserTypeId { get; set; }
        public string ClockInLocaton { get; set; }
        public DateTime? ClockInTime { get; set; }
        public string ClockOutLocaton { get; set; }
        public DateTime? ClockOutTime { get; set; }
        public string CheckInLocaton { get; set; }
        public DateTime? CheckInTime { get; set; }
        public Guid? GroupId { get; set; }
        public Guid? WelfareRoutineId { get; set; }
        public int? WelfareTime { get; set; }
        public string Eventlocation { get; set; }
        public DateTime? EventTime { get; set; }
        public DateTime? WelfareTimeOut { get; set; }
        public bool ShowWebPopup { get; set; }
        public Guid? EventId { get; set; }
        public int EventTypeId { get; set; }
        public string EventType { get; set; }
    }

    public class WelfareAlarmListViewModel
    {
        public List<WelfareAlarmViewModel> List { get; set; }
    }

    public class ApplicationUsersListViewModel
    {
        public List<ApplicationUsersViewModel> List { get; set; }
    }

    public class ApplicationUsersViewModel
    {
        public string ApplicationUserId { get; set; }
        public string UserName { get; set; }
        public bool IsAdded { get; set; }
    }
    public class TrainingRecordApplicationUserViewModel
    {
        public string ApplicationUserId { get; set; }
        public string UserName { get; set; }
        public string ProfileImageLink { get; set; }
        public string ProfileImageName { get; set; }
    }
}
