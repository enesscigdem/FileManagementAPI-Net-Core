using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.EntityLayer.Concrete;
using FileOrbisApi.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FileOrbisApi.JWT;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace FileOrbisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IGenericService<UserInfo> _genericService;
        private IGenericService<FolderInfo> _genericServiceFolder;
        public UserController(IGenericService<UserInfo> genericService, IGenericService<FolderInfo> genericServiceFolder)
        {
            _genericService = genericService;
            _genericServiceFolder = genericServiceFolder;
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await _genericService.GetListAll();
            return Ok(userList);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserByID(int id)
        {
            var user = await _genericService.GetListByID(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo user)
        {
            var createUser = await _genericService.Create(user);
            FolderInfo folderInfo= new FolderInfo
            {
                FolderName = createUser.UserName,
                Path = Path.Combine("C:\\server\\",createUser.UserName),
                CreationDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                UserID = createUser.UserID,
            };
            var createFolder = await _genericServiceFolder.Create(folderInfo);
            Directory.CreateDirectory("C:\\server\\" + createUser.UserName);
            return CreatedAtAction("GetAllUsers", new { id = user.UserID }, createUser);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _genericService.GetListAll();
            var username = model.Username;
            var password = model.Password;
            
            var authenticatedUser = user.FirstOrDefault(u => u.UserName == username);

            if (authenticatedUser != null && BCrypt.Net.BCrypt.Verify(password, authenticatedUser.Password))
            {
                var key = "fileorbisproject";
                CreateToken createToken = new CreateToken();
                var token = createToken.GenerateJwtToken(authenticatedUser, key);

                return Ok(new { Message = "Login successful!", Token = token, UserID = authenticatedUser.UserID });
            }

            return BadRequest(new { Message = "Username or password is wrong!" });
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _genericService.GetListByID(id) != null)
            {
                await _genericService.Delete(id);
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _genericService.GetListAll();
            var username = model.Username;

            HttpContext.Session.SetString("ResetPassUsername", model.Username);
            HttpContext.Session.SetString("ResetPassEmail", model.Email);

            var validUsername = user.FirstOrDefault(u => u.UserName == username);
            if (validUsername != null)
            {
                var userEmail = model.Email;
                string resetToken = GenerateResetToken();

                try
                {
                    var mail = new MailMessage();
                    var smtpServer = new SmtpClient("smtp.office365.com", 587);
                    smtpServer.Credentials = new NetworkCredential("Enes.Staj@hotmail.com", "Enes123*");
                    smtpServer.EnableSsl = true;
                    mail.From = new MailAddress("Enes.staj@hotmail.com");
                    mail.To.Add(userEmail);
                    mail.Subject = "Password Reset";
                    mail.Body = $"Click the link below to reset your password: http://localhost:5173/reset-password?token={resetToken}";
                    smtpServer.Send(mail);
                    validUsername.ResetToken = resetToken;
                    await _genericService.Update(validUsername);

                    return Ok(new { Message = "An email with instructions has been sent to the user's email address." });
                }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while sending the reset email." });
                }
            }
            else
            {
                return BadRequest(new { Message = "No such user found!" });
            }
        }
        private string GenerateResetToken()
        {
            return Guid.NewGuid().ToString();
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordModel model)
        {
            var user = await _genericService.GetListAll();

            var validUsername = user.FirstOrDefault(x=>x.ResetToken == model.ResetToken);
            if (validUsername != null)
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword, salt);
                validUsername.Password = hashedPassword;
                validUsername.ResetToken = null;
                await _genericService.Update(validUsername);

                return Ok(new { Message = "Password has been successfully reset." });
            }
            else
            {
                return BadRequest(new { Message = "Invalid reset token or user not found!" });
            }
        }


        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAllUsers()
        {
            try
            {
                await _genericService.DeleteAll();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while sending the new password." });
            }
        }
    }
}
