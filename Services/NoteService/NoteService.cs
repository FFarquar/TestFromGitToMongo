//using BikeChainUsage.Client.Services.UploadDownloadService;
using TestFromGitToMongo.Models;
using System.Net.Http.Json;

namespace TestFromGitToMongo.Services.NoteService

{
    public class NoteService : INoteService
    {
        //private readonly HttpClient _http;
        private readonly BikeAPIClient _bikeAPIClient;

        //private readonly IUploadDownloadServiceClient _UDSC;

        public NoteService(BikeAPIClient bikeAPIClient)
        {
            _bikeAPIClient = bikeAPIClient;
        }
        //public NoteService(HttpClient http, IUploadDownloadServiceClient Udsc)
        //{
        //    _http = http;
        //    _UDSC = Udsc;
        //}
        public List<BikeNote> BikeNotes { get; set ; }
        public BikeNote BikeNote { get; set ; }

        public async Task<ServiceResponse<int>> AddBikeNote(BikeNote bikeNote, List<FileUploadDTO> browserFiles)
        {

            ServiceResponse<int> response = new ServiceResponse<int>();

            bikeNote = await _bikeAPIClient.AddNote(bikeNote);
            if (bikeNote != null)
            {
                BikeNote = bikeNote;
                response.Message = "Note added";
            }
            else
            {
                response.Success = false;
                response.Message = "Note not added";
            }

            return response;



            //var filesUpload = new ServiceResponse<List<UploadResult>>();
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

                //var result = await _http.PostAsJsonAsync("/api/bikenote/addbikenote", bikeNote);

                //var srResponse = await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();

                //if (srResponse.Success)
                //{
                //    if (browserFiles.Count != 0)
                //    {

                //        UploadResultDTO uploadResultDTO = new UploadResultDTO();
                //        uploadResultDTO.relatedEntityId = srResponse.Data;

                //        uploadResultDTO.relatedEntityName = "BikeNote";

                //        uploadResultDTO.UploadResults = filesUpload.Data;

                //        var updateTable = await _UDSC.UpdateDBWIthFileAttachmentDetails(uploadResultDTO);
                //    }
                //}
                //else
                //{

                //}

                //return srResponse;
        }

        public async Task<ServiceResponse<bool>> DeleteBikeNote(string bikeNoteId)
        {

            var result = await _bikeAPIClient.DeleteNote(bikeNoteId);

            if (result == true)
                return new ServiceResponse<bool> { Message = "Deleted" };
            else
                return new ServiceResponse<bool> { Success = false, Message = "Not deleted" };

            //var result = await _http.DeleteAsync("/api/bikenote/" + bikeNoteId);

            //return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task GetBikeNote(string bikeNoteId)
        {
         
            BikeNote = await _bikeAPIClient.GetBikeNote(bikeNoteId);
            //var result = await _http.GetFromJsonAsync<ServiceResponse<Note>>("api/bikenote/note/" + bikeNoteId);
            //BikeNote = new Note();
            //if (result != null && result.Data != null)
            //    BikeNote = result.Data;
        }

        public async Task GetBikeNotes(int bikeId)
        {

            BikeNotes = await _bikeAPIClient.GetListOfNotesForBike(bikeId);


            //var result = await _http.GetFromJsonAsync<ServiceResponse<List<Note>>>("api/bikenote/notes/" + bikeId);

            //BikeNotes = new List<Note>();
            //if (result != null && result.Data != null)
            //    BikeNotes = result.Data;
        }

        public async Task<ServiceResponse<int>> UpdateBikeNote(BikeNote bikeNote)
        {

            var result = await _bikeAPIClient.UpdateNote(bikeNote);

            if (result != null)
                return new ServiceResponse<int> { Message = "Updated" };
            else
            {
                return new ServiceResponse<int> { Success = false, Message = "Not updated" };
            }
            //var result = await _http.PutAsJsonAsync("api/bikenote/updateBikeNote", bikeNote);

            //return await result.Content.ReadFromJsonAsync<ServiceResponse<int>>();
        }
    }
}
