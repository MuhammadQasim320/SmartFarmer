using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain;
using SmartFarmer.Domain.Model;
using System.Linq;
using System.Text.Json;
using static SmartFarmer.Core.ViewModel.MachinePreCheckHistoryViewModel;
using static SmartFarmer.Core.ViewModel.MasterViewModel;
using static SmartFarmer.Core.ViewModel.OperatorViewModel;
using static SmartFarmer.Domain.Model.TimeSheetWebRequestViewModel;
using ApplicationUserType = SmartFarmer.Domain.Model.ApplicationUserType;

namespace SmartFarmer.Core.Common
{
    public class Mapper
    {
        //start master tables files
        //public static ActionTypeViewModel MapActionTypeEntityToActionTypeViewModel(ActionType model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new ActionTypeViewModel
        //    {
        //        ActionTypeId = model.ActionTypeId,
        //        Type = model.Type
        //    };
        //}

        public static UserStatusViewModel MapUserStatusEntityToUserStatusViewModel(ApplicationUserStatus model)
        {
            if (model == null)
            {
                return null;
            }
            return new UserStatusViewModel
            {
                ApplicationUserStatusId = model.ApplicationUserStatusId,
                Status = model.Status
            };
        }
        public static OperatorStatusViewModel MapOperatorStatusEntityToOperatorStatusViewModel(OperatorStatus model)
        {
            if (model == null)
            {
                return null;
            }
            return new OperatorStatusViewModel
            {
                OperatorStatusId = model.OperatorStatusId,
                Status = model.Status
            };
        }

        public static UserTypeViewModel MapUserTypeEntityToUserTypeViewModel(ApplicationUserType model)
        {
            if (model == null)
            {
                return null;
            }
            return new UserTypeViewModel
            {
                ApplicationUserTypeId = model.ApplicationUserTypeId,
                Type = model.Type
            };
        }

        public static CheckTypeViewModel MapCheckTypeEntityToCheckTypeViewModel(CheckType model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckTypeViewModel
            {
                CheckTypeId = model.CheckTypeId,
                Type = model.Type
            };
        }

        public static EventTypeViewModel MapEventTypeEntityToEventTypeViewModel(EventType model)
        {
            if (model == null)
            {
                return null;
            }
            return new EventTypeViewModel
            {
                EventTypeId = model.EventTypeId,
                Type = model.Type
            };
        }
        public static HazardTypeViewModel MapHazardTypeEntityToHazardTypeViewModel(HazardType model)
        {
            if (model == null)
            {
                return null;
            }
            return new HazardTypeViewModel
            {
                HazardTypeId = model.HazardTypeId,
                Type = model.Type
            };
        }  
        
        public static MobileActionTypeViewModel MapMobileActionTypeEntityToMobileActionTypeViewModel(MobileActionType model)
        {
            if (model == null)
            {
                return null;
            }
            return new MobileActionTypeViewModel
            {
                MobileActionTypeId = model.MobileActionTypeId,
                Type = model.Type
            };
        }
        
        public static LastEventViewModel MapEventEntityToLastEventViewModel(Event model)
        {
            if (model == null)
            {
                return null;
            }
            return new LastEventViewModel
            {
                LastEventId = model.EventId,
                LastEventName = model.Message,
                EventTime = model.CreatedDate,
                EventLocation = model.Location,
                EventTypeId = model.EventTypeId,
                EventTypeName = model?.EventType?.Type,
                
            };
        }

        public static FrequencyTypeViewModel MapFrequencyTypeEntityToFrequencyTypeViewModel(FrequencyType model)
        {
            if (model == null)
            {
                return null;
            }
            return new FrequencyTypeViewModel
            {
                FrequencyTypeId = model.FrequencyTypeId,
                Type = model.Type
            };
        }

        public static TrainingTypeViewModel MapTrainingTypeEntityToTrainingTypeViewModel(TrainingType model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingTypeViewModel
            {
                TrainingTypeId = model.TrainingTypeId,
                Type = model.Type
            };
        }

        public static UnitsTypeViewModel MapUnitsTypeEntityToUnitsTypeViewModel(UnitsType model)
        {
            if (model == null)
            {
                return null;
            }
            return new UnitsTypeViewModel
            {
                UnitsTypeId = model.UnitsTypeId,
                Units = model.Units
            };
        }

        public static InitialRiskAndAdjustedRiskViewModel MapInitialRiskAndAdjustedRiskEntityToInitialRiskAndAdjustedRiskViewModel(InitialRiskAndAdjustedRisk model)
        {
            if (model == null)
            {
                return null;
            }
            return new InitialRiskAndAdjustedRiskViewModel
            {
                InitialRiskAndAdjustedRiskId = model.InitialRiskAndAdjustedRiskId,
                RiskValue = model.RiskValue,
            };
        }

        public static MachineStatusViewModel MapMachineStatusEntityToMachineStatusViewModel(MachineStatus model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineStatusViewModel
            {
                MachineStatusId = model.MachineStatusId,
                Status = model.Status,
            };
        }
        
        public static IssueTypeViewModel MapIssueTypeEntityToIssueTypeViewModel(IssueType model)
        {
            if (model == null)
            {
                return null;
            }
            return new IssueTypeViewModel
            {
                IssueTypeId = model.IssueTypeId,
                Type = model.Type,
            };
        }
        
        public static IssueStatusViewModel MapIssueStatusEntityToIssueStatusViewModel(IssueStatus model)
        {
            if (model == null)
            {
                return null;
            }
            return new IssueStatusViewModel
            {
                IssueStatusId = model.IssueStatusId,
                Status = model.Status,
            };
        }

        public static CheckResultViewModel MapCheckResultEntityToCheckResultViewModel(CheckResult model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckResultViewModel
            {
                ResultId = model.ResultId,
                Result = model.Result
            };
        }
        //end master tables files

        public static ApplicationUserViewModel MapApplicationUserEntityToApplicationUserViewModel(ApplicationUser model)
        {
            if (model == null)
            {
                return null;
            }
            return new ApplicationUserViewModel
            {
                ApplicationUserId = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                ApplicationUserStatusId = model.ApplicationUserStatusId,
                Status = model.ApplicationUserStatus?.Status,
                ProfileImageName = model.ProfileImageName,
                ProfileImageLink = model.ProfileImageLink,
                Mobile = model.Mobile,
                HouseNameNumber = model.HouseNameNumber,
                ApplicationUserTypeId = model.ApplicationUserTypeId,
                Type = model.ApplicationUserType?.Type,
                OperatorStatusId = model?.OperatorStatusId,
                OperatorStatusName = model?.OperatorStatus?.Status,
                UserGroupId = model?.UserGroupId,
                UserGroupName = model?.UserGroup?.GroupName,
                Location = model.Location,
                Street = model.Street,
                PostCode = model.PostCode,
                Addressline2 = model.Addressline2,
                Town = model.Town,
                County = model.County,
                FarmId = model?.FarmId,
            };
        }
        
        
        public static GetOperatorCheckInDetailsViewModel MapApplicationUserEntityToGetOperatorCheckInDetailsViewModel(ApplicationUser model)
        {
            if (model == null)
            {
                return null;
            }
            return new GetOperatorCheckInDetailsViewModel
            {
                UserId = model.Id,
                Name = model.FirstName +" "+ model.LastName,
                ProfileImageName = model.ProfileImageName,
                ProfileImageLink = model.ProfileImageLink,
                UserType=model.ApplicationUserType.Type,
                UserTypeId=model.ApplicationUserTypeId,
                GroupId=model.UserGroupId,
                GroupName=model?.UserGroup?.GroupName,
               Phone =model?.Mobile,
    };
        }

        public static ApplicationUser MapApplicationUserRequestViewModelToApplicationUserEntity(ApplicationUserRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new ApplicationUser
            {
                Id = model.ApplicationUserId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ApplicationUserStatusId = model.ApplicationUserStatusId,
                Mobile = model.Mobile,
                Street = model.Street,
                PostCode = model.PostCode,
                County = model.County,
                Town = model.Town,
                Addressline2 = model.Addressline2,
                HouseNameNumber = model.HouseNameNumber,
                ApplicationUserTypeId = model.ApplicationUserTypeId,
                UserGroupId = model?.UserGroupId,
            };
        }
        
        public static ApplicationUserNameViewModel MapApplicationUserEntityToApplicationUserNameViewModel(ApplicationUser model)
        {
            if (model == null)
            {
                return null;
            }
            return new ApplicationUserNameViewModel
            {
                ApplicationUserId=model.Id,
                UserName = model.FirstName+ " " + model.LastName,
                ApplicationUserTypeId = model.ApplicationUserTypeId,
                Type = model.ApplicationUserType?.Type
            };
        }


        public static ApplicationUser MapUpdateUserRequestViewModelToApplicationUserEntity(string userId, UpdateUserRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new ApplicationUser
            {
                Id = userId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                //ApplicationUserStatusId = model.ApplicationUserStatusId,
                Mobile = model.Mobile,
                Street = model.Street,
                PostCode = model.PostCode,
                County = model.County,
                Town = model.Town,
                HouseNameNumber = model.HouseNameNumber,
            };
        }


        public static UpdateUserResponseViewModel MapApplicationUserEntityToUpdateUserResponseViewModel(ApplicationUser model)
        {
            if (model == null)
            {
                return null;
            }
            return new UpdateUserResponseViewModel
            {
                UserId = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                //ApplicationUserStatusId = model.ApplicationUserStatusId,
                //UserStatus = model.ApplicationUserStatus?.Status,
                Mobile = model.Mobile,
                HouseNameNumber = model.HouseNameNumber,
                Street = model.Street,
                PostCode = model.PostCode,
                Town = model.Town,
                County = model.County,
                //NewPassword = model.PasswordHash
            };
        }



