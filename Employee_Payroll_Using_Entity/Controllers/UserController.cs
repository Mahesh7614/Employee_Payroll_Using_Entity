using BusinessLayer.Interface;
using CommonLayer;
using Employee_Payroll_Using_Entity.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Entities;
using System;

namespace Employee_Payroll_Using_Entity.Controllers
{
    [Route("EmployeePayroll/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL userBL;

        public UserController(IUserBL userBL)
        {
            this.userBL = userBL;
        }
        [HttpPost]
        [Route("EmployeePayroll/Registration")]
        public IActionResult UserRegistration(UserRegistrationModel userModel)
        {
            try
            {
                UserEntity registrationData = this.userBL.RegisterUser(userModel);
                if (registrationData != null)
                {
                    return this.Ok(new { success = true, message = "Registration Successfull", result = registrationData });
                }
                return this.BadRequest(new { success = true, message = "User Already Exists" });
            }
            catch (EmployeePayrollException ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }
        [HttpPost]
        [Route("EmployeePayroll/Login")]
        public IActionResult Login(string EmailID, string Password)
        {
            try
            {
                string userToken = this.userBL.Login(EmailID, Password);
                if (userToken != null)
                {
                    return this.Ok(new { success = true, message = "Login Successfull", result = userToken });
                }
                return this.BadRequest(new { success = true, message = "Enter Valid EmailID or Password" });
            }
            catch (EmployeePayrollException ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }
        [HttpGet]
        [Route("EmployeePayroll/ForgotPassword")]
        public IActionResult ForgotPassword(string EmailID)
        {
            try
            {
                string emailToken = this.userBL.ForgotPassword(EmailID);
                if (emailToken != null)
                {
                    return this.Ok(new { success = true, message = "Password Forgot Sucessfully", result = emailToken });
                }
                return this.BadRequest(new { success = true, message = "Enter Valid EmailID" });
            }
            catch (EmployeePayrollException ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }
        [Authorize]
        [HttpPut]
        [Route("EmployeePayroll/ResetPassword")]
        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                int UserID = Convert.ToInt32(User.FindFirst("UserID").Value);
                if (password == confirmPassword)
                {
                    bool userPassword = this.userBL.ResetPassword(password, UserID);
                    if (userPassword)
                    {
                        return this.Ok(new { success = true, message = "Password Reset Successfully", result = userPassword });
                    }
                }
                return this.BadRequest(new { success = true, message = "Enter Password same as above" });

            }
            catch (EmployeePayrollException ex)
            {
                return this.NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
