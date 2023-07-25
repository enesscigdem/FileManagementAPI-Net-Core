﻿using FileOrbis.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.DataAccessLayer.Abstract
{
    public interface IGenericDal<T>
    {
        Task<List<T>> GetListAll();
        Task<T> GetListByID(int id);
        Task<T> Update(T t);
        Task<T> Create(T t);
        Task Delete(int id);
        Task<List<FolderInfo>> GetFoldersByUserID(int userID);
        Task<List<FileInfos>> GetFilesByFolderID(int folderID);

    }
}