        public static GetOperatorBothUserDetailsViewModel MapApplicationUserEntityToGetOperatorBothUserDetailsViewModel(ApplicationUser model)
        {
            if (model == null)
            {
                return null;
            }
            return new GetOperatorBothUserDetailsViewModel
            {
                ApplicationUserId = model.Id,
                Name = model.FirstName + " " + model.LastName,
                ProfileImageLink = model?.ProfileImageLink,
                ProfileImageName = model?.ProfileImageName,
                UserTypeId = model.ApplicationUserTypeId,
                UserTypeName = model?.ApplicationUserType?.Type
            };

        }
        public static IssueCategoryResponseViewModel MapIssueCategoryEntityToIssueCategoryResponseViewModel(IssueCategory model)
        {
            if (model == null)
            {
                return null;
            }
            return new IssueCategoryResponseViewModel
            {
                IssueCategoryId = model.IssueCategoryId,
                Category = model.Category,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }
        public static IssueCategory MapIssueCategoryRequestViewModelToIssueCategoryEntity(string CreatedBy,IssueCategoryRequestViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new IssueCategory
            {
                Category = model.Category,
                CreatedBy = CreatedBy,
            };
        }
        
        public static IssueCategoryNameViewModel MapIssueCategoryEntityToIssueCategoryNameViewModel(IssueCategory model)
        {

            if (model == null)
            {
                return null;
            }
            return new IssueCategoryNameViewModel
            {
                IssueCategoryId = model.IssueCategoryId,
                IssueCategoryName = model.Category,
            };
        }

        public static RiskAssessmentNameViewModel MapRiskAssessmentEntityToRiskAssessmentNameViewModel(RiskAssessment model)
        {

            if (model == null)
            {
                return null;
            }
            return new RiskAssessmentNameViewModel
            {
                RiskAssessmentId = model.RiskAssessmentId,
                Name = model.Name,
                Validity=model.Validity
            };
        }

        public static FarmResponseViewModel MapFarmEntityToFarmResponseViewModel(Farm model)
        {
            if (model == null)
            {
                return null;
            }
            return new FarmResponseViewModel
            {
                FarmId = model.FarmId,
                FarmName = model.FarmName,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.CreatedByUser?.FirstName + " " + model.CreatedByUser?.LastName,
                MasterAdminId = model.MasterAdminId,
                CreatedDate = model.CreatedDate,
            };
        }

        public static FarmDetailViewModel MapFarmEntityToFarmDetailViewModel(Farm model)
        {
            if (model == null)
            {
                return null;
            }
            return new FarmDetailViewModel
            {
                FarmId = model.FarmId,
                FarmName = model.FarmName,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.CreatedByUser?.FirstName + " " + model.CreatedByUser?.LastName,
                MasterAdminId = model.MasterAdminId,
                MasterAdminFirstName = model?.ApplicationUser?.FirstName,
                MasterAdminLastName =  model?.ApplicationUser?.LastName,
                MasterAdminEmail = model?.ApplicationUser?.Email,
                CreatedDate = model.CreatedDate,
            };
        }

        public static AddAlarmActionResponseViewModel MapAlarmActionEntityToAlarmActionResponseViewModel(AlarmAction model)
        {
            if (model == null)
            {
                return null;
            }


            var smsNumbers = string.IsNullOrEmpty(model.SmsNumber) ? null : JsonSerializer.Deserialize<List<SmsNumberViewModel>>(model.SmsNumber);
            return new AddAlarmActionResponseViewModel
            {
                AlarmActionId = model.AlarmActionId,
                MobileNumber = model.MobileNumber,
                MakeSound = model.MakeSound,
                SMS = model.SMS,
                CreatedAt = model.CreatedAt,
                UpdatedAt = model.UpdatedAt,
                MobileActionTypeId = model.MobileActionTypeId,
                MobileActionTypeName = model.MobileActionTypes.Type,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
               // SmsNumbers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SmsNumberViewModel>>(model.SmsNumber),
                SmsNumbers = smsNumbers?.Select(s => new SmsNumberViewModel
                {
                    SmsNumber = s.SmsNumber,
                }).ToList(),

            };
        }

        public static Farm MapFarmViewModelToFarmEntity(FarmViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new Farm
            {
                FarmId = Guid.NewGuid(),
                FarmName = model.FarmName,
                CreatedBy = model.CreatedBy,
                CreatedDate = DateTime.Now,
                MasterAdminId = model.MasterAdminId,
            };
        }

        public static AlarmAction MapAddAlarmViewModelToAddAlarmEntity(AddAlarmActionRequestViewModel model,string CreatedBy, string smsNumbersJson)
        {

            if (model == null)
            {
                return null;
            }
            return new AlarmAction
            {
                AlarmActionId = Guid.NewGuid(),
                MobileNumber = model.MobileNumber,
                SMS = model.SMS,
                MakeSound = model.MakeSound,
                SmsNumber = smsNumbersJson,
                CreatedAt = DateTime.Now,
                MobileActionTypeId = model.MobileActionTypeId,
                CreatedBy = CreatedBy,
            };
        }

        public static AlarmAction MapNewAlarmActionRequestViewModelToAlarmActionEntity(AddAlarmActionRequestViewModel model, string CreatedBy, Guid alarmActionId, string smsNumbersJson)
        {

            if (model == null)
            {
                return null;
            }
            return new AlarmAction
            {
                AlarmActionId = alarmActionId,
                MobileNumber = model.MobileNumber,
                SMS = model.SMS,
                MakeSound = model.MakeSound,
                SmsNumber = smsNumbersJson,
                UpdatedAt = DateTime.Now,
                MobileActionTypeId = model.MobileActionTypeId,
                CreatedBy = CreatedBy,
            };

        }
        public static FieldResponseViewModel MapFieldEntityToFieldResponseViewModel(Field model)
        {
            if (model == null)
            {
                return null;
            }
            return new FieldResponseViewModel
            {
                FieldId = model.FieldId,
                Name = model.Name,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                //FarmId = model.FarmId,
                FarmName = model.Farm?.FarmName,
            };
        }
        public static Field MapFieldRequestViewModelToFieldEntity(string CreatedBy, FieldRequestViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new Field
            {
                FieldId = Guid.NewGuid(),
                Name = model.Name,
                CreatedBy = CreatedBy,
                CreatedDate = DateTime.Now,
                //FarmId = model.FarmId,
            };
        }
        public static UserGroupResponseViewModel MapUserGroupEntityToUserGroupResponseViewModel(UserGroup model)
        {
            if (model == null)
            {
                return null;
            }
            return new UserGroupResponseViewModel
            {
                UserGroupId = model.UserGroupId,
                GroupName = model.GroupName,
                Description = model?.Description,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }
        public static UserGroup MapUserGroupRequestViewModelToUserGroupEntity(string CreatedBy, UserGroupRequestViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new UserGroup
            {
                GroupName = model.GroupName,
                Description = model?.Description,
                CreatedBy = CreatedBy,
            };
        }
        public static UserGroup MapUserGroupRequestViewModelToUserGroupEntity( UserGroupRequestViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new UserGroup
            {
                GroupName = model.GroupName,
                Description = model?.Description,
            };
        }
        public static UserGroupNameListViewModel MapUserGroupEntityToUserGroupNameListViewModel(UserGroup model)
        {

            if (model == null)
            {
                return null;
            }
            return new UserGroupNameListViewModel
            {
                UserGroupName = model.GroupName,
                UserGroupId = model.UserGroupId,
            };
        }

        public static MachineNameViewModel MapMachineEntityToMachineNameViewModel(Machine model)
        {

            if (model == null)
            {
                return null;
            }
            return new MachineNameViewModel
            {
                MachineId = model.MachineId,
                MachineName = model.Name,
                MachineImage = model?.MachineImage,
                MachineImageUniqueName = model?.MachineImageUniqueName,
            };
        }

        public static MachineNickNameViewModel MapMachineEntityToMachineNickNameViewModel(Machine model)
        {

            if (model == null)
            {
                return null;
            }
            return new MachineNickNameViewModel
            {
                MachineId = model.MachineId,
                NickName = model.NickName,
                MachineImage = model.MachineImage,
                QRCode = model.QRCode,
                Name = model.Name,
                Make = model.Make
            };
        }

        public static MachineNickNameViewModel MapDashbordMachineEntityToMachineNickNameViewModel(Machine model)
        {

            if (model == null)
            {
                return null;
            }
            return new MachineNickNameViewModel
            {
                MachineId = model.MachineId,
                NickName = model.NickName,
                Name = model.Name,
            };
        }
        public static MachineTypeNameViewModel MapMachineTypeEntityToMachineTypeNameViewModel(MachineType model)
        {

            if (model == null)
            {
                return null;
            }
            return new MachineTypeNameViewModel
            {
                MachineTypeId = model.MachineTypeId,
                MachineTypeName = model.Name,
                UnitTypeId = model.UnitsTypeId,
                UnitName = model.UnitsType.Units,
            };
        }
        
        //public static SmartCourseNameViewModel MapSmartCourseEntityToSmartCourseNameViewModel(SmartCourse model)
        //{

        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new SmartCourseNameViewModel
        //    {
        //        SmartCourseId = model.SmartCourseId,
        //        SmartCourseName = model.Name
        //    };
        //}
        public static WelfareRoutineResponseViewModel MapWelfareRoutineEntityToWelfareRoutineResponseViewModel(WelfareRoutine model)
        {
            if (model == null)
            {
                return null;
            }
            return new WelfareRoutineResponseViewModel
            {
                WelfareRoutineId = model.WelfareRoutineId,
                Name = model.Name,
                Minutes = model.Minutes,
                UserGroupId = model?.UserGroupId,
                GroupName = model.UserGroup?.GroupName,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }
        public static WelfareRoutine MapWelfareRoutineRequestViewModelToWelfareRoutineEntity(string CreatedBy, WelfareRoutineRequestViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new WelfareRoutine
            {
                Name = model.Name,
                Minutes = model.Minutes,
                UserGroupId = model?.UserGroupId,
                CreatedBy = CreatedBy,
            };
        }
        public static WelfareRoutine MapWelfareRoutineRequestViewModelToWelfareRoutineEntity(WelfareRoutineRequestViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new WelfareRoutine
            {
                Name = model.Name,
                Minutes = model.Minutes,
                UserGroupId = model?.UserGroupId,
            };
        }
        //public static SmartCourseViewModel MapSmartCourseToSmartCourseViewModel(SmartCourse model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new SmartCourseViewModel
        //    {
        //        SmartCourseId = model.SmartCourseId,
        //        Name = model.Name,
        //        CreatedBy = model.CreatedBy,
        //        CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
        //    };
        //}
        //public static SmartCourse MapSmartCourseViewModelToSmartCourse(string CreatedBy,SmartCourseViewModel model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new SmartCourse
        //    {
        //        SmartCourseId = model.SmartCourseId,
        //        Name = model.Name,
        //        CreatedBy = CreatedBy,
        //    };
        //}
        //public static SmartCourse MapSmartCourseViewModelToSmartCourse(SmartCourseViewModel model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new SmartCourse
        //    {
        //        SmartCourseId = model.SmartCourseId,
        //        Name = model.Name,
        //    };
        //}
        public static TrainingViewModel MapTrainingEntityToTrainingViewModel(Training model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingViewModel
            {
                TrainingId = model.TrainingId,
                Name = model.Name,
                Description = model.Description,
                Validity = model.Validity,
                Expires = model.Expires,
                Link = model.Link,
                Certification = model.Certification,
                IsArchived = model.IsArchived,
                //SmartCourseId = model.SmartCourseId,
                TrainingTypeId = model.TrainingTypeId,
                //SmartCourseName = model?.SmartCourse?.Name,
                TrainingTypeName = model?.TrainingType?.Type,
                DueDate = model.DueDate,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }

        public static TrainingWithFileURLsResponseViewModel MapTrainingEntityToTrainingWithFileURLsResponseViewModel(Training model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingWithFileURLsResponseViewModel
            {
                TrainingId = model.TrainingId,
                Name = model.Name,
                Description = model.Description,
                Validity = model.Validity,
                Expires = model.Expires,
                Link = model.Link,
                Certification = model.Certification,
                IsArchived = model.IsArchived,
                //SmartCourseId = model.SmartCourseId,
                TrainingTypeId = model.TrainingTypeId,
                //SmartCourseName = model?.SmartCourse?.Name,
                TrainingTypeName = model?.TrainingType?.Type,
                DueDate = model.DueDate,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                Files = model.TrainingFiles?.Select(f => f.FileUrl).ToList()
            };
        }

        public static Training MapTrainingViewModelToTrainingEntity(TrainingViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Training
            {
                TrainingId = model.TrainingId,
                Name = model.Name,
                Description = model.Description,
                Validity = model.Validity,
                Expires = model.Expires,
                Link=model.Link,
                Certification = model.Certification,
                DueDate = model.DueDate,
                //SmartCourseId = model.SmartCourseId,
                TrainingTypeId = model.TrainingTypeId,

            };
        }

        public static Training MapTrainingRequestViewModelToTrainingEntity(string CreatedBy, TrainingRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Training
            {
                TrainingId = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                Validity = model.Validity,
                Expires = model.Expires,
                Link=model.Link,
                Certification = model.Certification,
                IsArchived = model.IsArchived,
                //SmartCourseId = model.SmartCourseId,
                TrainingTypeId = (int)Core.Common.Enums.TrainigTypeEnum.SmartFarmer,
                CreatedDate = DateTime.Now,
                CreatedBy = CreatedBy,
            };
        }



        public static UpdateTrainingViewModel MapTrainingEntityToUpdateTrainingViewModel(Training model)
        {
            if (model == null)
            {
                return null;
            }
            return new UpdateTrainingViewModel
            {
                TrainingId = model.TrainingId,
                Name = model.Name,
                Description = model.Description,
                Validity = model.Validity,
                Expires = model.Expires,
                Link = model.Link,
                Certification = model.Certification,
                //SmartCourseId = model.SmartCourseId,
                TrainingTypeId = model.TrainingTypeId,
                //SmartCourseName = model?.SmartCourse?.Name,
                TrainingTypeName = model?.TrainingType?.Type,
                CreatedBy = model.CreatedBy,
                DueDate = model.DueDate,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }
        public static Training MapUpdateTrainingViewModelToTrainingEntity(UpdateTrainingViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Training
            {
                TrainingId = model.TrainingId,
                Name = model.Name,
                Description = model.Description,
                Validity = model.Validity,
                Expires = model.Expires,
                Link = model.Link,
                Certification = model.Certification,
                DueDate = model.DueDate,
                //SmartCourseId = model.SmartCourseId,
                TrainingTypeId = model.TrainingTypeId,
            };
        }




        public static TrainingFile MapTrainingFileViewModelToTrainingFileEntity(TrainingFileViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingFile
            {
                TrainingFileId = Guid.NewGuid(),
                FileUrl = model.FileUrl,
                FileUniqueName = model.FileUniqueName,
                FileName = model.FileName,
                TrainingId = model.TrainingId,
                CreatedDate = DateTime.Now
            };
        }

        public static TrainingFileViewModel MapTrainingFileEntityToTrainingFileViewModel(TrainingFile model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingFileViewModel
            {
                TrainingFileId = model.TrainingFileId,
                FileUrl = model.FileUrl,
                FileUniqueName = model.FileUniqueName,
                FileName = model.FileName,
                TrainingId = model.TrainingId,
            };
        }

        public static TrainingNameListViewModel MapTrainingEntityToTrainingNameListViewModel(Training model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingNameListViewModel
            {
                TrainingId = model.TrainingId,
                Name = model.Name,
            };
        }

        public static RiskAssessmentViewModel MapRiskAssessmentToRiskAssessmentViewModel(RiskAssessment model)
        {
            if (model == null)
            {
                return null;
            }
            return new RiskAssessmentViewModel
            {
                RiskAssessmentId = model.RiskAssessmentId,
                Name = model.Name,
                Validity = model.Validity,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                CreatedDate=model.CreatedDate
            };
        }

        public static RiskAssessmentLogViewModel MapRiskAssessmentLogToRiskAssessmentLogViewModel(RiskAssessmentLog model)
        {
            if (model == null)
            {
                return null;
            }

            DateTime? expiresAt= null;
            if (model.RiskAssessmentId != null && model?.RiskAssessment?.Validity != null)
            {
                expiresAt = model.RiskAssessment.Validity.HasValue ? model.CreatedDate.AddMonths(model.RiskAssessment.Validity.Value) : (DateTime?)null;
            }
            
            return new RiskAssessmentLogViewModel
            {
                RiskAssessmentId = model.RiskAssessmentId,
                RiskAssessmentLogId = model.RiskAssessmentLogId,
                RiskAssessmentName = model?.RiskAssessment?.Name,
                Name = model.Name,
                Validity = model?.RiskAssessment?.Validity,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                Expires = model.Expires,
                InitialRiskId = model?.InitialRiskId,
                InitialRiskName = model?.InitialRisk?.RiskValue,
                AdjustedRiskId = model?.AdjustedRiskId,
                AdjustedRiskName = model?.AdjustedRisk?.RiskValue,
                Archived = model.Archived,
                CompletedDate = model.CompletedDate,
                //ActionId = model.ActionId,
                //ActionName = model?.Action?.Name,
                ExpiresAt = expiresAt,
                CreatedDate=model.CreatedDate,

            };
        }

        public static RiskAssessmentResponseViewModel MapRiskAssessmentToRiskAssessmentResponseViewModel(RiskAssessment model)
        {
            if (model == null)
            {
                return null;
            }
            return new RiskAssessmentResponseViewModel
            {
                RiskAssessmentId = model.RiskAssessmentId,
                Name = model.Name,
                Validity = model.Validity,
                CreatedBy = model.CreatedBy,
                CreatedDate=model.CreatedDate,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                RiskAssessmentFiles = model.RiskAssessmentFiles?.Select(f => new RiskAssessmentFileViewModel
                {
                    RiskAssessmentFileId = f.RiskAssessmentFileId,             
                    FileUrl = f.FileUrl,
                    FileUniqueName = f.FileUniqueName,
                    FileName = f.FileName,
                    RiskAssessmentId =f.RiskAssessmentId
                }).ToList()
            };
        }

        public static RiskAssessmentWithFileURLsResponseViewModel MapRiskAssessmentEntityToRiskAssessmentWithFileURLsResponseViewModel(RiskAssessment model)
        {
            if (model == null)
            {
                return null;
            }
            return new RiskAssessmentWithFileURLsResponseViewModel
            {
                RiskAssessmentId = model.RiskAssessmentId,
                Name = model.Name,
                Validity = model.Validity,
                CreatedBy = model.CreatedBy,
                CreatedDate=model.CreatedDate,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                Files = model.RiskAssessmentFiles?.Select(f => f.FileUrl).ToList()
            };
        }

        public static RiskAssessmentLogViewModel MapRiskAssessmentLogEntityToRiskAssessmentLogResponseViewModel(RiskAssessmentLog model)
        {
            if (model == null)
            {
                return null;
            }
            DateTime? expiresAt = null;
            if (model.RiskAssessmentId != null && model?.RiskAssessment?.Validity != null)
            {
                expiresAt = model.RiskAssessment.Validity.HasValue ? model.CreatedDate.AddMonths(model.RiskAssessment.Validity.Value) : (DateTime?)null;
            }
           
            bool Expired = false;
            if (expiresAt != null)
            {
                if(expiresAt.Value < DateTime.Now)
                {
                    Expired = true;
                }
            }
            return new RiskAssessmentLogViewModel
            {
                RiskAssessmentId = model.RiskAssessmentId,
                RiskAssessmentLogId = model.RiskAssessmentLogId,
                RiskAssessmentName = model?.RiskAssessment?.Name,
                Name = model.Name,
                Validity = model?.RiskAssessment?.Validity,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                Expires = model.Expires,
                InitialRiskId = model?.InitialRiskId,
                InitialRiskName = model?.InitialRisk?.RiskValue,
                AdjustedRiskId = model?.AdjustedRiskId,
                AdjustedRiskName = model?.AdjustedRisk?.RiskValue,
                Archived = model.Archived,
                CompletedDate = model.CompletedDate,
                //ActionId = model.ActionId,
                //ActionName = model?.Action?.Name,
                ExpiresAt = expiresAt,
                Expired = Expired,
                CreatedDate=model.CreatedDate
            };
        }

        public static RiskAssessment MapRiskAssessmentViewModelToRiskAssessment(RiskAssessmentViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new RiskAssessment
            {
                RiskAssessmentId = model.RiskAssessmentId,
                Name = model.Name,
                Validity = model.Validity,
                CreatedBy = model.CreatedBy,
            };
        }

        public static RiskAssessmentLog MapRiskAssessmentLogViewModelToRiskAssessmentLog(RiskAssessmentLogViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new RiskAssessmentLog
            {
                RiskAssessmentLogId = model.RiskAssessmentLogId,
                RiskAssessmentId = model.RiskAssessmentId,
                Name = model.Name,
                CompletedDate = model.CompletedDate,
                Expires = model.Expires,
                InitialRiskId = model.InitialRiskId,
                AdjustedRiskId = model.AdjustedRiskId,
                //ActionId = model.ActionId,
                Archived = model.Archived,
                Validity = model.Validity,
                CreatedBy = model.CreatedBy,
            };
        }

        public static RiskAssessment MapRiskAssessmentRequestViewModelToRiskAssessment(string CreatedBy, RiskAssessmentRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new RiskAssessment
            {
                RiskAssessmentId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Name = model.Name,
                Validity = model.Validity,
                CreatedBy = CreatedBy,
            };
        }

        public static RiskAssessmentLog MapRiskAssessmentLogRequestViewModelToRiskAssessmentLog(string CreatedBy, RiskAssessmentLogRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new RiskAssessmentLog
            {
                RiskAssessmentLogId = Guid.NewGuid(),
                RiskAssessmentId = model?.RiskAssessmentId,
                CreatedDate = DateTime.Now,
                Name = model.Name,
                Expires = model.Expires,
                InitialRiskId = model?.InitialRiskId,
                AdjustedRiskId = model?.AdjustedRiskId,
                Archived = model.Archived,
                CompletedDate =DateTime.Now,
                CreatedBy = CreatedBy,
                //ActionId = model?.ActionId,
            };
        }

        public static RiskAssessmentFile MapRiskAssessmentFileViewModelToRiskAssessmentFileEntity(RiskAssessmentFileViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new RiskAssessmentFile
            {
                RiskAssessmentFileId = Guid.NewGuid(),
                FileUrl = model.FileUrl,
                FileUniqueName = model.FileUniqueName,
                FileName = model.FileName,
                RiskAssessmentId = model.RiskAssessmentId,
                CreatedDate = DateTime.Now
            };
        }

        public static RiskAssessmentFileViewModel MapRiskAssessmentFileEntityToRiskAssessmentFileViewModel(RiskAssessmentFile model)
        {
            if (model == null)
            {
                return null;
            }
            return new RiskAssessmentFileViewModel
            {
                RiskAssessmentFileId = model.RiskAssessmentFileId,
                FileUrl = model.FileUrl,
                FileUniqueName = model.FileUniqueName,
                FileName = model.FileName,
                RiskAssessmentId = model.RiskAssessmentId,
            };
        }



        public static EquipmentResponseViewModel MapMachineEntityToEquipmentResponseViewModel(Machine model)
        {
            if (model == null)
            {
                return null;
            }
            return new EquipmentResponseViewModel
            {
                EquipmentId = model.MachineId,
                EquipmentImage = model.MachineImage,
                EquipmentImageUniqueName = model.MachineImageUniqueName,
                NickName = model.NickName,
                Make = model.Make,
                Model = model.Model,
                Name = model?.Name,
                Location = model.Location,
                EquipmentStatusId = model.MachineStatusId,
                Status = model?.MachineStatus?.Status,
                OperatorId = model?.OperatorId,
                OperatorName = model?.Operator?.FirstName + " " + model?.Operator?.LastName,
            };
        }
        
        public static MachineResponseViewModel MapMachineEntityToMachineResponseViewModel(Machine model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineResponseViewModel
            {
                MachineId = model.MachineId,
                NickName = model.NickName,
                Make = model.Make,
                Model = model.Model,
                SerialNumber = model.SerialNumber,
                MachineImage = model?.MachineImage,
                MachineImageUniqueName = model?.MachineImageUniqueName,
                QRCode = model?.QRCode,
                QRUniqueName = model?.QRUniqueName,
                Name = model?.Name,
                Description = model.Description,
                ManufacturedDate = model?.ManufacturedDate,
                PurchaseDate = model?.PurchaseDate,
                LOLERDate = model?.LOLERDate,
                MOTDate = model?.MOTDate,
                ServiceInterval = model.ServiceInterval,
                MachineTypeId = model.MachineTypeId,
                MachineType = model?.MachineType?.Name,
                MachineStatusId = model.MachineStatusId,
                Status = model?.MachineStatus?.Status,
                OperatorId = model?.OperatorId,
                OperatorName = model?.Operator?.FirstName + " " + model?.Operator?.LastName,
                WorkingIn = model.WorkingIn,
                ApplicationUserId = model.ApplicationUserId,
                ApplicationUserName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
                MachineCategoryId = model.MachineCategoryId,
                MachineCategoryName = model?.MachineCategory?.Name,
                Location = model?.Location,
                InSeason = model.InSeason,
                UnitTypeId=model.MachineType.UnitsTypeId,
                Unit =model?.MachineType?.UnitsType?.Units,
                MachineCode=model.MachineCode,
                Archived = model.Archived,
                ResultId = model.ResultId,
                Result = model?.CheckResult?.Result,
                RiskAssessmentId = model?.MachineType?.RiskAssessmentId,
                RiskAssessmentName = model?.MachineType?.RiskAssessment?.Name,
                Validity = model?.MachineType?.RiskAssessment?.Validity,
            };
        }
        
        public static MachineDetailViewModel MapMachineEntityToMachineDetailViewModel(Machine model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineDetailViewModel
            {
                MachineId = model.MachineId,
                MachineName = model.Name,
                NickName = model.NickName,
                MachineImage = model?.MachineImage,
                MachineTypeId = model.MachineTypeId,
                MachineTypeName = model?.MachineType?.Name,
                MachineStatusId = model.MachineStatusId,
                MachineStatusName = model?.MachineStatus?.Status,
            };
        }

        public static EventResponseViewModel MapMachineHistoryEntityToMachineHistoryViewModel(Event model)
        {
            if (model == null)
            {
                return null;
            }

            return new EventResponseViewModel
            {
                EventId = model.EventId, 
                CreatedDate = model.CreatedDate,
                EventTypeId = model.EventTypeId,
                EventType = model?.EventType?.Type,
                CreatedBy = model?.CreatedBy,
                CreatedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
                MachineId = model?.MachineId,
                MachineName = model?.Machine.Name,                   
                Message = model?.Message,
                Location = model?.Location,
                ResultId = model?.CheckListMachineMapping?.ResultId,
                Result = model?.CheckListMachineMapping?.CheckResult?.Result
            };
        } 
        public static PreCheckEventResponseViewModel MapMachineHistoryEntityToMachinePreCheckHistoryViewModel(Event model)
        {
            if (model == null)
            {
                return null;
            }

            return new PreCheckEventResponseViewModel
            {
                EventId = model.EventId, 
                CreatedDate = model.CreatedDate,
                EventTypeId = model.EventTypeId,
                EventType = model?.EventType?.Type,
                CreatedBy = model?.CreatedBy,
                CreatedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
                MachineId = model?.MachineId,
                MachineName = model?.Machine.Name,                   
                Message = model?.Message,
                Location = model?.Location,
                ResultId = model?.CheckListMachineMapping?.ResultId,
                Result = model?.CheckListMachineMapping?.CheckResult?.Result,
            };
        }

        public static RecentMachineResponseViewModel MapMachineEntityToRecentMachineResponseViewModel(Machine model)
        {
            if (model == null)
            {
                return null;
            }
            return new RecentMachineResponseViewModel
            {
                MachineId = model.MachineId,
                MachineImage = model.MachineImage,
                NickName = model.NickName,
                Make = model.Make,
                Name = model?.Name,
                MachineStatusId = model.MachineStatusId,
                Status = model?.MachineStatus?.Status,
                Location = model.Location,
                MachineTypeId = model.MachineTypeId,
                MachineTypeName = model?.MachineType?.Name
            };
        }

        public static MachineWithOperatorResponseViewModel MapMachineByOperatorEntityToMachineByOperatorResponseViewModel(Machine model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineWithOperatorResponseViewModel
            {
                MachineId = model.MachineId,
                NickName = model?.NickName,
                Make = model.Make,
                Model = model?.Model,
                Name = model.Name,
                MachineTypeId = model.MachineTypeId,
                MachineType = model?.MachineType?.Name,
                MachineStatusId = model.MachineStatusId,
                Status = model?.MachineStatus?.Status,
                OperatorId = model?.OperatorId,
                OperatorName = model?.Operator?.FirstName + " " + model?.Operator?.LastName,
                MachineCategoryId = model.MachineCategoryId,
                MachineCategoryName = model?.MachineCategory?.Name,
                Location = model?.Location,
                InSeason = model.InSeason,
                MachineImage = model?.MachineImage,
                MachineImageUniqueName = model?.MachineImageUniqueName
            };
        }

        public static Machine MapMachineRequestViewModelToMachineEntity(string createdBy, MachineRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Machine
            {
                MachineId = Guid.NewGuid(),
                NickName = model?.NickName,
                Make = model.Make,
                Model = model?.Model,
                SerialNumber = model?.SerialNumber,
                Name = model.Name,
                Description = model?.Description,
                ManufacturedDate = model?.ManufacturedDate,
                PurchaseDate = model?.PurchaseDate,
                LOLERDate = model?.LOLERDate,
                MOTDate = model?.MOTDate,
                ServiceInterval = model.ServiceInterval,
                MachineTypeId = model.MachineTypeId,
                CreatedDate = DateTime.Now,
                MachineStatusId = (int)Core.Common.Enums.MachineStatusEnum.Idle,
                WorkingIn = model.WorkingIn,
                ApplicationUserId = createdBy,
                MachineCategoryId = model.MachineCategoryId,
                //Location = model.Location,
                InSeason = model.InSeason,
            };
        }

        public static Machine MapMachineResponseViewModelToMachineEntity(MachineResponseViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new Machine
            {
                MachineId = model.MachineId,
                NickName = model?.NickName,
                Make = model.Make,
                Model = model?.Model,
                SerialNumber = model?.SerialNumber,
                QRCode = model?.QRCode,
                QRUniqueName = model?.QRUniqueName,
                Name = model.Name,
                Description = model?.Description,
                ManufacturedDate = model?.ManufacturedDate,
                PurchaseDate = model?.PurchaseDate,
                LOLERDate = model?.LOLERDate,
                ServiceInterval = model.ServiceInterval,
                MachineTypeId = model.MachineTypeId,
                MOTDate = model?.MOTDate,
                WorkingIn = model.WorkingIn,
                MachineCategoryId = model.MachineCategoryId,
                //Location = model.Location,
                InSeason = model.InSeason,
                Archived = model.Archived,
            };
        }

        public static MachineCategoryResponseViewModel MapMachineCategoryEntityToMachineCategoryResponseViewModel(MachineCategory model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineCategoryResponseViewModel
            {
                MachineCategoryId = model.MachineCategoryId,
                Name = model.Name,
                Description = model.Description,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }

        public static MachineCategory MapMachineCategoryRequestViewModelToMachineCategoryEntity(string CreatedBy, MachineCategoryRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineCategory
            {
                MachineCategoryId = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                CreatedDate = DateTime.Now,
                CreatedBy = CreatedBy,
            };
        }

        public static MachineCategoryNameViewModel MapMachineCategoryEntityToMachineCategoryNameViewModel(MachineCategory model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineCategoryNameViewModel
            {
                MachineCategoryId = model.MachineCategoryId,
                MachineCategoryName = model.Name,
            };
        }

        public static MachineCategory MapMachineCategoryResponseViewModelToMachineCategoryEntity(MachineCategoryResponseViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new MachineCategory
            {
                MachineCategoryId = model.MachineCategoryId,
                Name = model.Name,
                Description = model.Description,
                CreatedDate = DateTime.Now,
            };
        }

        public static FileViewModel MapMachineEntityToFileViewModel(Machine model)
        {

            if (model == null)
            {
                return null;
            }
            return new FileViewModel
            {
                Name = model.QRUniqueName,
                FileLink = model.QRCode
            };
        }

        public static MachineTypeViewModel MapMachineTypeToMachineTypeViewModel(MachineType model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineTypeViewModel
            {
                MachineTypeId = model.MachineTypeId,
                Name = model.Name,
                NeedsTraining = model.NeedsTraining,
                UnitsTypeId = model.UnitsTypeId,
                Units = model.UnitsType?.Units,
                RiskAssessmentId = model.RiskAssessmentId,  
                TrainingId = model?.TrainingId,
                TrainingName = model.Training?.Name,
                RiskAssessmentName=model?.RiskAssessment?.Name,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }

        public static MachineType MapMachineTypeViewModelToMachineType(MachineTypeViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineType
            {
                MachineTypeId = model.MachineTypeId,
                Name = model.Name,
                NeedsTraining = model.NeedsTraining,
                TrainingId = model?.TrainingId,
                RiskAssessmentId = model?.RiskAssessmentId,
                UnitsTypeId = model.UnitsTypeId,
            };
        }

        public static MachineType MapMachineTypeRequestViewModelToMachineType(string CreatedBy, MachineTypeRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineType
            {
                MachineTypeId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Name = model.Name,
                NeedsTraining = model.NeedsTraining,
                TrainingId = model?.TrainingId,
                RiskAssessmentId = model.RiskAssessmentId,
                UnitsTypeId = model.UnitsTypeId,
                CreatedBy = CreatedBy,
            };
        }

        public static Event MapEventRequestViewModelToEventEntity(int EventTypeId, string CreatedBy, EventRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Event
            {
                EventId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Location = model.Location,
                CreatedBy = CreatedBy,
                MachineId = null,
                EventTypeId = EventTypeId,
                ShowAppPopup=false,
                ShowWebPopup=false
            };
        }
        
        public static Event MapEventRequestViewModelToEventEntity(int EventTypeId, string CreatedBy, EventRequestViewModel model, Guid machineId)
        {
            if (model == null)
            {
                return null;
            }
            return new Event
            {
                EventId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                Location = model.Location,
                CreatedBy = CreatedBy,
                MachineId = machineId,
                EventTypeId = EventTypeId,
                ShowAppPopup = false,
                ShowWebPopup = false
            };
        }

        public static EventResponseViewModel MapEventEntityToEventResponseViewModel(Event model)
        {
            if (model == null)
            {
                return null;
            }
            return new EventResponseViewModel
            {
                EventId = model.EventId,
                Location = model.Location,
                Message = model.Message,
                MachineId = model.MachineId,
                MachineName = model.Machine?.Name,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                EventTypeId = model.EventTypeId,
                EventType = model.EventType.Type,
                CreatedDate = model.CreatedDate,
                ShowWebPopup=model.ShowWebPopup,
                

            };
        }
        public static Event MapEventResponseViewModelToEventEntity(EventResponseViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Event
            {
                Location = model.Location,
                Message = model.Message,
                MachineId = model.MachineId,
            };
        }

        public static CheckListMachineMapping MapStartCheckListViewModelToCheckListMachineMappingEntity(string CreatedBy, StartCheckListViewModel model, int resultId)
        {
            if (model == null)
            {
                return null;
            }
            var checkListMachineMappingId = Guid.NewGuid();
            return new CheckListMachineMapping
            {
                CheckListMachineMappingId = checkListMachineMappingId,
                CreatedDate = DateTime.Now,
                CheckListId = model.CheckListId,
                MachineId = model.MachineId,
                OperatorId = CreatedBy,
                ResultId = resultId,
                CheckListItemAnswerMappings = model.Items?.Select(item => new CheckListItemAnswerMapping
                {
                    CheckListItemAnswerMappingId = Guid.NewGuid(),
                    CheckListMachineMappingId = checkListMachineMappingId,
                    CreatedDate = DateTime.Now,
                    Answer = item.Answer,
                    CheckListItemId = item.CheckListItemId
                }).ToList()
            };
        }
        
        public static StartCheckListResponseViewModel MapCheckListMachineMappingEntityToStartCheckListResponseViewModel(CheckListMachineMapping model)
        {
            if (model == null)
            {
                return null;
            }
            return new StartCheckListResponseViewModel
            {
                CheckListMachineMappingId = model.CheckListMachineMappingId,
                CreatedDate = model.CreatedDate,
                CheckListId = model.CheckListId,
                CheckListName = model?.CheckList?.Name,
                MachineId = model.MachineId,
                MachineName = model?.Machine?.Name,
                OperatorId = model.OperatorId,
                ResultId = model.ResultId,
                Result = model?.CheckResult?.Result,
                OperatorName = model?.Operator?.FirstName + " " + model?.Operator?.LastName,
                ChecListItemAnswerMappings = model.CheckListItemAnswerMappings?.Select(item => new CheckListItemAnswerResponseViewModel
                {
                    ChecListItemAnswerMappingId = item.CheckListItemAnswerMappingId,
                    CreatedDate = item.CreatedDate,
                    Answer = item.Answer,
                    CheckListItemId = item.CheckListItemId
                }).ToList()
            };
        }
        
        public static CheckListViewModel MapCheckListEntityToCheckListViewModel(CheckList model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckListViewModel
            {
                CheckListId = model.CheckListId,
                Name = model.Name,
                Frequency = model.Frequency,
                CheckTypeId = model.CheckTypeId,
                FrequencyTypeId = model.FrequencyTypeId,
                MachineTypeId = model.MachineTypeId,
                CheckType = model?.CheckType?.Type,
                FrequencyType = model?.FrequencyType?.Type,
                MachineTypeName = model?.MachineType?.Name,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }
        public static MachineIssueViewModel MapIssueEntityToMachineIssueViewModel(Issue model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineIssueViewModel
            {
                IssueId = model.IssueId,
                IssueTitle = model.IssueTitle,
                Description = model.Description,
                IssueCategoryId = model.IssueCategoryId,
                IssueTypeId = (int)(model?.IssueTypeId),
                IsTargetDateExist = model.IsTargetDateExist,
                TargetDate = model.TargetDate,
                IssueCategory= model.IssueCategory?.Category,
                IssueNo= model.IssueNo,
                IssueStatus= model.IssueStatus?.Status,
                IssueStatusId=model.IssueStatusId,
                IssueType=model.IssueType?.Type,
                CreatedDate=model.CreatedDate,
                ResolvedBy = model.ResolvedBy,
                ResolvedByName = model?.ApplicationUser?.FirstName+" "+model?.ApplicationUser?.LastName,
                CreatedBy=model.CreatedBy,
                CreatedByName = model?.Operator?.FirstName + " " + model?.Operator?.LastName,

            };
        }

        public static CheckList MapCheckListViewModelToCheckListEntity(CheckListViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckList
            {
                CheckListId = model.CheckListId,
                Name = model.Name,
                Frequency = model.Frequency,
                CheckTypeId = model.CheckTypeId,
                FrequencyTypeId = model.FrequencyTypeId,
                MachineTypeId = model.MachineTypeId,
            };
        }

        public static CheckList MapCheckListRequestViewModelToCheckListEntity(string CreatedBy, CheckListRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckList
            {
                CheckListId = Guid.NewGuid(),
                Name = model.Name,
                Frequency = model.Frequency,
                FrequencyTypeId = model.FrequencyTypeId,
                CheckTypeId = model.CheckTypeId,
                MachineTypeId = model.MachineTypeId,
                CreatedBy = CreatedBy,
                CreatedDate = DateTime.Now,
                OperatorId = null,
                LastCheckDate = null,
                NextDueDate = null,
            };
        }

        public static CheckListNameListViewModel MapCheckListEntityToCheckListNameListViewModel(CheckList model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckListNameListViewModel
            {
                CheckListId = model.CheckListId,
                Name = model.Name,
            };
        }

        public static CheckListItemViewModel MapCheckListItemEntityToCheckListItemViewModel(CheckListItem model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckListItemViewModel
            {
                CheckListItemId = model.CheckListItemId,
                Name = model.Name,
                Instruction = model.Instruction,
                Order = model.Order,
                Priority = model.Priority,
                CheckListId = model.CheckListId,
            };
        }

        public static CheckListItem MapCheckListItemRequestViewModelToCheckListItemsListViewModel(Guid checkListId, CheckListItemsListViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckListItem
            {
                CheckListItemId = Guid.NewGuid(),
                Name = model.Name,
                Instruction = model.Instruction,
                Order = model.Order,
                Priority= model.Priority,
                CheckListId = checkListId,
            };
        }
        
        public static CheckListItem MapCheckListItemRequestViewModelToCheckListItemsListViewModelNull(Guid checkListId, CheckListItemsListViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckListItem
            {
                CheckListItemId = model.CheckListItemId.Value,
                Name = model.Name,
                Instruction = model.Instruction,
                Order = model.Order,
                Priority= model.Priority,
                CheckListId = checkListId,
            };
        }
        public static CheckList MapOperatorCheckListViewModelToOperatorCheckListEntity(OperatorCheckListResponseViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new CheckList
            {
                CheckListId = model.CheckListId,
                OperatorId = model.OperatorId,
                LastCheckDate = model.LastCheckDate,
                NextDueDate = model.NextDueDate,
            };
        }
        public static OperatorCheckListResponseViewModel MapOperatorCheckListEntityToOperatorCheckListViewModel(CheckList model)
        {
            if (model == null)
            {
                return null;
            }
            return new OperatorCheckListResponseViewModel
            {
                CheckListId = model.CheckListId,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                OperatorId = model.OperatorId,
                OperatorName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                LastCheckDate = model.LastCheckDate,
                NextDueDate = model.NextDueDate,
            };
        }



        public static MachineCheckListViewModel MapCheckListEntityToMachineCheckListViewModel(CheckList model)
        {
            if (model == null)
            {
                return null;
            }
            return new MachineCheckListViewModel
            {
                CheckListId = model.CheckListId,
                Name = model.Name,
                LastCheckDate = model.LastCheckDate,
                CheckTypeId = model.CheckTypeId,
                CheckType = model?.CheckType?.Type,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }
        public static ApplicationUser MapApplicationUserProfileImageViewModelToApplicationUserEntity(ApplicationUserProfileImageViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new ApplicationUser
            {
                Id = model.ApplicationUserId,
                ProfileImageLink = model.ProfileImageLink,
                ProfileImageName = model.ProfileImageName,
            };
        }
        
        public static ApplicationUserProfileImageViewModel MapApplicationUserEntityToApplicationUserProfileImageViewModel(ApplicationUser model)
        {
            if (model == null)
            {
                return null;
            }
            return new ApplicationUserProfileImageViewModel
            {
                ApplicationUserId = model.Id,
                ProfileImageLink = model.ProfileImageLink,
                ProfileImageName = model.ProfileImageName,
            };
        }
        
        public static FileViewModel MapApplicationUserEntityToFileViewModel(ApplicationUser model)
        {
            if (model == null)
            {
                return null;
            }
            return new FileViewModel
            {
                FileLink = model.ProfileImageLink,
                Name = model.ProfileImageName,
            };
        }
        public static Issue MapIssueRequestViewModelToIssueEntity(string CreatedBy, IssueRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Issue
            {
                IssueId = Guid.NewGuid(),
                CreatedDate = DateTime.Now,
                CreatedBy = CreatedBy,
                IssueTitle = model.IssueTitle,
                Description = model.Description,
                IssueCategoryId = model?.IssueCategoryId,
                IssueTypeId = model.IssueTypeId,
                MachineId = model.MachineId,
                RiskAssessmentLogId = model?.RiskAssessmentLogId,
                ResolvedBy = null,
                ResolvedDate = null,
                IsTargetDateExist = model.IsTargetDateExist,
                TargetDate = model?.TargetDate,
                IssueStatusId = (int)Core.Common.Enums.IssueStatusEnum.Open,
                
            };
        }
        public static IssueResponseViewModel MapIssueEntityToIssueResponseViewModel(Issue model)
        {
            if (model == null)
            {
                return null;
            }
            return new IssueResponseViewModel
            {
                IssueId = model.IssueId,
                IssueNo = model.IssueNo,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy,
                CreatedByName = model?.Operator?.FirstName + " " + model?.Operator?.LastName,
                ResolvedBy = model.ResolvedBy,
                ResolvedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
                ResolvedByProfile = model?.ApplicationUser?.ProfileImageLink,
                IssueTitle = model.IssueTitle,
                Description = model.Description,
                IssueCategoryId = model?.IssueCategoryId,
                IssueCategoryName = model?.IssueCategory?.Category,
                IssueTypeId = model.IssueTypeId,
                IssueTypeName = model?.IssueType?.Type,
                MachineId = model?.MachineId,
                MachineName = model?.Machine?.Name,
                MachineNickName = model?.Machine?.NickName,
                RiskAssessmentLogId = model?.RiskAssessmentLogId,
                RiskAssessmentLogName = model?.RiskAssessmentLog?.Name,
                ResolvedDate = model?.ResolvedDate,
                IssueStatusId= model.IssueStatusId,
                IssueStatusName = model?.IssueStatus?.Status,
                IsTargetDateExist = model.IsTargetDateExist,
                TargetDate = model.TargetDate,
                Note=model.Note
            };
        }
        
        public static Issue MapIssueResponseViewModelToIssueEntity(IssueResponseViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new Issue
            {
                IssueId = model.IssueId,
                ResolvedBy = model.ResolvedBy,
                IssueTitle = model.IssueTitle,
                Description = model.Description,
                IssueCategoryId = model.IssueCategoryId,
                IssueTypeId = (int)(model?.IssueTypeId),
                MachineId = model.MachineId,
                RiskAssessmentLogId = model?.RiskAssessmentLogId,
                ResolvedDate = model?.ResolvedDate,
                IssueStatusId= model.IssueStatusId,
                IsTargetDateExist = model.IsTargetDateExist,
                TargetDate = model.TargetDate,
                Note=model.Note
            };
        }
        //public static IssueResponseViewModel MapIssueEntityToIssueViewModel(Issue model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new IssueResponseViewModel
        //    {
        //        IssueId = model.IssueId,
        //        ResolvedBy = model.ResolvedBy,
        //        IssueTitle = model.IssueTitle,
        //        Description = model.Description,
        //        IssueCategoryId = model.IssueCategoryId,
        //        IssueTypeId = (int)(model?.IssueTypeId),
        //        MachineId = model.MachineId,
        //        RiskAssessmentLogId = model?.RiskAssessmentLogId,
        //        ResolvedDate = model?.ResolvedDate,
        //        IssueStatusId = model.IssueStatusId,
        //        IsTargetDateExist = model.IsTargetDateExist,
        //        TargetDate = model.TargetDate,
        //    };
        //}

        public static IssueFile MapIssueFileViewModelToIssueFileEntity(IssueFileViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new IssueFile
            {
                IssueFileId = Guid.NewGuid(),
                IssueId = model.IssueId,
                FileURL = model.FileURL,
                FileUniqueName = model.FileUniqueName,
            };
        }
        
        public static IssueFileViewModel MapIssueFileEntityToIssueFileViewModel(IssueFile model)
        {
            if (model == null)
            {
                return null;
            }
            return new IssueFileViewModel
            {
                IssueFileId = model.IssueFileId,
                IssueId = model.IssueId,
                FileURL = model.FileURL,
                FileUniqueName = model.FileUniqueName,
            };
        }
        
        public static ExistingIssuesResponseViewModel MapIssueEntityToExistingIssuesResponseViewModel(Issue model)
        {
            if (model == null)
            {
                return null;
            }
            return new ExistingIssuesResponseViewModel
            {
                IssueId = model.IssueId,
                IssueTitle = model.IssueTitle,
            };
        }
        //public static IssueComment MapIssueCommentRequestViewModelToIssueCommentEntity(string CreatedBy, IssueCommentRequestViewModel model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new IssueComment
        //    {
        //        IssueCommentId = Guid.NewGuid(),
        //        CreatedDate = DateTime.Now,
        //        CreatedBy = CreatedBy,
        //        IssueId = model.IssueId,
        //        Comment = model.Comment,
        //    };
        //}
        //public static IssueCommentResponseViewModel MapIssueCommentEntityToIssueCommentResponseViewModel(IssueComment model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new IssueCommentResponseViewModel
        //    {
        //        IssueCommentId = model.IssueCommentId,
        //        Comment = model.Comment,
        //        IssueId = model.IssueId,
        //        IssueName = model?.Issue?.IssueTitle,
        //        CreatedDate = model.CreatedDate,
        //        CreatedBy = model.CreatedBy,
        //        CreatedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
        //    };
        //}
        public static TimeSheetSearchViewModel MapTimeSheetToTimeSheetViewModel(Event model)
        {
            if (model == null)
            {
                return null;
            }
            return new TimeSheetSearchViewModel
            {
                CreatedDate = model.CreatedDate,
                EventTypeId = model.EventTypeId,
                EventTypeName = model.EventType.Type,
            };
        }

        public static TimeSheetListSearchViewModel MapTimeSheetListToTimeSheetViewModelList(Event model)
        {
            if (model == null)
            {
                return null;
            }
            return new TimeSheetListSearchViewModel
            {
                CreatedDate = model.CreatedDate,
                Date = model.CreatedDate.Date.ToString("yyyy-MM-dd"),
                CreatedDay = model.CreatedDate.DayOfWeek.ToString(),
                EventTypeId = model.EventTypeId,
                //EventTypeName = model.EventType.Type,
                CreatedBy = model.CreatedBy,
                CreatedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
            };
        }
        
        public static MachineOperatorMapping MapMachineOperatorMappingViewModelToMachineOperatorMappingEntity(Guid machineId, string operatorId)
        {
            return new MachineOperatorMapping
            {
                MachineOperatorMappingId = Guid.NewGuid(),
                MachineId = machineId,
                OperatorId = operatorId,
                CreatedDate = DateTime.Now,
                IsActive = true,
            };
        }

        public static OperatorUserResponseViewModel MapOperatorEntityToOperatorResponseViewModel(ApplicationUser model)
        {
            if (model == null)
            {
                return null;
            }
            return new OperatorUserResponseViewModel
            {
                ApplicationUserId = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                ProfileImageLink = model.ProfileImageLink,
                ProfileImageName = model.ProfileImageName,
                OperatorStatusId = model.ApplicationUserStatusId,
                OperatorStatusName = model?.ApplicationUserStatus?.Status,
                Location = model.Location,
            };
        }



        //public static SmartQuestion MapSmartQuestionViewModelToSmartQuestionEntity(string CreatedBy ,AddQuestionRequestViewModel model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new SmartQuestion
        //    {
        //        SmartQuestionId = Guid.NewGuid(),
        //        Name = model.Name,
        //        TrainingId = model.TrainingId,
        //        CreatedDate = DateTime.Now,
        //        CreatedBy = CreatedBy,
        //    };
        //}

        //public static AddQuestionResponseViewModel MapSmartQuestionEntityToSmartQuestionViewModel(SmartQuestion model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new AddQuestionResponseViewModel
        //    {
        //        SmartQuestionId =model.SmartQuestionId,
        //        Name = model.Name,
        //        CreatedDate = model.CreatedDate,
        //        CreatedBy = model.CreatedBy,
        //        TrainingId=model.TrainingId
        //    };
        //}


        //public static Answer MapAnswerRequestViewModelToEntity(string createdBy,AddAnswerRequestViewModel model)
        //{
        //    return new Answer
        //    {
        //        AnswerId = Guid.NewGuid(),
        //        Text = model.Text,
        //        IsCorrect = model.IsCorrect,
        //        CreatedBy = createdBy,
        //        CreatedDate = DateTime.Now
        //    };
        //}



            public static WebQuestionResponseViewModel MapQuestionEntityToWebQuestionResponseViewModel(SmartQuestion model)
            {
                if (model == null) return null;

                return new WebQuestionResponseViewModel
                {
                    SmartQuestionId = model.SmartQuestionId,
                    QuestionText = model.Name,
                    OrderNumber = model.OrderNumber,
                    CreatedBy = model.CreatedBy,
                    CreatedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
                    Answers = model.Answers.Select(a => new WebAnswerResponseViewModel
                    {
                        AnswerId = a.AnswerId,
                        AnswerText = a.Text,
                        IsCorrect = a.IsCorrect
                    }).ToList()
                };
            }
        public static WebQuestionResponseViewModel MapQuestionEntityToQuestionResponseViewModel(SmartQuestion model)
        {
            if (model == null) return null;

            return new WebQuestionResponseViewModel
            {
                SmartQuestionId = model.SmartQuestionId,
                QuestionText = model.Name,
                OrderNumber = model.OrderNumber,
                CreatedBy = model.CreatedBy,
                CreatedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
                Answers = model.Answers.Select(a => new WebAnswerResponseViewModel
                {
                    AnswerId = a.AnswerId,
                    AnswerText = a.Text,
                    IsCorrect = a.IsCorrect
                }).ToList()
            };
        }
        public static QuestionResponseViewModel MapQuestionEntityToQuestionResponseViewModelApp(SmartQuestion model)
        {
            if (model == null) return null;

            return new QuestionResponseViewModel
            {
                SmartQuestionId = model.SmartQuestionId,
                QuestionText = model.Name,
                OrderNumber = model.OrderNumber,
                CreatedBy = model.CreatedBy,
                CreatedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
                Answers = model.Answers.Select(a => new AnswerResponseViewModel
                {
                    AnswerId = a.AnswerId,
                    AnswerText = a.Text,
                    IsCorrect = a.IsCorrect,
                }).ToList()
            };
        }

        public static QuestionResponseViewModel MapQuestionResultEntityToQuestionResultResponseViewModelApp(SmartQuestion model)
        {
            if (model == null) return null;

            return new QuestionResponseViewModel
            {
                SmartQuestionId = model.SmartQuestionId,
                QuestionText = model.Name,
                OrderNumber = model.OrderNumber,
                CreatedBy = model.CreatedBy,
                CreatedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.LastName,
                Answers = model.Answers.Select(a => new AnswerResponseViewModel
                {
                    AnswerId = a.AnswerId,
                    AnswerText = a.Text,
                    IsCorrect = a.IsCorrect,
                }).ToList()
            };
        }

        public static SmartQuestion MapQuestionRequestViewModelToQuestionEntity(string createdBy, Guid trainingId, QuestionRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            var smartQuestionId = Guid.NewGuid();
            return new SmartQuestion
            {
                SmartQuestionId = smartQuestionId,
                Name = model.QuestionText,
                OrderNumber = model.OrderNumber,
                CreatedBy = createdBy,
                CreatedDate = DateTime.Now,
                TrainingId = trainingId,
                Answers = model.Answers.Select(a => new Answer
                {
                    AnswerId = Guid.NewGuid(),
                    Text = a?.AnswerText,
                    IsCorrect = a.IsCorrect,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now,
                    SmartQuestionId = smartQuestionId
                }).ToList()
            };
        }

        public static SmartQuestion MapNewQuestionRequestViewModelToQuestionEntity(string createdBy, Guid trainingId, QuestionRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new SmartQuestion
            {
                SmartQuestionId = model.SmartQuestionId.Value,
                Name = model.QuestionText,
                OrderNumber = model.OrderNumber,
                CreatedBy = createdBy,
                TrainingId = trainingId,
                Answers = model.Answers.Select(a => new Answer
                {
                    AnswerId = a.AnswerId.Value,
                    Text = a?.AnswerText,
                    IsCorrect = a.IsCorrect,
                    CreatedBy = createdBy,
                    SmartQuestionId = model.SmartQuestionId.Value,
                }).ToList()
            };
        }

        public static WebQuestionResponseViewModel MapSmartQuestionEntityToQuestionResponseViewModel(SmartQuestion model)
        {
            if (model == null)
            {
                return null;
            }
            return new WebQuestionResponseViewModel
            {
                SmartQuestionId = model.SmartQuestionId,
                QuestionText = model.Name,
                OrderNumber = model.OrderNumber,
                CreatedBy = model.CreatedBy,
                CreatedByName = model?.ApplicationUser?.FirstName + " " + model?.ApplicationUser?.FirstName,
                Answers = model.Answers.Select(a => new WebAnswerResponseViewModel
                {
                    AnswerId = a.AnswerId,
                    AnswerText = a?.Text,
                    IsCorrect = a.IsCorrect,
                }).ToList()
            };
        }

        public static SmartQuestion MapSmartQuestionResponseViewModelToSmartQuestionEntity(WebQuestionResponseViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new SmartQuestion
            {
                SmartQuestionId = model.SmartQuestionId,
                Name = model.QuestionText,
                OrderNumber = model.OrderNumber,
                Answers = model.Answers?.Select(a => new Answer
                {
                    AnswerId = a.AnswerId,
                    Text = a.AnswerText,
                    IsCorrect = a?.IsCorrect
                }).ToList()
            };
        }

        public static WebQuestionResponseViewModel MapUpdateQuestionOrderViewModelToQuestionResponseViewModel(UpdateQuestionOrderViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new WebQuestionResponseViewModel
            {
                SmartQuestionId = model.SmartQuestionId,
                OrderNumber = model.OrderNumber
            };
        }


        public static TrainingRecordViewModel MapTrainingRecordEntityToTrainingRecordViewModel(TrainingRecord model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingRecordViewModel
            {
                TrainingRecordId = model.TrainingRecordId,
                Name = model.Name,
                Validity = model.Validity,
                Expires = model.Expires,
                TrainingTypeId = model.TrainingTypeId,
                TrainingType = model?.TrainingType?.Type,
                Certification = model.Certification,
                Archived = model.Archived,
                DueDate = model.DueDate,
                Qualification = model.Qualification,
                Description = model.Description,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
                CompletedDate= model.CompletedDate,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }

        public static TrainingRecord MapTrainingRecordViewModelToTrainingRecordEntity(TrainingRecordViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingRecord
            {

                TrainingRecordId = model.TrainingRecordId,
                Name = model.Name,
                Validity = model.Validity,
                Expires = model.Expires,
                TrainingTypeId = model.TrainingTypeId,
                Certification = model.Certification,
                Archived = model.Archived,
                DueDate = model.DueDate,
                Qualification = model.Qualification,
                CreatedBy = model.CreatedBy,
            };
        }

        public static TrainingRecord MapTrainingRecordRequestViewModelToTrainingRecordEntity(string CreatedBy, TrainingRecordRequestViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingRecord
            {
                TrainingRecordId = Guid.NewGuid(),
                Name = model.Name,
                Validity = model.Validity,
                Expires = model.Expires,
                TrainingTypeId = model.TrainingTypeId,
                Certification = model.Certification,
                Archived = model.Archived,
                CreatedDate = DateTime.Now,
                Qualification = model.Qualification,
                CreatedBy = CreatedBy,
                CompletedDate=model.CompletedDate,
                Description = model.Description

            };
        }




        public static UpdateTrainingRecordViewModel MapTrainingRecordEntityToUpdateTrainingRecordViewModel(TrainingRecord model)
        {
            if (model == null)
            {
                return null;
            }
            return new UpdateTrainingRecordViewModel
            {
                TrainingRecordId = model.TrainingRecordId,
                Name = model.Name,
                Validity = model.Validity,
                Expires = model.Expires,
                TrainingTypeId = model.TrainingTypeId,
                TrainingType = model?.TrainingType?.Type,
                Certification = model.Certification,
                Archived = model.Archived,
                Description = model.Description,
                DueDate = model.DueDate,
                Qualification = model.Qualification,
                CompletedDate = model.CompletedDate,
                CreatedDate=model.CreatedDate,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
            };
        }

        public static TrainingRecord MapUpdateTrainingRecordViewModelToTrainingRecordEntity(UpdateTrainingRecordViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingRecord
            {

                TrainingRecordId = model.TrainingRecordId,
                Name = model.Name,
                Validity = model.Validity,
                Expires = model.Expires,
                TrainingTypeId = model.TrainingTypeId,
                Certification = model.Certification,
                Archived = model.Archived,
                DueDate = model.DueDate,
                Qualification = model.Qualification,
                CompletedDate = model.CompletedDate,
                CreatedDate= model.CreatedDate,
                CreatedBy = model.CreatedBy,
                Description = model.Description,
            };
        }
        public static TrainingRecordsViewModel MapTrainingRecordEntityToTrainingRecordsViewModel(TrainingRecord model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingRecordsViewModel
            {
                TrainingRecordId = model.TrainingRecordId,
                Name = model.Name,
                Validity = model.Validity,
                Expires = model.Expires,
                TrainingTypeId = model.TrainingTypeId,
                TrainingType = model?.TrainingType?.Type,
                Certification = model.Certification,
                Archived = model.Archived,
                Description = model.Description,
                DueDate = model.DueDate,
                Qualification = model.Qualification,
                CompletedDate = model.CompletedDate,
                CreatedDate = model.CreatedDate,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                Users = model.TrainingRecordOperatorMappings?.Select(u => new TrainingRecordApplicationUserViewModel
                {
                    ApplicationUserId = u.Operator.Id,
                    UserName = u.Operator.FirstName + " " + u.Operator.LastName,
                    ProfileImageLink = u.Operator.ProfileImageLink,
                    ProfileImageName = u.Operator.ProfileImageName,
                })
              .ToList()
            };
        }



        public static TrainingRecordOperatorMappingViewModel MapTrainingRecordOperatorMappingEntityToTrainingRecordOperatorMappingViewModel(TrainingRecordOperatorMapping model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingRecordOperatorMappingViewModel
            {
                TrainingRecordOperatorMappingId = model.TrainingRecordOperatorMappingId,
                TrainingRecordId = model.TrainingRecordId,
                OperatorId = model.OperatorId,
            };
        }

        public static TrainingOperatorMappingViewModel MapTrainingOperatorMappingEntityToTrainingOperatorMappingViewModel(TrainingOperatorMapping model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingOperatorMappingViewModel
            {
                TrainingOperatorMappingId = model.TrainingOperatorMappingId,
                TrainingId = model.TrainingId,
                OperatorId = model.OperatorId,
            };
        }
        public static TrainingRecordoperatorViewModel MapTrainingRecordOperatorMappingEntityToTrainingRecordoperatorViewModel(TrainingRecordOperatorMapping model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingRecordoperatorViewModel
            {
                
                OperatorId = model.OperatorId,
                Name = model?.Operator?.FirstName + " " + model.Operator?.LastName,
                Image = model?.Operator?.ProfileImageLink,
                UserGroupId=model?.Operator?.UserGroupId,
                GroupName=model?.Operator?.UserGroup?.GroupName
            };
        }

        public static TrainingRecordViewModel MapTrainingRecordOperatorMappingEntityToTrainingRecordViewModel(TrainingRecordOperatorMapping model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingRecordViewModel
            {

                TrainingRecordId = model.TrainingRecordId,
                Name = model.TrainingRecord.Name,
                Validity = model.TrainingRecord?.Validity,
                Expires = model.TrainingRecord.Expires,
                TrainingTypeId = model.TrainingRecord.TrainingTypeId,
                Certification = model.TrainingRecord.Certification,
                Archived = model.TrainingRecord.Archived,
                DueDate = model.TrainingRecord?.DueDate,
                Qualification = model.TrainingRecord.Qualification,
                CompletedDate = model.TrainingRecord.CompletedDate,
                CreatedDate = model.TrainingRecord.CreatedDate,
                CreatedBy = model.TrainingRecord.CreatedBy,
            };
        }
        public static TrainingOperatorViewModel MapTrainingOperatorMappingEntityToTrainingOperatorViewModel(TrainingOperatorMapping model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingOperatorViewModel
            {
                TrainingId = model.TrainingId,
                Name = model.Training?.Name,
                DueDate = model.Training?.DueDate,
                CreatedDate = model.CreatedDate,
                TrainingTypeId = model?.Training?.TrainingTypeId,
                TrainingType = model.Training.TrainingType?.Type,
                Expires=model.Training.Expires,
                Validity=model.Training.Validity,
                Certification=model.Training.Certification,
                IsArchived=model.Training.IsArchived
            };
        }
        public static OperatorTrainingRecordViewModel MapTrainingOperatorMappingEntityToOperatorTrainingRecordViewModel(TrainingRecordOperatorMapping model)
        {
            if (model == null)
            {
                return null;
            }
            return new OperatorTrainingRecordViewModel
            {
                TrainingRecordId = model.TrainingRecordId,
                Name = model.TrainingRecord?.Name,
                Expires = model.TrainingRecord.Expires,
                Validity = model?.TrainingRecord?.Validity,
                DueDate = model?.TrainingRecord?.DueDate,
                CreatedDate = model.CreatedDate,
                TrainingTypeId = model.TrainingRecord.TrainingTypeId,
                TrainingType = model.TrainingRecord.TrainingType?.Type,
                Certification = model.TrainingRecord.Certification,
                Qualification = model?.TrainingRecord?.Qualification,
                Archived = model.TrainingRecord.Archived,
                Description=model.TrainingRecord.Description,
                CreatedBy = model.TrainingRecord.CreatedBy,
                CreatedByName = model?.TrainingRecord?.ApplicationUser?.FirstName + " " + model?.TrainingRecord?.ApplicationUser?.LastName,

    };
        }
        //public static OperaorTrainingAndTrainingRecord MapTrainingAndTrainingRecordOperatorMappingEntityToTrainingOperatorViewModel(TrainingOperatorMapping model)
        //{
        //    if (model == null)
        //    {
        //        return null;
        //    }
        //    return new OperaorTrainingAndTrainingRecord
        //    {
        //        TrainingId = model.TrainingId,
        //        TrainingOperatorMappingId = model.TrainingOperatorMappingId,
        //    };
        //}

        public static TrainingOperatorViewModel MapTrainingEntityToTrainingOperatorViewModel(Training model)
        {
            if (model == null)
            {
                return null;
            }
            return new TrainingOperatorViewModel
            {
                TrainingId = model.TrainingId,
                Name = model.Name,
                DueDate = model.DueDate,
                CreatedDate = model.CreatedDate,

            };
        }




        public static RiskAssessmentLogNameViewModel MapRiskAssessmentLogEntityToRiskAssessmentLogNameViewModel(RiskAssessmentLog model)
        {

            if (model == null)
            {
                return null;
            }
            return new RiskAssessmentLogNameViewModel
            {
                RiskAssessmentLogId = model.RiskAssessmentLogId,
                Name = model.Name,
            };
        }




        public static HazardKeyResponseViewModel MapHazardKeyEntityToHazardKeyResponseViewModel(HazardKey model)
        {
            if (model == null)
            {
                return null;
            }
            return new HazardKeyResponseViewModel
            {
                HazardKeyId = model.HazardKeyId,
                Name = model.Name,
                Color = model.Color,
                HazardTypeId = model.HazardTypeId,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                CreatedDate = model.CreatedDate,
                HazartType = model?.HazardType?.Type,
            };
        }
        public static HazardKey MapHazardKeyRequestViewModelToHazardKeyEntity(string CreatedBy, HazardKeyRequestViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new HazardKey
            {
                HazardKeyId = Guid.NewGuid(),
                Name = model.Name,
                Color = model.Color,
                HazardTypeId = model.HazardTypeId,
                CreatedDate = DateTime.Now,
                CreatedBy = CreatedBy,
            };
        }


        public static HazardKey MapHazardKeyResponseViewModelToHazardKeyEntity(HazardKeyResponseViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new HazardKey
            {
                HazardKeyId = model.HazardKeyId,
                Name = model.Name,
                Color = model.Color,
                HazardTypeId = model.HazardTypeId,
                CreatedBy = model.CreatedBy,
                CreatedDate = model.CreatedDate,
            };
        }

        public static HazardKeyFieldResponseViewModel MapHazardKeyFieldMappingEntityToHazardKeyFieldResponseViewModel(HazardKeyFieldMapping model)
        {
            if (model == null)
            {
                return null;
            }

            return new HazardKeyFieldResponseViewModel
            {
                HazardKeyFieldMappingId = model.HazardKeyFieldMappingId,
                HazardKeyId = model.HazardKeyId,
                FieldId = model.FieldId,
                Locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LocationViewModel>>(model.Location), 
                CreatedDate = model.CreatedDate,
            };
        }



        public static HazardKeyFieldMapping MapHazardKeyFieldResponseViewModelToHazardKeyFieldMappingEntity(HazardKeyFieldResponseViewModel model)
        {
            if (model == null)
            {
                return null;
            }
            return new HazardKeyFieldMapping
            {
                HazardKeyFieldMappingId = model.HazardKeyFieldMappingId,
                HazardKeyId = model.HazardKeyId,
                FieldId = model.FieldId,
                Location = Newtonsoft.Json.JsonConvert.SerializeObject(model.Locations),
                CreatedDate = model.CreatedDate
            };
        }


        public static HazardKeyFieldMapping MapHazardKeyFieldViewModelToHazardKeyFieldMappingEntity(HazardKeyFieldViewModel model)
        {

            if (model == null)
            {
                return null;
            }
            return new HazardKeyFieldMapping
            {
                HazardKeyFieldMappingId = Guid.NewGuid(),
                HazardKeyId = model.HazardKeyId,
                FieldId = model.FieldId,
                Location = Newtonsoft.Json.JsonConvert.SerializeObject(model.Locations),
                CreatedDate = DateTime.Now,
            };

        }


        public static HazardKeyNameViewModel MapHazardKeyNameEntityToHazardKeyNameViewModel(HazardKey model)
        {

            if (model == null)
            {
                return null;
            }
            return new HazardKeyNameViewModel
            {
                HazardKeyId = model.HazardKeyId,
                Name = model.Name,
                Color = model.Color,
                HazardTypeId = model.HazardTypeId,
                HazardType = model?.HazardType?.Type,
            };
        }
        public static FieldNameViewModel MapFieldNameEntityToFieldNameViewModel(Field model)
        {

            if (model == null)
            {
                return null;
            }
            return new FieldNameViewModel
            {
                FieldId = model.FieldId,
                Name = model.Name,
            };
        }


        public static FieldHazardViewModel MapHazardKeyFieldMappingEntityToFieldHazardViewModel(HazardKeyFieldMapping model)
        {
            var locations= new List<LocationViewModel>();
            if (model == null)
            {
                return null;
            }
            if (!string.IsNullOrEmpty(model.Location))
            {
                try
                {
                    locations = JsonSerializer.Deserialize<List<LocationViewModel>>(model.Location);
                }
                catch (JsonException)
                {
                    // Handle deserialization error (log or return a default value)
                    locations = new List<LocationViewModel>();
                }
            }

            return new FieldHazardViewModel
            {

                HazardKeyId = model.HazardKeyId,
                Name = model?.HazardKey?.Name,
                Color = model?.HazardKey?.Color,
                CreatedDate = model.HazardKey.CreatedDate,
                HazardTypeId = model.HazardKey.HazardTypeId,
                HazartType = model?.HazardKey?.HazardType?.Type,
                HazardKeyFieldMappingId=model.HazardKeyFieldMappingId,
                // Map the list of locations directly
                Locations = locations?.Select(location => new LocationViewModel
                {
                    Lat = location.Lat,
                    Lng = location.Lng
                }).ToList()
            };
        }

        public static FieldCenterViewModel MapFieldEntityToFieldCenterViewModell(Field model)
        {
            if (model == null)
            {
                return null;
            }

            return new FieldCenterViewModel
            {
                FieldId = model.FieldId,
                Center = Newtonsoft.Json.JsonConvert.DeserializeObject<CenterViewModel>(model.Center),
            };
        }

        public static FieldBoundaryViewModel MapFieldEntityToFieldBoundaryViewModel(Field model)
        {
            if (model == null)
            {
                return null;
            }

            return new FieldBoundaryViewModel
            {
                FieldId = model.FieldId,
                Boundaries = Newtonsoft.Json.JsonConvert.DeserializeObject<List<BoundaryViewModel>>(model.Boundary),
            };
        }


        public static FieldDetailViewModel MapFieldEntityToFieldDetailViewModel(Field model)
        {
            if (model == null)
            {
                return null;
            }
            //var boundaries = JsonSerializer.Deserialize<List<BoundaryViewModel>>(model.Boundary);
            //var center = JsonSerializer.Deserialize<CenterViewModel>(model.Center);

            var boundaries = string.IsNullOrEmpty(model.Boundary)? null : JsonSerializer.Deserialize<List<BoundaryViewModel>>(model.Boundary);

            var center = string.IsNullOrEmpty(model.Center)? null : JsonSerializer.Deserialize<CenterViewModel>(model.Center);

            return new FieldDetailViewModel
            {
                FieldId = model.FieldId,
                Name = model.Name,
                CreatedBy = model.CreatedBy,
                CreatedByName = model.ApplicationUser?.FirstName + " " + model.ApplicationUser?.LastName,
                FarmName = model.Farm?.FarmName,
                Boundaries = boundaries?.Select(boundary => new BoundaryViewModel
                {
                    Lat = boundary.Lat,
                    Lng = boundary.Lng
                }).ToList(),

                Center = center == null ? null : new CenterViewModel
                {
                    Lat = center.Lat,
                    Lng = center.Lng
                },
            };
        }


        public static HazardFieldResponseViewModel MapHazardKeyFieldMappingEntityToHazardFieldResponseViewModel(HazardKeyFieldMapping entity)
        {
            return new HazardFieldResponseViewModel
            {
                HazardKeyFieldMappingId = entity.HazardKeyFieldMappingId,
                CreatedDate = entity.CreatedDate,
                FieldId = entity.FieldId,
                List = new List<HazardFieldViewModel>
            {
                new HazardFieldViewModel
                {
                    HazardKeyId = entity.HazardKeyId,
                    Locations = Newtonsoft.Json.JsonConvert.DeserializeObject<List<LocationViewModel>>(entity.Location)
                }
            }
            };
        }
        
        
        public static ApplicationUsersViewModel MapApplicationUserEntityToApplicationUsersViewModel(ApplicationUser model)
        {
            if (model == null)
            {
                return null;
            }
            return new ApplicationUsersViewModel
            {
                ApplicationUserId = model.Id,
                UserName = model.FirstName + " " + model.LastName,
            };
        }


        public static NotificationResponseViewModel MapNotificationEntityToNotificationResponseViewModel(Notification model)
        {

            if (model == null)
            {
                return null;
            }
            return new NotificationResponseViewModel
            {
                NotificationId = model.NotificationId,
                Title = model.Title,
                Description = model.Description,
                CreatedDate = model.CreatedDate,
                IsRead = model.IsRead,
                ToId = model.ToId
             };
        }

        public static FarmNameViewModel MapFarmEntityToFarmNameListViewModel(Farm model)
        {

            if (model == null)
            {
                return null;
            }
            return new FarmNameViewModel
            {
                FarmId = model.FarmId,
                FarmName = model.FarmName,
            };
        }

    }
}
