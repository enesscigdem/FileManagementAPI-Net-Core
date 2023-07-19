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
    public class FileItemController : ControllerBase
    {
        private readonly GenericManager<FileItem> _fileManager;
        public FileItemController(IGenericDal<FileItem> fileDal)
        {
            _fileManager = new GenericManager<FileItem>(fileDal);
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetListAll()
        {
            var list = _fileManager.GetListAll();
            return Ok(list);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetListByID(int id)
        {
            var listbyid = _fileManager.GetListByID(id);
            if (listbyid == null)
            {
                return NotFound();
            }
            return Ok(listbyid);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Create([FromBody] FileItem item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using (var context = new FileOrbisContext())
            {
                context.FileItems.Add(item);
                context.SaveChanges();
            }

            return CreatedAtAction(nameof(GetListByID), new { id = item.Id }, item);
        }
        [HttpDelete]
        [Route("[action]/{id}")]
        public IActionResult Delete(int id)
        {
            var item = _fileManager.GetListByID(id);
            if (item == null)
            {
                return NotFound();
            }

            using (var context = new FileOrbisContext())
            {
                context.FileItems.Remove(item);
                context.SaveChanges();
            }

            return NoContent();
        }
    }
}
