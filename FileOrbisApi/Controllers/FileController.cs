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
        private readonly GenericManager<FileInfos> _fileManager;

        public FileController(IGenericDal<FileInfos> fileDal)
        {
            _fileManager = new GenericManager<FileInfos>(fileDal);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult GetAllFiles()
        {
            var fileList = _fileManager.GetListAll();
            return Ok(fileList);
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult GetFileByID(int id)
        {
            var file = _fileManager.GetListByID(id);
            if (file == null)
            {
                return NotFound();
            }
            return Ok(file);
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult PostFile([FromBody] FileInfos file)
        {
            using (var context = new FileOrbisContext())
            {
                context.FileInfo.Add(file);
                context.SaveChanges();
            }

            return CreatedAtAction(nameof(GetAllFiles), new { id = file.FileID }, file);
        }

        [HttpDelete]
        [Route("[action]/{id}")]
        public IActionResult DeleteFile(int id)
        {
            var file = _fileManager.GetListByID(id);
            if (file == null)
            {
                return NotFound();
            }

            using (var context = new FileOrbisContext())
            {
                context.FileInfo.Remove(file);
                context.SaveChanges();
            }

            return NoContent();
        }
    }
}