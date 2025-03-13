using Microsoft.AspNetCore.Http;

namespace SmartFarmer.Core.ViewModel
{
    public class FileViewModel
    {
        public string Name { get; set; }
        public string FileLink { get; set; }
    }
    public class FileResponse
    {
        public string path { get; set; }
        public string imageUniqueName { get; set; }
    }
    public class AddFileViewModel
    {
        public string FileName { get; set; }
        public IFormFile File { get; set; }
    }
    public class DeleteMedia
    {
        public string imageUniqueName { get; set; }
        public string projectName { get; set; }
    }


}
