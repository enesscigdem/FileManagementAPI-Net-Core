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
    public class FolderController : ControllerBase
    {
        private IGenericService<FolderInfo> _genericService;

        public FolderController(IGenericService<FolderInfo> genericService)
        {
            _genericService = genericService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllFolders()
        {
            var folderList = await _genericService.GetListAll();
            return Ok(folderList);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetFolderByID(int id)
        {
            var folder = await _genericService.GetListByID(id);
            if (folder == null)
            {
                return NotFound();
            }
            return Ok(folder);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateFolder([FromBody] FolderInfo folder)
        {
            var createFolder = await _genericService.Create(folder);
            return CreatedAtAction("GetAllFolders", new { id = folder.FolderID }, createFolder);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            if (_genericService.GetListByID(id)!=null)
            {
                await _genericService.Delete(id);
                return Ok();
            }
            return NotFound();
        }
    }
}
