using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class IssueCategoryRepository : IIssueCategoryRepository
    {
        private readonly SmartFarmerContext _context;
        public IssueCategoryRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add issueCategory into system
        /// </summary>
        /// <param name="issueCategory"></param>
        /// <returns></returns>
        public IssueCategory AddIssueCategory(IssueCategory issueCategory)
        {
            issueCategory.IssueCategoryId = Guid.NewGuid();
            issueCategory.CreatedDate = DateTime.Now;
            _context.IssueCategories.Add(issueCategory);
            _context.SaveChanges();
            var response = _context.IssueCategories.Where(a => a.IssueCategoryId == issueCategory.IssueCategoryId).Include(a => a.ApplicationUser).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// get issueCategory list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<IssueCategory> GetIssueCategoryListBySearch(int pageNumber, int pageSize, string searchKey, string masterAdminId)
        {
            var issueCategories = _context.IssueCategories.Where(a=>a.ApplicationUser.MasterAdminId==masterAdminId).Include(a => a.ApplicationUser).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                issueCategories = issueCategories.Where(a => a.Category.ToLower().Contains(searchKey)).Include(a => a.ApplicationUser);
            }
            return issueCategories.Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderBy(a => a.Category).ToList();
        }

        /// <summary>
        /// get issueCategory count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetIssueCategoryCountBySearch(string searchKey, string masterAdminId)
        {
            var issueCategories = _context.IssueCategories.Where(a=>a.ApplicationUser.MasterAdminId == masterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                issueCategories = issueCategories.Where(a => a.Category.ToLower().Contains(searchKey));
            }
            return issueCategories.Count();
        }

        /// <summary>
        /// check issue category existence
        /// </summary>
        /// <param name="issueCategoryId"></param>
        /// <returns></returns>
        public bool IsIssueCategoryExist(Guid issueCategoryId)
        {
            return _context.IssueCategories.Find(issueCategoryId) == null ? false : true;
        }

        /// <summary>
        /// get issue category details
        /// </summary>
        /// <param name="issueCategoryId"></param>
        /// <returns></returns>
        public IssueCategory GetIssueCategoryDetails(Guid issueCategoryId)
        {
            return _context.IssueCategories.Where(a => a.IssueCategoryId == issueCategoryId).Include(a => a.ApplicationUser).FirstOrDefault();
        }

        /// <summary>
        ///update issue category details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public IssueCategory UpdateIssueCategoryDetails(Guid issueCategoryId, string Category)
        {
            var res = _context.IssueCategories.Find(issueCategoryId);
            if (res != null)
            {
                res.Category = Category;
                _context.IssueCategories.Update(res);
                _context.SaveChanges();
                var response = _context.IssueCategories.Where(a => a.IssueCategoryId == issueCategoryId).Include(a => a.ApplicationUser).FirstOrDefault();
                return response;
            }
            return null;
        }

        /// <summary>
        /// Get  IssueCategory Name List
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public List<IssueCategory> GetIssueCategoryNameList(string UserMasterAdminId)
        {
            return _context.IssueCategories.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).OrderBy(a=>a.Category).ToList();
        }

        /// <summary>
        /// delete  IssueCategory 
        /// </summary>
        /// <param name="issueCategoryId"></param>
        /// <returns></returns>
        public bool DeleteIssueCategory(Guid issueCategoryId)
        {
            var issueCategory = _context.IssueCategories.FirstOrDefault(m => m.IssueCategoryId == issueCategoryId);
            try
            {
                // Remove the issue category from the context
                _context.IssueCategories.Remove(issueCategory);

                // Save changes to the database
                _context.SaveChanges();

                // Return true indicating success
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception (logging mechanism assumed)
                // Logger.LogError(ex, "Error deleting issue category");

                // Return false indicating failure
                return false;
            }
        }
    }
}
