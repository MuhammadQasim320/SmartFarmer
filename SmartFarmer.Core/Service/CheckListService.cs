using Microsoft.AspNetCore.Http.Metadata;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;

namespace SmartFarmer.Core.Service
{
    public class CheckListService : ICheckListService
    {
        private ICheckListRepository _checkListRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IIssueRepository _issueRepository;

        public CheckListService(ICheckListRepository checkListRepository, IEventRepository eventRepository, IMachineRepository machineRepository, IIssueRepository issueRepository)
        {
            _checkListRepository = checkListRepository;
            _eventRepository = eventRepository;
            _machineRepository = machineRepository;
            _issueRepository = issueRepository;
        }

        /// <summary>
        /// Add CheckList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CheckListViewModel AddCheckList(string CreatedBy, CheckListRequestViewModel model)
        {
            return Mapper.MapCheckListEntityToCheckListViewModel(_checkListRepository.AddCheckList(Mapper.MapCheckListRequestViewModelToCheckListEntity(CreatedBy, model)));
        }

        /// <summary>
        /// Start CheckList
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public StartCheckListResponseViewModel StartCheckList(string CreatedBy, StartCheckListViewModel model,string UserMasterAdminId)
        {
            var checklist = _checkListRepository.GetCheckListDetails(model.CheckListId);
            var MachineIssue = _issueRepository.GetMachineIssues(model.MachineId, UserMasterAdminId);
            var Defect = MachineIssue.Where(a => a.IssueTypeId == (int)Core.Common.Enums.IssueTypeEnum.Defect).Count();
            var resultId= 1;
            if (Defect > 0)
            {
                resultId = 2;
            }
            foreach(var item in model.Items)
            {
                if(item.Answer == "false"|| item.Answer == "False")
                {
                    resultId = 2;
                }
            }
            var result = Mapper.MapCheckListMachineMappingEntityToStartCheckListResponseViewModel(_checkListRepository.StartCheckList(Mapper.MapStartCheckListViewModelToCheckListMachineMappingEntity(CreatedBy, model, resultId)));
            EventRequestViewModel eventRequestViewModel = new() { Location = model.Location };
            if (checklist.CheckTypeId == 1)
            {
                _eventRepository.AddCheckEvent(Mapper.MapEventRequestViewModelToEventEntity((int)Core.Common.Enums.EventTypeEnum.Pre_Check, CreatedBy, eventRequestViewModel, model.MachineId), result.CheckListMachineMappingId);
            }
            else if (checklist.CheckTypeId == 2)
            {
                _eventRepository.AddCheckEvent(Mapper.MapEventRequestViewModelToEventEntity((int)Core.Common.Enums.EventTypeEnum.Service, CreatedBy, eventRequestViewModel, model.MachineId), result.CheckListMachineMappingId);
            }
            return result;
        }

        /// <summary>
        /// Get Last CheckList
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public StartCheckListResponseViewModel GetLastCheckList(Guid checkListId, Guid machineId)
        {
            return Mapper.MapCheckListMachineMappingEntityToStartCheckListResponseViewModel(_checkListRepository.GetLastCheckList(checkListId, machineId));
        }

        /// <summary>
        /// get CheckList deatils 
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public CheckListViewModel GetCheckListDetails(Guid checkListId)
        {
            return Mapper.MapCheckListEntityToCheckListViewModel(_checkListRepository.GetCheckListDetails(checkListId));
        }

        /// <summary>
        /// Get CheckList List
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CheckListListViewModel GetCheckListList(string UserMasterAdminId)
        {
            CheckListListViewModel checkListListViewModel = new();
            checkListListViewModel.CheckListList = _checkListRepository.GetCheckListList(UserMasterAdminId).Select(a => Mapper.MapCheckListEntityToCheckListNameListViewModel(a))?.ToList();
            return checkListListViewModel;
        }

        /// <summary>
        /// Get CheckList List By Search With Pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CheckListSearchResponseViewModel GetCheckListListBySearchWithPagination(string UserMasterAdminId, CheckListSearchRequestViewModel model)
        {
            CheckListSearchResponseViewModel checkListSearchResponse = new CheckListSearchResponseViewModel();
            checkListSearchResponse.List = _checkListRepository.GetCheckListListBySearch(model.PageNumber, model.PageSize, model.SearchKey, model.MachineTypeId, model.CheckTypeId, model.FrequencyTypeId, UserMasterAdminId)?.Select(a => Mapper.MapCheckListEntityToCheckListViewModel(a))?.ToList();
            checkListSearchResponse.TotalCount = _checkListRepository.GetCheckListCountBySearch(model.SearchKey, model.MachineTypeId, model.CheckTypeId, model.FrequencyTypeId, UserMasterAdminId);
            return checkListSearchResponse;
        }

