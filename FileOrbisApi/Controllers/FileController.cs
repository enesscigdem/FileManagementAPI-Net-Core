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
using System.Text.Json.Serialization;
using System.Text.Json;

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
        public async Task<IActionResult> UploadFile(IFormFile file, int folderID)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file received or file is empty.");
            }

            try
            {
                string filePath;
                if (folderID == 0)
                    filePath = Path.Combine("C:\\server\\enescigdem", file.FileName);
                else
                {
                    var existingFolder = _context.FolderInfo.FirstOrDefault(x => x.FolderID == folderID);
                    filePath = Path.Combine(existingFolder.Path, file.FileName);
                }
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                long fileSizeBytes = file.Length;
                double fileSizeMB = (double)fileSizeBytes / (1024 * 1024);
                string fileSizeFormatted = fileSizeMB.ToString("0.00");

                FileInfos fileInfo = new FileInfos
                {
                    FileName = file.FileName,
                    Path = filePath,
                    Size = double.Parse(fileSizeFormatted),
                    CreationDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"),
                    FolderID = folderID
                };
                
                await _genericService.Create(fileInfo);

                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(fileInfo, jsonOptions);

                return Content(json, "application/json");
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
            var file = await _genericService.GetListByID(id);
            if (file!= null)
            {
                System.IO.File.Delete(file.Path);
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { Message = "An error occurred while sending the new password." });
            }
        }
        [HttpPut]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RenameFile([FromBody] FileInfos file)
        {
            var existingFolder = await _genericService.GetListByID(file.FileID);
            if (existingFolder != null)
            {
                existingFolder.FileName = file.FileName;
                string newPath = Path.Combine(Path.GetDirectoryName(existingFolder.Path), existingFolder.FileName);
                System.IO.File.Move(existingFolder.Path, newPath);
                existingFolder.Path = newPath;
                await _genericService.Update(existingFolder);
                return Ok(existingFolder);
            }

            return NotFound();
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public IActionResult DownloadFile(int id)
        {
            var file = _genericService.GetListByID(id).Result;
            if (file == null)
                return NotFound();

            string filePath = file.Path;

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            try
            {
                var memory = new MemoryStream();
                using (var stream = new FileStream(filePath, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;

                var contentType = "application/octet-stream";
                var fileName = Path.GetFileName(filePath);

                return File(memory, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while downloading the file: " + ex.Message);
            }
        }
        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public IActionResult GetImageByFileId(int id)
        {
            FileInfos fileInfo = _genericService.GetListByID(id).Result;

            if (fileInfo == null || !IsImageFile(fileInfo.FileName))
                return NotFound();

            string filePath = fileInfo.Path;

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "image/jpeg");
        }

        private bool IsImageFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp";
        }


        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public IActionResult GetPdfByFileId(int id)
        {
            FileInfos fileInfo = _genericService.GetListByID(id).Result;

            if (fileInfo == null || !IsPdfFile(fileInfo.FileName))
                return NotFound();

            string filePath = fileInfo.Path;

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "application/pdf");
        }

        private bool IsPdfFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            return extension == ".pdf";
        }
        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public IActionResult GetVideoByFileId(int id)
        {
            FileInfos fileInfo = _genericService.GetListByID(id).Result;

            if (fileInfo == null || !IsVideoFile(fileInfo.FileName))
                return NotFound();

            string filePath = fileInfo.Path;

            if (!System.IO.File.Exists(filePath))
                return NotFound();

            return PhysicalFile(filePath, "video/mp4");
        }

        private bool IsVideoFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            return extension == ".mp4" || extension == ".avi" || extension == ".mkv";
        }
    }
}