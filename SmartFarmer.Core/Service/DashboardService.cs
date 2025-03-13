using Microsoft.AspNetCore.Http.HttpResults;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IMachineRepository _machineRepository;
        private ICheckListRepository _checkListRepository;
        public DashboardService(IDashboardRepository dashboardRepository,IMachineRepository machineRepository, ICheckListRepository checkListRepository)
        {
            _dashboardRepository = dashboardRepository;
            _machineRepository = machineRepository;
            _checkListRepository = checkListRepository;
        }

        /// <summary>
        /// Get dashboard count
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public DashboardCountViewModel GetDashboardCount(string masterAdminId)
        {

            DashboardCountViewModel dashboardCountViewModel = new();
            dashboardCountViewModel.IssueDefect = _dashboardRepository.GetIssueDefectCount(masterAdminId);
            dashboardCountViewModel.IssueCorrective = _dashboardRepository.GetIssueCorrectiveCount(masterAdminId);
            dashboardCountViewModel.IssueWarning = _dashboardRepository.GetIssueWarningCount(masterAdminId);
            dashboardCountViewModel.TrainingExpired = _dashboardRepository.GetTrainingExpiredCount(masterAdminId);
            dashboardCountViewModel.TrainingDue = _dashboardRepository.GetTrainingDueCount(masterAdminId);
            dashboardCountViewModel.OperatorActive = _dashboardRepository.GetOperatorActiveCount(masterAdminId);
            dashboardCountViewModel.OperatorTotal = _dashboardRepository.GetOperatorTotalCount(masterAdminId);
            dashboardCountViewModel.MachineActive = _dashboardRepository.GetMachineActiveCount(masterAdminId);
            dashboardCountViewModel.MachineIdle = _dashboardRepository.GetMachineIdleCount(masterAdminId);
            dashboardCountViewModel.MachineTotal = _dashboardRepository.GetMachineTotalCount(masterAdminId);
            dashboardCountViewModel.MachineOutOfService = _dashboardRepository.GetMachineOutOfServiceCount(masterAdminId);
            dashboardCountViewModel.MachineOutOfSeason = _dashboardRepository.GetMachineOutOfSeasonCount(masterAdminId);
            return dashboardCountViewModel;
        }


        /// <summary>
        /// Get dashboard count
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public PreCheckCountViewModel GetPreCheckDashboardCount(string masterAdminId)
        {

            PreCheckCountViewModel preCheckdashboardCountViewModel = new();
            PreCheckLogsResponseViewModel preCheckLogsResponseViewModel = new PreCheckLogsResponseViewModel();

            // Counters for Due, Late, and Complete statuses (this week only)
            int dueCount = 0, lateCount = 0, completeCount = 0;

            // Define the start and end of the current week
            DateTime startOfWeek = DateTime.Now.Date.AddDays(-(int)DateTime.Now.DayOfWeek + (int)DayOfWeek.Monday); // Monday
            DateTime endOfWeek = startOfWeek.AddDays(7); // Sunday

            var machines = _machineRepository.GetMachineListForCheck(masterAdminId)
                .Select(a => Mapper.MapMachineEntityToMachineResponseViewModel(a)).ToList();

            foreach (var machine in machines)
            {
                var machineTypeId = _machineRepository.GetMachineType(machine.MachineId);
                var allCheckList = _checkListRepository.GetPreCheckListOfMachineTypeforDashboard(machineTypeId);

                foreach (var checklist in allCheckList)
                {
                    GetMachineCheckWithMachineViewModel getMachineCheckViewModel = new();
                    getMachineCheckViewModel.CheckListDetails = Mapper.MapCheckListEntityToCheckListViewModel(checklist);

                    var lastCheck = _checkListRepository.GetMachineCheckListDate(machine.MachineId, checklist.CheckListId);
                    var lastCheckDate = lastCheck?.CreatedDate;
                    // Initialize status variable
                    string status = string.Empty;

                    if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.FixedDate)
                    {
                        DateTime frequencyDate = DateTime.Parse(checklist.Frequency);

                        if (lastCheckDate == null)
                        {
                            status = DateTime.Now < frequencyDate ? "Due" : "Late";
                        }
                        else if (lastCheckDate.Value <= frequencyDate)
                        {
                            status = "Complete";
                        }
                        else
                        {
                            status = DateTime.Now > frequencyDate ? "Late" : "Due";
                        }
                    }

                    else if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Periodically)
                    {
                        if (checklist.Frequency == "Before Every Use")
                        {
                            status = machine.MachineStatusId == (int)Core.Common.Enums.MachineStatusEnum.Idle || lastCheckDate == null || lastCheckDate.Value.Date < DateTime.Now.Date
                                ? "Due"
                                : "Complete";
                        }
                        else if (checklist.Frequency == "Daily")
                        {
                            if (lastCheckDate == null)
                            {
                                status = "Due";
                            }
                            else if (lastCheckDate.Value.Date == DateTime.Now.Date)
                            {
                                status = "Complete";
                            }
                            else if (DateTime.Now.Date > lastCheckDate.Value.Date.AddDays(1))
                            {
                                status = "Late";
                            }
                            else
                            {
                                status = "Due";
                            }
                        }
                        else if (checklist.Frequency == "Weekly")
                        {
                            if (lastCheckDate == null)
                            {
                                status = "Due";
                            }
                            else
                            {
                                var nextCheckDate = lastCheckDate.Value.Date.AddDays(7);
                                var lateDate = nextCheckDate.AddDays(7);

                                if (DateTime.Now.Date < nextCheckDate)
                                {
                                    status = "Complete";
                                }
                                else if (DateTime.Now.Date >= nextCheckDate && DateTime.Now.Date < lateDate)
                                {
                                    status = "Due";
                                }
                                else
                                {
                                    status = "Late";
                                }
                            }
                        }
                        else if (checklist.Frequency == "Monthly")
                        {
                            if (lastCheckDate == null)
                            {
                                status = "Due";
                            }
                            else
                            {
                                var dueDate = lastCheckDate.Value.Date.AddMonths(1);
                                var lateDate = dueDate.AddMonths(1);

                                if (DateTime.Now.Date < dueDate)
                                {
                                    status = "Complete";
                                }
                                else if (DateTime.Now.Date >= dueDate && DateTime.Now.Date < lateDate)
                                {
                                    status = "Due";
                                }
                                else
                                {
                                    status = "Late";
                                }
                            }
                        }
                    }
                    else if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Hours)
                    {
                        if (lastCheckDate == null)
                        {
                            status = "Due";
                        }
                        else
                        {
                            int frequencyInHours = int.Parse(checklist.Frequency);
                            var nextCheckTime = lastCheckDate.Value.AddHours(frequencyInHours);
                            var lateTime = nextCheckTime.AddHours(frequencyInHours);

                            if (DateTime.Now < nextCheckTime)
                            {
                                status = "Complete";
                            }
                            else if (DateTime.Now >= nextCheckTime && DateTime.Now < lateTime)
                            {
                                status = "Due";
                            }
                            else
                            {
                                status = "Late";
                            }
                        }
                    }
                    else if (checklist.FrequencyTypeId == (int)Core.Common.Enums.FrequancyTypeEnum.Distance)
                    {
                        status = lastCheckDate == null ? "Due" : "Late";
                    }

                    // Update status in ViewModel and counts (only if this week)
                    if (status == "Due" && lastCheckDate >= startOfWeek && lastCheckDate <= endOfWeek) dueCount++;
                    if (status == "Late" && lastCheckDate >= startOfWeek && lastCheckDate <= endOfWeek) lateCount++;
                    if (status == "Complete" && lastCheckDate >= startOfWeek && lastCheckDate <= endOfWeek) completeCount++;

                    getMachineCheckViewModel.CheckListDetails.CheckListStatus = status;
                    getMachineCheckViewModel.MachineDetails = Mapper.MapMachineEntityToMachineDetailViewModel(
                        _machineRepository.GetMachineDetails(machine.MachineId));

                    preCheckLogsResponseViewModel.List.Add(getMachineCheckViewModel);
                }
            }

            // Set the counts in the view model
            preCheckdashboardCountViewModel.Due = dueCount;
            preCheckdashboardCountViewModel.Late = lateCount;
            preCheckdashboardCountViewModel.Completed = completeCount;

            return preCheckdashboardCountViewModel;

        }


        /// <summary>
        /// get machine by search
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineWithOperatorCountRequestViewModel GetMachineListBySearch(string MasterAdminId, SearchMachineWithOperatorRequestViewModel model)
        {
            MachineWithOperatorCountRequestViewModel machineList = new MachineWithOperatorCountRequestViewModel();
            machineList.List = _dashboardRepository.GetMachineListBySearch(MasterAdminId, model.SearchKey).Select(a => Mapper.MapMachineByOperatorEntityToMachineByOperatorResponseViewModel(a)).ToList();
            machineList.TotalCount = _dashboardRepository.GetMachineCountBySearch(MasterAdminId, model.SearchKey);
            return machineList;
        }

        /// <summary>
        /// get operator by search
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OperatorCountRequestViewModel GetOperatorListBySearch(string MasterAdminId, SearchOperatorsRequestViewModel model)
        {
            var operatorList = new OperatorCountRequestViewModel();
            var operators = _dashboardRepository.GetOperatorListBySearch(MasterAdminId,model.SearchKey);
            var applicationUserDetailsList = new List<OperatorUserDetailsViewModel>();

            foreach (var Operator in operators)
            {
                var applicationUserDetailsViewModel = new OperatorUserDetailsViewModel
                {
                    UserDetails = Mapper.MapOperatorEntityToOperatorResponseViewModel(Operator),
                    Operating = _machineRepository.GetUserOperating(Operator.Id).Select(Mapper.MapDashbordMachineEntityToMachineNickNameViewModel).ToList()
                };
                applicationUserDetailsList.Add(applicationUserDetailsViewModel);
            }
            operatorList.List = applicationUserDetailsList;
            operatorList.TotalCount = _dashboardRepository.GetOperatorCountBySearch(MasterAdminId,model.SearchKey);

            return operatorList;
        }
    }
}
