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
        private readonly GenericManager<UserInfo> _userManager;
        public UserController(IGenericDal<UserInfo> userDal)
        {
            _userManager = new GenericManager<UserInfo>(userDal);
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllUsers()
        {
            var userList = _userManager.GetListAll();
            return Ok(userList);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetUserByID(int id)
        {
            var user = _userManager.GetListByID(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateUser([FromBody] UserInfo user)
        {
            using (var context = new FileOrbisContext())
            {
                context.UserInfo.Add(user);
                context.SaveChanges();
            }

            return CreatedAtAction(nameof(GetAllUsers), new { id = user.UserID }, user);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _userManager.GetListByID(id);
            if (user == null)
            {
                return NotFound();
            }

            using (var context = new FileOrbisContext())
            {
                context.UserInfo.Remove(user);
                context.SaveChanges();
            }

            return NoContent();
        }
    }
}