        /// <summary>
        /// Is CheckList Exist
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public bool IsCheckListExist(Guid checkListId)
        {
            return _checkListRepository.IsCheckListExist(checkListId);
        }
        /// <summary>
        /// Is CheckListItem Exist
        /// </summary>
        /// <param name="checkListItemId"></param>
        /// <returns></returns>
        public bool IsCheckListItemExist(Guid checkListItemId)
        {
            return _checkListRepository.IsCheckListItemExist(checkListItemId);
        }

        /// <summary>
        /// Update CheckList Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public CheckListViewModel UpdateCheckListDetails(CheckListViewModel model)
        {
            return Mapper.MapCheckListEntityToCheckListViewModel(_checkListRepository.UpdateCheckListDetails(Mapper.MapCheckListViewModelToCheckListEntity(model)));
        }

        /// <summary>
        /// Update Operator CheckList Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OperatorCheckListResponseViewModel UpdateCheckListDetailByOperatorId(OperatorCheckListResponseViewModel model)
        {
            return Mapper.MapOperatorCheckListEntityToOperatorCheckListViewModel(_checkListRepository.UpdateCheckListDetailByOperatorId(Mapper.MapOperatorCheckListViewModelToOperatorCheckListEntity(model)));
        }

        /// <summary>
        /// delete CheckList 
        /// </summary>
        /// <param name="checkListId"></param>
        /// <returns></returns>
        public bool DeleteCheckList(Guid checkListId)
        {
            return _checkListRepository.DeleteCheckList(checkListId);

        }

        //CheckListItem functions.

        /// <summary>
        /// Add CheckListItem
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public GetCheckListItemsViewModel AddCheckListItems(Guid checkListId, List<CheckListItemsListViewModel> model)
        {
            GetCheckListItemsViewModel getCheckListItemsViewModel = new();
            foreach (var data in model)
            {
                CheckListItemViewModel item;
                if (data.CheckListItemId == null)
                {
                    item = Mapper.MapCheckListItemEntityToCheckListItemViewModel(_checkListRepository.AddCheckListItems(Mapper.MapCheckListItemRequestViewModelToCheckListItemsListViewModel(checkListId, data)));
                }
                else
                {
                    item = Mapper.MapCheckListItemEntityToCheckListItemViewModel(_checkListRepository.UpdateCheckListItems(Mapper.MapCheckListItemRequestViewModelToCheckListItemsListViewModelNull(checkListId, data)));
                }
                // Add each mapped item to the List collection
                getCheckListItemsViewModel.List.Add(item);
            }
            return getCheckListItemsViewModel;
        }

        /// <summary>
        /// Get CheckListItems
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public CheckListItemListViewModel GetCheckListItems(Guid checkListId)
        {
            CheckListItemListViewModel list = new();
           var CheckListDetail= _checkListRepository.GetCheckListDetails(checkListId);
            list.CheckListId = CheckListDetail.CheckListId;
            list.Name = CheckListDetail.Name;
            list.CheckListItemList = (_checkListRepository.GetCheckListItems(checkListId)).Select(a => Mapper.MapCheckListItemEntityToCheckListItemViewModel(a))?.ToList();
            return list;
        }

        /// <summary>
        /// checking checklistItem existance
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsCheckListItemExists(Guid checkListId, Guid checkListItemId)
        {
            return _checkListRepository.IsCheckListItemExists(checkListId, checkListItemId);
        }

        /// <summary>
        /// get pre-check logs list by search
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public PreCheckLogsResponseViewModel GetPreCheckLogsBySearch(string UserMasterAdminId, PreCheckLogsRequestViewModel model)
        {
            PreCheckLogsResponseViewModel preCheckLogsResponseViewModel = new PreCheckLogsResponseViewModel();
            DateTime startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday); // Start of the week (Monday)
            DateTime endOfWeek = startOfWeek.AddDays(6); // End of the week (Sunday)


