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

namespace FileOrbisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IGenericService<UserInfo> _genericService;

        public UserController(IGenericService<UserInfo> genericService)
        {
            _genericService = genericService;
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
        [Authorize]
        public async Task<IActionResult> CreateUser([FromBody] UserInfo user)
        {
            var createUser = await _genericService.Create(user);
            return CreatedAtAction("GetAllUsers", new { id = user.UserID }, createUser);
        }
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await _genericService.GetListAll();
            var username = model.Username;
            var password = model.Password;

            var authenticatedUser = user.FirstOrDefault(u => u.UserName == username && u.Password == password);

            if (authenticatedUser != null)
            {
                var key = "fileorbisproject";
                CreateToken createToken = new CreateToken();
                var token = createToken.GenerateJwtToken(authenticatedUser, key);

                return Ok(new { Message = "Giriş başarılı!", Token = token, UserID = authenticatedUser.UserID });
            }

            return BadRequest(new { Message = "Kullanıcı adı veya şifre hatalı!" });
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
    }
}
