using Microsoft.AspNetCore.Http;

namespace GymManagementSystem.BLL.Services.Interfaces
{
    public interface IAttachmentService
    {
        string? Upload(IFormFile file, string folderName);
        bool Delete(string fileName, string folderName);
    }
}

