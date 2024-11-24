using Microsoft.AspNetCore.Http;
using MyHotelManagementDemoService.Application.Contracts.FileStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyHotelManagementDemoService.Persistence.Implementation.FileStorage
{
    public class FileStorageService : IFileStorageService
    {
        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>
    {
        ".jpeg", ".jpg", ".png", ".gif", ".pdf"
    };

        public async Task<List<string>> UploadFilesAsync(List<IFormFile> files, string destinationFolder)
        {
            if (files == null || !files.Any())
                throw new ArgumentException("No files provided for upload.");

            var urls = new List<string>();
            var rootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", destinationFolder);

            if (!Directory.Exists(rootPath))
            {
                Directory.CreateDirectory(rootPath);
            }

            foreach (var file in files)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!AllowedExtensions.Contains(fileExtension))
                {
                    throw new InvalidOperationException($"File type {fileExtension} is not allowed.");
                }

                var fileName = $"{Guid.NewGuid()}_{SanitizeFileName(file.FileName)}";
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

        private string SanitizeFileName(string fileName)
        {
            var sanitizedFileName = Regex.Replace(fileName, @"[^a-zA-Z0-9_\-\.]", "_");
            return Path.GetFileName(sanitizedFileName); // Ensure only file name is returned, without directory paths
        }
    }
}
