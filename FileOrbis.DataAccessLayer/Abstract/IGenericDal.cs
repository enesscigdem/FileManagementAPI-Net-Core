using FileOrbis.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.DataAccessLayer.Abstract
{
    public interface IGenericDal<T>
    {
        Task<List<T>> GetListAll(); Task Delete(int id);
        Task<T> GetListByID(int id);
        Task<T> Create(T t);
        Task<T> Update(T t);
        Task DeleteAll();
        Task<int> SaveChangesAsync();
    }
}
