using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RestSharp;

namespace FX.Services.Bunny
{
    public class BunyimagesServices : IBunyimagesServices
    {
        private const string BASE_HOSTNAME = "storage.bunnycdn.com";
        private const string STORAGE_ZONE_NAME = "goldenatthenetproject";  // Replace with your actual storage zone name
        private const string ACCESS_KEY = "08389c76-91cc-4c81-8d8353fb6f0f-9c88-463e";  // Replace with your actual access key
        private const string CONTENT_TYPE = "application/octet-stream";

        public BunyimagesServices()
        {
        }

        //  var userdata = await context.users.where(uint=> uint.email = "").firstordefault();

        //  https://FX.b-cdn.net/{foldername}/imagename


        public async Task<byte[]> DownloadImage(string FolderName, string fileName)
        {
            var client = new RestClient($"https://storage.bunnycdn.com/FXe/{FolderName}/{fileName}");
            var request = new RestRequest();
            request.AddHeader("accept", "*/*");
            request.AddHeader("AccessKey", ACCESS_KEY);
            var response = await client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                return response.RawBytes;
            }
            else
            {
                throw new HttpRequestException($"Failed to download image. Response: {response.Content}");
            }
        }


        public Task<string> GetImageUrl(string FolderName, string fileName)
        {
            string url = $@"https://fxcandle.b-cdn.net/{FolderName}/{fileName}";
            return Task.FromResult(url);
        }





        public async Task<(bool Success, string FilePath, string FileName, string ImageUrl, string ErrorMessage)> UploadFileAsync(IFormFile image, string FolderName)
        {
            if (image == null || image.Length == 0)
                return (false, null, null, null, "No file uploaded.");

            string fileNameToUpload = image.FileName.Replace(" ", "_");
            string url = $@"https://{BASE_HOSTNAME}/{STORAGE_ZONE_NAME}/{FolderName}/{fileNameToUpload}";
            string Imageurl = $@"https://fxcandle.b-cdn.net/{FolderName}/{fileNameToUpload}";

            using (HttpClient client = new HttpClient())
            {
                // Set the access key in the header
                client.DefaultRequestHeaders.Add("AccessKey", ACCESS_KEY);
                // Read the file into a byte array
                byte[] fileBytes;
                using (var memoryStream = new MemoryStream())
                {
                    await image.CopyToAsync(memoryStream);
                    fileBytes = memoryStream.ToArray();
                }

                using (var content = new ByteArrayContent(fileBytes))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue(CONTENT_TYPE);
                    // Make the PUT request to upload the file
                    HttpResponseMessage response = await client.PutAsync(url, content);

                    if (response.IsSuccessStatusCode)
                    {
                        return (true, url, fileNameToUpload, Imageurl, null);
                    }
                    else
                    {
                        string responseBody = await response.Content.ReadAsStringAsync();
                        return (false, null, null, null, $"Failed to upload file. Response: {responseBody}");
                    }
                }
            }
        }


        public string GenerateNewFilename(string originalFilename)
        {
            string timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
            return $"{Path.GetFileNameWithoutExtension(originalFilename)}_{timestamp}{Path.GetExtension(originalFilename)}";
        }




    }
}