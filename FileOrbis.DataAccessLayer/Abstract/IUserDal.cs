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
        Task CreateUser(UserInfo user);
        Task DeleteUser(int id);
        Task<UserInfo> Login(string username, string password);
        Task<UserInfo> GetUserByUsername(string username);
        Task ForgotPassword(ForgotPasswordModel model);
        Task ResetPassword(ForgotPasswordModel model, string newPassword);
        Task<UserInfo> GetUserByResetToken(string resetToken);

    }
}
