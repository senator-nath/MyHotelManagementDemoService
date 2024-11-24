using Microsoft.AspNetCore.Http;
using MyHotelManagementDemoService.Application.Contracts.FileStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Persistence.Implementation.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        public async Task<List<string>> UploadFilesAsync(List<IFormFile> files, string destinationFolder)
        {
            var urls = new List<string>();
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", destinationFolder);

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            foreach (var file in files)
            {
                var fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(rootPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Generate URL (assume base URL is handled by middleware serving static files)
                urls.Add($"/{destinationFolder}/{fileName}");
            }

            return urls;
        }
    }
}

