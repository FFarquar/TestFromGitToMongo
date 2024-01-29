using TestFromGitToMongo.Shared;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Web;


namespace TestFromGitToMongo.Services.UploadDownloadService
{
    public class UploadDownloadService : IUploadDownloadService
    {
  //      private readonly HttpClient _http;
        private readonly BikeAPIClient _bikeAPIClient;
        private readonly ILogger _logger;
        private readonly IConfiguration _config;

        public UploadDownloadService(BikeAPIClient bikeAPIClient, ILogger<UploadDownloadService> logger, IConfiguration config)
        {
//            _http = http;
            _bikeAPIClient = bikeAPIClient;
            _logger = logger;
            _config = config;
        }

        public async Task<ServiceResponse<List<bool>>> DeleteFilesFromFileSystem(List<FileDetail> filesToDelete)

        {

            //var response = await _http.DeleteAsync("api/Filesave/deleteFilesFromServer");
            //Had to use a Put to do a batch delete. See controller for more details
            var response = await _bikeAPIClient.Attachment_Delete(filesToDelete);
            return response;


        }

        public async Task<ServiceResponse<bool>> DeleteUploadRecordsFromDB(List<FileDetail> filesToDelete, object relatedObject)
        {

            switch (relatedObject.GetType().Name.ToLower())
            {
                case "bikenote":
                    //cast the generic object to a BikeNote
                    BikeNote bikenotetoUpdate = (BikeNote)relatedObject;

                    foreach (var file in filesToDelete)
                    {

                        for (int i = bikenotetoUpdate.UploadResult.Count - 1; i >= 0; i--)
                        {
                            if (bikenotetoUpdate.UploadResult[i].ServerPath == file.ServerPath && bikenotetoUpdate.UploadResult[i].FileName == file.OriginalFileName)
                            {
                                //this file has to be removed
                                bikenotetoUpdate.UploadResult.RemoveAt(i);
                                //break;
                            }

                        }
                    }

                    ServiceResponse<BikeNote> responseNote = new ServiceResponse<BikeNote>();
                    responseNote = await _bikeAPIClient.Note_Update(bikenotetoUpdate);

                    //put together the response to the caller
                    if(responseNote.Success)
                    {
                        //Update worked
                        return new ServiceResponse<bool>();
                    }
                    else
                    {
                        //Update failed
                        return new ServiceResponse<bool>()
                        {
                            Success = false,
                            Message = responseNote.Message
                        };
                    }

                    break;

                default:
                    //Have to implement all other object types
                    throw new NotImplementedException();

            }
          


                    
            //TODO: Have to delete the file attachment from the note entry in Mongo. Need to know what the parent object is (could be note or bike or trip etc). Some refactoring
            //required here. Perhaps the correct place for this action is in the service dealing with the object (such as the note)
            //var response = await _http.PutAsJsonAsync("api/Filesave/deleteFilesFromFileDetailsTable", filesToDelete);

            //ServiceResponse<bool> newFileDeleteResults = new ServiceResponse<bool>();
            //if (response != null & response.IsSuccessStatusCode)
            //{
            //    newFileDeleteResults = await response.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
            //}

            //if (newFileDeleteResults != null)
            //{
            //    return newFileDeleteResults;
            //}
            //else
            //{
            //    //the response wasnt valid, need to pass back a failed response
            //    return new ServiceResponse<bool>
            //    {
            //        Data = false,
            //        Success = false,
            //        Message = "The response from the server was not valid. Not known if files removed"
            //    };
            //}

        }

        public async Task<ServiceResponse<DotNetStreamReference>> GeFile(string storedPath, string fileName)
        {

            var response = await _bikeAPIClient.Attachment_GeFile(storedPath, fileName);

            return response;
            //string newstring = storedPath.Replace(@"\", "/");

            //newstring = HttpUtility.UrlEncode(newstring);
            //var pathToFile = Path.Combine("api/Filesave/GetFile/", newstring, fileName);

            //var imageStreamRes = await _http.GetAsync(pathToFile);


            //if (imageStreamRes is not null)
            //{
            //    return new ServiceResponse<DotNetStreamReference>
            //    {
            //        Data = new DotNetStreamReference(imageStreamRes.Content.ReadAsStream())
            //    };
            //}
            //else
            //{
            //    return new ServiceResponse<DotNetStreamReference>
            //    {
            //        Success = false,
            //        Message = "Some issue getting file"
            //    };
            //}

        }

