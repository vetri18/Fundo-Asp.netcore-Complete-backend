using BusinessLayer.Interfaces;
using CommonLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

namespace FundoPrac.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LabelController : ControllerBase
    {

        ILabelBL ilableBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly ILogger<LabelController> logger;

        private readonly Context Context;

        public LabelController(ILabelBL ilableBL, IMemoryCache memoryCache, IDistributedCache distributedCache, Context Context, ILogger<LabelController> logger)
        {
            this.ilableBL = ilableBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.Context = Context;
            this.logger = logger;
        }


        

        [HttpPost]
        [Route("Create")]
        public IActionResult CreateLable(long notesId, LabelModel model)
        {

            try
            {


                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

                if (jwtUserId == 0 && notesId == 0)
                {
                    logger.LogWarning($"Lable not created for {notesId} noteID");
                    return BadRequest(new { Success = false, message = "Lable not created" });
                }
                else
                {

                    LabelResponseModel lable = ilableBL.CreateLable(notesId, jwtUserId, model);

                    logger.LogInformation($"Lable creatred for {notesId} NotesID");
                    return Ok(new { Success = true, message = "Lable Created", lable });

                }


            }
            catch (Exception)
            {
                logger.LogError("Exception occured in CreateLable API");
                throw;
            }


        }


       
        [HttpGet]
        [Route("ReadAll")]
        public IActionResult GetAllLable()
        {

            try
            {

                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);

                var result = ilableBL.GetAllLable(jwtUserId);

                if (result != null)
                {
                    logger.LogInformation("Read all Lables");

                    return Ok(new { Success = true, message = "Retrived All lables ", result });

                }
                else
                {
                    logger.LogWarning("No Lable Found");
                    return BadRequest(new { Success = false, message = "No lable in database " });

                }

            }
            catch (Exception)
            {
                logger.LogError("Exception occured in ReadAllLable API");
                throw;
            }

        }


        
        [HttpGet]
        [Route("GetLableById")]
        public IActionResult GetLableWithId(long lableId)
        {

            try
            {

                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = ilableBL.GetLableWithId(lableId, jwtUserId);

                if (result != null)
                {
                    logger.LogInformation($"Get Lable for {lableId} LableID");
                    return Ok(new { Success = true, message = "Retrived Lable ", result });

                }
                else
                {
                    logger.LogWarning($"Lable not found for {lableId} LableID");
                    return NotFound(new { Success = false, message = "No Lable found with particular LableId " });

                }

            }
            catch (Exception)
            {
                logger.LogError("Exception occured in GetLableByID API");
                throw;
            }

        }


        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateLable(long lableId, UpdateLableModel model)
        {
            try
            {

                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                LabelEntity updateLable = ilableBL.GetLablesWithId(lableId, jwtUserId);


                if (updateLable != null)
                {
                    LabelResponseModel lable = ilableBL.UpdateLable(updateLable, model, jwtUserId);

                    logger.LogInformation($"Lable updated successfully for {lableId} LableID");
                    return Ok(new { Success = true, message = "Lable Updated Sucessfully", lable });

                }
                else
                {
                    logger.LogWarning($"No Lable found for {lableId} LableID");
                    return BadRequest(new { Success = false, message = "No Lable found with LableId" });

                }



            }
            catch (Exception)
            {
                logger.LogError("Exception occured in UpdateLable API");
                throw;
            }

        }


        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteLable(long lableId)
        {

            try
            {

                long jwtUserId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                LabelEntity lable = ilableBL.GetLablesWithId(lableId, jwtUserId);

                if (lable != null)
                {

                    ilableBL.DeleteLable(lable, jwtUserId);
                    logger.LogInformation($"Lable removed for {lableId} lableID");
                    return Ok(new { Success = true, message = "Lable Removed" });

                }
                else
                {
                    logger.LogWarning("Lable not found");
                    return BadRequest(new { Success = false, message = "No Lable Found" });

                }

            }
            catch (Exception)
            {
                logger.LogError("Exception occured in DeleteLable API");
                throw;
            }

        }


        [HttpGet("redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            long userID = Convert.ToInt32(User.Claims.FirstOrDefault(user => user.Type == "userID").Value);

            var cacheKey = "LableList";
            string serializedLableList;
            var LableList = new List<LabelEntity>();
            var redisLableList = await distributedCache.GetAsync(cacheKey);
            if (redisLableList != null)
            {
                serializedLableList = Encoding.UTF8.GetString(redisLableList);
                LableList = JsonConvert.DeserializeObject<List<LabelEntity>>(serializedLableList);
            }
            else
            {


                LableList = Context.lableTable.ToList();
                serializedLableList = JsonConvert.SerializeObject(LableList);
                redisLableList = Encoding.UTF8.GetBytes(serializedLableList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisLableList, options);
            }
            return Ok(LableList);
        }

    }
}
