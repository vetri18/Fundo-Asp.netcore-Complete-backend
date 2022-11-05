using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RepositoryLayer.AddContext;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        INotesBL Notesbl;
        private readonly IMemoryCache memoryCache;
        
        private readonly IDistributedCache distributedCache;
        private readonly Context context;
        private readonly ILogger<NotesController> logger;
        public NotesController(IMemoryCache memoryCache,INotesBL Notesbl , Context context , IDistributedCache distributedCache, ILogger<NotesController> logger)
        
        {
            this.memoryCache = memoryCache;
            this.Notesbl = Notesbl;
            this.distributedCache = distributedCache;
            this.context=context;
            this.logger = logger;
        }
        [HttpPost("Add")]
        public IActionResult AddNotes(NotesModel addnote)
        {
            try
            {
                //long userid = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "UserId").Value);
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = Notesbl.AddNote(addnote, userID);
                if (result != null)
                {
                    logger.LogInformation($"Note Created succesfully for {userID} userId");
                    return this.Ok(new { Success = true, message = "Notes Added  Sucessfully", Response = result });
                }
                else
                {
                    logger.LogWarning("Note Creation Failed");
                    return this.BadRequest(new { Success = false, message = "notes adding is  Unsucessfull" });
                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception Occured in Create Note API");
                throw;
            }
        }
        [Authorize]
        [HttpDelete("Delete")]
        public IActionResult DeleteNotes(long NoteId)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var delete = Notesbl.DeleteNote(NoteId);
                if (delete != null)
                {
                    logger.LogInformation($"Note Deleted for {NoteId} NoteID");
                    return this.Ok(new { Success = true, message = "Notes Deleted Successfully" });
                }
                else
                {
                    logger.LogWarning($"Note not found for {NoteId} NoteID");
                    return this.BadRequest(new { Success = false, message = "Notes Deleted Unsuccessful" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured in DeleteNotes API");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("GetNoteByNoteID")]
        public IActionResult GetNote(long NoteId)
        {
            try
            {
                List<NotesEntity> result = Notesbl.GetNote(NoteId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = " Note got Successfully", data = result });
                }
                else
                    return this.BadRequest(new { Success = false, message = "Note not Available" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("GetNoteByUserId")]
        public IActionResult GetNoteByUserID(long userID)
        {
            try
            {
                //long note = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "userID").Value);
                List<NotesEntity> result = Notesbl.GetNotebyUserId(userID);
                if (result == null)
                {
                    return this.Ok(new { Success = false, message = " Note not Available" });
                }
                else
                {
                    if (result != null)
                    {
                        return this.Ok(new { Success = true, message = " Note got Successfully", data = result });
                    }
                    return this.BadRequest(new { Success = false, message = " error occured" });

                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpGet("GetAllNote")]
        public IActionResult GetAllNote()
        {
            try
            {
                List<NotesEntity> result = Notesbl.GetAllNote();
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = " Note got Successfully", data = result });
                }
                else
                    return this.BadRequest(new { Success = false, message = "Note not Available" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateNote(NotesModel noteModel, long NoteId)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);
                var result = Notesbl.UpdateNote(noteModel, NoteId);
                if (result != null)
                {
                    logger.LogInformation($"Note updated succesfully for {NoteId} NoteID");
                    return this.Ok(new { Success = true, message = "Notes Updated Successfully", data = result });
                }
                else
                {
                    logger.LogWarning($"Cannot update note for {NoteId} NoteID");
                    return this.BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured in Update note API");
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
        [HttpPut]
        [Route("Archive")]
        public IActionResult ArchiveNote(long NoteId)
        {
            try
            {
                logger.LogInformation($"Note Archive Successfully for {NoteId} NoteID");
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "userID").Value);
                var result = Notesbl.Archieved(NoteId, userid);
                if (result == true)
                {
                    return this.Ok(new { Success = true, message = "Archived Successfully", data = result });
                }
                else if (result == false)
                {
                    logger.LogInformation($"Note Unarchive Successfully for {NoteId} NoteID");
                    return this.Ok(new { Success = true, message = "Unarchived", data = result });
                }

                else
                {
                    logger.LogWarning($"Notes not found  for Archive/Unarchive Operation for {NoteId} NoteID");
                    return this.BadRequest(new { Success = false, Message = "Archived unsuccessful" });
                }
            }
            catch (Exception e)
            {
                logger.LogError("Exception occured in NotesArchive API");
                throw;
            }
        }

        [HttpPut]
        [Route("Pin")]
        public IActionResult PinNote(long NoteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(p => p.Type == "userID").Value);
                var result = Notesbl.Pinned(NoteId, userId);
                if (result == true)
                {
                    logger.LogError("Exception occured in NotesArchive API");
                    return this.Ok(new { Success = true, message = "Note Pinned Successfully", data = result });
                }
                else if (result == false)
                {
                    logger.LogInformation($"Note unpinned successfull for {NoteId} NoteID");
                    return this.Ok(new { Success = true, message = "Unpinned", data = result });
                }
                else
                {
                    logger.LogWarning($"Note not found for Pin/Unpin operation for {NoteId} NoteID");
                    return this.BadRequest(new { Success = false, message = "Note Pinned Unsuccessfully" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured in NotePin API");
                throw;
            }
        }

        [HttpPut]
        [Route("Trash")]
        public IActionResult TrashNote(long NotesId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(t => t.Type == "userID").Value);
                var result = Notesbl.Trashed(NotesId, userId);
                if (result == true)
                {
                    logger.LogInformation($"Note trashed Successfully for {NotesId} NoteID");
                    return this.Ok(new { Success = true, message = "Trashed Successfully", data = result });
                }
                else if (result == false)
                {
                    logger.LogInformation($"Note Untrashed Successfully for {NotesId} NoteID");
                    return this.Ok(new { Success = true, message = "Untrashed", data = result });
                }

                else
                {
                    logger.LogWarning($"Note not found for Trashed/Untrashed operation for {NotesId} NoteID");
                    return this.BadRequest(new { Success = false, message = "Trashed unsuccessfull" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception Occured in NoteTrash API");
                throw;
            }
        }

        [HttpPut]
        [Route("Color")]
        public IActionResult ColourNote(long NoteId, string color)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(r => r.Type == "userID").Value);
                var colors = Notesbl.ColorNote(NoteId, color);
                if (colors != null)
                {
                    logger.LogInformation($"Note Color changed to {color} successfully for {NoteId} NoteID");
                    return Ok(new { Success = true, message = "Added Colour Successfully", data = colors });
                }
                else
                {
                    logger.LogWarning($"Note not found to change the color for {NoteId} NoteID");
                    return BadRequest(new { Success = false, message = "Added Colour Unsuccessful" });
                }
            }
            catch (Exception)
            {
                logger.LogError("Exception occured in Note Color API");
                throw;
            }
        }
        [HttpPut]
        [Route("Image")]
        public IActionResult Imaged(long noteId, IFormFile image)
        {
            try
            {
                long userID = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = Notesbl.Imaged(noteId, userID, image);
                if (result != null)
                {
                    logger.LogInformation($"Image Upload succesffuly to Cloudinary for {noteId} NoteID");
                    return Ok(new { Status = true, Message = "Image Uploaded Successfully", Data = result });
                }
                else
                {
                    logger.LogWarning($"Note not found  to upload image to Cloudinary for {noteId} NoteID");
                    return BadRequest(new { Status = true, Message = "Image Uploaded Unsuccessfully", Data = result });
                }
            }
            catch (Exception)
            {
                logger.LogError("Exception occured in NoteImage API");
                throw;
            }
        }
        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            var cacheKey = "NoteList";
            string serializedNoteList;
            var NoteList = new List<NotesEntity>();
            var redisNoteList = await distributedCache.GetAsync(cacheKey);
            if (redisNoteList != null)
            {
                serializedNoteList = Encoding.UTF8.GetString(redisNoteList);
                NoteList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNoteList);
            }
            else
            {
                //  NoteList = await context.Notes.ToListAsync();
                NoteList = (List<NotesEntity>)Notesbl.GetAllNote();
                serializedNoteList = JsonConvert.SerializeObject(NoteList);
                redisNoteList = Encoding.UTF8.GetBytes(serializedNoteList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNoteList, options);
            }
            return Ok(NoteList);
        }


    }
}
