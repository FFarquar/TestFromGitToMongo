//using BikeChainUsage.Client.Services.UploadDownloadService;
using TestFromGitToMongo.Models;
using System.Net.Http.Json;

namespace TestFromGitToMongo.Services.NoteService

{
    public class NoteService : INoteService
    {
        //private readonly HttpClient _http;
        private readonly BikeAPIClient _bikeAPIClient;

        private readonly IUploadDownloadService _UDSC;

        public NoteService(BikeAPIClient bikeAPIClient, IUploadDownloadService Udsc)
        {
            _bikeAPIClient = bikeAPIClient;
            _UDSC = Udsc;
        }

        public List<BikeNote> BikeNotes { get; set ; }
        public BikeNote BikeNote { get; set ; }
        public IUploadDownloadService Udsc { get; }

        public async Task<ServiceResponse<bool>> AddBikeNote(BikeNote bikeNote, List<FileUploadDTO> browserFiles)
        {

            var filesUpload = new ServiceResponse<List<UploadResult>>();
            if (browserFiles.Count > 0)
            {
                string[] folderStruct = new string[2];
                folderStruct[0] = "notes";
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
                bikeNote.UploadResult = filesUpload.Data;
            }
            

            ServiceResponse<bool> response = new ServiceResponse<bool>();

            var result = await _bikeAPIClient.Note_Add(bikeNote);

            if (result.Success == true)
            {
                BikeNote = bikeNote;
                return new ServiceResponse<bool> { Message = "added" };
            }
            else
            {
                BikeNote = new BikeNote();
                return new ServiceResponse<bool> { Success = false, Message = "Not added. Reason = " + result.Message };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteBikeNote(string bikeNoteId)
        {

            var result = await _bikeAPIClient.Note_Delete(bikeNoteId);

            if (result.Success == true)
                return new ServiceResponse<bool> { Message = "Deleted" };
            else
                return new ServiceResponse<bool> { Success = false, Message = "Not deleted. Reason = " + result.Message };

        }

        public async Task GetBikeNote(string bikeNoteId)
        {
         
            BikeNote = await _bikeAPIClient.Note_GetNote(bikeNoteId);

        }

        public async Task GetBikeNotes(int bikeId)
        {

            BikeNotes = await _bikeAPIClient.Note_GetListForBike(bikeId);


        }

        public async Task<ServiceResponse<bool>> UpdateBikeNote(BikeNote bikeNote, List<FileUploadDTO>? files)
        {
            if(files != null && files.Count > 0)
            {
                //have to add the files to the S3 bucket
                var filesUpload = new ServiceResponse<List<UploadResult>>();
                if (files.Count > 0)
                {


                    string[] folderStruct = new string[2];
                    folderStruct[0] = "notes";
                    folderStruct[1] = DateTime.Now.ToString("yyyy_MM_dd");
                    filesUpload = await _UDSC.UploadFiles(files, folderStruct);
                    if (!filesUpload.Success)
                    {
                        //the files weren't uploaded
                        var sr = new ServiceResponse<bool>
                        {
                            Message = "The files weren't uploaded correctly. Note not amended " + filesUpload.Message,
                            Success = false
                        };
                        return sr;
                    }

                    //Attach the results of the upload to the bikenote object, so it can be written to the notes DB

                    foreach (var item in filesUpload.Data)
                    {
                        bikeNote.UploadResult.Add(item);
                    }
                    //bikeNote.UploadResult = filesUpload.Data;
                }
            }
            var result = await _bikeAPIClient.Note_Update(bikeNote);

            if (result.Success == true)
                return new ServiceResponse<bool> { Message = "Updated" };
            else
            {
                return new ServiceResponse<bool> { Success = false, Message = "Not updated. Reason = " + result.Message };
            }
        }
    }
}
