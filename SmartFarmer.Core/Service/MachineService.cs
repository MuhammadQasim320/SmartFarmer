using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System.Reflection;
using System.Reflection.PortableExecutable;
using static SmartFarmer.Core.Common.Enums;

namespace SmartFarmer.Core.Service
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IEventRepository _eventRepository;
        private readonly ICheckListRepository _checkListRepository;
        private readonly IIssueRepository _issueRepository;
        private readonly IUserRepository _userRepository;
        private readonly IRiskAssessmentRepository _riskAssessmentRepository;

        public MachineService(IMachineRepository machineRepository, IEventRepository eventRepository, ICheckListRepository checkListRepository, IIssueRepository issueRepository, IUserRepository userRepository,IRiskAssessmentRepository riskAssessmentRepository)
        {
            _machineRepository = machineRepository;
            _eventRepository = eventRepository;
            _checkListRepository = checkListRepository;
            _issueRepository = issueRepository;
            _userRepository = userRepository;
            _riskAssessmentRepository = riskAssessmentRepository;
        }

        /// <summary>
        /// add Machine into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineResponseViewModel AddMachine(string createdBy, MachineRequestViewModel model)
        {
            return Mapper.MapMachineEntityToMachineResponseViewModel(_machineRepository.AddMachine(Mapper.MapMachineRequestViewModelToMachineEntity(createdBy, model)));
        }

        /// <summary>
        /// get machine by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineCountRequestViewModel GetMachineListBySearchWithPagination(bool? Find, string LogInUserId, string MasterAdminId,SearchMachineRequestViewModel model)
        {
            MachineCountRequestViewModel machineList = new MachineCountRequestViewModel();
            var machines = _machineRepository.GetMachineListBySearch(Find, LogInUserId,MasterAdminId, model.PageNumber, model.PageSize, model.SearchKey, model?.MachineStatusId ,model?.MachineCategoryId,model?.MachineTypeId,model.Archived).Select(a => Mapper.MapMachineEntityToMachineResponseViewModel(a)).ToList();
            foreach(var machine in machines)
            {
                MachineResponseWithDueCheckViewModel machineResponseWithDueCheckViewModel = new();
                machineResponseWithDueCheckViewModel.Detail = Mapper.MapMachineEntityToMachineResponseViewModel(_machineRepository.GetMachineDetails(machine.MachineId));
                var recentEventDate = _eventRepository.GetRecentMachineEventDate(machine.MachineId);
                machineResponseWithDueCheckViewModel.Detail.StartOperatingTime = recentEventDate;
                var machineTypeId = _machineRepository.GetMachineType(machine.MachineId);
                var machineTypeDetails = _machineRepository.GetMachineTypeDetails(machineTypeId);
                if (machineTypeDetails != null && machineTypeDetails.NeedsTraining == true)
                {
                    machineResponseWithDueCheckViewModel.Detail.NeedsTraining = machineTypeDetails.NeedsTraining;
                    machineResponseWithDueCheckViewModel.Detail.TrainingId = machineTypeDetails.TrainingId;
                    if(machineTypeDetails.TrainingId != null)
                    {
                        var hasDoneTraining = _machineRepository.CheckTrainingOperatorMapping(LogInUserId, machineTypeDetails.TrainingId.Value);
                        machineResponseWithDueCheckViewModel.Detail.HasDoneTraining = hasDoneTraining;
                    }
                }
                var allCheckList = _checkListRepository.GetCheckListOfMachineType(machineTypeId);
                var MachineIssue = _issueRepository.GetMachineIssues(machine.MachineId, MasterAdminId);
                machineResponseWithDueCheckViewModel.Detail.Corrective = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Corrective).Count();
                machineResponseWithDueCheckViewModel.Detail.Defect = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect).Count();
                machineResponseWithDueCheckViewModel.Detail.Warning = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Warning).Count();
                if (machineResponseWithDueCheckViewModel.Detail.Defect > 0)
                {
                    machineResponseWithDueCheckViewModel.Detail.IsDefected = true;
                }
                foreach (var issue in MachineIssue)
                {
                    MachineIssueViewModel MachineIssueViewModel = new MachineIssueViewModel();
                    MachineIssueViewModel = Mapper.MapIssueEntityToMachineIssueViewModel(issue);
                    machineResponseWithDueCheckViewModel.IssueDetail.Add(MachineIssueViewModel);
                }
                foreach (var checklist in allCheckList)
                {
                    GetMachineCheckViewModel getMachineCheckViewModel = new GetMachineCheckViewModel();
                    getMachineCheckViewModel.CheckListDetails = Mapper.MapCheckListEntityToCheckListViewModel(checklist);
                    var lastCheck = _checkListRepository.GetMachineCheckListDate(machine.MachineId, checklist.CheckListId);
                    var lastCheckDate = lastCheck?.CreatedDate;
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.FixedDate)
                    {
                        DateTime frequencyDate = DateTime.Parse(checklist.Frequency);
                        if (lastCheckDate == null && DateTime.Now < frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else if (lastCheckDate == null && DateTime.Now > frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                        else if (lastCheckDate.Value <= frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                        }
                        else if (lastCheckDate == null && DateTime.Now > frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                        else if (lastCheckDate == null && DateTime.Now < frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Periodically)
                    {
                        if (checklist.Frequency == "Before Every Use")
                        {
                            if (lastCheckDate == null || lastCheckDate.Value.Date < DateTime.Now.Date)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                            }
                        }
                        else if (checklist.Frequency == "Daily")
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else if (lastCheckDate.Value.Date == DateTime.Now.Date)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                            }
                            else if (lastCheckDate.Value.Date.AddDays(1) == DateTime.Now.Date)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else if (DateTime.Now.Date > lastCheckDate.Value.Date.AddDays(1))
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                            }
                        }
                        else if (checklist.Frequency == "Weekly")
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                var nextCheckDate = lastCheckDate.Value.Date.AddDays(7);
                                var lateDate = nextCheckDate.AddDays(7); // 1 week after the due date

                                if (DateTime.Now.Date < nextCheckDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now.Date >= nextCheckDate && DateTime.Now.Date < lateDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else // DateTime.Now.Date >= lateDate
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                        else if (checklist.Frequency == "Monthly")
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                var dueDate = lastCheckDate.Value.Date.AddMonths(1);
                                var lateDate = dueDate.AddMonths(1);

                                if (DateTime.Now.Date < dueDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now.Date >= dueDate && DateTime.Now.Date < lateDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Hours)
                    {
                        if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Hours)
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                int frequencyInHours = int.Parse(checklist.Frequency);
                                var nextCheckTime = lastCheckDate.Value.AddHours(frequencyInHours);
                                var lateTime = nextCheckTime.AddHours(frequencyInHours);

                                if (DateTime.Now < nextCheckTime)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now >= nextCheckTime && DateTime.Now < lateTime)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Distance)
                    {
                        if (lastCheckDate == null)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                    }

                    machineResponseWithDueCheckViewModel.CheckLists.Add(getMachineCheckViewModel);
                }
                machineList.List.Add(machineResponseWithDueCheckViewModel);
            }
            machineList.TotalCount = _machineRepository.GetMachineCountBySearch(Find, LogInUserId, MasterAdminId,model.SearchKey, model?.MachineStatusId, model?.MachineCategoryId, model?.MachineTypeId,model?.Archived);
            return machineList;
        }

        /// <summary>
        ///get recent machines
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public RecentMachineCountRequestViewModel GetRecentMachineDetails(string userId,string UserMasterAdminId ,SearchMachineRequestViewModel model)
        {
            RecentMachineCountRequestViewModel machineList = new RecentMachineCountRequestViewModel();
            var machines = _machineRepository.GetRecentMachineDetails(userId, model.PageNumber, model.PageSize, model.SearchKey, model.MachineTypeId , model.MachineCategoryId).Select(a => Mapper.MapMachineEntityToRecentMachineResponseViewModel(a)).ToList();
            foreach (var machine in machines)
            {
                MachineResponseWithDueCheckViewModel machineResponseWithDueCheckViewModel = new();
                machineResponseWithDueCheckViewModel.Detail = Mapper.MapMachineEntityToMachineResponseViewModel(_machineRepository.GetMachineDetails(machine.MachineId));
                var recentEventDate = _eventRepository.GetRecentMachineEventDate(machine.MachineId);
                machineResponseWithDueCheckViewModel.Detail.StartOperatingTime = recentEventDate;
                var MachineIssue = _issueRepository.GetMachineIssues(machine.MachineId, UserMasterAdminId);
                machineResponseWithDueCheckViewModel.Detail.Corrective = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Corrective).Count();
                machineResponseWithDueCheckViewModel.Detail.Defect = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect).Count();
                machineResponseWithDueCheckViewModel.Detail.Warning = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Warning).Count();
                if (machineResponseWithDueCheckViewModel.Detail.Defect > 0)
                {
                    machineResponseWithDueCheckViewModel.Detail.IsDefected= true;
                }
                
                foreach (var issue in MachineIssue)
                {
                    MachineIssueViewModel MachineIssueViewModel = new MachineIssueViewModel();
                    MachineIssueViewModel = Mapper.MapIssueEntityToMachineIssueViewModel(issue);
                    machineResponseWithDueCheckViewModel.IssueDetail.Add(MachineIssueViewModel);
                }
                var machineTypeId = _machineRepository.GetMachineType(machine.MachineId);
                var machineTypeDetails = _machineRepository.GetMachineTypeDetails(machineTypeId);
                if (machineTypeDetails != null && machineTypeDetails.NeedsTraining == true)
                {
                    machineResponseWithDueCheckViewModel.Detail.NeedsTraining = machineTypeDetails.NeedsTraining;
                    machineResponseWithDueCheckViewModel.Detail.TrainingId = machineTypeDetails.TrainingId;
                    if (machineTypeDetails.TrainingId != null)
                    {
                        var hasDoneTraining = _machineRepository.CheckTrainingOperatorMapping(userId, machineTypeDetails.TrainingId.Value);
                        machineResponseWithDueCheckViewModel.Detail.HasDoneTraining = hasDoneTraining;
                    }

                }
                var allCheckList = _checkListRepository.GetCheckListOfMachineType(machineTypeId);
                foreach (var checklist in allCheckList)
                {
                    GetMachineCheckViewModel getMachineCheckViewModel = new GetMachineCheckViewModel();
                    getMachineCheckViewModel.CheckListDetails = Mapper.MapCheckListEntityToCheckListViewModel(checklist);
                    var lastCheck = _checkListRepository.GetMachineCheckListDate(machine.MachineId, checklist.CheckListId);
                    var lastCheckDate = lastCheck?.CreatedDate;
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.FixedDate)
                    {
                        DateTime frequencyDate = DateTime.Parse(checklist.Frequency);
                        if (lastCheckDate == null && DateTime.Now < frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else if (lastCheckDate == null && DateTime.Now > frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                        else if (lastCheckDate.Value <= frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                        }
                        else if (lastCheckDate == null && DateTime.Now > frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                        else if (lastCheckDate == null && DateTime.Now < frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Periodically)
                    {
                        if (checklist.Frequency == "Before Every Use")
                        {
                            if (lastCheckDate == null || lastCheckDate.Value.Date < DateTime.Now.Date)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                            }
                        }
                        else if (checklist.Frequency == "Daily")
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else if (lastCheckDate.Value.Date == DateTime.Now.Date)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                            }
                            else if (lastCheckDate.Value.Date.AddDays(1) == DateTime.Now.Date)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else if (DateTime.Now.Date > lastCheckDate.Value.Date.AddDays(1))
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                            }
                        }
                        else if (checklist.Frequency == "Weekly")
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                var nextCheckDate = lastCheckDate.Value.Date.AddDays(7);
                                var lateDate = nextCheckDate.AddDays(7); // 1 week after the due date

                                if (DateTime.Now.Date < nextCheckDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now.Date >= nextCheckDate && DateTime.Now.Date < lateDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else // DateTime.Now.Date >= lateDate
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                        else if (checklist.Frequency == "Monthly")
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                var dueDate = lastCheckDate.Value.Date.AddMonths(1);
                                var lateDate = dueDate.AddMonths(1);

                                if (DateTime.Now.Date < dueDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now.Date >= dueDate && DateTime.Now.Date < lateDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Hours)
                    {
                        if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Hours)
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                int frequencyInHours = int.Parse(checklist.Frequency);
                                var nextCheckTime = lastCheckDate.Value.AddHours(frequencyInHours);
                                var lateTime = nextCheckTime.AddHours(frequencyInHours);

                                if (DateTime.Now < nextCheckTime)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now >= nextCheckTime && DateTime.Now < lateTime)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Distance)
                    {
                        if (lastCheckDate == null)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                    }


                    machineResponseWithDueCheckViewModel.CheckLists.Add(getMachineCheckViewModel);
                }
                machineList.List.Add(machineResponseWithDueCheckViewModel);
            }
            machineList.TotalCount = _machineRepository.GetRecentMachineCountBySearch(userId, model.SearchKey, model.MachineTypeId, model.MachineCategoryId);
            return machineList;
        }

        /// <summary>
        ///get recent machines
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public ActiveMachineResponseViewModel GetActiveMachineDetails(string userId)
        {
            ActiveMachineResponseViewModel machineList = new ActiveMachineResponseViewModel();
            var machines = _machineRepository.GetActiceMachineDetails(userId).Select(a => Mapper.MapMachineEntityToRecentMachineResponseViewModel(a)).ToList();
            foreach (var machine in machines)
            {
                MachineResponseWithDueCheckViewModel machineResponseWithDueCheckViewModel = new();
                machineResponseWithDueCheckViewModel.Detail = Mapper.MapMachineEntityToMachineResponseViewModel(_machineRepository.GetMachineDetails(machine.MachineId));
                var recentEventDate = _eventRepository.GetRecentMachineEventDate(machine.MachineId);
                machineResponseWithDueCheckViewModel.Detail.StartOperatingTime = recentEventDate;
                var machineTypeId = _machineRepository.GetMachineType(machine.MachineId);
                var machineTypeDetails = _machineRepository.GetMachineTypeDetails(machineTypeId);
                if (machineTypeDetails != null && machineTypeDetails.NeedsTraining == true)
                {
                    machineResponseWithDueCheckViewModel.Detail.NeedsTraining = machineTypeDetails.NeedsTraining;
                    machineResponseWithDueCheckViewModel.Detail.TrainingId = machineTypeDetails.TrainingId;
                    if (machineTypeDetails.TrainingId != null)
                    {
                        var hasDoneTraining = _machineRepository.CheckTrainingOperatorMapping(userId, machineTypeDetails.TrainingId.Value);
                        machineResponseWithDueCheckViewModel.Detail.HasDoneTraining = hasDoneTraining;
                    }

                }
                var allCheckList = _checkListRepository.GetCheckListOfMachineType(machineTypeId);
                foreach (var checklist in allCheckList)
                {
                    GetMachineCheckViewModel getMachineCheckViewModel = new GetMachineCheckViewModel();
                    getMachineCheckViewModel.CheckListDetails = Mapper.MapCheckListEntityToCheckListViewModel(checklist);
                    var lastCheck = _checkListRepository.GetMachineCheckListDate(machine.MachineId, checklist.CheckListId);
                    var lastCheckDate = lastCheck?.CreatedDate;
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.FixedDate)
                    {
                        DateTime frequencyDate = DateTime.Parse(checklist.Frequency);
                        if (lastCheckDate == null && DateTime.Now < frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else if (lastCheckDate == null && DateTime.Now > frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                        else if (lastCheckDate.Value <= frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                        }
                        else if (lastCheckDate == null && DateTime.Now > frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                        else if (lastCheckDate == null && DateTime.Now < frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Periodically)
                    {
                        if (checklist.Frequency == "Before Every Use")
                        {
                            if (lastCheckDate == null || lastCheckDate.Value.Date < DateTime.Now.Date)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                            }
                        }
                        else if (checklist.Frequency == "Daily")
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else if (lastCheckDate.Value.Date == DateTime.Now.Date)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                            }
                            else if (lastCheckDate.Value.Date.AddDays(1) == DateTime.Now.Date)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else if (DateTime.Now.Date > lastCheckDate.Value.Date.AddDays(1))
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                            }
                        }
                        else if (checklist.Frequency == "Weekly")
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                var nextCheckDate = lastCheckDate.Value.Date.AddDays(7);
                                var lateDate = nextCheckDate.AddDays(7); // 1 week after the due date

                                if (DateTime.Now.Date < nextCheckDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now.Date >= nextCheckDate && DateTime.Now.Date < lateDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else // DateTime.Now.Date >= lateDate
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                        else if (checklist.Frequency == "Monthly")
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                var dueDate = lastCheckDate.Value.Date.AddMonths(1);
                                var lateDate = dueDate.AddMonths(1);

                                if (DateTime.Now.Date < dueDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now.Date >= dueDate && DateTime.Now.Date < lateDate)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Hours)
                    {
                        if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Hours)
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                int frequencyInHours = int.Parse(checklist.Frequency);
                                var nextCheckTime = lastCheckDate.Value.AddHours(frequencyInHours);
                                var lateTime = nextCheckTime.AddHours(frequencyInHours);

                                if (DateTime.Now < nextCheckTime)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now >= nextCheckTime && DateTime.Now < lateTime)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Distance)
                    {
                        if (lastCheckDate == null)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                    }


                    machineResponseWithDueCheckViewModel.CheckLists.Add(getMachineCheckViewModel);
                }
                machineList.List.Add(machineResponseWithDueCheckViewModel);
            }
            return machineList;
        }

        /// <summary>
        /// check machine existence
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public bool IsMachineExist(Guid machineId)
        {
            return _machineRepository.IsMachineExist(machineId);
        }

        /// <summary>
        /// get machine details
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public MachineResponseWithDueCheckViewModel GetMachineDetails(string LogInUser,Guid machineId, string UserMasterAdminId)
        {
            MachineResponseWithDueCheckViewModel machineResponseWithDueCheckViewModel = new();
            machineResponseWithDueCheckViewModel.Detail = Mapper.MapMachineEntityToMachineResponseViewModel(_machineRepository.GetMachineDetails(machineId));
            //if (machineResponseWithDueCheckViewModel.Detail.RiskAssessmentId != null)
            //{
            //    var riskAssessmentFiles = _riskAssessmentRepository.GetRiskAssessmentFilesForMachine(machineResponseWithDueCheckViewModel.Detail.RiskAssessmentId);
            //    foreach (var riskAssessmentFile in riskAssessmentFiles)
            //    {

            //            machineResponseWithDueCheckViewModel.Detail.RiskAssessmentFiles = riskAssessmentFile.RiskAssessmentFileId;
            //            machineResponseWithDueCheckViewModel.Detail.FileUrl = riskAssessmentFile.FileUrl;
            //            machineResponseWithDueCheckViewModel.Detail.FileUniqueName = riskAssessmentFile.FileUniqueName;
            //            machineResponseWithDueCheckViewModel.Detail.FileName = riskAssessmentFile.FileName;
            //    }
            //}
            if (machineResponseWithDueCheckViewModel.Detail.RiskAssessmentId != null)
            {
                var riskAssessmentFiles = _riskAssessmentRepository.GetRiskAssessmentFilesForMachine(machineResponseWithDueCheckViewModel.Detail.RiskAssessmentId);

                machineResponseWithDueCheckViewModel.Detail.RiskAssessmentFiles = riskAssessmentFiles
                    .Select(r => new MachineRiskAssessmentFileViewModel
                    {
                        RiskAssessmentFileId = r.RiskAssessmentFileId,
                        FileUrl = r.FileUrl,
                        FileUniqueName = r.FileUniqueName,
                        FileName = r.FileName
                    }).ToList();
            }
            var recentEventDate = _eventRepository.GetRecentMachineEventDate(machineId);
            machineResponseWithDueCheckViewModel.Detail.StartOperatingTime = recentEventDate;
            var MachineIssue = _issueRepository.GetMachineIssues(machineId, UserMasterAdminId);
            machineResponseWithDueCheckViewModel.Detail.Corrective = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Corrective && a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open).Count();
            machineResponseWithDueCheckViewModel.Detail.Defect = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect && a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open).Count();
            machineResponseWithDueCheckViewModel.Detail.Warning = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Warning && a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open).Count();
            if (machineResponseWithDueCheckViewModel.Detail.Defect > 0)
            {
                machineResponseWithDueCheckViewModel.Detail.IsDefected = true;
            }

            foreach (var issue in MachineIssue)
            {
                MachineIssueViewModel MachineIssueViewModel = new MachineIssueViewModel();
                MachineIssueViewModel = Mapper.MapIssueEntityToMachineIssueViewModel(issue);
                machineResponseWithDueCheckViewModel.IssueDetail.Add(MachineIssueViewModel);
            }

            var machineTypeId = _machineRepository.GetMachineType(machineId);
            var machineTypeDetails = _machineRepository.GetMachineTypeDetails(machineTypeId);
            if (machineTypeDetails != null && machineTypeDetails.NeedsTraining == true)
            {
                machineResponseWithDueCheckViewModel.Detail.NeedsTraining = machineTypeDetails.NeedsTraining;
                machineResponseWithDueCheckViewModel.Detail.TrainingId = machineTypeDetails.TrainingId;
                if (machineTypeDetails.TrainingId != null)
                {
                    var hasDoneTraining = _machineRepository.CheckTrainingOperatorMapping(LogInUser, machineTypeDetails.TrainingId.Value);
                    machineResponseWithDueCheckViewModel.Detail.HasDoneTraining = hasDoneTraining;
                }
            }
            var allCheckList = _checkListRepository.GetCheckListOfMachineType(machineTypeId);
            foreach (var checklist in allCheckList)
            {
                GetMachineCheckViewModel getMachineCheckViewModel = new GetMachineCheckViewModel();
                getMachineCheckViewModel.CheckListDetails = Mapper.MapCheckListEntityToCheckListViewModel(checklist);
                var lastCheck = _checkListRepository.GetMachineCheckListDate(machineId, checklist.CheckListId);
                var lastCheckDate = lastCheck?.CreatedDate;
                if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.FixedDate)
                {
                    // Parse the fixed frequency date
                    DateTime frequencyDate = DateTime.Parse(checklist.Frequency);

                    if (lastCheckDate == null)
                    {
                        // If no prior check exists
                        if (DateTime.Now.Date < frequencyDate.Date)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                    }
                    else
                    {
                        // If there is a prior check
                        if (lastCheckDate.Value.Date <= frequencyDate.Date)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                        }
                        else if (DateTime.Now.Date > frequencyDate.Date)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                        else
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                    }
                }
                if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Periodically)
                {
                    if (checklist.Frequency == "Before Every Use")
                    {
                        // No previous check exists; it's due before the machine is used.|| // The last check was performed on a previous date; a new check is required.
                        if (lastCheckDate == null || lastCheckDate.Value.Date < DateTime.Now.Date)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else
                        {
                            // The checklist was completed today; it's marked as complete.
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                        }
                    }
                    else if (checklist.Frequency == "Daily")
                    {
                        if (lastCheckDate == null)
                        {
                            // No previous check has been performed; it's due.
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else if (lastCheckDate.Value.Date == DateTime.Now.Date)
                        {
                            // Checklist was completed today; it's complete.
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                        }
                    
                        else if (lastCheckDate.Value.Date < DateTime.Now.Date)
                        {
                            // Calculate how many days have passed since the last check.
                            int daysSinceLastCheck = (DateTime.Now.Date - lastCheckDate.Value.Date).Days;

                            if (daysSinceLastCheck == 1)
                            {
                                // Checklist was completed yesterday; it's due today.
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else if (daysSinceLastCheck > 1)
                            {
                                // Checklist has not been completed for more than a day; it's late.
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                            }
                        }
                    }
                    else if (checklist.Frequency == "Weekly")
                    {
                        if (lastCheckDate == null)
                        {
                            // No previous check exists; it's due.
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else
                        {
                            // Calculate the next due date and the late date.
                            var nextCheckDate = lastCheckDate.Value.Date.AddDays(7); // 1 week from the last check date
                            var lateDate = nextCheckDate.AddDays(7); // 1 week after the next due date

                            if (DateTime.Now.Date < nextCheckDate)
                            {
                                // Current date is before the next due date; checklist is complete.
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                            }
                            else if (DateTime.Now.Date >= nextCheckDate && DateTime.Now.Date < lateDate)
                            {
                                // Current date is within the 7-day window after the due date; checklist is due.
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else 
                            {
                                // Current date is after the late date; checklist is late.
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                            }
                        }
                    }
                    else if (checklist.Frequency == "Monthly")
                    {
                        if (lastCheckDate == null)
                        {
                            // If there's no prior check, the checklist is immediately due.
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else
                        {
                            // Calculate the due date (1 month from the last check) and late date (1 month after the due date).
                            var dueDate = lastCheckDate.Value.Date.AddMonths(1);
                            var lateDate = dueDate.AddMonths(1);

                            if (DateTime.Now.Date < dueDate)
                            {
                                // If today's date is before the due date, the checklist is complete.
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                            }
                            else if (DateTime.Now.Date >= dueDate && DateTime.Now.Date < lateDate)
                            {
                                // If today's date is between the due date and the late date, the checklist is due.
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                // If today's date is on or after the late date, the checklist is late.
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                            }
                        }
                    }
                }
                if(checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Hours)
                {
                    if (lastCheckDate == null)
                    {
                        // No previous check recorded, so it's "Due"
                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                    }
                    else
                    {
                        // Frequency in hours
                        int frequencyInHours = int.Parse(checklist.Frequency);

                        // Calculate the next check time and the late time
                        var nextCheckTime = lastCheckDate.Value.AddHours(frequencyInHours);
                        var lateTime = nextCheckTime.AddHours(frequencyInHours);

                        if (DateTime.Now < nextCheckTime)
                        {
                            // If it's before the next check time, it's "Complete"
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                        }
                        else if (DateTime.Now >= nextCheckTime && DateTime.Now < lateTime)
                        {
                            // If it's past the next check time but before the late time, it's "Due"
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                        }
                        else
                        {
                            // If it's past the late time, it's "Late"
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                        }
                    }
                }
                if(checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Distance)
                {
                    if (lastCheckDate == null)
                    {
                        // If there is no last check date, the checklist is "Due"
                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                    }
                    else
                    {
                        // If there is a last check date, the checklist is "Late"
                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                    }
                }

                

                machineResponseWithDueCheckViewModel.CheckLists.Add(getMachineCheckViewModel);
            }
            return machineResponseWithDueCheckViewModel;
        }

        /// <summary>
        ///update machine details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineResponseViewModel UpdateMachineDetails(MachineResponseViewModel model)
        {
            return Mapper.MapMachineEntityToMachineResponseViewModel(_machineRepository.UpdateMachineDetails(Mapper.MapMachineResponseViewModelToMachineEntity(model)));
        }

        /// <summary>
        ///update machine working details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineResponseViewModel UpdateMachineWorkingDetails(Guid machineId, string WorkingIn)
        {
            return Mapper.MapMachineEntityToMachineResponseViewModel(_machineRepository.UpdateMachineWorkingDetails(machineId, WorkingIn));
        }

        /// <summary>
        ///get MachineImageFile details
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public FileViewModel GetMachineImageFile(Guid machineId)
        {
            return Mapper.MapMachineEntityToFileViewModel(_machineRepository.GetMachineImageFile(machineId));
        }

        /// <summary>
        ///update machine Image File
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateMachineImageFile(Guid machineId, string fileName, string fileLink)
        {
            return _machineRepository.UpdateMachineImageFile(machineId, fileName, fileLink);
        }

        /// <summary>
        ///get MachineQRFile details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FileViewModel GetMachineQRFile(Guid machineId)
        {
            return Mapper.MapMachineEntityToFileViewModel(_machineRepository.GetMachineQRFile(machineId));
        }

        /// <summary>
        ///update machine QR File
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool UpdateQRFile(Guid machineId, string fileName, string fileLink, long? machineCode)
        {
            return _machineRepository.UpdateQRFile(machineId, fileName, fileLink, machineCode);
        }
        
        /// <summary>
        ///get machine name list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public MachineNameListViewModel GetMachineNameList(string masterId)
        {
            MachineNameListViewModel machineNameListViewModel = new();
            machineNameListViewModel.List = _machineRepository.GetMachineNameList( masterId).Select(a => Mapper.MapMachineEntityToMachineNameViewModel(a))?.ToList();
            return machineNameListViewModel;
        }
        
        /// <summary>
        ///get machine name list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool UpdateMachineStatus(string operatorId, Guid machineId, string ReasonOfServiceRemoval, int machineStatusId)
        {
            return _machineRepository.UpdateMachineStatus(operatorId, machineId, ReasonOfServiceRemoval, machineStatusId);
        }
        
        /// <summary>
        /// operator starts operating machine
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool StartOperating(Guid machineId, string operatorId, string location, string masterAdminId)
        {
            var MachineIssue = _issueRepository.GetMachineIssues(machineId, masterAdminId);
            var Defect = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect).Count();
            if(Defect > 0)
            {
                return false;
            }
            if (_machineRepository.IsOperatorCanOperate(operatorId) == false)
            {
                return false;
            }
            var check= _machineRepository.IsMachineAlradyAssignedtoThisUser(machineId, operatorId);
            if (check == false)
            {
                _machineRepository.AssignMachineToOperator(Mapper.MapMachineOperatorMappingViewModelToMachineOperatorMappingEntity(machineId, operatorId));
            }
            else
            {
                _machineRepository.UpdateMachineOperatormapping(operatorId, machineId);
            }
           
         
            EventRequestViewModel eventRequestViewModel = new() { Location = location };
            _eventRepository.AddEvent(Mapper.MapEventRequestViewModelToEventEntity((int)Core.Common.Enums.EventTypeEnum.Operate,operatorId, eventRequestViewModel, machineId));
            return _machineRepository.UpdateMachineOperator(operatorId, machineId,location);
        }

        /// <summary>
        /// operator stops operating machine
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool StopOperating(Guid machineId, string operatorId, string location)
        {
            EventRequestViewModel eventRequestViewModel = new() { Location = location };
            _eventRepository.AddEvent(Mapper.MapEventRequestViewModelToEventEntity((int)Core.Common.Enums.EventTypeEnum.Idle,operatorId, eventRequestViewModel, machineId));
            return _machineRepository.StopOperating(machineId , operatorId, location);
        }

        /// <summary>
        /// Get machine details by search 
        /// </summary>
        /// <param name="SearchKey"></param>
        /// <param name="UserMasterAdminId"></param>
        /// <returns></returns>
        public IEnumerable<MachineResponseWithDueCheckViewModel> GetMachineDetailSearch(string SearchKey, string UserMasterAdminId)
        {
            var machinesData = _machineRepository.GetMachineDetailSearch(SearchKey, UserMasterAdminId);

            if (machinesData != null)
            {
                List<MachineResponseWithDueCheckViewModel> machines = new();

                foreach (var machine in machinesData)
                {
                    MachineResponseWithDueCheckViewModel machineResponseWithDueCheckViewModel = new()
                    {
                        Detail = Mapper.MapMachineEntityToMachineResponseViewModel(machine)
                    };

                    var recentEventDate = _eventRepository.GetRecentMachineEventDate(machine.MachineId);
                    machineResponseWithDueCheckViewModel.Detail.StartOperatingTime = recentEventDate;

                    var MachineIssue = _issueRepository.GetMachineIssues(machine.MachineId, UserMasterAdminId);
                    machineResponseWithDueCheckViewModel.Detail.Corrective = MachineIssue.Count(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Corrective);
                    machineResponseWithDueCheckViewModel.Detail.Defect = MachineIssue.Count(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect);
                    machineResponseWithDueCheckViewModel.Detail.Warning = MachineIssue.Count(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Warning);

                    foreach (var issue in MachineIssue)
                    {
                        var MachineIssueViewModel = Mapper.MapIssueEntityToMachineIssueViewModel(issue);
                        machineResponseWithDueCheckViewModel.IssueDetail.Add(MachineIssueViewModel);
                    }

                    var machineTypeId = _machineRepository.GetMachineType(machine.MachineId);
                    var allCheckList = _checkListRepository.GetCheckListOfMachineType(machineTypeId);
                  
                    foreach (var checklist in allCheckList)
                    {
                        GetMachineCheckViewModel getMachineCheckViewModel = new GetMachineCheckViewModel();
                        getMachineCheckViewModel.CheckListDetails = Mapper.MapCheckListEntityToCheckListViewModel(checklist);
                        var lastCheck = _checkListRepository.GetMachineCheckListDate(machine.MachineId, checklist.CheckListId);
                        var lastCheckDate = lastCheck?.CreatedDate;
                        if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.FixedDate)
                        {
                            DateTime frequencyDate = DateTime.Parse(checklist.Frequency);

                            if (lastCheckDate == null)
                            {
    
                                if (DateTime.Now.Date < frequencyDate.Date)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                            else
                            {
     
                                if (lastCheckDate.Value.Date <= frequencyDate.Date)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now.Date > frequencyDate.Date)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                            }
                        }
                        if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Periodically)
                        {
                            if (checklist.Frequency == "Before Every Use")
                            {
                               
                                if (lastCheckDate == null || lastCheckDate.Value.Date < DateTime.Now.Date)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                            }
                            else if (checklist.Frequency == "Daily")
                            {
                                if (lastCheckDate == null)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else if (lastCheckDate.Value.Date == DateTime.Now.Date)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }

                                else if (lastCheckDate.Value.Date < DateTime.Now.Date)
                                {
                                    int daysSinceLastCheck = (DateTime.Now.Date - lastCheckDate.Value.Date).Days;

                                    if (daysSinceLastCheck == 1)
                                    {
  
                                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                    }
                                    else if (daysSinceLastCheck > 1)
                                    {
                                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                    }
                                }
                            }
                            else if (checklist.Frequency == "Weekly")
                            {
                                if (lastCheckDate == null)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    var nextCheckDate = lastCheckDate.Value.Date.AddDays(7); 
                                    var lateDate = nextCheckDate.AddDays(7); 

                                    if (DateTime.Now.Date < nextCheckDate)
                                    {
                                    
                                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                    }
                                    else if (DateTime.Now.Date >= nextCheckDate && DateTime.Now.Date < lateDate)
                                    {
                                     
                                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                    }
                                    else
                                    {
                                        
                                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                    }
                                }
                            }
                            else if (checklist.Frequency == "Monthly")
                            {
                                if (lastCheckDate == null)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
   
                                    var dueDate = lastCheckDate.Value.Date.AddMonths(1);
                                    var lateDate = dueDate.AddMonths(1);

                                    if (DateTime.Now.Date < dueDate)
                                    {
                                        
                                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                    }
                                    else if (DateTime.Now.Date >= dueDate && DateTime.Now.Date < lateDate)
                                    {
                                        
                                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                    }
                                    else
                                    {
                                        getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                    }
                                }
                            }
                        }
                        if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Hours)
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                int frequencyInHours = int.Parse(checklist.Frequency);

                                var nextCheckTime = lastCheckDate.Value.AddHours(frequencyInHours);
                                var lateTime = nextCheckTime.AddHours(frequencyInHours);

                                if (DateTime.Now < nextCheckTime)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                                }
                                else if (DateTime.Now >= nextCheckTime && DateTime.Now < lateTime)
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                                }
                                else
                                {
                                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                                }
                            }
                        }
                        if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Distance)
                        {
                            if (lastCheckDate == null)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                            }
                        }



                        machineResponseWithDueCheckViewModel.CheckLists.Add(getMachineCheckViewModel);
                    }

                    machines.Add(machineResponseWithDueCheckViewModel);
                }

                return machines;
            }

            return null;
        }



        /// <summary>
        /// create machine result unsafe
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int UpdateMachineResultUnsafe(string masterAdminId)
        {
            var machines = _machineRepository.GetMachineNameList(masterAdminId);
            if (machines != null)
            {
                int updatedCount = 0; 

                foreach (var machine in machines)
                {
                    var issues = _issueRepository.GetMachineIssues(machine.MachineId, masterAdminId);
                    if (issues != null)
                    {
                        bool issuedefault = _issueRepository.IsIssuesDefected(issues);
                        if (issuedefault == true)
                        {
                            if(machine.ResultId != (int)CheckResultEnum.UnSafe)
                            {
                                machine.ResultId = (int)CheckResultEnum.UnSafe;
                                bool updateResult = _machineRepository.UpdateMachine(machine);

                                if (updateResult == true)
                                {
                                    updatedCount++;
                                }
                            }

                        }
                    }
                }

               
                return updatedCount; 
            }
            return 0; 
        }
        
        /// <summary>
        /// get acitve machine counts
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int GetOperatorActiveMachineCounts(string userId)
        {
            var machines = _machineRepository.GetActiceMachineDetails(userId).Select(a => Mapper.MapMachineEntityToRecentMachineResponseViewModel(a)).ToList();
            return machines.Count;
        }



    }
}
