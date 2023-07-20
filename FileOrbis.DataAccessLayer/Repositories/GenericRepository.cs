using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using Microsoft.EntityFrameworkCore;
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

        public async Task<T> Create(T t)
        {
            await _context.Set<T>().AddAsync(t);
            await _context.SaveChangesAsync();
            return t;
        }

        public async Task Delete(int id)
        {
            var item = await GetListByID(id);
            _context.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<List<T>> GetListAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetListByID(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }
    }
}