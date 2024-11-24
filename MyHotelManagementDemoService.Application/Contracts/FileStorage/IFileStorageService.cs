using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Application.Contracts.FileStorage
{
    public interface IFileStorageService
    {
        Task<List<string>> UploadFilesAsync(List<IFormFile> files, string destinationFolder);
    }
}
