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
        Task<ServiceResponse<bool>> UpdateBikeNote(BikeNote bikeNote);
        Task<ServiceResponse<bool>> DeleteBikeNote(string bikeNoteId);
    }
}
