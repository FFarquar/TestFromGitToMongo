using TestFromGitToMongo.Models;
using Microsoft.AspNetCore.Components.Forms;

namespace TestFromGitToMongo.Services.BikePartService
{
    public interface IBikePartService
    {
        List<BikePart> BikeParts { get; set; }
        BikePart BikePart { get; set; }
        Task GetBikeParts(int bikeId);
        Task GetBikePart(string bikePartId);

        //Task<ServiceResponse<int>> AddBikePart(BikePart trip);




        /// <summary>  
        /// A method for uploading files that a user may have selected at different times.
        /// </summary>  
        /// <param name="trip">The trip undertaken</param>  
        /// <param name="browserFiles">The list of FileUploadDTO to be uploaded</param>  
        /// <returns>ServiceResponse&lt;int&gt;</returns>  
        Task<ServiceResponse<bool>> AddBikePart(BikePart trip, List<FileUploadDTO> browserFiles);

        //Task<ServiceResponse<int>> UpdateBikePart(BikePart trip);
        Task<ServiceResponse<bool>> UpdateBikePart(BikePart trip, List<FileUploadDTO>? files);
        Task<ServiceResponse<bool>> DeleteBikePart(string bikePartId);

    }
}
