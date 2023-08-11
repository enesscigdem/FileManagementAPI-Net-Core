using FileOrbis.BusinessLayer.Abstract;
using FileOrbis.BusinessLayer.Concrete;
using FileOrbis.DataAccessLayer.Abstract;
using FileOrbis.DataAccessLayer.Context;
using FileOrbis.EntityLayer.Concrete;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.IO.Compression;

namespace FileOrbisApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FolderController : ControllerBase
    {
        private IFolderService _folderService;

        public FolderController(IFolderService folderService)
        {
            _folderService = folderService;
        }
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetAllFolders()
        {
            try
            {
                var folderList = await _folderService.GetListAll();
                return Ok(folderList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the folder: " + ex.Message);
            }

        }

        [HttpGet]
        [Route("[action]/{id}")]
        public async Task<IActionResult> GetFolderByID(int id)
        {
            try
            {
                var folder = await _folderService.GetListByID(id);
                return Ok(folder);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the folder: " + ex.Message);
            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateFolder([FromBody] FolderInfo folder)
        {
            try
            {
                await _folderService.CreateFolder(folder);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the folder: " + ex.Message);
            }
        }


        [HttpDelete]
        [Route("[action]/{id}")]
        public async Task<IActionResult> DeleteFolder(int id)
        {
            try
            {
                await _folderService.DeleteFolder(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the folder: " + ex.Message);
            }
        }
        [HttpGet]
        [Route("[action]/{userID}")]
        public async Task<IActionResult> GetFoldersByUserID(int userID)
        {
            try
            {
                var folderList = await _folderService.GetFoldersByUserID(userID);
                return Ok(folderList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the folder: " + ex.Message);
            }
        }
        [HttpGet]
        [Route("[action]/{parentFolderID}")]
        public async Task<IActionResult> GetFoldersByParentFolderID(int parentFolderID)
        {
            try
            {
                var subFolderList = await _folderService.GetFoldersByParentFolderID(parentFolderID);
                return Ok(subFolderList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the folder: " + ex.Message);
            }
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAllFolders()
        {
            try
            {
                await _folderService.DeleteAll();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the folder: " + ex.Message);
            }
        }
        [HttpPut]
        [Route("[action]")]
        public async Task<IActionResult> RenameFolder([FromBody] FolderInfo folder)
        {
            try
            {
                await _folderService.RenameFolder(folder);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while renaming the folder: " + ex.Message);
            }
        }
        [HttpGet]
        [Route("[action]/{folderName}")]
        public async Task<IActionResult> DownloadFolder(string folderName, string folderPath)
        {
            try
            {
                var zipFileBytes = await _folderService.DownloadFolder(folderName, folderPath);

                var contentType = "application/octet-stream";
                var fileName = folderName + ".zip";

                return File(zipFileBytes, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating or downloading the zip folder: " + ex.Message);
            }
        }
    }
}