            var machines = _machineRepository.GetMachineListBySearch(UserMasterAdminId, model.SearchKey, model.MachineId).Select(a => Mapper.MapMachineEntityToMachineResponseViewModel(a)).ToList();
            foreach (var machine in machines)
            {
                var machineTypeId = _machineRepository.GetMachineType(machine.MachineId);
                var allCheckList = _checkListRepository.GetPreCheckListOfMachineType(machineTypeId,model.SearchKey, model.FrequencyTypeId);
                foreach (var checklist in allCheckList)
                {
                    GetMachineCheckWithMachineViewModel getMachineCheckViewModel = new GetMachineCheckWithMachineViewModel();
                    getMachineCheckViewModel.CheckListDetails = Mapper.MapCheckListEntityToCheckListViewModel(checklist);

                    var lastCheck = _checkListRepository.GetMachineCheckListDate(machine.MachineId, checklist.CheckListId);

                    getMachineCheckViewModel.CheckListDetails.ResultId = lastCheck?.ResultId;
                    getMachineCheckViewModel.CheckListDetails.Result = lastCheck?.CheckResult?.Result;
                    getMachineCheckViewModel.CheckListDetails.LastCheckDate = lastCheck?.CreatedDate;
                    getMachineCheckViewModel.CheckListDetails.OperatorId = lastCheck?.OperatorId;
                    getMachineCheckViewModel.CheckListDetails.OperatorName = lastCheck?.Operator?.FirstName+" "+ lastCheck?.Operator?.LastName;
                    var lastCheckDate = lastCheck?.CreatedDate;

                    //Fixed Date
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.FixedDate)
                    {
                        DateTime frequencyDate = DateTime.Parse(checklist.Frequency);

                        if (lastCheckDate == null)
                        {
                            if (DateTime.Now < frequencyDate)
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Due";
                            }
                            else
                            {
                                getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late";
                            }
                        }
                        else if (lastCheckDate.Value <= frequencyDate)
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Complete";
                        }
                        else
                        {
                            getMachineCheckViewModel.CheckListDetails.CheckListStatus = "Late"; // This covers cases where `lastCheckDate.Value > frequencyDate`
                        }
                    }
                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Periodically)
                    {
                        if (checklist.Frequency == "Before Every Use")
                        {
                            if (machine.MachineStatusId== (int)Core.Common.Enums.MachineStatusEnum.Idle|| lastCheckDate == null || lastCheckDate.Value.Date < DateTime.Now.Date)
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
                    // Add filter for "This Week" if specified
                   if (model.ThisWeek==true)
                    {
                        if (lastCheckDate == null || (lastCheckDate < startOfWeek || lastCheckDate > endOfWeek))
                        {
                            continue; // Skip items not within this week
                        }
                    }
                    // Add filter for CheckListStatusId
                    if (model.CheckListStatusId.HasValue)
                    {
                        var status = getMachineCheckViewModel.CheckListDetails.CheckListStatus;
                        if (model.CheckListStatusId == 1 && status != "Due") continue;
                        if (model.CheckListStatusId == 2 && status != "Late") continue;
                        if (model.CheckListStatusId == 3 && status != "Complete") continue;
                    }
                    getMachineCheckViewModel.MachineDetails = Mapper.MapMachineEntityToMachineDetailViewModel(_machineRepository.GetMachineDetails(machine.MachineId));
                    preCheckLogsResponseViewModel.List.Add(getMachineCheckViewModel);
                }
            }
            //machineList.TotalCount = _machineRepository.GetMachineCountBySearch(MasterAdminId, model.SearchKey, model?.MachineStatusId);
            return preCheckLogsResponseViewModel;
        }




        /// <summary>
        /// Get checkList List by machineId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineCheckListListViewModel GetCheckListList(Guid machineId ,string UserMasterAdminId)
        {
            MachineCheckListListViewModel machineCheckListListViewModel = new();
            machineCheckListListViewModel.List = _checkListRepository.GetCheckListList(machineId,UserMasterAdminId).Select(a => Mapper.MapCheckListEntityToMachineCheckListViewModel(a))?.ToList();
            return machineCheckListListViewModel;
        }


        public bool DeleteCheckListItem(Guid checkListItemId)
        {
            return _checkListRepository.DeleteCheckListItem(checkListItemId);
        }
    }
}
