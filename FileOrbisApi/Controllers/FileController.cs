using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.EntityLayer.Concrete;
using FileOrbisApi.Folder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using System.IO;
namespace FileOrbisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        private IGenericService<FileInfos> _genericService;
        private readonly FileOrbisContext _context;

        public FileController(IGenericService<FileInfos> genericService, FileOrbisContext context)
        {
            _genericService = genericService;
            _context = context;
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
            // nasıl upload ederim
            // file olarak gönder
            try
            {
                if (file.FolderID == 0)  // path ön yüzden gelecek.
                    System.IO.File.Create(file.Path);
                else
                {
                    var existingFolder = _context.FolderInfo.FirstOrDefault(x => x.FolderID == file.FolderID);
                    string newPath = Path.Combine(existingFolder.Path, file.FileName);
                    System.IO.File.Copy(file.Path,newPath);
                    file.Path = newPath;
                }
                var createFile = await _genericService.Create(file);

                return CreatedAtAction("GetAllFiles", new { id = createFile.FileID }, createFile);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the folder: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file received or file is empty.");
            }

            try
            {
                string uploadPath = @"C:\server\enescigdem";
                string filePath = Path.Combine(uploadPath, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                return Ok("File uploaded successfully!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while uploading the file: " + ex.Message);
            }
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

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAllFiles()
        {
            try
            {
                await _genericService.DeleteAll();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while sending the new password." });
            }
        }
    }
}