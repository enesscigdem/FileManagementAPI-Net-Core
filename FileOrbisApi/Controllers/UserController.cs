using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FileOrbisApi.JWT;
using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Mail;
using System.Net;
using Microsoft.EntityFrameworkCore;
using FileOrbis.DataAccessLayer.Model;

namespace FileOrbisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await _userService.GetListAll();
            return Ok(userList);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserByID(int id)
        {
            var user = await _userService.GetListByID(id);
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
            await _userService.CreateUser(user);

            return CreatedAtAction("GetAllUsers", new { id = user.UserID }, user);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _userService.GetListAll();
            var username = model.Username;
            var password = model.Password;

            var authenticatedUser = user.FirstOrDefault(u => u.UserName == username);

            if (authenticatedUser != null && BCrypt.Net.BCrypt.Verify(password, authenticatedUser.Password))
            {
                var key = "fileorbisproject";
                CreateToken createToken = new CreateToken();
                var token = createToken.GenerateJwtToken(authenticatedUser, key);

                return Ok(new { Message = "Login successful!", Token = token, userID = authenticatedUser.UserID });
            }

            return BadRequest(new { Message = "Username or password is wrong!" });
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (await _userService.GetListByID(id) != null)
            {
                await _userService.Delete(id);
                await _userService.SaveChangesAsync();
                return Ok();
            }
            return NotFound();
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {

            var validUsername = await _userService.GetUserByUsername(model.Username);

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
                    await _userService.Update(validUsername);
                    await _userService.SaveChangesAsync();

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
            var user = await _userService.GetListAll();
            var validUsername = user.FirstOrDefault(x => x.ResetToken == model.ResetToken);

            if (validUsername != null)
            {
                var salt = BCrypt.Net.BCrypt.GenerateSalt();
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(model.NewPassword, salt);
                validUsername.Password = hashedPassword;
                validUsername.ResetToken = null;
                await _userService.Update(validUsername);
                await _userService.SaveChangesAsync();

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
                await _userService.DeleteAll();
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while sending the new password." });
            }
        }
    }
}
