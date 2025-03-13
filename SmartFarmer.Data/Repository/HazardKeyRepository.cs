using Microsoft.EntityFrameworkCore;
using SmartFarmer.Data.Context;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Data.Repository
{
    public class HazardKeyRepository: IHazardKeyRepository
    {
        private readonly SmartFarmerContext _context;
        public HazardKeyRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Add hazardKey
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public HazardKey AddHazardKey(HazardKey model)
        {
            _context.HazardKeys.Add(model);
            _context.SaveChanges();
            var response = _context.HazardKeys.Where(a => a.HazardKeyId == model.HazardKeyId).Include(a => a.ApplicationUser).Include(a => a.HazardType).FirstOrDefault();
            return response;
        }


        /// <summary>
        ///  check the hazardKey existence
        /// </summary>
        /// <param name="hazardKeyId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsHazardKeyExist(Guid hazardKeyId)
        {
            return _context.HazardKeys.Find(hazardKeyId) == null ? false : true;
        }

        /// <summary>
        ///  check the hazardKeyFieldMapping existence
        /// </summary>
        /// <param name="hazardKeyFieldMappingId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool IsHazardKeyFieldMappingExist(Guid hazardKeyFieldMappingId)
        {
            return _context.HazardKeyFieldMappings.Find(hazardKeyFieldMappingId) == null ? false : true;
        }

        /// <summary>
        ///  check the hazardKey existence
        /// </summary>
        /// <param name="hazardKeyId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public HazardKey UpdateHazardKeyDetails(HazardKey model)
        {
            var hazardKey = _context.HazardKeys.Find(model.HazardKeyId);
            if(hazardKey != null)
            {
                hazardKey.Name = model.Name;
                hazardKey.Color = model.Color;
                hazardKey.HazardTypeId = model.HazardTypeId;
                _context.HazardKeys.Update(hazardKey);
                _context.SaveChanges();
                var response = _context.HazardKeys.Where(a => a.HazardKeyId == model.HazardKeyId).Include(a => a.ApplicationUser).Include(a => a.HazardType).FirstOrDefault();
                return response;
            }
            return null;
        }



        /// <summary>
        ///delete hazardKey 
        /// </summary>
        /// <param name="hazardKeyId"></param>
        /// <returns></returns>
        public bool DeleteHazardKey(Guid hazardKeyId)
        {
            var hazardKey = _context.HazardKeys.Find(hazardKeyId);
            if (hazardKey != null)
            {
                var hazardKeyFieldMapping = _context.HazardKeyFieldMappings.FirstOrDefault(m => m.HazardKeyId == hazardKeyId);
                if (hazardKeyFieldMapping != null)
                {
                    return false;
                }
                _context.HazardKeys.Remove(hazardKey);
                _context.SaveChanges();
                return true;
            }
            return false;
        }





        /// <summary>
        /// get hazardKey list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <param name="hazardTypeId"></param>
        /// <returns></returns>
        public IEnumerable<HazardKey> GetHazardKeyListBySearch(int pageNumber, int pageSize, string searchKey,int? hazardTypeId, string UserMasterAdminId)
        {
            var hazardKey = _context.HazardKeys.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                hazardKey = hazardKey.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (hazardTypeId != null)
            {
                hazardKey = hazardKey.Where(a => a.HazardTypeId == hazardTypeId);
            }
            
            return hazardKey.Include(a => a.ApplicationUser).Include(a => a.HazardType).Skip((pageNumber - 1) * pageSize).Take(pageSize).OrderBy(a => a.CreatedDate).ToList();
        }

        /// <summary>
        /// get hazardKey count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <param name="hazardTypeId,"></param>
        /// <returns></returns>
        public int GetHazardKeyCountBySearch(string searchKey, int? hazardTypeId, string UserMasterAdminId)
        {
            var hazardKey = _context.HazardKeys.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                hazardKey = hazardKey.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            if (hazardTypeId != null)
            {
                hazardKey = hazardKey.Where(a => a.HazardTypeId == hazardTypeId);
            }
            return hazardKey.Count();
        }


        /// <summary>
        /// get hazardKey details
        /// </summary>
        /// <param name="hazardKeyId"></param>
        /// <returns></returns>
        public HazardKey GetHazardKeyDetails(Guid hazardKeyId)
        {
            return _context.HazardKeys.Where(a => a.HazardKeyId == hazardKeyId).Include(a => a.ApplicationUser).Include(a => a.HazardType).FirstOrDefault();
        }


        /// <summary>
        /// Add hazardKey field locations
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public HazardKeyFieldMapping AddHazardKeyFieldLocations(HazardKeyFieldMapping model)
        {
                _context.HazardKeyFieldMappings.Add(model);
                _context.SaveChanges();
                var response = _context.HazardKeyFieldMappings.Where(a => a.HazardKeyFieldMappingId == model.HazardKeyFieldMappingId).FirstOrDefault();
                return response;
        }



        /// <summary>
        /// Get  hazardKey Name List
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public IEnumerable<HazardKey> GetHazardKeyNameListWithSearch(string searchKey, string UserMasterAdminId)
        {
            var hazardKeys = _context.HazardKeys.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (hazardKeys != null)
            {
                if (searchKey != null)
                {
                    hazardKeys = hazardKeys.Where(a => a.Name.ToLower().Contains(searchKey.ToLower()));
                }
              
                return hazardKeys.Include(a => a.HazardType).OrderByDescending(a => a.Name).ToList();
            }
            return null;
        }

        /// <summary>
        /// get hazardKeys by fieldId
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public IEnumerable<HazardKeyFieldMapping> GetFieldHazardKeys(Guid fieldId)
        {
            var hazards = _context.HazardKeyFieldMappings.Where(a => a.FieldId == fieldId).Include(a => a.HazardKey).Include(a => a.HazardKey.HazardType).ToList();
            return hazards.DistinctBy(a => a.HazardKeyFieldMappingId);

        }

        /// <summary>
        /// remove hazardKeys from field
        /// </summary>
        /// <param name="hazardKeyFieldMappingId"></param>
        /// <returns></returns>
        public bool RemoveHazardKeyField(Guid hazardKeyFieldMappingId)
        {
            var hazardKeyField = _context.HazardKeyFieldMappings.FirstOrDefault(a => a.HazardKeyFieldMappingId == hazardKeyFieldMappingId);
            if (hazardKeyField != null)
            {
                _context.HazardKeyFieldMappings.Remove(hazardKeyField);
                _context.SaveChanges();
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// remove hazardKeys field location
        /// </summary>
        /// <param name="hazardKeyFieldMappingId"></param>
        /// <returns></returns>
        public bool RemoveHazardKeyFieldLocation(Guid hazardKeyFieldMappingId)
        {
            var hazardKeyField = _context.HazardKeyFieldMappings.FirstOrDefault(a => a.HazardKeyFieldMappingId == hazardKeyFieldMappingId);
            if (hazardKeyField != null)
            {
                hazardKeyField.Location = null;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
              /// <summary>
        /// remove hazardKeys field location
        /// </summary>
        /// <param name="hazardKeyFieldMappingId"></param>
        /// <returns></returns>
        public bool RemoveFieldLocation(Guid fieldId)
        {
            var fieldshazardKeys = _context.HazardKeyFieldMappings.Where(a => a.FieldId == fieldId).ToList();

            if (fieldshazardKeys != null)
            {
                foreach (var item in fieldshazardKeys)
                {
                    _context.HazardKeyFieldMappings.Remove(item);
                }
                _context.SaveChanges();
                return true;
            }
            return false;
        }


        /// <summary>
        /// Add hazardKey locations by field
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public HazardKeyFieldMapping AddHazardKeyLocationsByField(HazardKeyFieldMapping model)
        {
            _context.HazardKeyFieldMappings.Add(model);
            _context.SaveChanges();
            var response = _context.HazardKeyFieldMappings.Where(a => a.HazardKeyFieldMappingId == model.HazardKeyFieldMappingId).FirstOrDefault();
            return response;
        }
    }
}
