using CommonLayer;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using BusinessLayer.Interface;
using MassTransit;
using Employee_Payroll_Using_Entity.Helper;

namespace Employee_Payroll_Using_Entity.Controllers
{
    [Route("Employee_Payroll/[controller]")]
    [ApiController]
    public class UserTicketController : ControllerBase
    {
        private readonly IBus bus;
        private readonly IUserBL userBL;

        public UserTicketController(IBus bus, IUserBL userBL)
        {
            this.bus = bus;
            this.userBL = userBL;
        }
        [HttpGet("ForgotPassword")]
        public async Task<IActionResult> CreateTicketForPassword(string EmailID)
        {
            try
            {
                if (EmailID != null)
                {
                    var token = this.userBL.ForgotPassword(EmailID);
                    if (!string.IsNullOrEmpty(token))
                    {
                        UserTicket userTicket = this.userBL.CreateTicketForPassword(EmailID, token);
                        Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                        var endPoint = await this.bus.GetSendEndpoint(uri);
                        await endPoint.Send(userTicket);
                        return Ok(new { sucess = true, message = "Email Sent Successfully" });
                    }
                    else
                    {
                        return BadRequest(new { success = false, message = "EmailID not Registered" });
                    }
                }
                else
                {
                    return BadRequest(new { success = false, message = "Something went Wrong" });
                }
            }
            catch (EmployeePayrollException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }
    }
}
