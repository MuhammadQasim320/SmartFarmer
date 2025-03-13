//using Microsoft.EntityFrameworkCore;
//using SmartFarmer.Core.Interface;
//using SmartFarmer.Data.Context;
//using SmartFarmer.Domain.Interface;
//using SmartFarmer.Domain.Model;

//namespace SmartFarmer.Data.Repository
//{
//    public class SmartCourseRepository : ISmartCourseRepository
//    {
//        private SmartFarmerContext _dbContext;
//        private IFileService _fileService;
//        public SmartCourseRepository(SmartFarmerContext dbContext, IFileService fileService)
//        {
//            _dbContext = dbContext;
//            _fileService = fileService;
//        }

//        /// <summary>
//        /// Add Smart Course
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        /// <exception cref="NotImplementedException"></exception>
//        public SmartCourse AddSmartCourse(SmartCourse model)
//        {
//            model.CreatedDate = DateTime.Now;
//            _dbContext.SmartCourses.Add(model);
//            _dbContext.SaveChanges();
//            var response = _dbContext.SmartCourses.Where(a => a.SmartCourseId == model.SmartCourseId).Include(a => a.ApplicationUser).FirstOrDefault();
//            return response;
//        }

//        /// <summary>
//        /// get smart course details 
//        /// </summary>
//        /// <param name="smartCourseId"></param>
//        /// <returns></returns>
//        public SmartCourse GetSmartCourseDetails(Guid smartCourseId)
//        {
//            return _dbContext.SmartCourses.Where(a => a.SmartCourseId == smartCourseId).Include(a => a.ApplicationUser).FirstOrDefault();
//        }

//        /// <summary>
//        /// get smart course list by search pagination 
//        /// </summary>
//        /// <param name="pageNumber"></param>
//        /// <param name="pageSize"></param>
//        /// <param name="searchKey"></param>
//        /// <returns></returns>
//        /// <exception cref="NotImplementedException"></exception>
//        public IEnumerable<SmartCourse> GetSmartCourseListBySearch(int pageNumber, int pageSize, string searchKey, string UserMasterAdminId)
//        {
//            var smartCourses = _dbContext.SmartCourses.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUser).AsQueryable();
//            if (searchKey != null)
//            {
//                searchKey = searchKey.ToLower();
//                smartCourses = smartCourses.Where(a => a.Name.ToLower().Contains(searchKey)).Include(a => a.ApplicationUser);
//            }
//            return smartCourses.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderByDescending(a => a.CreatedDate).ToList();
//        }

//        /// <summary>
//        /// get smart course count by search 
//        /// </summary>
//        /// <param name="searchKey"></param>
//        /// <returns></returns>
//        /// <exception cref="NotImplementedException"></exception>
//        public int GetSmartCourseCountBySearch(string searchKey, string UserMasterAdminId)
//        {
//            var smartCourses = _dbContext.SmartCourses.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
//            if (searchKey != null)
//            {
//                searchKey = searchKey.ToLower();
//                smartCourses = smartCourses.Where(a => a.Name.ToLower().Contains(searchKey));
//            }
//            return smartCourses.Count();
//        }

//        /// <summary>
//        ///  check the smart course existence
//        /// </summary>
//        /// <param name="smartCourseId"></param>
//        /// <returns></returns>
//        /// <exception cref="NotImplementedException"></exception>
//        public bool IsSmartCourseExist(Guid smartCourseId)
//        {
//            return _dbContext.SmartCourses.Find(smartCourseId) == null ? false : true;
//        }

//        /// <summary>
//        /// Update Smart Course Details
//        /// </summary>
//        /// <param name="model"></param>
//        /// <returns></returns>
//        /// <exception cref="NotImplementedException"></exception>
//        public SmartCourse UpdateSmartCourseDetails(SmartCourse model)
//        {
//            var smartCourse = _dbContext.SmartCourses.Find(model.SmartCourseId);
//            if (smartCourse != null)
//            {
//                smartCourse.Name = model.Name;
//                _dbContext.SmartCourses.Update(smartCourse);
//                _dbContext.SaveChanges();
//                var response = _dbContext.SmartCourses.Where(a => a.SmartCourseId == model.SmartCourseId).Include(a => a.ApplicationUser).FirstOrDefault();
//                return response;
//            }
//            return null;
//        }

//        /// <summary>
//        /// delete  Smart Course 
//        /// </summary>
//        /// <param name="smartCourseId"></param>
//        /// <returns></returns>
//        public bool DeleteSmartCourse(Guid smartCourseId)
//        {
//            var smartCourse = _dbContext.SmartCourses.FirstOrDefault(m => m.SmartCourseId == smartCourseId);
//            if (smartCourse == null)
//            {
//                return false;
//            }
//            var trainings = _dbContext.Trainings.Where(m => m.SmartCourseId == smartCourseId).ToList();
//            if (trainings.Count != 0)
//            {
//                foreach (var training in trainings)
//                {
//                    var trainingFiles = _dbContext.TrainingFiles.Where(m => m.TrainingId == training.TrainingId).ToList();
//                    if (trainingFiles.Count != 0)
//                    {
//                        foreach (var trainingFile in trainingFiles)
//                        {
//                            var chk = _fileService.DeleteUploadFile(trainingFile.FileUniqueName);
//                            _dbContext.TrainingFiles.Remove(trainingFile);
//                        }
//                    }
//                    _dbContext.Trainings.Remove(training);
//                }
//            }
//            _dbContext.SmartCourses.Remove(smartCourse);
//            _dbContext.SaveChanges();
//            return true;
//        }
        
//        /// <summary>
//        /// get smartcourse name list 
//        /// </summary>
//        /// <param name=""></param>
//        /// <returns></returns>
//        public List<SmartCourse> GetSmartCourseNameList(string UserMasterAdminId)
//        {
//            return _dbContext.SmartCourses.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).ToList();
//        }
//    }
//}
