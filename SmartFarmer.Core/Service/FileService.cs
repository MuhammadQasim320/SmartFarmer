using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using SmartFarmer.Core.Interface;
using SmartFarmer.Core.ViewModel;

namespace SmartFarmer.Core.Service
{
    public class FileService : IFileService
    {
        private readonly IConfiguration _configuration;
        public FileService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        /// <summary>
        ///  get the file url and return to string
        /// </summary>
        /// <param name="file"></param>
        /// <returns>image view model</returns>

        public FileViewModel UploadFile(IFormFile file)
        {
            var imagePath = file;
            // Saving Image on Server
            if (imagePath?.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    imagePath.CopyTo(stream);
                    var client = new RestClient(_configuration["FileUpload:APIURL"])
                    {
                        Timeout = -1
                    };
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Content-Type", _configuration["FileUpload:ContentType"]);
                    request.AddHeader("Authorization", _configuration["FileUpload:Authorization"]);
                    request.AddFile("Image", stream.ToArray(), imagePath.FileName);
                    request.AddParameter("ProjectName", _configuration["FileUpload:ProjectName"]);
                    IRestResponse response = client.Execute(request);
                    if (response.IsSuccessful)
                    {
                        var result = JsonConvert.DeserializeObject<FileResponse>(response.Content);
                        return new FileViewModel
                        {
                            Name = result.imageUniqueName,
                            FileLink = result.path
                        };
                    }
                }
            }
            return null;
        }
        /// <summary>
        ///  delete the file from server
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public string DeleteUploadFile(string fileName)
        {
            // delete Image on Server

            var client = new RestClient(_configuration["DeleteImage:APIURL"]);
            client.Timeout = -1;
            var request = new RestRequest(Method.DELETE);
            request.AddHeader("Content-Type", _configuration["DeleteImage:ContentType"]);
            request.AddHeader("Authorization", _configuration["DeleteImage:Authorization"]);
            DeleteMedia body = new DeleteMedia { imageUniqueName = fileName, projectName = _configuration["DeleteImage:ProjectName"] };
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(body);
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.IsSuccessful)
            {

                return response.Content;
            }
            return null;
        }
    }
}
