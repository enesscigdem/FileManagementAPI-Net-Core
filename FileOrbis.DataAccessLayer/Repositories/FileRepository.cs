using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.DataAccessLayer.UnitOfWork;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.DataAccessLayer.Repositories
{
    public class FileRepository : GenericRepository<FileInfos>, IFileDal
    {
        private readonly FileOrbisContext _context;

        public FileRepository(IUnitOfWork unitOfWork, FileOrbisContext context) : base(unitOfWork, context)
        {
            _context = context;
        }

        public async Task DeleteFile(int id)
        {
            var file = await GetListByID(id);
            if (file != null)
            {
                System.IO.File.Delete(file.Path);
                await Delete(id);
                await SaveChangesAsync();
            }
        }
        public async Task<FileInfos> UploadFile(IFormFile file, int folderID)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("No file received or file is empty.");
            }

            try
            {
                string filePath;
                var existingFolder = _context.FolderInfo.FirstOrDefault(x => x.FolderID == folderID);
                filePath = Path.Combine(existingFolder.Path, file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                long fileSizeBytes = file.Length;
                double fileSizeMB = (double)fileSizeBytes / (1024 * 1024);
                string fileSizeFormatted = fileSizeMB.ToString("0.00");

                FileInfos fileInfo = new FileInfos
                {
                    FileName = file.FileName,
                    Path = filePath,
                    Size = double.Parse(fileSizeFormatted),
                    CreationDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                    FolderID = folderID
                };

                await Create(fileInfo);
                await SaveChangesAsync();

                return fileInfo;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while uploading the file: " + ex.Message);
            }
        }

        public async Task<(byte[] fileBytes, string fileName)> DownloadFile(int id)
        {
            var file = await GetListByID(id);
            if (file == null)
                return (null, null);

            if (!System.IO.File.Exists(file.Path))
                return (null, null);

            var fileBytes = await System.IO.File.ReadAllBytesAsync(file.Path);

            return (fileBytes, file.FileName);
        }

        public async Task<List<FileInfos>> GetFilesByFolderID(int folderID)
        {
            return await _context.FileInfo
                .Where(f => f.FolderID == folderID)
                .ToListAsync();
        }
        public async Task RenameFile(FileInfos file)
        {
            var existingFolder = await GetListByID(file.FileID);
            if (existingFolder != null)
            {
                existingFolder.FileName = file.FileName;
                string newPath = Path.Combine(Path.GetDirectoryName(existingFolder.Path), existingFolder.FileName);
                System.IO.File.Move(existingFolder.Path, newPath);
                existingFolder.Path = newPath;
                await Update(existingFolder);
                await SaveChangesAsync();
            }
        }
    }
}
