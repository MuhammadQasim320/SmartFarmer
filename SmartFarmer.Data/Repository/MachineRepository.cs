using Azure;
using Microsoft.EntityFrameworkCore;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using static SmartFarmer.Core.Common.Enums;
using Machine = SmartFarmer.Domain.Model.Machine;

namespace SmartFarmer.Data.Repository
{
    public class MachineRepository : IMachineRepository
    {
        private readonly SmartFarmerContext _context;
        public MachineRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add machine into system
        /// </summary>
        /// <param name="machine"></param>
        /// <returns></returns>
        public Machine AddMachine(Machine machine)
        {
            var res = _context.MachineTypes.FirstOrDefault(a => a.MachineTypeId == machine.MachineTypeId);
            if(res.UnitsTypeId == (int)Core.Common.Enums.UnitsTypeEnum.KM)
            {
                machine.WorkingIn = machine.WorkingIn + " KiloMeters";
            }
            if (res.UnitsTypeId == (int)Core.Common.Enums.UnitsTypeEnum.Miles)
            {
                machine.WorkingIn = machine.WorkingIn + " Miles";
            }
            if (res.UnitsTypeId == (int)Core.Common.Enums.UnitsTypeEnum.Hours)
            {
                machine.WorkingIn = machine.WorkingIn + " Hours";
            }
            machine.ResultId = (int)Core.Common.Enums.CheckResultEnum.Safe;
            _context.Machines.Add(machine);
            _context.SaveChanges();
            return _context.Machines.Where(a => a.MachineId == machine.MachineId).Include(a => a.MachineCategory).Include(a => a.Operator).Include(a => a.ApplicationUser).Include(a => a.MachineStatus).Include(a => a.MachineType).FirstOrDefault();
        }

        /// <summary>
        /// get machine list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Machine> GetMachineListBySearch(bool? Find, string LogInUserId, string MasterAdminId, int pageNumber, int pageSize, string searchKey, int? MachineStatusId, Guid? MachineCategoryId, Guid? MachineTypeId, bool? Archived)
        {

            if (Find == true)
            {
                var machine = _context.Machines.Where(a => (a.ApplicationUser.MasterAdminId == MasterAdminId) && (a.OperatorId == LogInUserId || a.OperatorId ==null)).Include(a => a.ApplicationUser).Include(a => a.Operator).Include(a => a.MachineCategory).Include(a => a.MachineType).AsQueryable();

                if (searchKey != null)
                {
                    searchKey = searchKey.ToLower();
                    machine = machine.Where(a => a.Name.ToLower().Contains(searchKey));
                }
                if (MachineStatusId != null)
                {
                    machine = machine.Where(a => a.MachineStatusId == MachineStatusId);

                } 
                if (Archived != null)
                {
                    machine = machine.Where(a => a.Archived == Archived);

                }
                if (MachineCategoryId != null)
                {
                    machine = machine.Where(a => a.MachineCategoryId == MachineCategoryId);

                }
                if (MachineTypeId != null)
                {
                    machine = machine.Where(a => a.MachineTypeId == MachineTypeId);

                }
                return machine.Include(a => a.MachineStatus).Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderBy(a => a.Name).ToList();
            }
            else
            {
                var machine = _context.Machines.Where(a => a.ApplicationUser.MasterAdminId == MasterAdminId).Include(a => a.ApplicationUser).Include(a => a.Operator).Include(a => a.MachineCategory).Include(a => a.MachineType).AsQueryable();
                if (searchKey != null)
                {
                    searchKey = searchKey.ToLower();
                    machine = machine.Where(a => a.Name.ToLower().Contains(searchKey));
                }
                if (MachineStatusId != null)
                {
                        machine = machine.Where(a => a.MachineStatusId == MachineStatusId);
                   
                }
                if (MachineCategoryId != null)
                {
                        machine = machine.Where(a => a.MachineCategoryId == MachineCategoryId);

                }
                if (MachineTypeId != null)
                {
                        machine = machine.Where(a => a.MachineTypeId == MachineTypeId);

                }
                if (Archived != null)
                {
                    machine = machine.Where(a => a.Archived == Archived);

                }
                return machine.Include(a => a.MachineStatus).Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderBy(a => a.Name).ToList();
            }
           
        }

        /// <summary>
        /// get Machine List For Check  search 
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Machine> GetMachineListForCheck(string MasterAdminId)
        {
            var machine = _context.Machines.Where(a=>a.ApplicationUser.MasterAdminId== MasterAdminId).Include(a=>a.ApplicationUser).Include(a => a.Operator).Include(a => a.MachineCategory).Include(a => a.MachineType).AsQueryable();
           
           
            return machine.Include(a=>a.MachineStatus).OrderBy(a => a.Name).ToList();
        }

