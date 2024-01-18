
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

namespace TestFromGitToMongo.Services.UploadDownloadService
{
    public interface IUploadDownloadService
    {
        /// <summary>  
        /// A method for uploading files as soon as a user selects them from a selection box
        /// </summary>  
        /// <param name="trip">The trip undertaken</param>  
        /// <param name="browserFiles">The list of IBrowserFile to be uploaded</param>  
        /// <returns>ServiceResponse&lt;List&lt;UploadResult&gt;&gt; </returns>  
        Task<ServiceResponse<List<UploadResult>>> UploadFiles(InputFileChangeEventArgs e);

        //Task<ServiceResponse<List<UploadResult>>> UploadFiles(List<IBrowserFile> e);
        Task<ServiceResponse<List<UploadResult>>> UploadFiles(List<FileUploadDTO> e);
       // Task<ServiceResponse<bool>> UpdateDBWIthFileAttachmentDetails(UploadResultDTO uploadResultDTO);       //Not needed in this application as attachments details are stored with parent object


        Task<ServiceResponse<DotNetStreamReference>> GeFile(string storedPath, string fileName);

        Task<ServiceResponse<List<bool>>> DeleteFilesFromFileSystem(List<FileDetail> filesToDelete);
        Task<ServiceResponse<bool>> DeleteUploadRecordsFromDB(List<FileDetail> filesToDelete);
        Task<ServiceResponse<List<FileDetail>>> ReturnListOfFileAttachments(int relatedRecordId, string classType);
    }

}
