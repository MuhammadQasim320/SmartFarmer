using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SmartFarmer.Domain.Model.TimeSheetWebRequestViewModel;
using static System.Net.Mime.MediaTypeNames;

namespace SmartFarmer.Core.Interface
{
    public interface ITimeSheetService
    {
        // TimeSheetSearchResponseViewModel GetTimeSheetListBySearchWithPagination(string masterAdminId, TimeSheetRequestViewModel model);
        TimeSheetSearchResponseViewModel GetTimeSheetList(string masterAdminId,TimeSheetRequestViewModel model);
        TimeSheetResponseViewModel GetTimeSheetListAPP(string loginUserId, TimeSheetAppRequestViewModel model);

    }
}
