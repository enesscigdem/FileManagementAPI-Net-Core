using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.BusinessLayer.Abstract
{
    public interface IGenericService <T>
    {
        List<T> GetListAll();
        void GetListByID(int id);
    }
}
