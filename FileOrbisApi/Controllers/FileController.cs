using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace FileOrbisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private IGenericService<FileInfos> _genericService;

        public FileController(IGenericService<FileInfos> genericService)
        {
            _genericService = genericService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllFiles()
        {
            var fileList = await _genericService.GetListAll();
            return Ok(fileList);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetFileByID(int id)
        {
            var file = await _genericService.GetListByID(id);
            if (file == null)
            {
                return NotFound();
            }
            return Ok(file);
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateFile([FromBody] FileInfos file)
        {
            var createFile = await _genericService.Create(file);
            return CreatedAtAction("GetAllFiles", new { id = file.FileID }, createFile);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            if (await _genericService.GetListByID(id) != null)
            {
                await _genericService.Delete(id);
                return Ok();
            }
            return NotFound();
        }
        [HttpGet]
        [Route("[action]/{folderID}")]
        public async Task<IActionResult> GetFilesByFolderID(int folderID)
        {
            var fileList = await _genericService.GetFilesByFolderID(folderID);
            return Ok(fileList);
        }
    }
}