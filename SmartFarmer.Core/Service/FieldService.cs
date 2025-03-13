using SmartFarmer.Core.Common;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Interface;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.Service
{
    public class FieldService : IFieldService
    {
        private readonly IFieldRepository _fieldRepository;
        public FieldService(IFieldRepository fieldRepository)
        {
            _fieldRepository = fieldRepository;
        }

        /// <summary>
        /// add field into system
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public FieldResponseViewModel AddField(string CreatedBy,string UserMasterAdminId, FieldRequestViewModel field)
        {

            return Mapper.MapFieldEntityToFieldResponseViewModel(_fieldRepository.AddField(Mapper.MapFieldRequestViewModelToFieldEntity(CreatedBy, field), UserMasterAdminId));
        }

        /// <summary>
        /// get field by search with pagination
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FieldCountRequestViewModel GetFieldListBySearchWithPagination(SearchFieldRequestViewModel model,string masterAdminId)
        {
            FieldCountRequestViewModel fieldList = new FieldCountRequestViewModel();
            fieldList.List = _fieldRepository.GetFieldListBySearch(model.PageNumber, model.PageSize, model.SearchKey, masterAdminId).Select(a => Mapper.MapFieldEntityToFieldDetailViewModel(a)).ToList();
            fieldList.TotalCount = _fieldRepository.GetFieldCountBySearch(model.SearchKey, masterAdminId);
            return fieldList;
        }

        /// <summary>
        /// check field existence
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public bool IsFieldExist(Guid fieldId)
        {
            return _fieldRepository.IsFieldExist(fieldId);
        }
          /// <summary>
        /// check field existence
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public bool IsFieldAssigned(Guid fieldId)
        {
            return _fieldRepository.IsFieldAssigned(fieldId);
        }

        /// <summary>
        /// get field details
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public FieldDetailViewModel GetFieldDetails(Guid fieldId)
        {
            return Mapper.MapFieldEntityToFieldDetailViewModel(_fieldRepository.GetFieldDetails(fieldId));
        }

        /// <summary>
        ///update field details
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public FieldResponseViewModel UpdateFieldDetails(FieldResponseViewModel model)
        {
            return Mapper.MapFieldEntityToFieldResponseViewModel(_fieldRepository.UpdateFieldDetails(model.FieldId, model.Name));
        }

        /// <summary>
        ///delete field 
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public bool DeleteField(Guid fieldId)
        {
            return _fieldRepository.DeleteField(fieldId);
        }


        /// <summary>
        /// get field name list 
        /// </summary>
        /// <param name="createdBy"></param>
        /// <returns></returns>
        public FieldNameListViewModel GetFieldNameListWithSearch(string SearchKey, string UserMasterAdminId)
        {
            FieldNameListViewModel fieldNameListView = new();
            fieldNameListView.List = _fieldRepository.GetFieldNameListWithSearch(SearchKey, UserMasterAdminId).Select(a => Mapper.MapFieldNameEntityToFieldNameViewModel(a))?.ToList();
            return fieldNameListView;
        }




        //// <summary>
        /////add field center
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        public FieldCenterViewModel AddFieldCenter(FieldCenterViewModel model, string serializedLocations)
        {
            var entity = new Field
            {
                FieldId = model.FieldId,
                Center = serializedLocations,
            };
            var savedEntity = _fieldRepository.AddFieldCenter(entity);

            return Mapper.MapFieldEntityToFieldCenterViewModell(savedEntity);
        }


        //// <summary>
        /////add field boundaries
        ///// </summary>
        ///// <param name="model"></param>
        ///// <returns></returns>
        public FieldBoundaryViewModel AddFieldBoundaries(FieldBoundaryViewModel model, string serializedLocations)
        {
            var entity = new Field
            {
                FieldId = model.FieldId,
                Boundary = serializedLocations,
            };
            var savedEntity = _fieldRepository.AddFieldBoundaries(entity);

            return Mapper.MapFieldEntityToFieldBoundaryViewModel(savedEntity);
        }


        /// <summary>
        ///remove field boundaries
        /// </summary>
        /// <param name="fieldId"></param>
        /// <returns></returns>
        public bool RemoveFieldBoundaries(Guid fieldId)
        {
            return _fieldRepository.RemoveFieldBoundaries(fieldId);
        }
    }
}
