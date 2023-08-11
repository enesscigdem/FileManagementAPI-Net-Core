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
            try
            {
                var userList = await _userService.GetListAll();
                return Ok(userList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while get all the users: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserByID(int id)
        {
            try
            {
                var user = await _userService.GetListByID(id);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while get user: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo user)
        {
            try
            {
                await _userService.CreateUser(user);
                return CreatedAtAction("GetAllUsers", new { id = user.UserID }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred create a user: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            try
            {
                var user = await _userService.Login(model.Username, model.Password);
                if (user!=null)
                {
                    var key = "fileorbisproject";
                    CreateToken createToken = new CreateToken();
                    var token = createToken.GenerateJwtToken(user, key);
                    return Ok(new { Message = "Login successful!", Token = token, userID = user.UserID });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred in login process: " + ex.Message);
            }

        }
        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                await _userService.DeleteUser(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while deleting the user: " + ex.Message);
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
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel model)
        {
            try
            {
                await _userService.GetUserByUsername(model.Username);

                await _userService.ForgotPassword(model);

                return Ok(new { Message = "An email with instructions has been sent to the user's email address." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while forgot password process " + ex.Message);
            }
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgotPasswordModel model)
        {
            try
            {
                var user = await _userService.GetUserByResetToken(model.ResetToken);
                if (user!=null)
                {
                    await _userService.ResetPassword(model, model.NewPassword);
                    return Ok(new { Message = "Password has been successfully reset." });
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while reset password process " + ex.Message);
            }
        }
    }
}
