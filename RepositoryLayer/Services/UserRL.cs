using CommonLayer.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.AddContext;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRL : IUserRL
    {
        public readonly Context context;
        private readonly IConfiguration Config;

        public UserRL(Context context, IConfiguration Config)
        {
            this.context = context;
            this.Config = Config;

        }
        public UserEntity Registration(UserRegistration User)
        {
            
            try
            {
                UserEntity entity = new UserEntity();
                entity.FirstName = User.Firstname;
                entity.LastName = User.Lastname;
                entity.Email = User.Email;
                entity.Password = User.Password;
                string Password = EncryptPassword(User.Password);
                this.context.Users.Add(entity);
                int Result = this.context.SaveChanges();
                if (Result > 0)
                {
                    return entity;
                }
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string Login(UserLogin Login)
        {
            
            try
            {
                UserEntity entity = new UserEntity();
                entity = context.Users.FirstOrDefault(x => x.Email == Login.Email && x.Password == Login.Password);
                var id = entity.UserId;
                var email = entity.Email;
                string Password = DecryptPassword(Login.Password);
                if (entity != null)
                {
                    var Token = GenerateSecurityToken(email, id);
                    return Token;

                }
                else
                    return null;
            }
            catch (Exception)
            {

                throw;
            }

        }
        public string GenerateSecurityToken(string email, long userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.Config[("JWT:key")]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("userID", userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }
        // method to Encrypt Password
        public static string EncryptPassword(string Password)
        {
            try
            {
                if (Password == null)
                {
                    return null;
                }
                else
                {
                    byte[] x = Encoding.ASCII.GetBytes(Password);
                    string encryptedpass = Convert.ToBase64String(x);
                    return encryptedpass;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //Method to Decrypt Encrypted password
        public static string DecryptPassword(string encryptedPass)
        {
            byte[] x;
            string decrypted;
            try
            {
                if (encryptedPass == null)
                {
                    return null;
                }
                else
                {
                    x = Convert.FromBase64String(encryptedPass);
                    decrypted = Encoding.ASCII.GetString(x);
                    return decrypted;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //method to forgetpassword
        public string ForgetPassword(string EmailId)
        {
            try
            {
                var Result = context.Users.FirstOrDefault(x => x.Email == EmailId);
                if (Result != null)
                {
                    var Token = GenerateSecurityToken(EmailId, Result.UserId);
                    new MSMQmodel().sendData2Queue(Token);
                    return Token;
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool ResetPassword(string email, string password, string confirmPassword)
        {
            try
            {
                if (password.Equals(confirmPassword))
                {
                    var Result = context.Users.FirstOrDefault(x => x.Email == email);
                    Result.Password = password;
                    context.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception)
            {

                throw;
            }

        }



    }
}
