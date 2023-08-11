using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.EntityLayer.Concrete;
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
        private IFileService _fileService;

        public FileController(IFileService fileService)
        {
            _fileService = fileService;

        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllFiles()
        {
            try
            {
                var fileList = await _fileService.GetListAll();
                return Ok(fileList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while get the files: " + ex.Message);
            }
        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetFileByID(int id)
        {
            try
            {
                var file = await _fileService.GetListByID(id);
                return Ok(file);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while get the files: " + ex.Message);
            }

        }
        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteFile(int id)
        {
            try
            {
                await _fileService.DeleteFile(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the files: " + ex.Message);
            }
        }
        [HttpGet]
        [Route("[action]/{folderID}")]
        public async Task<IActionResult> GetFilesByFolderID(int folderID)
        {
            try
            {
                var fileList = await _fileService.GetFilesByFolderID(folderID);
                return Ok(fileList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while get the files: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAllFiles()
        {
            try
            {
                await _fileService.DeleteAll();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while delete all files: " + ex.Message);
            }
        }
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> RenameFile([FromBody] FileInfos file)
        {
            try
            {
                await _fileService.RenameFile(file);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the file: " + ex.Message);
            }
        }
        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            try
            {
                var result = await _fileService.DownloadFile(id);
                if (result.fileBytes == null)
                    return NotFound();

                var contentType = "application/octet-stream";

                return File(result.fileBytes, contentType, result.fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while downloading the file: " + ex.Message);
            }
        }
        [HttpPost]
        [Route("[action]")]
        [RequestFormLimits(ValueLengthLimit = int.MaxValue,
        MultipartBodyLengthLimit = long.MaxValue)]
        [DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile(IFormFile file, int folderID)
        {
            try
            {
                var uploadedFile = await _fileService.UploadFile(file, folderID);

                var jsonOptions = new JsonSerializerOptions
                {
                    ReferenceHandler = ReferenceHandler.Preserve,
                    WriteIndented = true
                };

                string json = JsonSerializer.Serialize(uploadedFile, jsonOptions);

                return Content(json, "application/json");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while uploading the file: " + ex.Message);
            }
        }

       

        private async Task<IActionResult> GetFileByType(int id, Func<string, bool> extensionCheck, string contentType)
        {
            try
            {
                FileInfos fileInfo = await _fileService.GetListByID(id);
                if (fileInfo == null || !extensionCheck(fileInfo.FileName))
                    return NotFound();

                string filePath = fileInfo.Path;
                if (!System.IO.File.Exists(filePath))
                    return NotFound();

                return PhysicalFile(filePath, contentType);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while downloading the file: " + ex.Message);
            }

        }

        [HttpGet("[action]/{id}")]
        public async Task<IActionResult> GetImageByFileId(int id)
        {
            return await GetFileByType(id, IsImageFile, "image/jpeg");
        }


        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPdfByFileId(int id)
        {
            return await GetFileByType(id, IsPdfFile, "application/pdf");
        }

        [HttpGet("[action]/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetVideoByFileId(int id)
        {
            return await GetFileByType(id, IsVideoFile, "video/mp4");
        }


        private bool IsImageFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            return extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif" || extension == ".bmp" || extension == ".webp";
        }

        private bool IsPdfFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            return extension == ".pdf";
        }

        private bool IsVideoFile(string fileName)
        {
            string extension = Path.GetExtension(fileName);
            return extension == ".mp4" || extension == ".avi" || extension == ".mkv";
        }
    }
}