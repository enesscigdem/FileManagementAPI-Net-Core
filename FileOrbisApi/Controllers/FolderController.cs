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
    public class FolderController : ControllerBase
    {
        private readonly GenericManager<FolderInfo> _folderManager;
        public FolderController(IGenericDal<FolderInfo> folderDal)
        {
            _folderManager = new GenericManager<FolderInfo>(folderDal);
        }
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllFolders()
        {
            var folderList = _folderManager.GetListAll();
            return Ok(folderList);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetFolderByID(int id)
        {
            var folder = _folderManager.GetListByID(id);
            if (folder == null)
            {
                return NotFound();
            }
            return Ok(folder);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult PostFolder([FromBody] FolderInfo folder)
        {
            using (var context = new FileOrbisContext())
            {
                context.FolderInfo.Add(folder);
                context.SaveChanges();
            }

            return CreatedAtAction(nameof(GetAllFolders), new { id = folder.FolderID }, folder);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public IActionResult DeleteFolder(int id)
        {
            var folder = _folderManager.GetListByID(id);
            if (folder == null)
            {
                return NotFound();
            }

            using (var context = new FileOrbisContext())
            {
                context.FolderInfo.Remove(folder);
                context.SaveChanges();
            }

            return NoContent();
        }
    }
}
