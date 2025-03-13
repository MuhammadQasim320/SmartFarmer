using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;

namespace SmartFarmer.Data.Repository
{
    public class MachineTypeRepository : IMachineTypeRepository
    {
        private SmartFarmerContext _dbContext;
        public MachineTypeRepository(SmartFarmerContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Add MachineType
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MachineType AddMachineType(MachineType model)
        {
            _dbContext.MachineTypes.Add(model);
            _dbContext.SaveChanges();
            return model;
        }

        /// <summary>
        /// get MachineType details 
        /// </summary>
        /// <param name="machineTypeId"></param>
        /// <returns></returns>
        public MachineType GetMachineTypeDetails(Guid machineTypeId)
        {
            return _dbContext.MachineTypes.Where(a => a.MachineTypeId == machineTypeId).Include(a => a.Training).Include(a => a.RiskAssessment).Include(a => a.UnitsType).FirstOrDefault();
        }

        /// <summary>
        /// get MachineType list by search pagination 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<MachineType> GetMachineTypeListBySearch(string masterAdminId,int pageNumber, int pageSize, string searchKey, bool? NeedsTraining, Guid? RiskAssessmentId)
        {
            var machineType = _dbContext.MachineTypes.Where(a=>a.ApplicationUser.MasterAdminId== masterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                machineType = machineType.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (RiskAssessmentId != null)
            {
                machineType = machineType.Where(a => a.RiskAssessmentId == RiskAssessmentId);

            }
            if (NeedsTraining != null)
            {
                machineType = machineType.Where(a => a.NeedsTraining == NeedsTraining);

            }
            return machineType.Include(a => a.Training).Include(a => a.RiskAssessment).Include(a => a.UnitsType).Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderBy(a => a.Name).ToList();
        }

        /// <summary>
        /// get MachineType count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public int GetMachineTypeCountBySearch(string masterAdminId,string searchKey, bool? NeedsTraining, Guid? RiskAssessmentId)
        {
            var machineType = _dbContext.MachineTypes.Where(a => a.ApplicationUser.MasterAdminId == masterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                machineType = machineType.Where(a => a.Name.ToLower().Contains(searchKey)).Include(a => a.Training).Include(a => a.RiskAssessment).Include(a => a.UnitsType);
            }
            if (RiskAssessmentId != null)
            {
                machineType = machineType.Where(a => a.RiskAssessmentId == RiskAssessmentId);

            }
            if (NeedsTraining != null)
            {
                machineType = machineType.Where(a => a.NeedsTraining == NeedsTraining);

            }
            return machineType.Count();
        }

        /// <summary>
        ///  check the MachineType existence
        /// </summary>
        /// <param name="machineTypeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsMachineTypeExist(Guid machineTypeId)
        {
            return _dbContext.MachineTypes.Find(machineTypeId) == null ? false : true;
        }
        /// <summary>
        ///  check the MachineSattsu existence
        /// </summary>
        /// <param name="machineStatusId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsMachineStatusExist(int machineStatusId)
        {
            return _dbContext.MachineStatuses.Find(machineStatusId) == null ? false : true;
        }

        /// <summary>
        ///  check the UnitsType existence
        /// </summary>
        /// <param name="unitsTypeId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsUnitsTypeExist(int unitsTypeId)
        {
            return _dbContext.UnitsTypes.Find(unitsTypeId) == null ? false : true;
        }

        /// <summary>
        /// Update MachineType Details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public MachineType UpdateMachineTypeDetails(MachineType model)
        {
            var machineType = _dbContext.MachineTypes.FirstOrDefault(a => a.MachineTypeId == model.MachineTypeId);
            machineType.Name = model.Name;
            machineType.NeedsTraining = model.NeedsTraining;
            machineType.TrainingId = model?.TrainingId;
            //machineType.InitialRiskAndAdjustedRiskId = model.InitialRiskAndAdjustedRiskId;
            machineType.RiskAssessmentId = model?.RiskAssessmentId;
            machineType.UnitsTypeId = model.UnitsTypeId;

            _dbContext.MachineTypes.Update(machineType);
            _dbContext.SaveChanges();
            return machineType;
        }

        /// <summary>
        /// get machine type name list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<MachineType> GetMachineTypeNameList( string masterAdminId)
        {
            return _dbContext.MachineTypes.Where(a=>a.ApplicationUser.MasterAdminId== masterAdminId).Include(a=>a.UnitsType).OrderBy(a=>a.Name).ToList();
        }
    }
}
