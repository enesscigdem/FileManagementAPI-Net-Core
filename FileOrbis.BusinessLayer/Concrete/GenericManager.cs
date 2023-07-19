using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.DataAccessLayer.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.BusinessLayer.Concrete
{
    public class GenericManager<T> where T : class
    {
        IGenericDal<T> _genericDal;

        public GenericManager(IGenericDal<T> genericDal)
        {
            _genericDal = genericDal;
        }

        public List<T> GetListAll()
        {
            return _genericDal.GetListAll();
        }

        public T GetListByID(int id)
        {
            return _genericDal.GetListByID(id);
        }
    }
}
