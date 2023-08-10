using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.BusinessLayer.Concrete
{
    public class FileManager : GenericManager<FileInfos>, IFileService
    {
        private readonly IFileDal _fileDal;
        public FileManager(IFileDal fileDal) : base(fileDal)
        {
            _fileDal = fileDal;
        }

        public async Task DeleteFile(int id)
        {
            await _fileDal.DeleteFile(id);
        }

        public async Task<(byte[] fileBytes, string fileName)> DownloadFile(int id)
        {
            return await _fileDal.DownloadFile(id);
        }

        public async Task RenameFile(FileInfos file)
        {
            await _fileDal.RenameFile(file);
        }
        public async Task<List<FileInfos>> GetFilesByFolderID(int folderID)
        {
            return await _fileDal.GetFilesByFolderID(folderID);
        }

        public async Task<FileInfos> UploadFile(IFormFile file, int folderID)
        {
            return await _fileDal.UploadFile(file, folderID);
        }
    }
}
