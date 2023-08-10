using FileOrbis.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.DataAccessLayer.Abstract
{
    public interface IFolderDal : IGenericDal<FolderInfo>
    {
        Task DeleteFolder(int id);
        Task DeleteFolderContents(int folderID);
        Task<List<FolderInfo>> GetFoldersByParentFolderID(int parentFolderID);
        Task<FolderInfo> CreateFolder(FolderInfo folder);
        Task RenameFolder(FolderInfo folder);
        Task<List<FolderInfo>> GetFoldersByUserID(int userID);
        Task<byte[]> DownloadFolder(string folderName, string folderPath);
    }
}
