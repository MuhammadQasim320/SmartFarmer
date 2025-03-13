using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SmartFarmer.Core.Interface;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartFarmer.Domain.Model.TimeSheetWebRequestViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SmartFarmer.Data.Repository
{
    public class TimeSheetRepository : ITimeSheetRepository
    {
        private SmartFarmerContext _dbContext;
        public TimeSheetRepository(SmartFarmerContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// get TimeSheet list by search pagination 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Event> GetTimeSheetListBySearchFilter(string masterAdminId, /*int pageNumber, int pageSize,*/ string searchKey, int? Month, DateTime? FromDate, DateTime? ToDate, int? year)
        {
            var timeSheets = _dbContext.Events.Where(a => (a.ApplicationUser.MasterAdminId == masterAdminId) && (a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out)).AsQueryable();


            if (Month != null && Month != 0)
            {
                timeSheets = timeSheets.Where(a => a.CreatedDate.Month == Month);
            }
            if (year != null && year != 0)
            {
                timeSheets = timeSheets.Where(a => a.CreatedDate.Year == year);
            }
            if (FromDate != null)
            {
                timeSheets = timeSheets.Where(a => FromDate.Value.Date >= a.CreatedDate.Date );
            }
            if (ToDate != null)
            {
                timeSheets = timeSheets.Where(a =>  ToDate.Value.Date <= a.CreatedDate.Date );
            }

            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                //searchKey = searchKey.ToLower();
                //timeSheets = timeSheets.Where(a => a.ApplicationUser.FirstName.ToLower().Contains(searchKey) || a.ApplicationUser.LastName.ToLower().Contains(searchKey));
                var searchWords = searchKey.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                timeSheets = timeSheets.Where(a =>
                    searchWords.Any(word =>a.ApplicationUser.FirstName.ToLower().Contains(word) || a.ApplicationUser.LastName.ToLower().Contains(word) )
                );
            }

            return timeSheets.Include(a => a.EventType).Include(a => a.ApplicationUser).OrderBy(a => a.CreatedDate).ToList();
        }

        /// <summary>
        /// get TimeSheet list by search pagination 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<Event> GetTimeSheetListAPP(string loginUserId, int? Month,  int? year)
        {
            var timeSheets = _dbContext.Events.Where(a => (a.CreatedBy == loginUserId) && (a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out)).AsQueryable();


            if (Month != null && Month != 0)
            {
                timeSheets = timeSheets.Where(a => a.CreatedDate.Month == Month);
            }
            if (year != null && year != 0)
            {
                timeSheets = timeSheets.Where(a => a.CreatedDate.Year == year);
            }
           

            return timeSheets.Include(a => a.EventType).Include(a => a.ApplicationUser).OrderBy(a => a.CreatedDate).ToList();
        }     

        /// <summary>
        /// get TimeSheet count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetTimeSheetCount(string masterAdminId, string searchKey, int? Month, DateTime? FromDate, DateTime? ToDate, int? year)
        {
            var timeSheets = _dbContext.Events.Where(a => (a.ApplicationUser.MasterAdminId == masterAdminId) && (a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out)).AsQueryable();


            if (Month != null && Month != 0)
            {
                timeSheets = timeSheets.Where(a => a.CreatedDate.Month == Month);
            }
            if (year != null && year != 0)
            {
                timeSheets = timeSheets.Where(a => a.CreatedDate.Year == year);
            }
            if (FromDate != null)
            {
                timeSheets = timeSheets.Where(a => FromDate.Value.Date >= a.CreatedDate.Date);
            }
            if (ToDate != null)
            {
                timeSheets = timeSheets.Where(a => ToDate.Value.Date <= a.CreatedDate.Date);
            }
            if (!string.IsNullOrWhiteSpace(searchKey))
            {
                timeSheets = timeSheets.Where(a => (a.ApplicationUser.FirstName.ToLower().Contains(searchKey.ToLower())) || (a.ApplicationUser.LastName.ToLower().Contains(searchKey.ToLower())));
            }
           var sheet= timeSheets.Include(a => a.EventType).Include(a => a.ApplicationUser).OrderBy(a => a.CreatedDate).ToList();
            var CheckinCount = sheet.Where(a=> a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In).Count();
            var CheckOutCount = sheet.Where(a=> a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out).Count();


            var count = CheckinCount - CheckOutCount;
            var Totalcount = count + CheckOutCount;

            //double result = (double)count / 2;
            //int truncatedResult = (int)result;
            ////  int roundedResult = (result % 1 == 0) ? (int)result : (int)result + 1;
            // var groupedData = sheet.GroupBy(e => new { e.CreatedBy, e.CreatedDate.Date });
            return Totalcount;
        }

        /// <summary>
        /// get TimeSheet count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetTimeSheetCountAPP(string masterAdminId, int? Month, int? year)
        {
            var timeSheets = _dbContext.Events.Where(a => (a.ApplicationUser.MasterAdminId == masterAdminId) && (a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In || a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out)).AsQueryable();


            if (Month != null && Month != 0)
            {
                timeSheets = timeSheets.Where(a => a.CreatedDate.Month == Month);
            }
            if (year != null && year != 0)
            {
                timeSheets = timeSheets.Where(a => a.CreatedDate.Year == year);
            }
           
            var sheet = timeSheets.Include(a => a.EventType).Include(a => a.ApplicationUser).OrderBy(a => a.CreatedDate).ToList();
            var CheckinCount = sheet.Where(a => a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_In).Count();
            var CheckOutCount = sheet.Where(a => a.EventTypeId == (int)Core.Common.Enums.EventTypeEnum.Clock_Out).Count();
            var count = CheckinCount - CheckOutCount;
            var Totalcount = count + CheckOutCount;

            //double result = (double)count / 2;
            //int truncatedResult = (int)result;
            ////  int roundedResult = (result % 1 == 0) ? (int)result : (int)result + 1;
            // var groupedData = sheet.GroupBy(e => new { e.CreatedBy, e.CreatedDate.Date });
            return Totalcount;
        }


        public string GetCreatedByName(string CreatedBy)
        {
            var User= _dbContext.ApplicationUsers.Where(a=>a.Id == CreatedBy).FirstOrDefault();
            var name = User?.FirstName + " " + User?.LastName;
            return name;
        }
    }
}
