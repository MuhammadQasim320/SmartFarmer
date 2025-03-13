using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.Interface
{
    public interface IFieldService
    {
        FieldResponseViewModel AddField(string CreatedBy,string UserMasterAdminId, FieldRequestViewModel field);
        FieldCountRequestViewModel GetFieldListBySearchWithPagination(SearchFieldRequestViewModel model,string masterAdminId);
        bool IsFieldExist(Guid fieldId);
        bool IsFieldAssigned(Guid fieldId);
        FieldDetailViewModel GetFieldDetails(Guid fieldId);
        FieldResponseViewModel UpdateFieldDetails(FieldResponseViewModel model);
        bool DeleteField(Guid fieldId);
       FieldNameListViewModel GetFieldNameListWithSearch( string SearchKey, string UserMasterAdminId);
        FieldCenterViewModel AddFieldCenter(FieldCenterViewModel model, string serializedLocations);
        FieldBoundaryViewModel AddFieldBoundaries(FieldBoundaryViewModel model, string serializedLocations);
        bool RemoveFieldBoundaries(Guid fieldId);
    }
}
