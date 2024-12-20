﻿using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace MVC.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Credentials = new NetworkCredential("rocinanterompante@gmail.com", "keya kegd ohfy fakn"),
                Port = 587,
                EnableSsl = true,
            };


            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress("rocinanterompante@gmail.com", "Seguro Saúde Municipal"),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);

            mailMessage.Bcc.Add("rocinanterompante@gmail.com");

            smtpClient.Send(mailMessage);

            return Task.CompletedTask;
        }
    }
}