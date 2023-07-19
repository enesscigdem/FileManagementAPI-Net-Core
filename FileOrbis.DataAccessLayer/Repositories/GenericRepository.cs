using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.DataAccessLayer.Repositories
{
    public class GenericRepository<T> : IGenericDal<T> where T : class
    {
        private readonly FileOrbisContext _context;

        public GenericRepository(FileOrbisContext context)
        {
            _context = context;
        }

        public List<T> GetListAll()
        {
            using var c = new FileOrbisContext();
            return _context.Set<T>().ToList();
        }

        public T GetListByID(int id)
        {
            using var c = new FileOrbisContext();
            return _context.Set<T>().Find(id);
        }
    }
}