
using CommonLayer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entities;
using RepositoryLayer.Interface;
using System;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly IConfiguration config;
        private EmployeePayrollContext employeePayrollContext;

        public UserRL(EmployeePayrollContext employeePayrollContext, IConfiguration config)
        {
            this.employeePayrollContext = employeePayrollContext;
            this.config = config;
        }
        public static string EncryptPassword(string password)
        {
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(encode);
        }
        public string GenerateJWTToken(string emailID, int UserID)
        {
            try
            {
                var loginSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(this.config[("Jwt:key")]));
                var loginTokenDescripter = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.Email, emailID),
                        new Claim("UserID",UserID.ToString())
                    }),
                    Expires = DateTime.UtcNow.AddHours(1),
                    SigningCredentials = new SigningCredentials(loginSecurityKey, SecurityAlgorithms.HmacSha256Signature)
                };
                var token = new JwtSecurityTokenHandler().CreateToken(loginTokenDescripter);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
        /// <summary>
        /// Registers the user.
        /// </summary>
        /// <param name="userRegistration">The user registration.</param>
        /// <returns></returns>
        public UserEntity RegisterUser(UserRegistrationModel userRegistration)
        {
            try
            {
                UserEntity objUEntity = new UserEntity();
                objUEntity.Fullname = userRegistration.Fullname;
                objUEntity.EmailID = userRegistration.EmailID;
                objUEntity.MobileNumber = userRegistration.MobileNumber;
                objUEntity.Password = EncryptPassword(userRegistration.Password);
                this.employeePayrollContext.UserTable.Add(objUEntity);
                int result = employeePayrollContext.SaveChanges();
                if (result > 0)
                {
                    return objUEntity;
                }
                else
                {
                    return null;
                }
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
                var result = employeePayrollContext.UserTable.Where(x => x.EmailID == EmailID).FirstOrDefault();
                if (result != null && result.Password == EncryptPassword(Password))
                {
                    var token = GenerateJWTToken(result.EmailID, result.UserID);
                    return token;
                }
                else
                {
                    return null;
                }
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

                var result = employeePayrollContext.UserTable.Where(x => x.EmailID == emailID).FirstOrDefault();
                if (result != null)
                {
                    UserTicket ticket = new UserTicket()
                    {
                        Token = token,
                        EmailId = result.EmailID,
                        FullName = result.Fullname,
                        IssueAt = DateTime.Now
                    };
                    return ticket;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Forgot Password.
        /// </summary>
        /// <param name="emailID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string ForgotPassword(string emailID)
        {
            try
            {
                var result = employeePayrollContext.UserTable.Where(x => x.EmailID == emailID).FirstOrDefault();
                if (result != null)
                {
                    var token = GenerateJWTToken(result.EmailID, result.UserID);
                    MSMQModel mSMQModel = new MSMQModel();
                    mSMQModel.SendMessage(token, result.EmailID, result.Fullname);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// Reset Password.
        /// </summary>
        /// <param name="Password"></param>
        /// <param name="UserID"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool ResetPassword(string Password, int UserID)
        {
            try
            {
                var result = employeePayrollContext.UserTable.Where(x => x.UserID == UserID).FirstOrDefault();
                if (result != null)
                {

                    result.Password = EncryptPassword(Password);
                    employeePayrollContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
