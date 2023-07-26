using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.EntityLayer.Concrete;
using FileOrbisApi.Folder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FileOrbisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
            if (await _genericService.GetListByID(id) != null)
            {
                await _genericService.Delete(id);
                return Ok();
            }
            return NotFound();
        }
        [HttpGet]
        [Route("[action]/{userID}")]
        public async Task<IActionResult> GetFoldersByUserID(int userID)
        {
            var folderList = await _genericService.GetFoldersByUserID(userID);
            return Ok(folderList);
        }
        [Route("[action]")]
        [HttpPost]
        public IActionResult CreateEmptyFolder([FromBody] CreateFolder createFolder)
        {
            try
            {
                if (createFolder == null || string.IsNullOrWhiteSpace(createFolder.FolderPath) || string.IsNullOrWhiteSpace(createFolder.FolderName))
                {
                    return BadRequest("Invalid folder path or folder name.");
                }

                string newPath = Path.Combine(createFolder.FolderPath, createFolder.FolderName);

                if (Directory.Exists(newPath))
                {
                    return BadRequest("This folder already exists.");
                }

                Directory.CreateDirectory(newPath);

                return Ok("Empty folder created successfully: " + newPath);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }
    }
}

