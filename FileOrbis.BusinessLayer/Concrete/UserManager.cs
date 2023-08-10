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

        public Task ForgotPassword(ForgotPasswordModel model)
        {
            throw new NotImplementedException();
        }

        public Task<UserInfo> GetUserByUsername(string username)
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
