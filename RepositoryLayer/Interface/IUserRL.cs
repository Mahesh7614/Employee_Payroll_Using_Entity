
using CommonLayer;
using RepositoryLayer.Entities;

namespace RepositoryLayer.Interface
{
    public interface IUserRL
    {
        public UserEntity RegisterUser(UserRegistrationModel userRegistration);
        public string Login(string EmailID, string Password);
        public UserTicket CreateTicketForPassword(string emailID, string token);
        public string ForgotPassword(string emailID);
        public bool ResetPassword(string Password, int UserID);
    }
}
