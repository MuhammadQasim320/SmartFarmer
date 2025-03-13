using SmartFarmer.Core.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.Interface
{
    public interface IDashboardService
    {
        DashboardCountViewModel GetDashboardCount(string masterAdminId);
        PreCheckCountViewModel GetPreCheckDashboardCount(string masterAdminId);
        MachineWithOperatorCountRequestViewModel GetMachineListBySearch(string MasterAdminId, SearchMachineWithOperatorRequestViewModel model);
        OperatorCountRequestViewModel GetOperatorListBySearch(string MasterAdminId, SearchOperatorsRequestViewModel model);
    }
}
