using CommonLayer.Models;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interfaces
{
    public interface IUserBL
    {
        public UserEntity Registration(UserRegistration User);
        public string Login(UserLogin Login);
        public string ForgetPassword(string EmailId);
        public bool ResetPassword(string email, string password, string confirmPassword);

    }
}
