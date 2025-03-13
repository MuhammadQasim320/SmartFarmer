using Microsoft.AspNetCore.Http;
using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Interface
{
    public interface IFileService
    {
        FileViewModel UploadFile(IFormFile file);
        string DeleteUploadFile(string fileName);
    }
}
