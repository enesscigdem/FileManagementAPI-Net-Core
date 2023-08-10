using FileOrbis.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.BusinessLayer.Abstract
{
    public interface IFileService : IGenericService<FileInfos>
    {
        Task<List<FileInfos>> GetFilesByFolderID(int folderID);
        Task DeleteFile(int id);
        Task RenameFile(FileInfos file);
        Task<(byte[] fileBytes, string fileName)> DownloadFile(int id);
        Task<FileInfos> UploadFile(IFormFile file, int folderID);

    }
}
