using Airbnb.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Airbnb.APIs.Controllers
{
    public class SendController : APIBaseController
    {
        private readonly MailService mailService;

        public SendController(MailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost]
        public async Task<ActionResult> SendEmail(string mailTo, string subject, string body, IList<IFormFile>? attachedFiles = null)
        {
            await mailService.SendEmailAsync(mailTo, subject, body, attachedFiles);
            return Ok();
        }
    }
}
