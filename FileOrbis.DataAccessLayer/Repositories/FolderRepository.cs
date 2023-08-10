using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.DataAccessLayer.UnitOfWork;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.DataAccessLayer.Repositories
{
    public class FolderRepository : GenericRepository<FolderInfo>, IFolderDal
    {
        private readonly FileOrbisContext _context;

        public FolderRepository(IUnitOfWork unitOfWork, FileOrbisContext context) : base(unitOfWork, context)
        {
            _context = context;
        }
        public async Task<List<FolderInfo>> GetFoldersByUserID(int userID)
        {
            return await _context.FolderInfo
                .Where(f => f.UserID == userID && f.ParentFolderID == null)
                .ToListAsync();
        }
        public async Task DeleteFolder(int id)
        {
            var folder = await GetListByID(id);

            if (folder != null)
            {
                await DeleteFolderContents(id);

                await Delete(id);
                DeleteDirectory(folder.Path);
                await SaveChangesAsync();
            }
        }
        public async Task DeleteFolderContents(int folderID)
        {
            var subFolders = _context.FolderInfo.Where(x => x.ParentFolderID == folderID).ToList();
            foreach (var subFolder in subFolders)
            {
                await DeleteFolderContents(subFolder.FolderID);
                _context.FolderInfo.Remove(subFolder);
            }

            var files = _context.FileInfo.Where(x => x.FolderID == folderID).ToList();
            foreach (var file in files)
            {
                _context.FileInfo.Remove(file);
            }
        }
        public async Task RenameFolder(FolderInfo folder)
        {
            var existingFolder = await GetListByID(folder.FolderID);
            if (existingFolder != null)
            {
                existingFolder.FolderName = folder.FolderName;
                string newPath = Path.Combine(Path.GetDirectoryName(existingFolder.Path), existingFolder.FolderName);
                MoveFolder(existingFolder.Path, newPath);
                existingFolder.Path = newPath;
                await Update(existingFolder);
                await SaveChangesAsync();
            }
        }

        public async Task<List<FolderInfo>> GetFoldersByParentFolderID(int parentFolderID)
        {
            return await _context.FolderInfo
                .Where(f => f.ParentFolderID == parentFolderID && f.ParentFolderID != null)
                .ToListAsync();
        }

        public async Task<FolderInfo> CreateFolder(FolderInfo folder)
        {
            if (folder == null || string.IsNullOrWhiteSpace(folder.FolderName) || folder.UserID == 0)
                throw new ArgumentException("Invalid folder data.");

            string folderPath;
            try
            {
                var user = await _context.UserInfo.FirstOrDefaultAsync(x => x.UserID == folder.UserID);
                var parentFolder = await _context.FolderInfo.FirstOrDefaultAsync(x => x.FolderID == folder.ParentFolderID);

                if (folder.ParentFolderID == null)
                {
                    var folderInfo = await _context.FolderInfo.FirstOrDefaultAsync(x => x.UserID == folder.UserID);
                    folder.ParentFolderID = folderInfo.FolderID;
                    folderPath = Path.Combine(folderInfo.Path, folder.FolderName);
                    folder.Path = folderPath;
                }
                else
                {
                    folderPath = Path.Combine(parentFolder.Path, folder.FolderName);
                    folder.Path = folderPath;
                }

                var existingFolder = await _context.FolderInfo
                    .FirstOrDefaultAsync(x => x.UserID == folder.UserID && x.ParentFolderID == folder.ParentFolderID && x.FolderName == folder.FolderName);

                int count = 1;
                while (existingFolder != null)
                {
                    string newName = $"{folder.FolderName}{count}";
                    folder.FolderName = newName;
                    folderPath = Path.Combine(parentFolder.Path, newName);
                    folder.Path = folderPath;
                    existingFolder = await _context.FolderInfo
                        .FirstOrDefaultAsync(x => x.UserID == folder.UserID && x.ParentFolderID == folder.ParentFolderID && x.FolderName == newName);

                    count++;
                }

                folder.CreationDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
                Directory.CreateDirectory(folderPath);
                var createdFolder = await Create(folder);
                await SaveChangesAsync();
                return createdFolder;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating the folder: " + ex.Message);
            }
        }
        public async Task<byte[]> DownloadFolder(string folderName, string folderPath)
        {
            try
            {
                if (!Directory.Exists(folderPath))
                {
                    throw new Exception("Folder not found.");
                }

                string zipFileName = folderName + ".zip";
                string zipFilePath = Path.Combine(Path.GetTempPath(), zipFileName);

                if (System.IO.File.Exists(zipFilePath))
                {
                    System.IO.File.Delete(zipFilePath);
                }

                ZipFile.CreateFromDirectory(folderPath, zipFilePath);

                byte[] zipFileBytes = await System.IO.File.ReadAllBytesAsync(zipFilePath);

                System.IO.File.Delete(zipFilePath);

                return zipFileBytes;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while creating or downloading the zip folder: " + ex.Message);
            }
        }

        public static void DeleteDirectory(string targetDirectory)
        {
            string[] files = Directory.GetFiles(targetDirectory);
            string[] subDirectories = Directory.GetDirectories(targetDirectory);
            foreach (string file in files)
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
            }
            foreach (string subDirectory in subDirectories)
                DeleteDirectory(subDirectory);

            Directory.Delete(targetDirectory, false);
        }
        static void MoveFolder(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                System.IO.File.Move(file, destFile);
            }
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
                MoveFolder(subDir, destSubDir);
            }
            Directory.Delete(sourceDir, true);
        }

    }
}
