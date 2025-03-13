using SmartFarmer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFarmer.Domain.Interface
{
    public interface IFieldRepository
    {
        Field AddField(Field field,string UserMasterAdminId);
        IEnumerable<Field> GetFieldListBySearch(int pageNumber, int pageSize, string searchKey,string masterAdminId);
        int GetFieldCountBySearch(string searchKey,string masterAdminId);
        bool IsFieldExist(Guid fieldId);
        bool IsFieldAssigned(Guid fieldId);
        Field GetFieldDetails(Guid fieldId);
        Field UpdateFieldDetails(Guid fieldId, string name);
        bool DeleteField(Guid fieldId);
        IEnumerable<Field> GetFieldNameListWithSearch(string searchKey, string UserMasterAdminId);
        Field AddFieldCenter(Field model);
        Field AddFieldBoundaries(Field model);
        bool RemoveFieldBoundaries(Guid fieldId);
    }
}
