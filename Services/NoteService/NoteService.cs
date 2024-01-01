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

        public async Task<ServiceResponse<bool>> AddBikeNote(BikeNote bikeNote, List<FileUploadDTO> browserFiles)
        {

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

            //if (bikeNote != null)
            //{
            //    BikeNote = bikeNote;
            //    response.Message = "Note added";
            //}
            //else
            //{
            //    response.Success = false;
            //    response.Message = "Note not added";
            //}

            //return response;


        }

        public async Task<ServiceResponse<bool>> DeleteBikeNote(string bikeNoteId)
        {

            var result = await _bikeAPIClient.Note_Delete(bikeNoteId);

            if (result.Success == true)
                return new ServiceResponse<bool> { Message = "Deleted" };
            else
                return new ServiceResponse<bool> { Success = false, Message = "Not deleted. Reason = " + result.Message };

            //var result = await _http.DeleteAsync("/api/bikenote/" + bikeNoteId);

            //return await result.Content.ReadFromJsonAsync<ServiceResponse<bool>>();
        }

        public async Task GetBikeNote(string bikeNoteId)
        {
         
            BikeNote = await _bikeAPIClient.Note_GetNote(bikeNoteId);
            //var result = await _http.GetFromJsonAsync<ServiceResponse<Note>>("api/bikenote/note/" + bikeNoteId);
            //BikeNote = new Note();
            //if (result != null && result.Data != null)
            //    BikeNote = result.Data;
        }

        public async Task GetBikeNotes(int bikeId)
        {

            BikeNotes = await _bikeAPIClient.Note_GetListForBike(bikeId);


        }

        public async Task<ServiceResponse<bool>> UpdateBikeNote(BikeNote bikeNote)
        {

            var result = await _bikeAPIClient.Note_Update(bikeNote);

            if (result.Success == true)
                return new ServiceResponse<bool> { Message = "Updated" };
            else
            {
                return new ServiceResponse<bool> { Success = false, Message = "Not updated. Reason = " +result.Message };
            }
        }
    }
}