        public async Task<ServiceResponse<List<FileDetail>>> ReturnListOfFileAttachments(int relatedRecordId, string classType)
        {
            throw new NotImplementedException();
            //var response = await _http.GetAsync("api/Filesave/getlistOfFileAttachments/" + relatedRecordId + "/" + classType);

            //ServiceResponse<List<FileDetail>> fileAttachments = new ServiceResponse<List<FileDetail>>();
            //if (response != null & response.IsSuccessStatusCode)
            //{
            //    fileAttachments = await response.Content.ReadFromJsonAsync<ServiceResponse<List<FileDetail>>>();
            //}

            //if (fileAttachments != null)
            //{
            //    return fileAttachments;
            //}
            //else
            //{
            //    //the response wasnt valid, need to pass back a failed response
            //    return new ServiceResponse<List<FileDetail>>
            //    {
            //        Data = new List<FileDetail>(),
            //        Success = false,
            //        Message = "The response from the server was not valid. Not known if file attachments exist"
            //    };
            //}
        }

        public async Task<ServiceResponse<List<UploadResult>>> UploadFiles(InputFileChangeEventArgs e)
        {
            //This method not used in source application, not this one. It can be removed
            throw new NotImplementedException();
        //    //This task uses a list of broswerFiles. It is not tirggered as soon as the dialog box is closed, but later in the process
        //    List<File> files = new List<File>();
        //    //long maxFileSize = 100024 * 15;
        //    //long maxFileSize = 1000024 * 15;
        //    //long maxFileSize = Settings.maxFileSize;
        //    long maxFileSize = (long)Convert.ToDouble(_config["Max_File_Size"]);
        //    int maxNumFIlesToUpload = (int)Convert.ToInt16(_config["Max_Number_Of_Files_Upload"]);

        //    bool upload = false;
        //    List<UploadResult> uploadResults = new();

        //    _logger.LogInformation("Uploading files from UploadDownloadServiceClient");
        //    using var content = new MultipartFormDataContent();
        //    //foreach (var file in e.GetMultipleFiles(Settings.maxFilesToUpload))
        //    foreach (var file in e.GetMultipleFiles(maxNumFIlesToUpload))
        //        {
        //        if (uploadResults.SingleOrDefault(f => f.FileName == file.Name) is null)
        //        {
        //            try
        //            {
        //                _logger.LogInformation("...file.name = " + file.Name);
        //                files.Add(new() { Name = file.Name });
        //                //the path to the file is not exposed by IBrowserFile interface, its a security risk.
        //                //it must be stored as an inaccessable property

        //                var fileContent =
        //                    new StreamContent(file.OpenReadStream(maxFileSize));
        //                //The OpenReadStream must know the path to the source file.

        //                _logger.LogInformation("..fileContent is null = " + (fileContent is null));
        //                fileContent.Headers.ContentType =
        //                    new MediaTypeHeaderValue(file.ContentType);

        //                content.Add(
        //                    content: fileContent,
        //                    name: "\"files\"",
        //                    fileName: file.Name);
        //                _logger.LogInformation("...Adding content ");
        //                _logger.LogInformation("...Content = " + JsonConvert.SerializeObject(content));
        //                upload = true;
        //            }
        //            catch (Exception ex)
        //            {
        //                _logger.LogInformation("{FileName} not uploaded (Err: 6): {Message}", file.Name, ex.Message);

        //                uploadResults.Add(
        //                    new()
        //                    {
        //                        FileName = file.Name,
        //                        ErrorCode = 6,
        //                        Uploaded = false
        //                    });
        //            }
        //        }
        //    }

        //    _logger.LogInformation("upload variable = " + upload);

        //https://stackoverflow.com/questions/64809676/there-is-no-file-with-id-1-the-file-list-may-have-changed-blazor                
        //    if (upload)
        //    {
        //        _logger.LogInformation("...about to call FileSave controller. Content = " + JsonConvert.SerializeObject(content));
        //        HttpResponseMessage response = new HttpResponseMessage();
        //        bool proceedAfterPost = false;
        //        try
        //        {
        //            response = await _http.PostAsync("api/Filesave", content);
        //            proceedAfterPost = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogInformation("...exception thrown = " + ex.Message);
        //        }

        //        //var response = await _http.PostAsync("api/Filesave", content);
        //        if (response.IsSuccessStatusCode == false | proceedAfterPost == false)
        //        {
        //            _logger.LogInformation("{FileName} could not reach FileSave endpoint (Err: 7)");
        //        }
        //        else
        //        {
        //            _logger.LogInformation("Got a response from FileSaveController");
        //            var newUploadResults = await response.Content.ReadFromJsonAsync<ServiceResponse<List<UploadResult>>>();

        //            if (newUploadResults is not null)
        //            {
        //                return newUploadResults;
        //            }
        //            else
        //            {
        //                return new ServiceResponse<List<UploadResult>>
        //                {
        //                    Success = false,
        //                    Message = "Null response from service on server"
        //                };
        //            }
        //        }
        //    }

        //    return new ServiceResponse<List<UploadResult>>
        //    {
        //        Success = false,
        //        Message = "Some issue creating file content to pass to server"
        //    };
        }

