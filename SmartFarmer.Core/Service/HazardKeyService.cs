using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SmartFarmer.Core.Service
{
    public class HazardKeyService:IHazardKeyService
    {
        private IHazardKeyRepository _hazardKeyRepository;
        private readonly IFieldRepository _fieldRepository;
        public HazardKeyService(IHazardKeyRepository hazardKeyRepository , IFieldRepository fieldRepository   )
        {
                _hazardKeyRepository = hazardKeyRepository;
                   _fieldRepository = fieldRepository;
        }


        /// <summary>
        /// add hazardKey into system
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HazardKeyResponseViewModel AddHazardKey(string CreatedBy, HazardKeyRequestViewModel model)
        {

            return Mapper.MapHazardKeyEntityToHazardKeyResponseViewModel(_hazardKeyRepository.AddHazardKey(Mapper.MapHazardKeyRequestViewModelToHazardKeyEntity(CreatedBy, model)));
        }

        /// <summary>
        /// Is hazardKey Exist
        /// </summary>
        /// <param name="hazardKeyId"></param>
        /// <returns></returns>
        public bool IsHazardKeyExist(Guid hazardKeyId)
        {
            return _hazardKeyRepository.IsHazardKeyExist(hazardKeyId);
        }
        
        /// <summary>
        /// Is hazardKeyFieldmapping Exist
        /// </summary>
        /// <param name="hazardKeyId"></param>
        /// <returns></returns>
        public bool IsHazardKeyFieldMappingExist(Guid hazardKeyFieldMappingId)
        {
            return _hazardKeyRepository.IsHazardKeyFieldMappingExist(hazardKeyFieldMappingId);
        }


        /// <summary>
        ///update hazardKey details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HazardKeyResponseViewModel UpdateHazardKeyDetails(HazardKeyResponseViewModel model)
        {
            return Mapper.MapHazardKeyEntityToHazardKeyResponseViewModel(_hazardKeyRepository.UpdateHazardKeyDetails(Mapper.MapHazardKeyResponseViewModelToHazardKeyEntity(model)));
        }


        /// <summary>
        ///delete hazardKey 
        /// </summary>
        /// <param name="hazardKeyId"></param>
        /// <returns></returns>
        public bool DeleteHazardKey(Guid hazardKeyId)
        {
            return _hazardKeyRepository.DeleteHazardKey(hazardKeyId);
        }




        /// <summary>
        /// get hazardKey by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public HazardKeyCountRequestViewModel GetHazardKeyListBySearchWithPagination(string UserMasterAdminId, SearchHazardKeyRequestViewModel model)
        {
            HazardKeyCountRequestViewModel hazardKeyList = new HazardKeyCountRequestViewModel();
            hazardKeyList.List = _hazardKeyRepository.GetHazardKeyListBySearch(model.PageNumber, model.PageSize, model.SearchKey,model.HazardTypeId, UserMasterAdminId).Select(a => Mapper.MapHazardKeyEntityToHazardKeyResponseViewModel(a)).ToList();
            hazardKeyList.TotalCount = _hazardKeyRepository.GetHazardKeyCountBySearch(model.SearchKey,model.HazardTypeId, UserMasterAdminId);
            return hazardKeyList;
        }


        /// <summary>
        /// get hazardKey details
        /// </summary>
        /// <param name="hazardKeyId"></param>
        /// <returns></returns>
        public HazardKeyResponseViewModel GetHazardKeyDetails(Guid hazardKeyId)
        {
            return Mapper.MapHazardKeyEntityToHazardKeyResponseViewModel(_hazardKeyRepository.GetHazardKeyDetails(hazardKeyId));
        }


        ///// <summary>
        /////add HazardKeyField locations
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public HazardKeyFieldResponseViewModel AddHazardKeyFieldLocations(HazardKeyFieldViewModel model,string serializedLocations)
        //{

        //    var entity = new HazardKeyFieldMapping
        //    {
        //        HazardKeyFieldMappingId = Guid.NewGuid(),
        //        HazardKeyId = model.HazardKeyId,
        //        FieldId = model.FieldId,
        //        Location = serializedLocations,
        //        CreatedDate = DateTime.Now
        //    };
        //    if(entity != null)
        //    {
        //        var savedEntity = _hazardKeyRepository.AddHazardKeyFieldLocations(entity);
        //        return Mapper.MapHazardKeyFieldMappingEntityToHazardKeyFieldResponseViewModel(savedEntity);
        //    }
        //    return null;
        //    //return Mapper.MapHazardKeyFieldMappingEntityToHazardKeyFieldResponseViewModel(_hazardKeyRepository.AddHazardKeyFieldLocations(Mapper.MapHazardKeyFieldViewModelToHazardKeyFieldMappingEntity(model)));
        //}



        //// <summary>
        /////add HazardKeyField locations
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        public HazardKeyFieldResponseViewModel AddHazardKeyFieldLocations(HazardKeyFieldViewModel model, string serializedLocations)
        {
            var entity = new HazardKeyFieldMapping
            {
                HazardKeyFieldMappingId = Guid.NewGuid(),
                HazardKeyId = model.HazardKeyId,
                FieldId = model.FieldId,
                Location = serializedLocations,
                CreatedDate = DateTime.Now
            };
            var savedEntity = _hazardKeyRepository.AddHazardKeyFieldLocations(entity);

            return Mapper.MapHazardKeyFieldMappingEntityToHazardKeyFieldResponseViewModel(savedEntity);
        }


        /// <summary>
        /// get hazardKey name list 
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public HazardKeyNameListViewModel GetHazardKeyNameListWithSearch(string SearchKey, string UserMasterAdminId)
        {
            HazardKeyNameListViewModel hazardKeyNameList = new();
            hazardKeyNameList.List = _hazardKeyRepository.GetHazardKeyNameListWithSearch(SearchKey,UserMasterAdminId).Select(a => Mapper.MapHazardKeyNameEntityToHazardKeyNameViewModel(a))?.ToList();
            return hazardKeyNameList;
        }

        /// <summary>
        /// get  hazardKeys by fieldId
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>s
        public IEnumerable<FieldHazardViewModel> GetFieldHazardKeys(Guid fieldId)
        {
            var hazards = _hazardKeyRepository.GetFieldHazardKeys(fieldId).Select(a => Mapper.MapHazardKeyFieldMappingEntityToFieldHazardViewModel(a)).ToList();
            return hazards;
        }


        /// <summary>
        /// remove hazardKey from field 
        /// </summary>
        /// <param name="hazardKeyFieldMappingId"></param>
        /// <returns></returns>
        public bool RemoveHazardKeyField(Guid hazardKeyFieldMappingId)
        {
            return _hazardKeyRepository.RemoveHazardKeyField(hazardKeyFieldMappingId);
        }
       

    /// <summary>
    /// remove hazardKey field location
    /// </summary>
    /// <param name="hazardKeyFieldMappingId"></param>
    /// <returns></returns>
    public bool RemoveHazardKeyFieldLocation(Guid hazardKeyFieldMappingId)
        {
            return _hazardKeyRepository.RemoveHazardKeyFieldLocation(hazardKeyFieldMappingId);
        }

    /// <summary>
    /// remove hazardKey field location
    /// </summary>
    /// <param name="hazardKeyFieldMappingId"></param>
    /// <returns></returns>
       public bool RemoveFieldLocation(Guid FieldId)
        {
            try
            {
                _hazardKeyRepository.RemoveFieldLocation(FieldId);
               // _fieldRepository.RemoveFieldBoundaries(FieldId);
                return true;
            }
            catch
            {
                // Return false if any exception occurs
                return false;
            }
        }

        //// <summary>
        /////add Hazard locations by fieldId
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        public HazardFieldResponseViewModel AddHazardKeyLocationsByField(Guid fieldId, Guid hazardKeyId, string serializedLocations)
        {
            var entity = new HazardKeyFieldMapping
            {
                HazardKeyFieldMappingId = Guid.NewGuid(),
                HazardKeyId = hazardKeyId,
                FieldId = fieldId,
                Location = serializedLocations,
                CreatedDate = DateTime.Now
            };

            var response=_hazardKeyRepository.AddHazardKeyLocationsByField(entity);
            return Mapper.MapHazardKeyFieldMappingEntityToHazardFieldResponseViewModel(response);

        }


    }
}
