using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Repositories;
using FileOrbis.DataAccessLayer.UnitOfWork;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace FileOrbis.DataAccessLayer.Context
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FileOrbisContext _context;

        public UnitOfWork(FileOrbisContext context)
        {
            _context = context;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
