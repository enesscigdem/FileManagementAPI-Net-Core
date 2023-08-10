using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.DataAccessLayer.UnitOfWork;
using FileOrbis.EntityLayer.Concrete;
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
        private readonly IUnitOfWork _unitOfWork;

        public GenericRepository(IUnitOfWork unitOfWork, FileOrbisContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        public async Task<T> Create(T t)
        {
            await _context.Set<T>().AddAsync(t);
            return t;
        }

        public async Task Delete(int id)
        {
            var item = await GetListByID(id);
            _context.Remove(item);
        }

        public async Task<List<T>> GetListAll()
        {
            return await _context.Set<T>().ToListAsync();
        }

        public async Task<T> GetListByID(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<T> Update(T t)
        {
            _context.Set<T>().Update(t);
            return t;
        }

        public async Task DeleteAll()
        {
            var allItems = await _context.Set<T>().ToListAsync();
            _context.Set<T>().RemoveRange(allItems);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _unitOfWork.SaveChangesAsync();
        }
    }
}
