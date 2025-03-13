using Microsoft.AspNetCore.Http.HttpResults;
using SmartFarmer.Core.ViewModel;
using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Core.Interface
{
    public interface IHazardKeyService
    {
        HazardKeyResponseViewModel AddHazardKey(string CreatedBy, HazardKeyRequestViewModel model);
        bool IsHazardKeyExist(Guid hazardKeyId);
        bool IsHazardKeyFieldMappingExist(Guid hazardKeyFieldMappingId);
        HazardKeyResponseViewModel UpdateHazardKeyDetails(HazardKeyResponseViewModel model);
        bool DeleteHazardKey(Guid hazardKeyId);
        HazardKeyCountRequestViewModel GetHazardKeyListBySearchWithPagination(string UserMasterAdminId, SearchHazardKeyRequestViewModel model);
        HazardKeyResponseViewModel GetHazardKeyDetails(Guid hazardKeyd);
        HazardKeyFieldResponseViewModel AddHazardKeyFieldLocations(HazardKeyFieldViewModel model,string serializedLocations);
        HazardKeyNameListViewModel GetHazardKeyNameListWithSearch(string SearchKey, string UserMasterAdminId);
        IEnumerable<FieldHazardViewModel> GetFieldHazardKeys(Guid fieldId);
        bool RemoveHazardKeyField(Guid hazardKeyFieldMappingId);
        bool RemoveHazardKeyFieldLocation(Guid hazardKeyFieldMappingId);
        bool RemoveFieldLocation(Guid fieldId);
        HazardFieldResponseViewModel AddHazardKeyLocationsByField(Guid fieldId, Guid hazardKeyId, string serializedLocations);


    }
}
