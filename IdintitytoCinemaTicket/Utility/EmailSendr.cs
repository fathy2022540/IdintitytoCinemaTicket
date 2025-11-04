using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace IdintitytoCinemaTicket.Utility
{
    public class EmailSendr : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var claint = new SmtpClient("smtp.gmail.com",
                587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("fathyosama1020@gmail.com",
                "ojvm xipi qhtj jldq")
            };


            return claint.SendMailAsync(new
                MailMessage(from: "fathyosama1020@gmail.com",
                to: email,
                subject: subject,
                htmlMessage)

            {
                IsBodyHtml = true
            });



        }
    }
}
