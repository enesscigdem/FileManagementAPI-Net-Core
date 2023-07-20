using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> GetAllUsers()
        {
            var userList = await _genericService.GetListAll();
            return Ok(userList);
        }

        [HttpGet]
        [Route("[action]/{id}")]
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
            return CreatedAtAction("GetAllUsers", new { id = user.UserID }, createUser);
        }
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_genericService.GetListByID(id) != null)
            {
                await _genericService.Delete(id);
                return Ok();
            }
            return NotFound();
        }
    }
}
