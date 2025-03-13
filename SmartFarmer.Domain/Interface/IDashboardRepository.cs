using Microsoft.AspNetCore.Http.HttpResults;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Interface
{
    public interface IDashboardRepository
    {
        int GetIssueDefectCount(string masterAdminId);
        int GetIssueCorrectiveCount(string masterAdminId);
        int GetIssueWarningCount(string masterAdminId);
        int GetTrainingExpiredCount(string masterAdminId);
        int GetTrainingDueCount(string masterAdminId);
        int GetOperatorActiveCount(string masterAdminId);
        int GetOperatorTotalCount(string masterAdminId);
        int GetMachineActiveCount(string masterAdminId);
        int GetMachineIdleCount(string masterAdminId);
        int GetMachineTotalCount(string masterAdminId);
        int GetMachineOutOfServiceCount(string masterAdminId);
        int GetMachineOutOfSeasonCount(string masterAdminId);
        IEnumerable<Machine> GetMachineListBySearch(string MasterAdminId, string searchKey);
        int GetMachineCountBySearch(string MasterAdminId, string searchKey);
        IEnumerable<ApplicationUser> GetOperatorListBySearch(string masterAdminId,string searchKey);
        int GetOperatorCountBySearch(string masterAdminId, string searchKey);
    }
}