        //public async Task<ServiceResponse<List<UploadResult>>> UploadFiles(List<FileUploadDTO> e)
        //{
        //    //this method should be deprecated in favour of the method that can accept an array of folder values (below)
        //    //This task uses a list of fileUploadDTO's. It is not tirggered as soon as the dialog box is closed, but later in the process
        //    List<File> files = new List<File>();
        //    //long maxFileSize = Settings.maxFileSize;
        //    long maxFileSize = (long)Convert.ToDouble(_config["Max_File_Size"]);

        //    bool upload = false;
        //    List<UploadResult> uploadResults = new();

        //    _logger.LogInformation("Uploading files from UploadDownloadServiceClient");
        //    using var content = new MultipartFormDataContent();
        //    StringContent directoryupper = new StringContent("1");
        //    StringContent directorylower = new StringContent("4");
        //    content.Add(content: directoryupper, name: "directoryupper");
        //    content.Add(content: directorylower, name: "directorylower");


        //    foreach (var file in e)
        //    {
        //        try
        //        {
        //            files.Add(new() { Name = file.FileName });
        //            var fileData = file.FileContent;
        //            ByteArrayContent byteContent = new ByteArrayContent(fileData);
        //            byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

        //            content.Add(content: byteContent, name: "\"file\"", fileName: file.FileName);
        //            upload = true;

        //            _logger.LogInformation("\"file\"");

        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogInformation("{FileName} not uploaded (Err: 6): {Message}", file.FileName, ex.Message);

        //            uploadResults.Add(
        //                new()
        //                {
        //                    FileName = file.FileName,
        //                    ErrorCode = 6,
        //                    Uploaded = false
        //                });
        //        }

        //    }

        //    _logger.LogInformation("upload variable = " + upload);


        //    if (upload)
        //    {
        //        _logger.LogInformation("...about to call Attachment_Add  " + JsonConvert.SerializeObject(content));
        //        var response = await _bikeAPIClient.Attachment_Add(content);
        //        bool proceedAfterPost = false;
        //        if (response.Success)
        //        {
        //            proceedAfterPost = true;

        //            _logger.LogInformation("Got a response from Attachment_Add");


        //            if (response.Data is not null)
        //            {

        //                return response;
        //            }
        //            else
        //            {
        //                return new ServiceResponse<List<UploadResult>>
        //                {
        //                    Success = false,
        //                    Message = "Null response from service on server"
        //                };
        //            }

        //        }
        //        else
        //        {
        //            _logger.LogInformation("{FileName} could not reach FileSave endpoint (Err: 7)");
        //        }
        //    }

        //    //    HttpResponseMessage response = new HttpResponseMessage();
        //    //    bool proceedAfterPost = false;
        //    //    try
        //    //    {
        //    //        //response = await _http.PostAsync("api/Filesave", content);
        //    //        response = await _bikeAPIClient.Attachment_Add(content);

        //    //        proceedAfterPost = true;
        //    //    }
        //    //    catch (Exception ex)
        //    //    {
        //    //        _logger.LogInformation("...exception thrown = " + ex.Message);
        //    //    }

        //    //    //var response = await _http.PostAsync("api/Filesave", content);
        //    //    if (response.IsSuccessStatusCode == false | proceedAfterPost == false)
        //    //    {
        //    //        _logger.LogInformation("{FileName} could not reach FileSave endpoint (Err: 7)");
        //    //    }
        //    //    else
        //    //    {
        //    //        _logger.LogInformation("Got a response from FileSaveController");
        //    //        var newUploadResults = await response.Content.ReadFromJsonAsync<ServiceResponse<List<UploadResult>>>();

        //    //        if (newUploadResults is not null)
        //    //        {
        //    //            return newUploadResults;
        //    //        }
        //    //        else
        //    //        {
        //    //            return new ServiceResponse<List<UploadResult>>
        //    //            {
        //    //                Success = false,
        //    //                Message = "Null response from service on server"
        //    //            };
        //    //        }
        //    //    }
        //    //}

        //    return new ServiceResponse<List<UploadResult>>
        //    {
        //        Success = false,
        //        Message = "Some issue creating file content to pass to server"
        //    };
        //}

