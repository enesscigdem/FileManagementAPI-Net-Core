using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.EntityLayer.Concrete;
using FileOrbisApi.Folder;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace FileOrbisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FolderController : ControllerBase
    {
        private IGenericService<FolderInfo> _genericService;
        private readonly FileOrbisContext _context;

        public FolderController(IGenericService<FolderInfo> genericService, FileOrbisContext context)
        {
            _genericService = genericService;
            _context = context;
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
            try
            {
                if (folder == null || string.IsNullOrWhiteSpace(folder.FolderName) || folder.UserID == 0)
                {
                    return BadRequest("Invalid folder data.");
                }

                string folderPath;
                folderPath = Path.Combine(folder.FolderPath, folder.FolderName);

                Directory.CreateDirectory(folderPath);
                var createFolder = await _genericService.Create(folder);

                return CreatedAtAction("GetAllFolders", new { id = createFolder.FolderID }, createFolder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating the folder: " + ex.Message);
            }
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
        [HttpGet]
        [Route("[action]/{parentFolderID}")]
        public async Task<IActionResult> GetFoldersByParentFolderID(int parentFolderID)
        {
            var subfolders = await _context.FolderInfo
                .Where(f => f.ParentFolderID == parentFolderID && f.ParentFolderID!=null)
                .ToListAsync();

            return Ok(subfolders);
        }
    }
}

