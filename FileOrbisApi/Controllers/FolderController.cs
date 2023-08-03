﻿using FileOrbis.BusinessLayer.Abstract;
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
using System.IO.Compression;
using FileOrbisApi.ContextSingleton;

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
            string folderPath;
            try
            {
                var user = _context.UserInfo.FirstOrDefault(x => x.UserID == folder.UserID);
                var parentFolder = _context.FolderInfo.FirstOrDefault(x => x.FolderID == folder.ParentFolderID);

                if (folder == null || string.IsNullOrWhiteSpace(folder.FolderName) || folder.UserID == 0)
                    return BadRequest("Invalid folder data.");

                if (folder.ParentFolderID == null)
                {
                    var folderInfo = _context.FolderInfo.FirstOrDefault(x => x.UserID == folder.UserID);
                    folder.ParentFolderID = folderInfo.FolderID;
                    folderPath = Path.Combine(folderInfo.Path, folder.FolderName);
                    folder.Path = folderPath;
                }
                else
                {
                    folderPath = Path.Combine(parentFolder.Path, folder.FolderName);
                    folder.Path = folderPath;
                }

                var existingFolder = _context.FolderInfo
                    .FirstOrDefault(x => x.UserID == folder.UserID && x.ParentFolderID == folder.ParentFolderID && x.FolderName == folder.FolderName);

                int count = 1;
                while (existingFolder != null)
                {
                    string newName = $"{folder.FolderName}{count}";
                    folder.FolderName = newName;
                    folderPath = Path.Combine(parentFolder.Path, newName);
                    folder.Path = folderPath;
                    existingFolder = _context.FolderInfo
                        .FirstOrDefault(x => x.UserID == folder.UserID && x.ParentFolderID == folder.ParentFolderID && x.FolderName == newName);
                    
                    count++;
                }
                folder.CreationDate = DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss");
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
            var folder = await _genericService.GetListByID(id);

            if (folder != null)
            {
                await DeleteFolderContents(id);

                await _genericService.Delete(id);
                DeleteDirectory(folder.Path);

                return Ok();
            }

            return NotFound();
        }

        private async Task DeleteFolderContents(int folderId)
        {
            var subFolders = _context.FolderInfo.Where(x => x.ParentFolderID == folderId).ToList();
            foreach (var subFolder in subFolders)
            {
                await DeleteFolderContents(subFolder.FolderID);
                _context.FolderInfo.Remove(subFolder);
            }

            var files = _context.FileInfo.Where(x => x.FolderID == folderId).ToList();
            foreach (var file in files)
            {
                _context.FileInfo.Remove(file);
            }
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
                .Where(f => f.ParentFolderID == parentFolderID && f.ParentFolderID != null)
                .ToListAsync();

            return Ok(subfolders);
        }

        [HttpDelete]
        [Route("[action]")]
        public async Task<IActionResult> DeleteAllFolders()
        {
            await _genericService.DeleteAll();
            return Ok();

        }
        [HttpPut]
        [Route("[action]")]
        [AllowAnonymous]
        public async Task<IActionResult> RenameFolder([FromBody] FolderInfo folder)
        {
            var existingFolder = await _genericService.GetListByID(folder.FolderID);
            if (existingFolder != null)
            {
                existingFolder.FolderName = folder.FolderName;
                string newPath = Path.Combine(Path.GetDirectoryName(existingFolder.Path), existingFolder.FolderName);
                MoveFolder(existingFolder.Path, newPath);
                existingFolder.Path = newPath;
                await _genericService.Update(existingFolder);
                return Ok(existingFolder);
            }

            return NotFound();
        }
        [HttpGet]
        [Route("[action]/{folderName}")]
        public IActionResult DownloadFolder(string folderName, string folderPath)
        {
            if (!Directory.Exists(folderPath))
            {
                return NotFound();
            }

            string zipFileName = folderName + ".zip";
            string zipFilePath = Path.Combine(Path.GetTempPath(), zipFileName);

            try
            {
                if (System.IO.File.Exists(zipFilePath))
                {
                    System.IO.File.Delete(zipFilePath);
                }

                ZipFile.CreateFromDirectory(folderPath, zipFilePath);

                var memory = new MemoryStream();
                using (var stream = new FileStream(zipFilePath, FileMode.Open))
                {
                    stream.CopyTo(memory);
                }
                memory.Position = 0;

                var contentType = "application/octet-stream";
                var fileName = Path.GetFileName(zipFilePath);

                return File(memory, contentType, fileName);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while creating or downloading the zip folder: " + ex.Message);
            }
            finally
            {
                if (System.IO.File.Exists(zipFilePath))
                {
                    System.IO.File.Delete(zipFilePath);
                }
            }
        }
        public static void DeleteDirectory(string targetDirectory)
        {
            string[] files = Directory.GetFiles(targetDirectory);
            string[] subDirectories = Directory.GetDirectories(targetDirectory);
            foreach (string file in files)
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
            }
            foreach (string subDirectory in subDirectories)
                DeleteDirectory(subDirectory);

            Directory.Delete(targetDirectory, false);
        }
        static void MoveFolder(string sourceDir, string destDir)
        {
            Directory.CreateDirectory(destDir);
            foreach (string file in Directory.GetFiles(sourceDir))
            {
                string destFile = Path.Combine(destDir, Path.GetFileName(file));
                System.IO.File.Move(file, destFile);
            }
            foreach (string subDir in Directory.GetDirectories(sourceDir))
            {
                string destSubDir = Path.Combine(destDir, Path.GetFileName(subDir));
                MoveFolder(subDir, destSubDir);
            }
            Directory.Delete(sourceDir, true);
        }
    }
}