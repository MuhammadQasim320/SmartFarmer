using Microsoft.AspNetCore.Http.HttpResults;
using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using static SmartFarmer.Core.ViewModel.OperatorViewModel;

namespace SmartFarmer.Core.Service
{
    public class OperatorService : IOperatorService
    {
        private readonly IOperatorRepository _operatorRepository;
        private readonly IEventRepository _eventRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly ITrainingRepository _trainingRepository;
        public OperatorService(IOperatorRepository operatorRepository, IEventRepository eventRepository, IMachineRepository machineRepository,ITrainingRepository trainingRepository)
        {
            _operatorRepository = operatorRepository;
            _eventRepository = eventRepository;
            _machineRepository = machineRepository;
            _trainingRepository = trainingRepository;
        }

        /// <summary>
        /// get operator by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public OperatorListViewModel GetOperatorListBySearchWithPagination(string UserMasterAdminId, SearchOperatorRequestViewModel model)
        {
            var operatorList = new OperatorListViewModel();
            var operators = _operatorRepository.GetOperatorListBySearch(model.PageNumber, model.PageSize, model.SearchKey, model.OperatorStatusId, model.UserGroupId, UserMasterAdminId);
            var applicationUserDetailsList = new List<ApplicationUserDetailsViewModel>();

            foreach (var Operator in operators)
            {
                var applicationUserDetailsViewModel = new ApplicationUserDetailsViewModel
                {
                    UserDetails = Mapper.MapApplicationUserEntityToApplicationUserViewModel(Operator),
                    LastEventDetails = Mapper.MapEventEntityToLastEventViewModel(_eventRepository.GetLastEvent(Operator.Id)),
                    Operating = _machineRepository.GetUserOperating(Operator.Id).Select(Mapper.MapMachineEntityToMachineNickNameViewModel).ToList(),
                    TrainingRecords = _trainingRepository.GetOperatorTrainingRecords(Operator.Id).Select(Mapper.MapTrainingOperatorMappingEntityToOperatorTrainingRecordViewModel).ToList()
                };


                foreach (var trainingRecord in applicationUserDetailsViewModel.TrainingRecords)
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
                applicationUserDetailsList.Add(applicationUserDetailsViewModel);
            }

            operatorList.List = applicationUserDetailsList;
            operatorList.TotalCount = _operatorRepository.GetOperatorCountBySearch(model.SearchKey, model.OperatorStatusId, model.UserGroupId, UserMasterAdminId);

            return operatorList;
        }

        /// <summary>
        /// get operator by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsOperatorStatusExist(int operatorStatusId)
        {
            return _operatorRepository.IsOperatorStatusExist(operatorStatusId);
        }    
        
        /// <summary>
        /// get operator by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IEnumerable<NotificationResponseViewModel> GetAllNotification(string userId)
        {
            return _operatorRepository.GetAllNotification(userId).Select(a=> Mapper.MapNotificationEntityToNotificationResponseViewModel(a))?.ToList();
        }
    }
}
