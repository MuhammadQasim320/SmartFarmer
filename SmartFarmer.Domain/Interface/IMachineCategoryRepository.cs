using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Interface
{
    public interface IMachineCategoryRepository
    {
        MachineCategory AddMachineCategory(MachineCategory MachineCategory);
        IEnumerable<MachineCategory> GetMachineCategoryListBySearch(string masterAdminId,int pageNumber, int pageSize, string searchKey);
        int GetMachineCategoryCountBySearch(string masterAdminId, string searchKey);
        bool IsMachineCategoryExist(Guid machineCategoryId);
        bool IsMachineCategoryAssign(Guid machineCategoryId);
        MachineCategory GetMachineCategoryDetails(Guid machineCategoryId);
        MachineCategory UpdateMachineCategoryDetails(Guid machineCategoryId, MachineCategory Machine);
        bool DeleteMachineCategory(Guid machineCategoryId);
        List<MachineCategory> GetMachineCategoryNameList(string createdBy);
    }
}
