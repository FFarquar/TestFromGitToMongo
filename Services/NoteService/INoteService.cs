//using BikeChainUsage.Shared;
namespace TestFromGitToMongo.Services.NoteService
{
    public interface INoteService
    {
        List<BikeNote> BikeNotes { get; set; }
        BikeNote BikeNote { get; set; }
        Task GetBikeNotes(int bikeId);
        Task GetBikeNote(string bikeNoteId);

        Task<ServiceResponse<bool>> AddBikeNote(BikeNote bikeNote, List<FileUploadDTO> browserFiles);
        /// <summary>
        /// This method updates an bikenote
        /// </summary>
        /// <param name="bikeNote">The new updated BikeNote ojbect</param>
        /// <param name="files">These are any new files that may have been added to the note. It should only include new additions</param>
        /// <returns></returns>
        Task<ServiceResponse<bool>> UpdateBikeNote(BikeNote bikeNote, List<FileUploadDTO>? files);
        Task<ServiceResponse<bool>> DeleteBikeNote(string bikeNoteId);
    }
}
