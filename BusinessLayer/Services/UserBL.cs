using BusinessLayer.Interfaces;
using CommonLayer.Models;
using RepositoryLayer.Entity;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Services
{
    public class UserBL : IUserBL
    {
        IUserRL UserRL;
        public UserBL(IUserRL UserRL)
        {
            this.UserRL = UserRL;
        }

        public UserEntity Registration(UserRegistration User)
        {

            try
            {
                return this.UserRL.Registration(User);

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
                return this.UserRL.Login(Login);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public string ForgetPassword(string EmailId)
        {
            try
            {
                return this.UserRL.ForgetPassword(EmailId);

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
                return this.UserRL.ResetPassword(email, password, confirmPassword);
            }
            catch (Exception)
            {

                throw;
            }

        }




    }
}

