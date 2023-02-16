
using BusinessLayer.Interface;
using CommonLayer;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL userRL;

        public UserBL(IUserRL userRL)
        {
            this.userRL = userRL;
        }
        public UserEntity RegisterUser(UserRegistrationModel userRegistration)
        {
            try
            {
                return this.userRL.RegisterUser(userRegistration);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string Login(string EmailID, string Password)
        {
            try
            {
                return this.userRL.Login(EmailID, Password);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public UserTicket CreateTicketForPassword(string emailID, string token)
        {
            try
            {
                return this.userRL.CreateTicketForPassword(emailID, token);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public string ForgotPassword(string emailID)
        {
            try
            {
                return this.userRL.ForgotPassword(emailID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool ResetPassword(string Password, int UserID)
        {
            try
            {
                return this.userRL.ResetPassword(Password, UserID);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
