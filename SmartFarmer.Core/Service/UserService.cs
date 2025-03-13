using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System.Reflection;
using System.Threading;

namespace SmartFarmer.Core.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IEventRepository _eventRepository;
        private ITrainingRepository _trainingRepository;
        private IWelfareRoutineRepository _welfareRoutineRepository;
        public UserService(IUserRepository userRepository, IMachineRepository machineRepository, IEventRepository eventRepository, ITrainingRepository trainingRepository, IWelfareRoutineRepository welfareRoutineRepository)
        {
            _userRepository = userRepository;
            _machineRepository = machineRepository;
            _eventRepository = eventRepository;
            _trainingRepository = trainingRepository;
            _welfareRoutineRepository = welfareRoutineRepository;
        }
        /// <summary>
        /// Is User Exist
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsUserExist(string userId)
        {
            return _userRepository.IsUserExist(userId);
        }

        /// <summary>
        /// Is User Type Exist
        /// </summary>
        /// <param name="userTypeId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsUserTypeExist(int userTypeId)
        {
            return _userRepository.IsUserTypeExist(userTypeId);
        }

        /// <summary>
        /// Is User Status Exist
        /// </summary>
        /// <param name="userStatusId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsUserStatusExist(int userStatusId)
        {
            return _userRepository.IsUserStatusExist(userStatusId);
        }

        /// <summary>
        /// Is Operator Exist
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool IsOperatorExist(string userId)
        {
            return _userRepository.IsOperatorExist(userId);
        }

        /// <summary>
        /// get User role
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public string GetUserRole(string userId)
        {
            return _userRepository.GetUserRole(userId);
        }

        /// <summary>
        /// Is Email Exist
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool CheckUserEmailExistence(string email)
        {
            return _userRepository.CheckUserEmailExistence(email);
        }
        
        /// <summary>
        /// Is User Name Exists
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool CheckFarmUserNameExist(string email, Guid farmId)
        {
            return _userRepository.CheckFarmUserNameExist(email,farmId);
        }
        
        /// <summary>
        /// Is existing user Email in this farm
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <response code="400">If the item is null</response>
        public bool CheckUserExistsInTheSameFarm(string email, string masterAdminId)
        {
            return _userRepository.CheckUserExistsInTheSameFarm(email, masterAdminId);
        }

        /// <summary>
        /// get user by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public UserRequestViewModel GetUserListBySearchWithPagination(SearchUserRequestViewModel model, string masterAdminId)
        {
            UserRequestViewModel userList = new UserRequestViewModel();
            userList.List = _userRepository.GetUserListBySearch(model.PageNumber, model.PageSize, model.SearchKey, masterAdminId).Select(a => Mapper.MapApplicationUserEntityToApplicationUserViewModel(a)).ToList();
            userList.TotalCount = _userRepository.GetUserCountBySearch(model.SearchKey, masterAdminId);
            return userList;
        }

        /// <summary>
        /// get user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApplicationUserDetailsViewModel GetUserDetails(string userId)
        {
            ApplicationUserDetailsViewModel applicationUserDetailsView = new ApplicationUserDetailsViewModel();
            applicationUserDetailsView.LastEventDetails = Mapper.MapEventEntityToLastEventViewModel(_eventRepository.GetLastEvent(userId));
            applicationUserDetailsView.UserDetails = Mapper.MapApplicationUserEntityToApplicationUserViewModel(_userRepository.GetUserDetails(userId));
            applicationUserDetailsView.Operating = _machineRepository.GetUserOperating(userId).Select(Mapper.MapMachineEntityToMachineNickNameViewModel).ToList();
            applicationUserDetailsView.Training = _trainingRepository.GetOperatorTrainings(userId).Select(a => Mapper.MapTrainingOperatorMappingEntityToTrainingOperatorViewModel(a)).ToList();
            applicationUserDetailsView.TrainingRecords = _trainingRepository.GetOperatorTrainingRecords(userId).Select(a => Mapper.MapTrainingOperatorMappingEntityToOperatorTrainingRecordViewModel(a)).ToList();


            foreach (var training in applicationUserDetailsView.Training)
            {
                if (training.DueDate != null)
                {
                    var today = DateTime.Today;
                    if (training.DueDate.Value <= today)
                    {
                        training.Expired = true;
                    }
                }
            }
            foreach(var trainingRecord in applicationUserDetailsView.TrainingRecords)
            {

                if (trainingRecord.Expires == true && trainingRecord.Validity != null)
                {
                    var today = DateTime.Today;
                    var dueThreshold = today.AddDays(30);

                    if (trainingRecord.DueDate.Value < today)
                    {
                        trainingRecord.Expired = true;
                    }
                    else if (trainingRecord.DueDate.Value >= today && trainingRecord.DueDate.Value <= dueThreshold)
                    {
                        trainingRecord.Due = true;
                    }
                }
            }

            return applicationUserDetailsView;
        }



        /// <summary>
        /// get user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public GetOperatorCheckInDetailsViewModel GetUserChcekInDetails(string userId)
        {
            var model = new GetOperatorCheckInDetailsViewModel();

            var User = Mapper.MapApplicationUserEntityToGetOperatorCheckInDetailsViewModel(_userRepository.GetUserDetails(userId));
            if (User != null)
            {
                model.UserId = User.UserId;
                model.Name = User.Name;
                model.UserTypeId = User.UserTypeId;
                model.UserType = User.UserType;
                model.ProfileImageLink = User.ProfileImageLink;
                model.ProfileImageName = User.ProfileImageName;
                model.GroupId = User.GroupId;
                model.GroupName = User.GroupName;
            }

            var alarmAction = _userRepository.GetAlarmActionDetail(userId);
            if (alarmAction != null)
            {
                model.MobileNumber = alarmAction.MobileNumber;
                model.MobileActionTypeId = alarmAction.MobileActionTypeId;
            }

            var WelfareRoutine = _welfareRoutineRepository.GetWelfareRoutineDetail(User?.GroupId);
            if (WelfareRoutine != null)
            {
                model.WelfareRoutineId = WelfareRoutine.WelfareRoutineId;
                model.WelfareRoutineName = WelfareRoutine.Name;
                model.CheckInMinutes = WelfareRoutine.Minutes;
            }
            var Events = _eventRepository.GetEvents(userId);
            if (Events != null)
            {
                var ClockIn = Events.FirstOrDefault(a => a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In);
                var clockOut = Events.FirstOrDefault(a => a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out);
                var checkIn = Events.FirstOrDefault(a => a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Check_In);
                var welfare = Events.FirstOrDefault(a => a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Welfare);
                if (WelfareRoutine != null && ClockIn != null)
                {

              
                if(WelfareRoutine?.Minutes!=null || WelfareRoutine?.Minutes != 0)
                {

                        if (checkIn != null && ClockIn != null && checkIn.CreatedDate > ClockIn.CreatedDate /*&& welfare != null && welfare.CreatedDate < checkIn.CreatedDate*/)
                        {
                            model.TimeOut = checkIn.CreatedDate.AddMinutes(WelfareRoutine.Minutes);
                        }
                        else if (welfare != null && checkIn != null && welfare.CreatedDate > checkIn.CreatedDate)
                        {
                            model.TimeOut = welfare.CreatedDate.AddMinutes(WelfareRoutine.Minutes);
                        }
                        else
                        {
                            model.TimeOut = ClockIn.CreatedDate.AddMinutes(WelfareRoutine.Minutes);
                        }

                        // var timeleft = model.TimeOut - DateTime.Now;
                        DateTime timeOut = model.TimeOut.Value; // Assuming model.TimeOut is already calculated
                DateTime now = DateTime.Now; // Current time

                // Calculate the time difference
                TimeSpan timeLeft = timeOut - now;

                // Check if the time has passed
                if (timeLeft.TotalSeconds <= 0)
                {
                    model.TimeLeft="Time has passed.";
                }
                else
                {
                    model.TimeLeft = $"{timeLeft.Days} days, {timeLeft.Hours} hours, {timeLeft.Minutes} minutes, and {timeLeft.Seconds} seconds.";
                }
                }
                }
                if (ClockIn != null && clockOut != null)
                {
                    if (clockOut.CreatedDate > ClockIn.CreatedDate)
                    {
                        model.ClockOutLocaton = clockOut.Location;
                        model.ClockOutTime = clockOut.CreatedDate;
                    }
                }
                if (ClockIn != null)
                {
                    model.ClockInLocaton = ClockIn.Location;
                    model.ClockInTime = ClockIn.CreatedDate;
                }
                //if (clockOut != null)
                //{
                //    model.ClockOutLocaton = clockOut.Location;
                //    model.ClockOutTime = clockOut.CreatedDate;
                //}
                if (checkIn != null)
                {
                    model.CheckInLocaton = checkIn.Location;
                    model.CheckInTime = checkIn.CreatedDate;
                }
            }

            return model;
        }

        public List<string> GetSystemMasterAdminIds()
        {
            return _userRepository.GetSystemMasterAdminIds();
        }

        /// <summary>
        /// API for task Schedular
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CheckWelfareAlarm(string masterAdminId)
        {


            // Fetch users with the given masterAdminId
            var users = _userRepository.GetUsers(masterAdminId)
                .Select(user => Mapper.MapApplicationUserEntityToApplicationUserViewModel(user))
                .ToList();

            // If no users found, return an empty result
            if (users == null || users.Count == 0)
                return false;

            foreach (var user in users)
            {
                var userDetails = Mapper.MapApplicationUserEntityToGetOperatorCheckInDetailsViewModel(
                    _userRepository.GetUserDetails(user.ApplicationUserId)
                );
                // Get welfare routine details
                var welfareRoutine = _welfareRoutineRepository.GetWelfareRoutineDetail(userDetails.GroupId);

                // Get events for the user
                var events = _eventRepository.GetEvents(userDetails.UserId);
                if (events != null)
                {
                    var clockIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In);
                    var clockOut = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out);
                    var checkIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Check_In);
                    var welfare = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Welfare);
                    var TimeOut = DateTime.Now;
                    // Process clock-in event
                    if((clockOut!=null && clockOut.CreatedDate< clockIn.CreatedDate)|| clockOut == null)
                    {                  
                    if (welfareRoutine != null && clockIn!=null)
                    {
                    if (welfareRoutine?.Minutes != null || welfareRoutine?.Minutes != 0)
                    {


                        if (checkIn != null && clockIn != null && checkIn.CreatedDate > clockIn.CreatedDate&& welfare != null && welfare.CreatedDate< checkIn.CreatedDate)
                        {
                            TimeOut = checkIn.CreatedDate.AddMinutes(welfareRoutine.Minutes);
                        }
                       
                           else if(welfare != null&& checkIn != null && welfare.CreatedDate > checkIn.CreatedDate)
                        {
                           TimeOut = welfare.CreatedDate.AddMinutes(welfareRoutine.Minutes);
                        }
                        else if (welfare != null && checkIn != null && welfare.CreatedDate < checkIn.CreatedDate)
                         {
                          TimeOut = checkIn.CreatedDate.AddMinutes(welfareRoutine.Minutes);
                          }
                    else
                        {
                            TimeOut = clockIn.CreatedDate.AddMinutes(welfareRoutine.Minutes);
                        }

                        // var timeleft = model.TimeOut - DateTime.Now;
                        DateTime timeOut = TimeOut; // Assuming model.TimeOut is already calculated
                        DateTime now = DateTime.Now; // Current time

                        // Calculate the time difference
                        TimeSpan timeLeft = timeOut - now;

                        // Check if the time has passed
                        if (timeLeft.TotalSeconds <= 0)
                        {
                            if (welfare!=null &&welfare.CreatedDate < TimeOut )
                            {
                                // model.TimeLeft = "Time has passed.";
                                if (userDetails.UserLocaton == null)
                                {
                                    userDetails.UserLocaton = clockIn.Location;
                                }
                                // Create a Welfare Event if not already created
                                var model = new EventRequestViewModel
                                {
                                    Location = userDetails.UserLocaton
                                };

                                var welfareEvent = new Event
                                {
                                    CreatedBy = userDetails.UserId,
                                    EventTypeId = (int)Core.Common.Enums.EventTypeEnum.Welfare,
                                    CreatedDate = DateTime.Now,
                                    Location = userDetails.UserLocaton,
                                };
                                // Create a new welfare event
                                var createdEvent = _eventRepository.AddEvent(
                                    Mapper.MapEventRequestViewModelToEventEntity(welfareEvent.EventTypeId, welfareEvent.CreatedBy, model)
                                );

                              
                            }
                               
                        }
                        //else
                        //{
                        //    return false;
                        //   // model.TimeLeft = $"{timeLeft.Days} days, {timeLeft.Hours} hours, {timeLeft.Minutes} minutes, and {timeLeft.Seconds} seconds.";
                        //}
                    }
                    }
                    }
                   

                }
            }

            return true;
        }


        /// <summary>
        /// check Welare for app
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CheckWelfareForApp(string userId)
        {

            var userDetails = Mapper.MapApplicationUserEntityToGetOperatorCheckInDetailsViewModel(
                _userRepository.GetUserDetails(userId)
            );
            var welfareCheckViewModel = new WelfareChcekViewModel
            {
                UserId = userDetails.UserId,
                UserTypeId = userDetails.UserTypeId,
                GroupId = userDetails.GroupId
            };

            // Get welfare routine details
            var welfareRoutine = _welfareRoutineRepository.GetWelfareRoutineDetail(userDetails.GroupId);
            if (welfareRoutine != null)
            {
                welfareCheckViewModel.WelfareRoutineId = welfareRoutine.WelfareRoutineId;
                welfareCheckViewModel.WelfareTime = welfareRoutine.Minutes;
            }

            // Get events for the user
            var events = _eventRepository.GetEvents(userDetails.UserId);
            if (events != null)
            {
                var clockIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In);
                var clockOut = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out);
                var checkIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Check_In);

                // Process clock-in event
                if (clockIn != null)
                {
                    welfareCheckViewModel.ClockInLocaton = clockIn.Location;
                    welfareCheckViewModel.ClockInTime = clockIn.CreatedDate;
                    if (checkIn == null || checkIn.CreatedDate < clockIn.CreatedDate)
                    {
                        // Calculate WelfareTimeOut only if there is no clock-out yet
                        if ((clockOut == null && welfareRoutine != null) || ((clockOut.CreatedDate < clockIn.CreatedDate) && (welfareRoutine != null)))
                        {
                            welfareCheckViewModel.WelfareTimeOut = clockIn.CreatedDate.AddMinutes(welfareRoutine.Minutes);

                            // Check if the user has checked in before the WelfareTimeOut
                            if ((checkIn == null) || (checkIn.CreatedDate < welfareCheckViewModel.WelfareTimeOut))
                            {
                                if (clockIn.ShowAppPopup == true)
                                {
                                    //if (welfareCheckViewModel.WelfareTimeOut < DateTime.Now)
                                    //{
                                    //    _eventRepository.UpdateEventForApp(clockIn.EventId, true);
                                    return true;
                                    //}
                                }

                            }
                        }
                    }
                    else
                    {
                        if ((clockOut == null && welfareRoutine != null) || ((clockOut.CreatedDate < clockIn.CreatedDate) && (welfareRoutine != null)))
                        {
                            welfareCheckViewModel.WelfareTimeOut = checkIn.CreatedDate.AddMinutes(welfareRoutine.Minutes);

                            // Check if the user has checked in before the WelfareTimeOut
                            if ((checkIn.CreatedDate < clockIn.CreatedDate) || (checkIn.CreatedDate < welfareCheckViewModel.WelfareTimeOut))
                            {
                                if (checkIn.ShowAppPopup == true)
                                {
                                    return true;
                                }
                                //if (welfareCheckViewModel.WelfareTimeOut < DateTime.Now)
                                //{
                                //    _eventRepository.UpdateEventForApp(checkIn.EventId, true);
                                //    return true;
                                //}


                            }
                        }

                    }
                }
            }


            return false;
        }
        /// <summary>
        /// Create alart on app far task schedular
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CreateWelfareForApp(string masterAdminId)
        {
            var users = _userRepository.GetUsers(masterAdminId)
              .Select(user => Mapper.MapApplicationUserEntityToApplicationUserViewModel(user))
              .ToList();

            // If no users found, return an empty result
            if (users == null || users.Count == 0)
                return false;

            foreach (var user in users)
            {
                var userDetails = Mapper.MapApplicationUserEntityToGetOperatorCheckInDetailsViewModel(
                _userRepository.GetUserDetails(user.ApplicationUserId)
            );
                var welfareCheckViewModel = new WelfareChcekViewModel
                {
                    UserId = userDetails.UserId,
                    UserTypeId = userDetails.UserTypeId,
                    GroupId = userDetails.GroupId
                };

                // Get welfare routine details
                var welfareRoutine = _welfareRoutineRepository.GetWelfareRoutineDetail(userDetails.GroupId);
                if (welfareRoutine != null)
                {
                    welfareCheckViewModel.WelfareRoutineId = welfareRoutine.WelfareRoutineId;
                    welfareCheckViewModel.WelfareTime = welfareRoutine.Minutes;
                }

                // Get events for the user
                var events = _eventRepository.GetEvents(userDetails.UserId);
                if (events != null)
                {
                    var clockIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In);
                    var clockOut = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out);
                    var checkIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Check_In);

                    // Process clock-in event
                    if (clockIn != null)
                    {
                        welfareCheckViewModel.ClockInLocaton = clockIn.Location;
                        welfareCheckViewModel.ClockInTime = clockIn.CreatedDate;
                        if (checkIn == null || checkIn.CreatedDate < clockIn.CreatedDate)
                        {
                            // Calculate WelfareTimeOut only if there is no clock-out yet
                            if ((clockOut == null && welfareRoutine != null) || ((clockOut.CreatedDate < clockIn.CreatedDate) && (welfareRoutine != null)))
                            {
                                welfareCheckViewModel.WelfareTimeOut = clockIn.CreatedDate.AddMinutes(welfareRoutine.Minutes);

                                // Check if the user has checked in before the WelfareTimeOut
                                if ((checkIn == null) || (checkIn.CreatedDate < welfareCheckViewModel.WelfareTimeOut))
                                {

                                    if (welfareCheckViewModel.WelfareTimeOut < DateTime.Now)
                                    {
                                        _eventRepository.UpdateEventForApp(clockIn.EventId, true);
                                        return true;
                                    }

                                }
                            }
                        }
                        else
                        {
                            if ((clockOut == null && welfareRoutine != null) || ((clockOut.CreatedDate < clockIn.CreatedDate) && (welfareRoutine != null)))
                            {
                                welfareCheckViewModel.WelfareTimeOut = checkIn.CreatedDate.AddMinutes(welfareRoutine.Minutes);

                                // Check if the user has checked in before the WelfareTimeOut
                                if ( (checkIn.CreatedDate < clockIn.CreatedDate) || (checkIn.CreatedDate < welfareCheckViewModel.WelfareTimeOut))
                                {

                                    if (welfareCheckViewModel.WelfareTimeOut < DateTime.Now)
                                    {
                                        _eventRepository.UpdateEventForApp(checkIn.EventId, true);
                                        return true;
                                    }
                                }
                            }

                        }
                    }
                }

            }
            return false;
        }
        /// <summary>
        /// check Welare
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public WelfareAlarmListViewModel CheckWelfareForWeb(string masterAdminId)
        {
            var welfareAlarmListView = new WelfareAlarmListViewModel
            {
                List = new List<WelfareAlarmViewModel>()
            };
            var users = _userRepository.GetUsers(masterAdminId)
                 .Select(user => Mapper.MapApplicationUserEntityToApplicationUserViewModel(user))
                 .ToList();

            // If no users found, return an empty result
            if (users == null || users.Count == 0)
                return welfareAlarmListView;

            foreach (var user in users)
            {
                var userDetails = Mapper.MapApplicationUserEntityToGetOperatorCheckInDetailsViewModel(
                    _userRepository.GetUserDetails(user.ApplicationUserId)
                );

                // Get events for the user
                var events = _eventRepository.GetEvents(userDetails.UserId);
                if (events != null)
                {
                    var clockIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In);
                    var clockOut = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out);
                    var SOS = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.SOS);
                    var FALL = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Fall);
                    var checkIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Check_In);
                    var Welfare = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Welfare);

                    if (Welfare != null && Welfare.ShowWebPopup == true)
                    {
                        var welfareAlarmViewModel = new WelfareAlarmViewModel
                        {
                            UserId = userDetails.UserId,
                            UserName = userDetails.Name,
                            ProfileImageLink = userDetails.ProfileImageLink,
                            UserTypeId = userDetails.UserTypeId,
                            EventId = Welfare.EventId,
                            EventType = Welfare.EventType?.Type,
                            EventTime = Welfare.CreatedDate,
                            Eventlocation = Welfare.Location,
                            EventTypeId = Welfare.EventTypeId,
                            ShowWebPopup = Welfare.ShowWebPopup,
                            Phone= userDetails.Phone
                        };
                        welfareAlarmListView.List.Add(welfareAlarmViewModel);
                    }

                    if (SOS != null && SOS.ShowWebPopup == true)
                    {
                        var sosEventViewModel = new WelfareAlarmViewModel
                        {
                            UserId = userDetails.UserId,
                            UserName = userDetails.Name,
                            ProfileImageLink = userDetails.ProfileImageLink,
                            UserTypeId = userDetails.UserTypeId,
                            EventId = SOS.EventId,
                            EventType = SOS.EventType?.Type,
                            EventTime = SOS.CreatedDate,
                            Eventlocation = SOS.Location,
                            EventTypeId = SOS.EventTypeId,
                            ShowWebPopup = SOS.ShowWebPopup,
                            Phone = userDetails.Phone
                        };
                        welfareAlarmListView.List.Add(sosEventViewModel);
                    }

                    // Add FALL event if it exists and ShowWebPopup == true
                    if (FALL != null && FALL.ShowWebPopup == true)
                    {
                        var fallEventViewModel = new WelfareAlarmViewModel
                        {
                            UserId = userDetails.UserId,
                            UserName = userDetails.Name,
                            ProfileImageLink = userDetails.ProfileImageLink,
                            UserTypeId = userDetails.UserTypeId,
                            EventId = FALL.EventId,
                            EventTime = FALL.CreatedDate,
                            Eventlocation = FALL.Location,
                            EventTypeId = FALL.EventTypeId,
                            EventType = FALL.EventType?.Type,
                            ShowWebPopup = FALL.ShowWebPopup,
                            Phone = userDetails.Phone
                        };
                        welfareAlarmListView.List.Add(fallEventViewModel);
                    }


                }
            }
            return welfareAlarmListView;
        }

         

