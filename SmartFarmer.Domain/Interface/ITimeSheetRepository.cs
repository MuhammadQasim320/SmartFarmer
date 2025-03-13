using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Interface
{
    public interface ITimeSheetRepository
    {
        List<Event> GetTimeSheetListBySearchFilter(string masterAdminId, /*int pageNumber, int pageSize,*/ string searchKey, int? Month, DateTime? FromDate, DateTime? ToDate,int? year);

        int GetTimeSheetCount(string masterAdminId, string searchKey, int? Month, DateTime? FromDate, DateTime? ToDate, int? year);
        List<Event> GetTimeSheetListAPP(string masterAdminId,int? Month, int? year);

        int GetTimeSheetCountAPP(string masterAdminId,  int? Month, int? year);
        string GetCreatedByName(string CreatedBy);
    }
}
