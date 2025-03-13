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
    public class FieldRepository : IFieldRepository
    {
        private readonly SmartFarmerContext _context;
        public FieldRepository(SmartFarmerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// add field into system
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public Field AddField(Field field, string UserMasterAdminId)
        {
          var farm = _context.Farms.Where(a => a.MasterAdminId == UserMasterAdminId).Include(a => a.ApplicationUser).FirstOrDefault();
            field.FarmId = farm.FarmId;
            field.FieldId = Guid.NewGuid();
            field.CreatedDate = DateTime.Now;
            _context.Fields.Add(field);
            _context.SaveChanges();
            var response = _context.Fields.Where(a => a.FieldId == field.FieldId).Include(a => a.ApplicationUser).FirstOrDefault();
            return response;
        }

        /// <summary>
        /// get field list by search with pagination
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public IEnumerable<Field> GetFieldListBySearch(int pageNumber, int pageSize, string searchKey, string masterAdminId)
        {
            var Fields = _context.Fields.Where(a=>a.ApplicationUser.MasterAdminId == masterAdminId).Include(a => a.ApplicationUser).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                Fields = Fields.Where(a => a.Name.ToLower().Contains(searchKey)).Include(a => a.ApplicationUser).Include(a => a.Farm);
            }
            return Fields.OrderByDescending(a => a.CreatedDate).Skip((pageNumber - 1) * pageSize).Take(pageSize).Include(a=>a.Farm).ToList();
        }

        /// <summary>
        /// get field count by search 
        /// </summary>
        /// <param name="searchKey"></param>
        /// <returns></returns>
        public int GetFieldCountBySearch(string searchKey, string masterAdminId)
        {
            var Fields = _context.Fields.Where(a=>a.ApplicationUser.MasterAdminId == masterAdminId).AsQueryable();
            if (searchKey != null)
            {
                searchKey = searchKey.ToLower();
                Fields = Fields.Where(a => a.Name.ToLower().Contains(searchKey));
            }
            return Fields.Count();
        }

        /// <summary>
        /// check field existence
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public bool IsFieldExist(Guid fieldId)
        {
            return _context.Fields.Find(fieldId) == null ? false : true;
        } 
        /// <summary>
        /// check field existence
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public bool IsFieldAssigned(Guid fieldId)
        {
            return _context.HazardKeyFieldMappings.Find(fieldId) == null ? false : true;
        }

        /// <summary>
        /// get field details
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public Field GetFieldDetails(Guid fieldId)
        {
            return _context.Fields.Where(a => a.FieldId == fieldId).Include(a => a.ApplicationUser).Include(a => a.Farm).FirstOrDefault();
        }

        /// <summary>
        ///update field details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public Field UpdateFieldDetails(Guid fieldId, string name)
        {
            var res = _context.Fields.Find(fieldId);
            if (res != null)
            {
                res.Name = name;
                //res.FarmId = farmId;
                _context.Fields.Update(res);
                _context.SaveChanges();
                var response = _context.Fields.Where(a => a.FieldId == fieldId).Include(a => a.ApplicationUser).Include(a => a.Farm).FirstOrDefault();
                return response;
            }
            return null;
        }

        /// <summary>
        ///delete field 
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public bool DeleteField(Guid fieldId)
        {
            var field = _context.Fields.Find(fieldId);
            if(field != null)
            {
                var hazardKeyFieldMapping = _context.HazardKeyFieldMappings.FirstOrDefault(m => m.FieldId == fieldId);
                if (hazardKeyFieldMapping != null)
                {
                    return false;
                }
                _context.Fields.Remove(field);
                _context.SaveChanges();
                return true;
            }
            return false;
        }



        /// <summary>
        /// Get  field Name List
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public IEnumerable<Field> GetFieldNameListWithSearch(string searchKey, string UserMasterAdminId)
        {
            var fields = _context.Fields.Where(a => a.ApplicationUser.MasterAdminId == UserMasterAdminId).AsQueryable();
            if (fields != null)
            {
                if (searchKey != null)
                {
                    fields = fields.Where(a => a.Name.ToLower().Contains(searchKey.ToLower()));
                }

                return fields.OrderByDescending(a => a.Name).ToList();
            }
            return null;
        }


        /// <summary>
        /// Add field center
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Field AddFieldCenter(Field model)
        {
            var existingField = _context.Fields.FirstOrDefault(f => f.FieldId == model.FieldId);
            if (existingField != null)
            {
                existingField.Center = model.Center;
                _context.Fields.Update(existingField);
                _context.SaveChanges();
                var response = _context.Fields.Where(a => a.FieldId == model.FieldId).FirstOrDefault();
                return response;
            }

            return null;
        }

        /// <summary>
        /// Add  field boundaries
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Field AddFieldBoundaries(Field model)
        {
            var fieldBoundaries = _context.Fields.FirstOrDefault(a => a.FieldId == model.FieldId);
            if(fieldBoundaries != null)
            {
                fieldBoundaries.Boundary= model.Boundary;
                _context.Fields.Update(fieldBoundaries);
                _context.SaveChanges();
                var response = _context.Fields.Where(a => a.FieldId == model.FieldId).FirstOrDefault();
                return response;
            }
            return null;
        }


        /// <summary>
        ///remove field boundaries
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public bool RemoveFieldBoundaries(Guid fieldId)
        {
            var field = _context.Fields.Find(fieldId);
            if (field != null)
            {
                field.Boundary = null;
                field.Center = null;
                _context.SaveChanges();
                return true;
            }
            return false;
        }
    }
}
