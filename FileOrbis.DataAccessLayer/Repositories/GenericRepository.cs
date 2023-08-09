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
            await _unitOfWork.SaveChangesAsync();
            return t;
        }

        public async Task Delete(int id)
        {
            var item = await GetListByID(id);
            _context.Remove(item);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<FolderInfo>> GetFoldersByUserID(int userID)
        {
            return await _context.FolderInfo
                .Where(f => f.UserID == userID && f.ParentFolderID == null)
                .ToListAsync();
        }
        public async Task<List<FileInfos>> GetFilesByFolderID(int folderID)
        {
            return await _context.FileInfo
                .Where(f => f.FolderID == folderID)
                .ToListAsync();
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
            await _unitOfWork.SaveChangesAsync();
            return t;
        }

        public async Task DeleteAll()
        {
            var allFolders = await _context.Set<T>().ToListAsync();
            _context.Set<T>().RemoveRange(allFolders);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}