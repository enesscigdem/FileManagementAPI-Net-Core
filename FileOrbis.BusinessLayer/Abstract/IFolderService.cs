using FileOrbis.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.BusinessLayer.Abstract
{
    public interface IFolderService : IGenericService<FolderInfo>
    {
        Task<List<FolderInfo>> GetFoldersByUserID(int userID);
        Task<List<FolderInfo>> GetFoldersByParentFolderID(int parentFolderID);
        Task RenameFolder(FolderInfo folder);
        Task DeleteFolder(int id);
        Task<byte[]> DownloadFolder(string folderName, string folderPath);
        Task<FolderInfo> CreateFolder(FolderInfo folder);

    }
}
