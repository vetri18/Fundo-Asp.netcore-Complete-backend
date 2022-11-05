using BusinessLayer.Interfaces;
using CommonLayer.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Fundoo.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserBL Userbl;
        private readonly ILogger<UsersController> logger;
        public UsersController(IUserBL Userbl, ILogger<UsersController> logger)
        {
            this.Userbl = Userbl;
            this.logger = logger;
        }
        [HttpPost("Resgister")]
        public IActionResult AddUser(UserRegistration userRegistration)
        {
            try
            {
                var reg = this.Userbl.Registration(userRegistration);
                if (reg != null)

                {
                    logger.LogInformation("Registration Successful");
                    return this.Ok(new { Success = true, message = "Registration Sucessfull", Response = reg });
                }
                else
                {
                    logger.LogWarning("Registration Unsuccessful");
                    return this.BadRequest(new { Success = false, message = "Registration Unsucessfull" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception ocuured in Registration API");
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [HttpPost("Login")]
        public IActionResult Add(UserLogin LoginRegistration)
        {
            try
            {
                var reg = this.Userbl.Login(LoginRegistration);
                if (reg != null)

                {
                    logger.LogInformation("Login Successful");
                    return this.Ok(new { Success = true, message = "Login Sucessfull", Data = reg });
                }
                else
                {
                    logger.LogWarning("Login Unsuccessful");
                    return this.BadRequest(new { Success = false, message = "Login Unsucessfull" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception occured  in User Login API");
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [HttpPost("ForgetPassWord")]
        public IActionResult ForgetPassword(string EmailId)
        {
            try
            {

                var reg = this.Userbl.ForgetPassword(EmailId);
                if (reg != null)

                {
                    logger.LogInformation($"Email sent successfull to {EmailId}");
                    return this.Ok(new { Success = true, message = "Token sent Sucessfully please check your mail" });
                }
                else
                {
                    logger.LogWarning("Email not found in our database");
                    return this.BadRequest(new { Success = false, message = "unable to send token to mail" });
                }
            }
            catch (Exception ex)
            {
                logger.LogError("Exception Occured in Forget Password API");
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPost("ResetPassword")]
        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                var Email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                //var email = User.Claims.First(e => e.Type == "Email").Value;
                //var result = userBL.ResetPassword(email, password, confirmPassword);

                if (Userbl.ResetPassword(Email, password, confirmPassword))
                {
                    logger.LogWarning("Email not found in our database");
                    logger.LogInformation("Reset Password Succesfull");
                    return Ok(new { success = true, message = "Password Reset Successful" });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Password Reset Unsuccessful" });
                }
            }
            catch (System.Exception)
            {
                logger.LogError("Exception occured in Reset Link API");
                throw;
            }
        }





    }


}










