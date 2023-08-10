using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.DataAccessLayer.Model;
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
    public class UserRepository : GenericRepository<UserInfo>, IUserDal
    {
        private readonly FileOrbisContext _context;
        private readonly IFolderDal _folderDal;

        public UserRepository(IUnitOfWork unitOfWork, FileOrbisContext context) : base(unitOfWork, context)
        {
            _context = context;
        }

        public async Task<UserInfo> GetUserByUsername(string username)
        {
            return await _context.UserInfo.FirstOrDefaultAsync(u => u.UserName == username);
        }
        public async Task CreateUser(UserInfo user)
        {
            var createdUser = await Create(user);
            FolderInfo folderInfo = new FolderInfo
            {
                FolderName = createdUser.UserName,
                Path = Path.Combine("C:\\server\\", createdUser.UserName),
                CreationDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                UserID = createdUser.UserID,
            };
            _context.Add(folderInfo);
            Directory.CreateDirectory("C:\\server\\" + createdUser.UserName);
            await SaveChangesAsync();
        }


        public Task ForgotPassword(ForgotPasswordModel model)
        {
            throw new NotImplementedException();
        }



        public Task Login(LoginModel model)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(ForgotPasswordModel model)
        {
            throw new NotImplementedException();
        }
    }
}
