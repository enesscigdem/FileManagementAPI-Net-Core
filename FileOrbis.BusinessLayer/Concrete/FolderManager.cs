using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.BusinessLayer.Concrete
{
    public class FolderManager : GenericManager<FolderInfo>, IFolderService
    {
        private readonly IFolderDal _folderDal;
        public FolderManager(IFolderDal folderDal) : base(folderDal)
        {
            _folderDal = folderDal;
        }
        public async Task<List<FolderInfo>> GetFoldersByUserID(int userID)
        {
            return await _folderDal.GetFoldersByUserID(userID);
        }
        public async Task RenameFolder(FolderInfo folder)
        {
            await _folderDal.RenameFolder(folder);
        }
        public async Task<byte[]> DownloadFolder(string folderName, string folderPath)
        {
            return await _folderDal.DownloadFolder(folderName, folderPath);
        }
        public async Task DeleteFolder(int id)
        {
            await _folderDal.DeleteFolder(id);
        }
        public async Task<List<FolderInfo>> GetFoldersByParentFolderID(int parentFolderID)
        {
            return await _folderDal.GetFoldersByParentFolderID(parentFolderID);
        }

        public async Task<FolderInfo> CreateFolder(FolderInfo folder)
        {
            return await _folderDal.CreateFolder(folder);
        }
    }
}
