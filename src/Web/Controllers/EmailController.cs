using System;
using Microsoft.AspNetCore.Mvc;
using Services;
using Web.ViewModels;

namespace Web.Controllers
{
    public class EmailController : Controller
    {
        private readonly IEmailSender _emailSender;

        public EmailController(IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        public IActionResult Index(string confirmationMessage = null)
        {
            return View(new EmailViewModel() { ConfirmationMessage = confirmationMessage});
        }

        public IActionResult SendEmail(EmailViewModel email)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", email);
            }

            try
            {
                _emailSender.SendEmail(email.Recipient, email.Message);
                return RedirectToAction("Index", new { ConfirmationMessage = "Your email was sent" });

            }
            catch (Exception)
            {
                return RedirectToAction("Index", new { ConfirmationMessage = "We had some problems with sending the email"});
            }
        }
    }
}
