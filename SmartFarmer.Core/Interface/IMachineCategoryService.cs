using SmartFarmer.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.Interface
{
    public interface IMachineCategoryService
    {
        MachineCategoryResponseViewModel AddMachineCategory(string CreatedBy,MachineCategoryRequestViewModel machineCategory);
        MachineCategoryCountRequestViewModel GetMachineCategoryListBySearchWithPagination(string masterAdminId,SearchMachineCategoryRequestViewModel model);
        bool IsMachineCategoryExist(Guid machineCategoryId);
        bool IsMachineCategoryAssign(Guid machineCategoryId);
        MachineCategoryResponseViewModel GetMachineCategoryDetails(Guid machineCategoryId);
        MachineCategoryResponseViewModel UpdateMachineCategoryDetails(MachineCategoryResponseViewModel model);
        bool DeleteMachineCategory(Guid machineCategoryId);
        MachineCategoryNameListViewModel GetMachineCategoryNameList(string masterAdminId);

    }
}
