using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Core.Service
{
    public class RiskAssessmentService : IRiskAssessmentService
    {
        private IRiskAssessmentRepository _riskAssessmentRepository;
        private readonly IIssueRepository _issueRepository;
        public RiskAssessmentService(IRiskAssessmentRepository riskAssessment, IIssueRepository issueRepository)
        {
            _riskAssessmentRepository = riskAssessment;
            _issueRepository = issueRepository;
        }

        /// <summary>
        /// Add RiskAssessment
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public RiskAssessmentViewModel AddRiskAssessment(string CreatedBy, RiskAssessmentRequestViewModel model)
        {
            return Mapper.MapRiskAssessmentToRiskAssessmentViewModel(_riskAssessmentRepository.AddRiskAssessment(Mapper.MapRiskAssessmentRequestViewModelToRiskAssessment(CreatedBy,model)));
        }

        /// <summary>
        /// Add RiskAssessmentLog
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public RiskAssessmentLogViewModel AddRiskAssessmentLog(string CreatedBy, RiskAssessmentLogRequestViewModel model)
        {
            return Mapper.MapRiskAssessmentLogToRiskAssessmentLogViewModel(_riskAssessmentRepository.AddRiskAssessmentLog(Mapper.MapRiskAssessmentLogRequestViewModelToRiskAssessmentLog(CreatedBy, model)));
        }

        /// <summary>
        /// get RiskAssessment deatils 
        /// </summary>
        /// <param name="riskAssessmentId"></param>
        /// <returns></returns>
        public RiskAssessmentResponseViewModel GetRiskAssessmentDetails(Guid riskAssessmentId)
        {
            return Mapper.MapRiskAssessmentToRiskAssessmentResponseViewModel(_riskAssessmentRepository.GetRiskAssessmentDetails(riskAssessmentId));
        }

        /// <summary>
        /// get RiskAssessmentLog deatils 
        /// </summary>
        /// <param name="riskAssessmentLogId"></param>
        /// <returns></returns>
        public RiskAssessmentLogViewModel GetRiskAssessmentLogDetails(Guid riskAssessmentLogId)
        {
            return Mapper.MapRiskAssessmentLogEntityToRiskAssessmentLogResponseViewModel(_riskAssessmentRepository.GetRiskAssessmentLogDetails(riskAssessmentLogId));
        }

        /// <summary>
        /// Get RiskAssessment List By Search With Pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RiskAssessmentSearchResponseViewModel GetRiskAssessmentListBySearchWithPagination(string UserMasterAdminId, RiskAssessmentSearchRequestViewModel model)
        {
            RiskAssessmentSearchResponseViewModel riskAssessmentSearchResponse = new();
            riskAssessmentSearchResponse.List = _riskAssessmentRepository.GetRiskAssessmentListBySearch(model.PageNumber, model.PageSize, model.SearchKey,UserMasterAdminId)?.Select(a => Mapper.MapRiskAssessmentEntityToRiskAssessmentWithFileURLsResponseViewModel(a))?.ToList();
            riskAssessmentSearchResponse.TotalCount = _riskAssessmentRepository.GetRiskAssessmentCountBySearch(model.SearchKey, UserMasterAdminId);
            return riskAssessmentSearchResponse;
        }

        /// <summary>
        /// Get RiskAssessmentLog List By Search With Pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RiskAssessmentLogSearchResponseViewModel GetRiskAssessmentLogListBySearchWithPagination(string UserMasterAdminId, RiskAssessmentLogSearchRequestViewModel model)
        {
            RiskAssessmentLogSearchResponseViewModel riskAssessmentLogSearchResponse = new();
            riskAssessmentLogSearchResponse.List = _riskAssessmentRepository.GetRiskAssessmentLogListBySearch(model.PageNumber, model.PageSize, model.SearchKey, UserMasterAdminId,model.Archived,model.Expires,model.RiskAssessmentId)?.Select(a => Mapper.MapRiskAssessmentLogEntityToRiskAssessmentLogResponseViewModel(a))?.ToList();

            foreach (var item in riskAssessmentLogSearchResponse.List)
            {
                if (item.RiskAssessmentLogId != null) {
                    var actions = _issueRepository.GetActions(item.RiskAssessmentLogId, UserMasterAdminId);
                    item.Open = actions.Where(a => a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open).Count();
                    item.Complete = actions.Where(a => a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Resolved).Count();
                        }
            }


            riskAssessmentLogSearchResponse.TotalCount = _riskAssessmentRepository.GetRiskAssessmentLogCountBySearch(model.SearchKey, UserMasterAdminId, model.Archived, model.Expires,model.RiskAssessmentId);
            return riskAssessmentLogSearchResponse;
        }

        /// <summary>
        /// Is RiskAssessment Exist
        /// </summary>
        /// <param name="riskAssessmentId"></param>
        /// <returns></returns>
        public bool IsRiskAssessmentExist(Guid riskAssessmentId)
        {
            return _riskAssessmentRepository.IsRiskAssessmentExist(riskAssessmentId);
        }

        /// <summary>
        /// Is RiskAssessmentLog Exist
        /// </summary>
        /// <param name="riskAssessmentLogId"></param>
        /// <returns></returns>
        public bool IsRiskAssessmentLogExist(Guid riskAssessmentLogId)
        {
            return _riskAssessmentRepository.IsRiskAssessmentLogExist(riskAssessmentLogId);
        }

        /// <summary>
        /// Is InitialRisk Exist
        /// </summary>
        /// <param name="initialRiskId"></param>
        /// <returns></returns>
        public bool IsInitialRiskExist(int initialRiskId)
        {
            return _riskAssessmentRepository.IsInitialRiskExist(initialRiskId);
        }

        /// <summary>
        /// Is Action Exist
        /// </summary>
        /// <param name="actionId"></param>
        /// <returns></returns>
        //public bool IsActionExist(Guid actionId)
        //{
        //    return _riskAssessmentRepository.IsActionExist(actionId);
        //}

        /// <summary>
        /// Update RiskAssessment Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RiskAssessmentViewModel UpdateRiskAssessmentDetails(RiskAssessmentViewModel model)
        {
            return Mapper.MapRiskAssessmentToRiskAssessmentViewModel(_riskAssessmentRepository.UpdateRiskAssessmentDetails(Mapper.MapRiskAssessmentViewModelToRiskAssessment(model)));
        }

        /// <summary>
        /// Update RiskAssessmentLog Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public RiskAssessmentLogViewModel UpdateRiskAssessmentLogDetails(RiskAssessmentLogViewModel model)
        {
            return Mapper.MapRiskAssessmentLogToRiskAssessmentLogViewModel(_riskAssessmentRepository.UpdateRiskAssessmentLogDetails(Mapper.MapRiskAssessmentLogViewModelToRiskAssessmentLog(model)));
        }

        /// <summary>
        /// get RiskAssessment name list 
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public RiskAssessmentNameListViewModel GetRiskAssessmentNameList(string createdBy)
        {
            RiskAssessmentNameListViewModel riskAssessmentNameListViewModel = new();
            riskAssessmentNameListViewModel.List = _riskAssessmentRepository.GetRiskAssessmentNameList(createdBy).Select(a => Mapper.MapRiskAssessmentEntityToRiskAssessmentNameViewModel(a))?.ToList();
            return riskAssessmentNameListViewModel;
        }

        //RiskAssessment file functions
        public RiskAssessmentFileViewModel AddRiskAssessmentFile(RiskAssessmentFileViewModel model)
        {
            return Mapper.MapRiskAssessmentFileEntityToRiskAssessmentFileViewModel(_riskAssessmentRepository.AddRiskAssessmentFile(Mapper.MapRiskAssessmentFileViewModelToRiskAssessmentFileEntity(model)));
        }

        public IEnumerable<RiskAssessmentFileViewModel> GetRiskAssessmentFiles(Guid riskAssessmentId)
        {
            return _riskAssessmentRepository.GetRiskAssessmentFiles(riskAssessmentId).Select(a => Mapper.MapRiskAssessmentFileEntityToRiskAssessmentFileViewModel(a)).ToList();

        }

        public RiskAssessmentFileViewModel GetRiskAssessmentFile(Guid riskAssessmentFileId)
        {
            return Mapper.MapRiskAssessmentFileEntityToRiskAssessmentFileViewModel(_riskAssessmentRepository.GetRiskAssessmentFile(riskAssessmentFileId));

        }

        public bool IsRiskAssessmentFileExist(Guid riskAssessmentFileId)
        {
            return _riskAssessmentRepository.IsRiskAssessmentFileExist(riskAssessmentFileId);
        }

        public RiskAssessmentFileViewModel UploadRiskAssessmentFile(RiskAssessmentFileViewModel model)
        {
            return Mapper.MapRiskAssessmentFileEntityToRiskAssessmentFileViewModel(_riskAssessmentRepository.UploadRiskAssessmentFile(Mapper.MapRiskAssessmentFileViewModelToRiskAssessmentFileEntity(model)));
        }

        public bool DeleteRiskAssessmentFile(Guid riskAssessmentFileId)
        {
            return _riskAssessmentRepository.DeleteRiskAssessmentFile(riskAssessmentFileId);
        }
        
        public List<IssueResponseViewModel> GetCorrectiveIssueList(Guid riskAssessmentLogId, string UserMasterAdminId)
        {
            return _riskAssessmentRepository.GetCorrectiveIssueList(riskAssessmentLogId, UserMasterAdminId).Select(a => Mapper.MapIssueEntityToIssueResponseViewModel(a))?.ToList();
        }


        /// <summary>
        /// get RiskAssessmentLog  name list 
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public RiskAssessmentLogNameListViewModel GetRiskAssessmentLogNameList(string createdBy)
        {
            RiskAssessmentLogNameListViewModel riskAssessmentLogNameListViewModel = new();
            riskAssessmentLogNameListViewModel.List = _riskAssessmentRepository.GetRiskAssessmentLogNameList(createdBy).Select(a => Mapper.MapRiskAssessmentLogEntityToRiskAssessmentLogNameViewModel(a))?.ToList();
            return riskAssessmentLogNameListViewModel;
        }



        /// <summary>
        /// get RiskAssessment with Log deatils 
        /// </summary>
        /// <param name="riskAssessmentId"></param>
        /// <returns></returns>
        public RiskAssessmentWithLogDetailViewModel GetRiskAssessmentWithLogDetail(Guid riskAssessmentId)
        {
            RiskAssessmentWithLogDetailViewModel riskAssessmentWithLog = new();
            riskAssessmentWithLog.Detail=Mapper.MapRiskAssessmentToRiskAssessmentResponseViewModel(_riskAssessmentRepository.GetRiskAssessmentDetails( riskAssessmentId));
            riskAssessmentWithLog.Log=Mapper.MapRiskAssessmentLogEntityToRiskAssessmentLogResponseViewModel(_riskAssessmentRepository.GetRiskAssessmentLogDetailsByRiskAssessment(riskAssessmentId));
            riskAssessmentWithLog.Files= _riskAssessmentRepository.GetRiskAssessmentFiles(riskAssessmentId).Select(a => Mapper.MapRiskAssessmentFileEntityToRiskAssessmentFileViewModel(a)).ToList();
            return riskAssessmentWithLog;
        }
    }
}
