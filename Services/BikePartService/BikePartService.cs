using TestFromGitToMongo.Services.UploadDownloadService;
using TestFromGitToMongo.Models;
using Microsoft.AspNetCore.Components.Forms;
using System.Net.Http.Json;

namespace TestFromGitToMongo.Services.BikePartService
{
    public class BikePartService: IBikePartService
    {
        private readonly BikeAPIClient _bikeAPIClient;

        //private readonly HttpClient _http;
        private readonly IUploadDownloadService _UDSC;
        private readonly ILogger _logger;

        public BikePartService(BikeAPIClient bikeAPIClient, IUploadDownloadService Udsc, ILogger<BikePartService> logger)
        {
            _bikeAPIClient = bikeAPIClient;
            _UDSC = Udsc;
            _logger = logger;
        }
        public List<BikePart> BikeParts { get; set; }
        public BikePart BikePart { get; set; }


        //public async Task<ServiceResponse<int>> AddBikePart(BikePart bikePart)
        //{
        //    throw new NotImplementedException();

        //    //var result = await _http.PostAsJsonAsync("/api/bikepart/addbikepart", bikePart);

        //    //return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        //}
  

        public async Task<ServiceResponse<bool>> AddBikePart(BikePart bikePart, List<FileUploadDTO> browserFiles)
        {

            //TODO Up to here

            var filesUpload = new ServiceResponse<List<UploadResult>>();
            if (browserFiles.Count > 0)
            {
                string[] folderStruct = new string[2];
                folderStruct[0] = "bikeparts";
                folderStruct[1] = DateTime.Now.ToString("yyyy_MM_dd");
                filesUpload = await _UDSC.UploadFiles(browserFiles, folderStruct);
                if (!filesUpload.Success)
                {
                    //the files weren't uploaded
                    var sr = new ServiceResponse<bool>
                    {
                        Message = "The files weren't uploaded correctly. Note not added " + filesUpload.Message,
                        Success = false
                    };
                    return sr;
                }

                //Attach the results of the upload to the bikenote object, so it can be written to the notes DB
                bikePart.UploadResult = filesUpload.Data;
            }



            ServiceResponse<bool> response = new ServiceResponse<bool>();

            var result = await _bikeAPIClient.BikePart_Add(bikePart);

            if (result.Success == true)
            {
                BikePart = bikePart;
                return new ServiceResponse<bool> { Message = "added" };
            }
            else
            {
                BikePart = new BikePart();
                return new ServiceResponse<bool> { Success = false, Message = "Not added. Reason = " + result.Message };
            }
            
            ////this method will deal with any file attachments that may have been added.
            //_logger.LogWarning("In BikePartSeriviceClient...");
            //_logger.LogWarning("...AddBikePart");
            //var filesUpload = new ServiceResponse<List<UploadResult>>();
            //_logger.LogWarning("..count of files received for upload = " + browserFiles.Count);
            //if (browserFiles.Count > 0)
            //{

            //    filesUpload = await _UDSC.UploadFiles(browserFiles);

            //    if (!filesUpload.Success)
            //    {
            //        //the files weren't uploaded
            //        var sr = new ServiceResponse<int>
            //        {
            //            Message = "The files weren't uploaded correctly. Bike part not added " + filesUpload.Message,
            //            Success = false
            //        };

            //        return sr;
            //    }
            //}

            //var addBikePartResult = await AddBikePart(trip);

            //if (addBikePartResult.Success)
            //{
            //    if (browserFiles.Count != 0)
            //    {

            //        //Console.WriteLine("Returned value from addbilke result. This should be the ID of the record " + addBikePartResult.Data);
            //        UploadResultDTO uploadResultDTO = new UploadResultDTO();
            //        uploadResultDTO.relatedEntityId = addBikePartResult.Data;

            //        uploadResultDTO.relatedEntityName = "BikePart";

            //        uploadResultDTO.UploadResults = filesUpload.Data;

            //        //var resultOfFileDetails = await _http.PostAsJsonAsync("/api/Filesave/uploadedFileDetails", uploadResultDTO);

            //        var updateTable = await _UDSC.UpdateDBWIthFileAttachmentDetails(uploadResultDTO);
            //    }
            //}
            //else
            //{

            //}
            ////temp return value
            //return addBikePartResult;
        }

        public async Task<ServiceResponse<bool>> DeleteBikePart(string bikePartId)
        {

            var result = await _bikeAPIClient.BikePart_Delete(bikePartId);

            if (result.Success == true)
                return new ServiceResponse<bool> { Message = "Deleted" };
            else
                return new ServiceResponse<bool> { Success = false, Message = "Not deleted. Reason = " + result.Message };


        }

        public async Task GetBikePart(string bikePartId)
        {
            BikePart = await _bikeAPIClient.BikePart_GetBikePart(bikePartId);

        }

        public async Task GetBikeParts(int bikeId)
        {

            BikeParts = await _bikeAPIClient.BikePart_GetListForBike(bikeId);

        }

        public async Task<ServiceResponse<bool>> UpdateBikePart(BikePart bikePart, List<FileUploadDTO>? files)
        {
            if (files != null && files.Count > 0)
            {
                //have to add the files to the S3 bucket
                var filesUpload = new ServiceResponse<List<UploadResult>>();
                if (files.Count > 0)
                {

                    string[] folderStruct = new string[2];
                    folderStruct[0] = "bikeparts";
                    folderStruct[1] = DateTime.Now.ToString("yyyy_MM_dd");
                    filesUpload = await _UDSC.UploadFiles(files, folderStruct);
                    if (!filesUpload.Success)
                    {
                        //the files weren't uploaded
                        var sr = new ServiceResponse<bool>
                        {
                            Message = "The files weren't uploaded correctly. BikePart not amended " + filesUpload.Message,
                            Success = false
                        };
                        return sr;
                    }

                    //Attach the results of the upload to the bikenote object, so it can be written to the notes DB

                    foreach (var item in filesUpload.Data)
                    {
                        bikePart.UploadResult.Add(item);
                    }
                    //bikeNote.UploadResult = filesUpload.Data;
                }
            }
            var result = await _bikeAPIClient.BikePart_Update(bikePart);

            if (result.Success == true)
                return new ServiceResponse<bool> { Message = "Updated" };
            else
            {
                return new ServiceResponse<bool> { Success = false, Message = "Not updated. Reason = " + result.Message };
            }
        }
    }
}