        /// <summary>
        /// get machine count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetMachineCountBySearch(bool? Find, string LogInUserId, string MasterAdminId, string searchKey, int? MachineStatusId, Guid? MachineCategoryId, Guid? MachineTypeId, bool? Archived)
        {

            if (Find == true)
            {
                var machine = _context.Machines.Where(a =>(a.ApplicationUser.MasterAdminId == MasterAdminId) && (a.OperatorId == LogInUserId || a.OperatorId == null)).AsQueryable();
                if (searchKey != null)
                {
                    searchKey = searchKey.ToLower();
                    machine = machine.Where(a => a.Name.ToLower().Contains(searchKey));
                }
                if (MachineStatusId != null)
                {
                    machine = machine.Where(a => a.MachineStatusId == MachineStatusId);

                }
                if (MachineCategoryId != null)
                {
                    machine = machine.Where(a => a.MachineCategoryId == MachineCategoryId);

                }
                if (MachineTypeId != null)
                {
                    machine = machine.Where(a => a.MachineTypeId == MachineTypeId);

                }
                if (Archived != null)
                {
                    machine = machine.Where(a => a.Archived == Archived);

                }
                return machine.Count();
            }
            else
            {
                var machine = _context.Machines.Where(a => a.ApplicationUser.MasterAdminId == MasterAdminId).AsQueryable();
                if (searchKey != null)
                {
                    searchKey = searchKey.ToLower();
                    machine = machine.Where(a => a.Name.ToLower().Contains(searchKey));
                }
                if (MachineStatusId != null)
                {
                    machine = machine.Where(a => a.MachineStatusId == MachineStatusId);

                }
                if (MachineCategoryId != null)
                {
                    machine = machine.Where(a => a.MachineCategoryId == MachineCategoryId);

                }
                if (MachineTypeId != null)
                {
                    machine = machine.Where(a => a.MachineTypeId == MachineTypeId);

                }
                if (Archived != null)
                {
                    machine = machine.Where(a => a.Archived == Archived);

                }
                return machine.Count();
            }
       
        }

        /// <summary>
        /// get machine list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Machine> GetMachineListBySearch(string MasterAdminId, string searchKey, Guid? MachineId)
        {
            var machine = _context.Machines.Where(a => a.ApplicationUser.MasterAdminId == MasterAdminId).Include(a => a.ApplicationUser).Include(a => a.Operator).Include(a => a.MachineCategory).Include(a => a.MachineType).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                machine = machine.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (MachineId != null)
            {
                machine = machine.Where(a => a.MachineId == MachineId);
            }
            return machine.Include(a => a.MachineStatus).OrderBy(a => a.Name).ToList();
        }
        ///// <summary>
        ///// get machine count by search 
        ///// </summary>
        ///// <param name="searchKey"></param>
        ///// <returns></returns>
        //public int GetMachineCountBySearch(string MasterAdminId, string searchKey, Guid? MachineId)
        //{
        //    var machine = _context.Machines.Where(a => a.ApplicationUser.MasterAdminId == MasterAdminId).AsQueryable();
        //    if (searchKey != null)
        //    {
        //        searchKey = searchKey.ToLower();
        //        machine = machine.Where(a => a.Name.ToLower().Contains(searchKey));
        //    }
        //    if (MachineId != null)
        //    {
        //        machine = machine.Where(a => a.MachineId == MachineId);
        //    }
        //    return machine.Count();
        //}

