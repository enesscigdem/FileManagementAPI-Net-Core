using FileOrbis.DataAccessLayer.Model;
using FileOrbis.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileOrbis.DataAccessLayer.Abstract
{
    public interface IUserDal : IGenericDal<UserInfo>
    {
        Task<UserInfo> GetUserByUsername(string username);
        Task CreateUser(UserInfo user);
        Task Login(LoginModel model);
        Task ForgotPassword(ForgotPasswordModel model);
        Task ResetPassword(ForgotPasswordModel model);

    }
}
