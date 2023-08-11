using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Model;
using FileOrbis.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.BusinessLayer.Concrete
{
    public class UserManager : GenericManager<UserInfo>, IUserService
    {
        private readonly IUserDal _userDal;
        public UserManager(IUserDal userDal) : base(userDal)
        {
            _userDal = userDal;
        }

        public async Task CreateUser(UserInfo user)
        {
            await _userDal.CreateUser(user);
        }

        public async Task DeleteUser(int id)
        {
            await _userDal.DeleteUser(id);
        }

        public async Task ForgotPassword(ForgotPasswordModel model)
        {
            await _userDal.ForgotPassword(model);
        }

        public async Task<UserInfo> GetUserByResetToken(string resetToken)
        {
            return await _userDal.GetUserByResetToken(resetToken);
        }

        public async Task<UserInfo> GetUserByUsername(string username)
        {
            return await _userDal.GetUserByUsername(username);
        }

        public async Task<UserInfo> Login(string username, string password)
        {
            return await _userDal.Login(username, password);
        }

        public async Task ResetPassword(ForgotPasswordModel model, string newPassword)
        {
            await _userDal.ResetPassword(model, newPassword);
        }
    }
}