        public async Task<ServiceResponse<List<UploadResult>>> UploadFiles(List<FileUploadDTO> e, string[] folderStructure)
        {
            //This task uses a list of fileUploadDTO's. It is not tirggered as soon as the dialog box is closed, but later in the process
            List<File> files = new List<File>();
            //long maxFileSize = Settings.maxFileSize;
            long maxFileSize = (long)Convert.ToDouble(_config["Max_File_Size"]);

            bool upload = false;
            List<UploadResult> uploadResults = new();

            _logger.LogInformation("Uploading files from UploadDownloadServiceClient");

            var folderNames = string.Join(",", folderStructure);  //The folder structure, seperated by comma's to be passed to the API

            using var content = new MultipartFormDataContent();
            //StringContent directoryupper = new StringContent("1");
            //StringContent directorylower = new StringContent("4");
            //content.Add(content: directoryupper, name: "directoryupper");
            //content.Add(content: directorylower, name: "directorylower");
            StringContent folders = new StringContent(folderNames);
            content.Add(content: folders, name: "folders");
            foreach (var file in e)
            {
                try
                {
                    files.Add(new() { Name = file.FileName });
                    var fileData = file.FileContent;
                    ByteArrayContent byteContent = new ByteArrayContent(fileData);
                    byteContent.Headers.ContentType = MediaTypeHeaderValue.Parse(file.ContentType);

                    content.Add(content: byteContent, name: "\"file\"", fileName: file.FileName);
                    upload = true;

                    _logger.LogInformation("\"file\"");

                }
                catch (Exception ex)
                {
                    _logger.LogInformation("{FileName} not uploaded (Err: 6): {Message}", file.FileName, ex.Message);

                    uploadResults.Add(
                        new()
                        {
                            FileName = file.FileName,
                            ErrorCode = 6,
                            Uploaded = false
                        });
                }

            }

            _logger.LogInformation("upload variable = " + upload);


            if (upload)
            {
                _logger.LogInformation("...about to call Attachment_Add  " + JsonConvert.SerializeObject(content));
                var response = await _bikeAPIClient.Attachment_Add(content);
                bool proceedAfterPost = false;
                if (response.Success)
                {
                    proceedAfterPost = true;

                    _logger.LogInformation("Got a response from Attachment_Add");


                    if (response.Data is not null)
                    {

                        return response;
                    }
                    else
                    {
                        return new ServiceResponse<List<UploadResult>>
                        {
                            Success = false,
                            Message = "Null response from service on server"
                        };
                    }

                }
                else
                {
                    _logger.LogInformation("{FileName} could not reach FileSave endpoint (Err: 7)");
                }
            }

            //    HttpResponseMessage response = new HttpResponseMessage();
            //    bool proceedAfterPost = false;
            //    try
            //    {
            //        //response = await _http.PostAsync("api/Filesave", content);
            //        response = await _bikeAPIClient.Attachment_Add(content);

            //        proceedAfterPost = true;
            //    }
            //    catch (Exception ex)
            //    {
            //        _logger.LogInformation("...exception thrown = " + ex.Message);
            //    }

            //    //var response = await _http.PostAsync("api/Filesave", content);
            //    if (response.IsSuccessStatusCode == false | proceedAfterPost == false)
            //    {
            //        _logger.LogInformation("{FileName} could not reach FileSave endpoint (Err: 7)");
            //    }
            //    else
            //    {
            //        _logger.LogInformation("Got a response from FileSaveController");
            //        var newUploadResults = await response.Content.ReadFromJsonAsync<ServiceResponse<List<UploadResult>>>();

            //        if (newUploadResults is not null)
            //        {
            //            return newUploadResults;
            //        }
            //        else
            //        {
            //            return new ServiceResponse<List<UploadResult>>
            //            {
            //                Success = false,
            //                Message = "Null response from service on server"
            //            };
            //        }
            //    }
            //}

            return new ServiceResponse<List<UploadResult>>
            {
                Success = false,
                Message = "Some issue creating file content to pass to server"
            };
        }

        //private async Task<byte[]> GetImageBytes(IBrowserFile file)
        //{
        //    var path = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
        //    await using var fileStream = new FileStream(path, FileMode.Create);
        //    await file.OpenReadStream(file.Size).CopyToAsync(fileStream);
        //    var bytes = new byte[file.Size];
        //    fileStream.Position = 0;
        //    await fileStream.ReadAsync(bytes);
        //    fileStream.Close();
        //    return bytes;
        //}
    }

    public class File
    {
        public string? Name { get; set; }
    }
}