        /// <summary>
        /// get recent machines
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Machine> GetRecentMachineDetails(string userId, int pageNumber, int pageSize, string searchKey, Guid? MachineTypeId, Guid? MachineCategoryId)
        {
            var assignedMachineIds = _context.MachineOperatorMappings.Where(a => a.OperatorId == userId && a.IsActive==false).Select(a => a.MachineId).ToList();
            var machinesQuery = _context.Machines.Where(machine => assignedMachineIds.Contains(machine.MachineId))
                .Include(machine => machine.ApplicationUser)
                .Include(machine => machine.Operator)
                .Include(machine => machine.MachineCategory)
                .Include(machine => machine.MachineType)
                .Include(machine => machine.MachineStatus)
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchKey))
            {
                searchKey = searchKey.ToLower();
                machinesQuery = machinesQuery.Where(machine => machine.Name.ToLower().Contains(searchKey)|| machine.NickName.ToLower().Contains(searchKey));
            }
            if (MachineTypeId != null)
            {
                machinesQuery = machinesQuery.Where(a => a.MachineTypeId == MachineTypeId);
            }
            if (MachineCategoryId != null)
            {
                machinesQuery = machinesQuery.Where(a => a.MachineCategoryId == MachineCategoryId);
            }
            var paginatedMachines = machinesQuery.OrderBy(machine => machine.Name).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return paginatedMachines;
        }
        /// <summary>
        /// get recent machines
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Machine> GetActiceMachineDetails(string userId)
        {
            var assignedMachineIds = _context.MachineOperatorMappings.Where(a => a.OperatorId == userId && a.IsActive == true).Select(a => a.MachineId).ToList();
            var machinesQuery = _context.Machines.Where(machine => assignedMachineIds.Contains(machine.MachineId) && machine.MachineStatusId== (int)Core.Common.Enums.MachineStatusEnum.Active)
                .Include(machine => machine.ApplicationUser)
                .Include(machine => machine.Operator)
                .Include(machine => machine.MachineCategory)
                .Include(machine => machine.MachineType)
                .Include(machine => machine.MachineType.UnitsType)
                .Include(machine => machine.MachineStatus)
                .AsQueryable();
           
            var paginatedMachines = machinesQuery.OrderBy(machine => machine.Name).ToList();
            return paginatedMachines;
        }

        /// <summary>
        /// get recent machine count 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetRecentMachineCountBySearch(string userId, string searchKey, Guid? MachineTypeId, Guid? MachineCategoryId)
        {
            var assignedMachineIds = _context.MachineOperatorMappings.Where(a => a.OperatorId == userId && a.IsActive == false).Select(a => a.MachineId).ToList();
            var machinesQuery = _context.Machines.Where(machine => assignedMachineIds.Contains(machine.MachineId))
                .Include(machine => machine.ApplicationUser)
                .Include(machine => machine.Operator)
                .Include(machine => machine.MachineCategory)
                .Include(machine => machine.MachineType)
                 .Include(machine => machine.MachineType.UnitsType)
                .Include(machine => machine.MachineStatus)
                .AsQueryable();
            if (!string.IsNullOrEmpty(searchKey))
            {
                searchKey = searchKey.ToLower();
                machinesQuery = machinesQuery.Where(machine => machine.Name.ToLower().Contains(searchKey)||machine.NickName.ToLower().Contains(searchKey));
            }
            if (MachineTypeId != null)
            {
                machinesQuery = machinesQuery.Where(a => a.MachineTypeId == MachineTypeId);
            }
            if (MachineCategoryId != null)
            {
                machinesQuery = machinesQuery.Where(a => a.MachineCategoryId == MachineCategoryId);
            }
            var paginatedMachines = machinesQuery.Count();
            return paginatedMachines;
        }

        /// <summary>
        /// check machine existence
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public bool IsMachineExist(Guid machineId)
        {
            return _context.Machines.Find(machineId) == null ? false : true;
        }

        /// <summary>
        /// get machine category details
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public Machine GetMachineDetails(Guid machineId)
        {
            return _context.Machines.Where(a=>a.MachineId==machineId).Include(a => a.MachineCategory).Include(a=> a.Operator).Include(a=>a.ApplicationUser).Include(a => a.MachineType).Include(a => a.MachineType.RiskAssessment).Include(a => a.MachineType.UnitsType).Include(a=>a.MachineStatus).FirstOrDefault();
        }

        /// <summary>
        ///update machine details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Machine UpdateMachineDetails(Machine machine)
        {
            var res = _context.Machines.Find(machine.MachineId);
            if (res != null)
            {
                res.Name = machine.Name;
                res.Description = machine.Description;
                res.LOLERDate = machine.LOLERDate;
                res.PurchaseDate = machine.PurchaseDate;
                res.MOTDate = machine.MOTDate;
                res.ManufacturedDate = machine.ManufacturedDate;
                res.Make = machine.Make;
                res.NickName = machine.NickName;
                res.MachineTypeId = machine.MachineTypeId;
                res.SerialNumber = machine.SerialNumber;
                res.ServiceInterval = machine.ServiceInterval;
                res.Model = machine.Model;
                res.MachineCategoryId = machine.MachineCategoryId;
                res.Archived = machine.Archived;
                //res.Location = machine?.Location;
                res.InSeason = machine.InSeason;
                var resp = _context.MachineTypes.FirstOrDefault(a => a.MachineTypeId == machine.MachineTypeId);
                if (resp.UnitsTypeId == (int)Core.Common.Enums.UnitsTypeEnum.KM)
                {
                    res.WorkingIn = machine.WorkingIn + " KiloMeters";
                }
                if (resp.UnitsTypeId == (int)Core.Common.Enums.UnitsTypeEnum.Miles)
                {
                    res.WorkingIn = machine.WorkingIn + " Miles";
                }
                if (resp.UnitsTypeId == (int)Core.Common.Enums.UnitsTypeEnum.Hours)
                {
                    res.WorkingIn = machine.WorkingIn + " Hours";
                }
                _context.Machines.Update(res);
                _context.SaveChanges();
                return _context.Machines.Where(a => a.MachineId == machine.MachineId).Include(a => a.Operator).Include(a => a.ApplicationUser).Include(a => a.MachineType).Include(a => a.MachineStatus).FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        ///update machine working details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Machine UpdateMachineWorkingDetails(Guid machineId, string WorkingIn)
        {
            
            var res = _context.Machines.Find(machineId);
            var res1 = _context.MachineTypes.FirstOrDefault(a => a.MachineTypeId == res.MachineTypeId);
            if (res1.UnitsTypeId == (int)Core.Common.Enums.UnitsTypeEnum.KM)
            {
                res.WorkingIn = WorkingIn + " KiloMeters";
            }
            if (res1.UnitsTypeId == (int)Core.Common.Enums.UnitsTypeEnum.Miles)
            {
                res.WorkingIn = WorkingIn + " Miles";
            }
            if (res1.UnitsTypeId == (int)Core.Common.Enums.UnitsTypeEnum.Hours)
            {
                res.WorkingIn = WorkingIn + " Hours";
            }
            _context.Machines.Update(res);
            _context.SaveChanges();
            return _context.Machines.Where(a => a.MachineId == machineId).Include(a => a.Operator).Include(a => a.ApplicationUser).Include(a => a.MachineStatus).Include(a => a.MachineType).Include(a => a.MachineType.UnitsType).FirstOrDefault();
        }

        /// <summary>
        /// get machine Image File
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public Machine GetMachineImageFile(Guid machineId)
        {
            return _context.Machines.Find(machineId);
        }

        /// <summary>
        /// update machine Image File
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public bool UpdateMachineImageFile(Guid machineId, string fileName, string fileLink)
        {
            var res = _context.Machines.Find(machineId);
            res.MachineImage = fileLink;
            res.MachineImageUniqueName = fileName;
            if (_context.Machines.Update(res) == null)
            {
                return false;
            };
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// get machine QRFile
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public Machine GetMachineQRFile(Guid machineId)
        {
            return _context.Machines.Find(machineId);
        }

        /// <summary>
        /// update machine QRFile
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public bool UpdateQRFile(Guid machineId, string fileName, string fileLink, long? machineCode)
        {
            var res = _context.Machines.Find(machineId);
            res.QRCode = fileLink;
            res.QRUniqueName = fileName;
            res.MachineCode = machineCode;
            if (_context.Machines.Update(res) == null)
            {
                return false;
            };
            _context.SaveChanges();
            return true;
        }
        
        /// <summary>
        /// get machine name list
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Machine> GetMachineNameList(string masterId)
        {
            return _context.Machines.Where(a => a.ApplicationUser.MasterAdminId == masterId).OrderBy(a=>a.Name).ToList();
        }

        /// <summary>
        /// get user operating counts
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public List<Machine> GetUserOperating(string userId)
        {
            return _context.Machines.Where(a => a.OperatorId == userId).ToList();
        }
        
        /// <summary>
        /// update machine status
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public bool UpdateMachineStatus(string operatorId, Guid machineId, string ReasonOfServiceRemoval, int machineStatusId)
        {
            var machine = _context.Machines.FirstOrDefault(a => a.MachineId == machineId);
            machine.ApplicationUserId = operatorId;
            machine.MachineStatusId = machineStatusId;
            machine.ReasonOfServiceRemoval = ReasonOfServiceRemoval;
            var res = _context.Update(machine);
            if(res == null)
            {
                return false;
            }
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// update machine operator
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public bool UpdateMachineOperator(string operatorId, Guid machineId,string location)
        {
            var machine = _context.Machines.FirstOrDefault(a => a.MachineId == machineId);
            machine.OperatorId = operatorId;
            machine.MachineStatusId = (int)Core.Common.Enums.MachineStatusEnum.Active;
            machine.Location = location;
            var res = _context.Update(machine);
            _context.SaveChanges();
            return true;
        }
        /// <summary>
        /// update machine operator mapping
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public bool UpdateMachineOperatormapping(string operatorId, Guid machineId)
        {
            var machine = _context.MachineOperatorMappings.FirstOrDefault(a => a.MachineId == machineId && a.OperatorId== operatorId);
            machine.IsActive = true;
            var res = _context.Update(machine);
            _context.SaveChanges();
            return true;
        }
        
        /// <summary>
        /// Stop Operating
        /// </summary>
        /// <param name="operatorId"></param>
        /// <returns></returns>
        public bool StopOperating(Guid machineId, string operatorId,string location)
        {
            var machineAndoperator = _context.MachineOperatorMappings.Where(a => a.MachineId == machineId && a.OperatorId == operatorId).FirstOrDefault();

            machineAndoperator.IsActive = false;
           
            _context.Update(machineAndoperator);
            var machine = _context.Machines.FirstOrDefault(a => a.MachineId == machineId);
            machine.OperatorId = null;
            machine.MachineStatusId = (int)Core.Common.Enums.MachineStatusEnum.Idle;
            machine.Location = location;
            var res = _context.Update(machine);
            _context.SaveChanges();
            return true;
        }

        /// <summary>
        /// get machine Issue counts
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public int GetMachineIssuesCount(Guid machineId,string loginUserMasterAdminId)
        {
            return _context.Issues.Where(a => a.MachineId == machineId && a.Operator.MasterAdminId == loginUserMasterAdminId && a.IssueStatusId == (int)Core.Common.Enums.IssueStatusEnum.Open).Count();
        }

        /// <summary>
        /// get machine Issue counts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool AssignMachineToOperator(MachineOperatorMapping model)
        {
            var res = _context.MachineOperatorMappings.Add(model);
            _context.SaveChanges();
            return true;
        }
        
        /// <summary>
        /// get machine Issue counts
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public MachineOperatorMapping GetMachineLastOperator(Guid machineId)
        {
            var machine = _context.Machines.FirstOrDefault(a => a.MachineId == machineId);
            if (machine.OperatorId == null)
            {
                return _context.MachineOperatorMappings.Where(a => a.MachineId == machineId).Include(a => a.Operator).OrderByDescending(a=>a.CreatedDate).FirstOrDefault();
            }
            return _context.MachineOperatorMappings.Where(a => a.MachineId == machineId).Include(a=>a.Operator).OrderByDescending(a => a.CreatedDate).Skip(1).FirstOrDefault();
        }
        
        /// <summary>
        /// check operators operating limit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsOperatorCanOperate(string operatorId)
        {
            var count = _context.Machines.Where(a => a.OperatorId == operatorId).Count();
            if (count >=3)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// check operators operating limit
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public bool IsMachineAlradyAssignedtoThisUser(Guid machineId, string operatorId)
        {
            return _context.MachineOperatorMappings.FirstOrDefault(a => a.OperatorId == operatorId && a.MachineId== machineId) ==null? false :true;
           
           
        }

        /// <summary>
        /// Get MachineType of the machine
        /// </summary>
        /// <param name="machineId"></param>
        /// <returns></returns>
        public Guid GetMachineType(Guid machineId)
        {
            return _context.Machines.FirstOrDefault(a => a.MachineId == machineId).MachineTypeId;
        }






        /// <summary>
        /// Get  machine detail by search
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public IEnumerable<Machine> GetMachineDetailSearch(string SearchKey, string UserMasterAdminId)
        {
            var machines = _context.Machines.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (machines != null)
            {
                if (SearchKey != null)
                {
                    machines = machines.Where(a => a.MachineCode.ToString().Contains(SearchKey.ToLower()));
                   // machines = machines.Where(a => a.MachineCode== MachineCode);

                }
                return machines.Include(a => a.MachineCategory).Include(a => a.Operator).Include(a => a.ApplicationUser).Include(a => a.MachineType).Include(a => a.MachineType.UnitsType).Include(a => a.MachineStatus).OrderByDescending(a => a.Name).ToList();
            }
            return null;
        }



        public MachineType GetMachineTypeDetails(Guid machineTypeId)
        {
            return _context.MachineTypes.Where(mt => mt.MachineTypeId == machineTypeId)
                .Select(mt => new MachineType
                {
                    MachineTypeId = mt.MachineTypeId,
                    NeedsTraining = true,
                    TrainingId = mt.TrainingId
                })
                .FirstOrDefault();
        }


        public bool CheckTrainingOperatorMapping(string userId, Guid trainingId)
        {
            return _context.TrainingOperatorMappings.Any(t => t.OperatorId == userId && t.TrainingId == trainingId);
        }


        public bool UpdateMachine(Machine machine)
        {
            if (machine != null)
            {
                    _context.Machines.Update(machine);
                    _context.SaveChanges();
                    return true;

            }

            return false;
        }
    }
}
