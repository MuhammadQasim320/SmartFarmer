using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Interface
{
    public interface IHazardKeyRepository
    {
        HazardKey AddHazardKey(HazardKey model);
        bool IsHazardKeyExist(Guid hazardKeyId);
        bool IsHazardKeyFieldMappingExist(Guid hazardKeyFieldMappingId);
        HazardKey UpdateHazardKeyDetails(HazardKey model);
        bool DeleteHazardKey(Guid hazardKeyId);

        IEnumerable<HazardKey> GetHazardKeyListBySearch(int pageNumber, int pageSize, string searchKey,int? hazardTypeId, string UserMasterAdminId);
        int GetHazardKeyCountBySearch(string searchKey, int? hazardTypeId, string UserMasterAdminId);
        HazardKey GetHazardKeyDetails(Guid hazardKeyId);

        HazardKeyFieldMapping AddHazardKeyFieldLocations(HazardKeyFieldMapping model);
       IEnumerable<HazardKey> GetHazardKeyNameListWithSearch(string searchKey,string UserMasterAdminId);
        IEnumerable<HazardKeyFieldMapping> GetFieldHazardKeys(Guid fieldId);
        bool RemoveHazardKeyField(Guid hazardKeyFieldMappingId);
       bool RemoveHazardKeyFieldLocation(Guid hazardKeyFieldMappingId);
       bool RemoveFieldLocation(Guid fieldId);
        HazardKeyFieldMapping AddHazardKeyLocationsByField(HazardKeyFieldMapping model);
    }
}
