using BCrypt.Net;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.DataAccessLayer.Model;
using FileOrbis.DataAccessLayer.UnitOfWork;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
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
        public async Task DeleteUser(int id)
        {
            if (await GetListByID(id) != null)
            {
                await Delete(id);
                await SaveChangesAsync();
            }
        }
        public async Task<UserInfo> Login(string username, string password)
        {
            var user = await _context.UserInfo.FirstOrDefaultAsync(u => u.UserName == username);

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return user;
            }

            return null;
        }
        public async Task ForgotPassword(ForgotPasswordModel model)
        {
            var user = await _context.UserInfo.FirstOrDefaultAsync(u => u.UserName == model.Username);

            if (user != null)
            {
                string resetToken = Guid.NewGuid().ToString();
                user.ResetToken = resetToken;
                await SaveChangesAsync();

                try
                {
                    var mail = new MailMessage();
                    var smtpServer = new SmtpClient("smtp.office365.com", 587);
                    smtpServer.Credentials = new NetworkCredential("Enes.Staj@hotmail.com", "Enes123*");
                    smtpServer.EnableSsl = true;
                    mail.From = new MailAddress("Enes.staj@hotmail.com");
                    mail.To.Add(model.Email); // Use user's email field here
                    mail.Subject = "Password Reset";
                    mail.Body = $"Click the link below to reset your password: http://localhost:5173/reset-password?token={resetToken}";
                    smtpServer.Send(mail);
                }
                catch (Exception ex)
                {
                    throw new Exception("An error occurred while sending the reset email.", ex);
                }
            }
        }
        public async Task<UserInfo> GetUserByResetToken(string resetToken)
        {
            return await _context.UserInfo.FirstOrDefaultAsync(u => u.ResetToken == resetToken);
        }
        public async Task ResetPassword(ForgotPasswordModel model, string newPassword)
        {
            var user = await _context.UserInfo.FirstOrDefaultAsync(u => u.ResetToken == model.ResetToken);
            if (user != null)
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword, salt);
                user.Password = hashedPassword;
                user.ResetToken = null;
                await SaveChangesAsync();
            }
        }
    }
}
