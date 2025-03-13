using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using static SmartFarmer.Core.ViewModel.IssueViewModel;

namespace SmartFarmer.Core.Service
{
    public class IssueService : IIssueService
    {

        private readonly IIssueRepository _issueRepository;
        public IssueService(IIssueRepository issueRepository)
        {
            _issueRepository = issueRepository;
        }
        /// <summary>
        /// add issue into system
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public IssueResponseViewModel AddIssue(string CreatedBy, IssueRequestViewModel model)
        {
            return Mapper.MapIssueEntityToIssueResponseViewModel(_issueRepository.AddIssue(Mapper.MapIssueRequestViewModelToIssueEntity(CreatedBy, model)));
        }
        
        /// <summary>
        /// check issue existance
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public bool IsIssueExist(Guid issueId)
        {
            return _issueRepository.IsIssueExist(issueId);
        } 
        
        /// <summary>
        /// check issueFile existance
        /// </summary>
        /// <param name="issueFileId"></param>
        /// <returns></returns>
        public bool IsIssueFileExist(Guid issueFileId)
        {
            return _issueRepository.IsIssueFileExist(issueFileId);
        }

        /// <summary>
        /// get issue details
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IssueDetailViewModel GetIssueDetails(Guid issueId)
        {
            IssueDetailViewModel issueDetailViewModel = new IssueDetailViewModel();
            issueDetailViewModel.Details = Mapper.MapIssueEntityToIssueResponseViewModel(_issueRepository.GetIssueDetails(issueId));
            issueDetailViewModel.issueFiles = _issueRepository.GetIssueFilesList(issueId).Select(a => Mapper.MapIssueFileEntityToIssueFileViewModel(a)).ToList();
            //issueDetailViewModel.Comments = _issueRepository.GetIssueCommentList(issueId).Select(a => Mapper.MapIssueCommentEntityToIssueCommentResponseViewModel(a)).ToList();
            return issueDetailViewModel;
        }

        /// <summary>
        ///update issue category details
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IssueResponseViewModel UpdateIssueDetails(IssueResponseViewModel issueResponseViewModel)
        {
            return Mapper.MapIssueEntityToIssueResponseViewModel(_issueRepository.UpdateIssueDetails(Mapper.MapIssueResponseViewModelToIssueEntity(issueResponseViewModel)));
        }

        /// <summary>
        ///get machine issues list
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public IssueListViewModel GetMachineIssuesList(Guid machineId, string masterAdminId)
        {
            IssueListViewModel issueListViewModel = new IssueListViewModel();
            issueListViewModel.List = _issueRepository.GetMachineIssuesList(machineId, masterAdminId).Select(a => Mapper.MapIssueEntityToIssueResponseViewModel(a)).ToList();
            return issueListViewModel;
        }

        /// <summary>
        ///upload issue file
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IssueFileViewModel UploadIssueFile(IssueFileViewModel model)
        {
            return Mapper.MapIssueFileEntityToIssueFileViewModel(_issueRepository.UploadIssueFile(Mapper.MapIssueFileViewModelToIssueFileEntity(model)));
        }
        
        /// <summary>
        ///get issue files
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IssueFileListViewModel GetIssueFilesList(Guid issueId)
        {
            IssueFileListViewModel issueFileListViewModel = new IssueFileListViewModel();
            issueFileListViewModel.issueFiles = _issueRepository.GetIssueFilesList(issueId).Select(a => Mapper.MapIssueFileEntityToIssueFileViewModel(a)).ToList();
            return issueFileListViewModel;
        }

        /// <summary>
        /// get issue by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IssueSearchResponseViewModel GetIssueListBySearchWithPagination(SearchIssueRequestViewModel model, string masterAdminId)
        {
            IssueSearchResponseViewModel issueList = new IssueSearchResponseViewModel();
            issueList.List = _issueRepository.GetIssueListBySearch(model.PageNumber, model.PageSize, model.SearchKey, model.IssueStatusId, model.IssueCategoryId, model.IssueTypeId,model.MachineId ,masterAdminId).Select(a => Mapper.MapIssueEntityToIssueResponseViewModel(a)).ToList();
            issueList.TotalCount = _issueRepository.GetIssueCountBySearch(model.SearchKey, model.IssueStatusId, model.IssueCategoryId, model.IssueTypeId,model.MachineId, masterAdminId);

            foreach (var item in issueList.List)
            {
                if (item.TargetDate != null && item.IssueStatusId== (int)Core.Common.Enums.IssueStatusEnum.Open)
                {
                    if (item.TargetDate < DateTime.Today)
                    {
                      item.Status=  "late"; // If the target date is in the past
                    }
                    else
                    {
                        item.Status = "due"; // If the target date is today or in the future
                    }
                }
               
            }
            return issueList;
        }
        
        /// <summary>
        /// get machine existing issues list
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public List<ExistingIssuesResponseViewModel> GetMachineExistingIssuesList(Guid MachineId)
        {
            return _issueRepository.GetMachineExistingIssuesList(MachineId).Select(a => Mapper.MapIssueEntityToExistingIssuesResponseViewModel(a)).ToList();
        }
        
        ///// <summary>
        ///// add issue comment
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public IssueCommentResponseViewModel AddIssueComment(string CreatedBy, IssueCommentRequestViewModel model)
        //{
        //    return Mapper.MapIssueCommentEntityToIssueCommentResponseViewModel(_issueRepository.AddIssueComment(Mapper.MapIssueCommentRequestViewModelToIssueCommentEntity(CreatedBy, model)));
        //}

        /// <summary>
        /// add issue comment
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool AddIssueComment(IssueCommentRequestViewModel model)
        {
            return _issueRepository.AddIssueComment(model.IssueId,model.Comment);
        }

        /// <summary>
        ///get issue file
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IssueFileViewModel GetIssueFile(Guid issueFileId)
        {
            return Mapper.MapIssueFileEntityToIssueFileViewModel(_issueRepository.GetIssueFile(issueFileId));
        }


        /// <summary>
        ///delete issue file
        /// </summary>
        /// <param name="issueFileId"></param>
        /// <returns></returns>
        public bool DeleteIssueFile(Guid issueFileId)
        {
            return _issueRepository.DeleteIssueFile(issueFileId);
        }
    }
}
