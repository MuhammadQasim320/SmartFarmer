//using SmartFarmer.Core.Common;
//using SmartFarmer.Core.Interface;
//using SmartFarmer.Core.ViewModel;
//using SmartFarmer.Domain.Interface;

//namespace SmartFarmer.Core.Service
//{
//    public class SmartCourseService : ISmartCourseService
//    {
//        private ISmartCourseRepository _smartCourseRepository;
//        public SmartCourseService(ISmartCourseRepository smartCourseRepository)
//        {
//            _smartCourseRepository = smartCourseRepository;
//        }

//        /// <summary>
//        /// Add Smart Course
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        /// <exception cref="NotImplementedException"></exception>
//        public SmartCourseViewModel AddSmartCourse(string CreatedBy, SmartCourseViewModel model)
//        {
//            return Mapper.MapSmartCourseToSmartCourseViewModel(_smartCourseRepository.AddSmartCourse(Mapper.MapSmartCourseViewModelToSmartCourse(CreatedBy,model)));
//        }

//        /// <summary>
//        /// get smart course deatils 
//        /// </summary>
//        /// <param name="smartCourseId"></param>
//        /// <returns></returns>
//        public SmartCourseViewModel GetSmartCourseDetails(Guid smartCourseId)
//        {
//            return Mapper.MapSmartCourseToSmartCourseViewModel(_smartCourseRepository.GetSmartCourseDetails(smartCourseId));
//        }

//        /// <summary>
//        /// Get Smart Course List By Search With Pagination
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        public SmartCourseSearchResponseViewModel GetSmartCourseListBySearchWithPagination(string UserMasterAdminId, SmartCourseSearchRequestViewModel model)
//        {
//            SmartCourseSearchResponseViewModel smartCourseSearchResponse = new();
//            smartCourseSearchResponse.List = _smartCourseRepository.GetSmartCourseListBySearch(model.PageNumber, model.PageSize, model.searchKey, UserMasterAdminId)?.Select(a => Mapper.MapSmartCourseToSmartCourseViewModel(a))?.ToList();
//            smartCourseSearchResponse.TotalCount = _smartCourseRepository.GetSmartCourseCountBySearch(model.searchKey, UserMasterAdminId);
//            return smartCourseSearchResponse;
//        }

//        /// <summary>
//        /// Is Smart Course Exist
//        /// </summary>
//        /// <param name="smartCourseId"></param>
//        /// <returns></returns>
//        public bool IsSmartCourseExist(Guid smartCourseId)
//        {
//            return _smartCourseRepository.IsSmartCourseExist(smartCourseId);
//        }

//        /// <summary>
//        /// Update Smart Course Details
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        public SmartCourseViewModel UpdateSmartCourseDetails(SmartCourseViewModel model)
//        {
//            return Mapper.MapSmartCourseToSmartCourseViewModel(_smartCourseRepository.UpdateSmartCourseDetails(Mapper.MapSmartCourseViewModelToSmartCourse(model)));
//        }

//        /// <summary>
//        /// delete Smart Course 
//        /// </summary>
//        /// <param name="smartCourseId"></param>
//        /// <returns></returns>
//        public bool DeleteSmartCourse(Guid smartCourseId)
//        {
//            return _smartCourseRepository.DeleteSmartCourse(smartCourseId);
//        }
        
//        /// <summary>
//        /// get smartcourse name list 
//        /// </summary>
//        /// <param name=""></param>
//        /// <returns></returns>
//        public SmartCourseNameListViewModel GetSmartCourseNameList(string UserMasterAdminId)
//        {
//            SmartCourseNameListViewModel smartCourseNameListViewModel = new();
//            smartCourseNameListViewModel.List = _smartCourseRepository.GetSmartCourseNameList(UserMasterAdminId).Select(a => Mapper.MapSmartCourseEntityToSmartCourseNameViewModel(a))?.ToList();
//            return smartCourseNameListViewModel;
//        }
//    }
//}