/// <summary>
/// check Welare
/// </summary>s
/// <param name="userId"></param>
/// <returns></returns>
public bool CancleWelfareForWeb(string masterAdminId)
        {
            // Fetch users with the given masterAdminId
            var users = _userRepository.GetUsers(masterAdminId)
                .Select(user => Mapper.MapApplicationUserEntityToApplicationUserViewModel(user))
                .ToList();

            // If no users found, return an empty result
            if (users == null || users.Count == 0)
                return false;

            foreach (var user in users)
            {
                var userDetails = Mapper.MapApplicationUserEntityToGetOperatorCheckInDetailsViewModel(
                _userRepository.GetUserDetails(user.ApplicationUserId)
            );
                var welfareCheckViewModel = new WelfareChcekViewModel
                {
                    UserId = userDetails.UserId,
                    UserTypeId = userDetails.UserTypeId,
                    GroupId = userDetails.GroupId
                };

                // Get welfare routine details
                var welfareRoutine = _welfareRoutineRepository.GetWelfareRoutineDetail(userDetails.GroupId);
                if (welfareRoutine != null)
                {
                    welfareCheckViewModel.WelfareRoutineId = welfareRoutine.WelfareRoutineId;
                    welfareCheckViewModel.WelfareTime = welfareRoutine.Minutes;
                }

                // Get events for the user
                var events = _eventRepository.GetEvents(userDetails.UserId);
                if (events != null)
                {
                    var clockIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In);
                    var clockOut = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out);
                    var checkIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Check_In);

                    // Process clock-in event
                    if (clockIn != null)
                    {
                        welfareCheckViewModel.ClockInLocaton = clockIn.Location;
                        welfareCheckViewModel.ClockInTime = clockIn.CreatedDate;
                        if (checkIn == null || checkIn.CreatedDate < clockIn.CreatedDate)
                        {
                            
                            // Calculate WelfareTimeOut only if there is no clock-out yet
                            if ((welfareRoutine != null) || ((clockOut.CreatedDate < clockIn.CreatedDate) && (welfareRoutine != null)))
                            {

                                _eventRepository.UpdateEventForApp(clockIn.EventId, false);
                                _eventRepository.UpdateEventForWeb(clockIn.EventId, false);
                                return true;

                            }
                        }
                        else
                            {
                                if ((checkIn.CreatedDate < clockIn.CreatedDate)||(welfareRoutine != null) || ((clockOut.CreatedDate < clockIn.CreatedDate) && (welfareRoutine != null)))
                                {

                                    _eventRepository.UpdateEventForApp(checkIn.EventId, false);
                                _eventRepository.UpdateEventForWeb(clockIn.EventId, false);
                                return true;
                                }
                            }
                        
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// check Welare
        /// </summary>s
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CancleWelfareFromWeb(Guid eventId, string loginUserId)
        {
            bool appUpdateResult = _eventRepository.UpdateEventForApp(eventId, false);
            bool webUpdateResult = _eventRepository.UpdateEventForWeb(eventId, false);
           
            if (appUpdateResult && webUpdateResult)
            {
                var existingEvent = _eventRepository.GetEventDetails(eventId);
                string welfareCreatedByName = existingEvent?.ApplicationUser?.FirstName + " " + existingEvent?.ApplicationUser?.LastName;
                var userDetail = _userRepository.GetUserDetails(loginUserId);
                var cancelledEvent = new Event
                {
                    EventId = Guid.NewGuid(),
                    EventTypeId = (int)Core.Common.Enums.EventTypeEnum.Cancelled,
                    CreatedBy = loginUserId,
                    MachineId = existingEvent.MachineId,
                    CreatedDate = DateTime.Now,
                    Location = existingEvent.Location,
                    Message = $"{userDetail.FirstName} {userDetail.LastName} CANCELLED Alarm of {welfareCreatedByName} at {DateTime.Now:hh:mm tt}"

            };

                _eventRepository.AddEvent(cancelledEvent);
                return true;
            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// check Welare
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CancleWelfareForApp(string userId)
        {
            var userDetails = Mapper.MapApplicationUserEntityToGetOperatorCheckInDetailsViewModel(
                _userRepository.GetUserDetails(userId)
            );
            var welfareCheckViewModel = new WelfareChcekViewModel
            {
                UserId = userDetails.UserId,
                UserTypeId = userDetails.UserTypeId,
                GroupId = userDetails.GroupId
            };

            // Get welfare routine details
            var welfareRoutine = _welfareRoutineRepository.GetWelfareRoutineDetail(userDetails.GroupId);
            if (welfareRoutine != null)
            {
                welfareCheckViewModel.WelfareRoutineId = welfareRoutine.WelfareRoutineId;
                welfareCheckViewModel.WelfareTime = welfareRoutine.Minutes;
            }

            // Get events for the user
            var events = _eventRepository.GetEvents(userDetails.UserId);
            if (events != null)
            {
                var clockIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In);
                var clockOut = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out);
                var checkIn = events.FirstOrDefault(e => e.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Check_In);

                // Process clock-in event
                if (clockIn != null)
                {
                    welfareCheckViewModel.ClockInLocaton = clockIn.Location;
                    welfareCheckViewModel.ClockInTime = clockIn.CreatedDate;
                    if (checkIn == null || checkIn.CreatedDate < clockIn.CreatedDate)
                      { 
                        // Calculate WelfareTimeOut only if there is no clock-out yet
                        if (( welfareRoutine != null) || ((clockOut.CreatedDate < clockIn.CreatedDate) && (welfareRoutine != null)))
                        {

                            _eventRepository.UpdateEventForApp(clockIn.EventId, false);
                            _eventRepository.UpdateEventForWeb(clockIn.EventId, false);
                            return true;

                        }
                    }
                    else
                        {
                            if ((welfareRoutine != null) || ((clockOut.CreatedDate < clockIn.CreatedDate) && (welfareRoutine != null)))
                            {

                                _eventRepository.UpdateEventForApp(checkIn.EventId, false);
                            _eventRepository.UpdateEventForWeb(clockIn.EventId, false);
                            return true;
                            }
                        }
                    
                 }
             }
      
            return false;
        }





        /// <summary>
        /// get user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApplicationUserViewModel UpdateUserDetails(ApplicationUserRequestViewModel model)
        {
            return Mapper.MapApplicationUserEntityToApplicationUserViewModel(_userRepository.UpdateUserDetails(Mapper.MapApplicationUserRequestViewModelToApplicationUserEntity(model)));
        }

        /// <summary>
        /// get user details list
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApplicationUserNameListViewModel GetUserNameList(string UserMasterAdminId, string searchKey)
        {
            ApplicationUserNameListViewModel applicationUserNameListViewModel = new();
            applicationUserNameListViewModel.List = _userRepository.GetUserNameList(UserMasterAdminId, searchKey).Select(a => Mapper.MapApplicationUserEntityToApplicationUserNameViewModel(a))?.ToList();
            foreach (var user in applicationUserNameListViewModel.List)
            {
             var isAdded =  _userRepository.InTrainingRecordExistance(user.ApplicationUserId);
                user.IsAddedInTrainingRecord = isAdded;
            } 

            return applicationUserNameListViewModel;
        }

        /// <summary>
        /// Get operator and both user details 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <summary>
        public List<GetOperatorBothUserDetailsViewModel> GetOperatorBothUserDetails(string masterAdminId)
        {
            return _userRepository.GetOperatorBothUserDetails(masterAdminId).Select(a => Mapper.MapApplicationUserEntityToGetOperatorBothUserDetailsViewModel(a))?.ToList();
        }

        /// <summary>
        /// update user profile
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ApplicationUserProfileImageViewModel UpdateProfileImage(ApplicationUserProfileImageViewModel model)
        {
            return Mapper.MapApplicationUserEntityToApplicationUserProfileImageViewModel(_userRepository.UpdateProfileImage(Mapper.MapApplicationUserProfileImageViewModelToApplicationUserEntity(model)));
        }
        
        /// <summary>
        /// get user details
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public FileViewModel GetProfileImage(string userId)
        {
            return Mapper.MapApplicationUserEntityToFileViewModel(_userRepository.GetProfileImage(userId));
        }

        /// <summary>
        ///  Update User Location
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool UpdateUserLocation(string userId, string location)
        {
            return _userRepository.UpdateUserLocation(userId, location);
        }

        /// <summary>
        /// Get User MsaterAdminId into the system 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <summary>
        public string GetMasterAdminId(string userId)
        {
            return _userRepository.GetMasterAdminId(userId);
        }


        /// <summary>
        /// update user details app
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UpdateUserResponseViewModel UpdateUserDetailApp(string userId, UpdateUserRequestViewModel model)
        {
            return Mapper.MapApplicationUserEntityToUpdateUserResponseViewModel(_userRepository.UpdateUserDetailApp(Mapper.MapUpdateUserRequestViewModelToApplicationUserEntity(userId,model)));
        }

        /// <summary>
        /// Get  user detail by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <summary>
       public  ApplicationUserViewModel GetUserDetailByEmail(string email)
        {
            return Mapper.MapApplicationUserEntityToApplicationUserViewModel(_userRepository.GetUserDetailByEmail(email));
        }
        
        /// <summary>
        /// Get existing user detail by email in another farm
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        /// <summary>
       public ApplicationUserViewModel GetExistingUserDetailByEmail(string email, string LoginUserMasterAdminId)
        {
            return Mapper.MapApplicationUserEntityToApplicationUserViewModel(_userRepository.GetExistingUserDetailByEmail(email, LoginUserMasterAdminId));
        }

        /// <summary>
        /// get users list
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <param name="UserMasterAdminId"></param>
        /// <returns></returns>
        public ApplicationUsersListViewModel GetUsersList(string UserMasterAdminId, Guid trainingRecordId)
        {
            ApplicationUsersListViewModel model = new();
            model.List = _userRepository.GetUsersList(UserMasterAdminId).Select(a => Mapper.MapApplicationUserEntityToApplicationUsersViewModel(a))?.ToList();
            foreach (var user in model.List)
            {
                user.IsAdded = _userRepository.IsUserAddedTrainingRecord(trainingRecordId, user.ApplicationUserId);
              
            }

            return model;
        }
        
        /// <summary>
        /// get users list by email
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <param name="UserMasterAdminId"></param>
        /// <returns></returns>
        public List<ApplicationUser> GetUsersListByEmail(string email)
        {
            return _userRepository.GetUsersListByEmail(email);
        }

        /// <summary>
        /// get farm name by masterAdminId
        /// </summary>
        /// <param name="trainingRecordId"></param>
        /// <param name="UserMasterAdminId"></param>
        /// <returns></returns>
        public string GetFarmNameByMasterAdminId(string masterAdminId)
        {
            return _userRepository.GetFarmNameByMasterAdminId(masterAdminId);
        }


        /// <summary>
        /// get farm  by masterAdminId
        /// </summary>
        /// <param name="UserMasterAdminId"></param>
        /// <returns></returns>
        public Farm GetFarmByMasterAdminId(string masterAdminId)
        {
            return _userRepository.GetFarmByMasterAdminId(masterAdminId);
        }

    }
}
