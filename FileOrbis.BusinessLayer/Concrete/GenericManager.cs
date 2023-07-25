using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.BusinessLayer.Concrete
{
    public class GenericManager<T> : IGenericService<T> where T : class
    {
        IGenericDal<T> _genericDal;

        public GenericManager(IGenericDal<T> genericDal)
        {
            _genericDal = genericDal;
        }
        public async Task<List<FolderInfo>> GetFoldersByUserID(int userID)
        {
            return await _genericDal.GetFoldersByUserID(userID);
        }
        public async Task<T> Create(T t)
        {
            return await _genericDal.Create(t);
        }

        public async Task Delete(int id)
        {
            await _genericDal.Delete(id);
        }

        public async Task<List<T>> GetListAll()
        {
            return await _genericDal.GetListAll();
        }

        public async Task<T> GetListByID(int id)
        {
            if (id > 0)
            {
                return await _genericDal.GetListByID(id);
            }
            throw new Exception("ID can not be less than 1");
        }

        public async Task<List<FileInfos>> GetFilesByFolderID(int folderID)
        {
            return await _genericDal.GetFilesByFolderID(folderID);
        }

        public async Task<T> Update(T t)
        {
            return await _genericDal.Update(t);
        }
    }
}
