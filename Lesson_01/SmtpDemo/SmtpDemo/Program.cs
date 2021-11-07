using System;
using System.IO;
using System.Net;
using System.Net.Mail;

namespace SmtpDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            const string FROM_EMAIL = "vvvdp2021@gmail.com";
            const string PASSWORD = "123456q!";
            const string DISPLAY_NAME = "The best service!";

            string toEmail = FROM_EMAIL;

            MailAddress from = new MailAddress(FROM_EMAIL, DISPLAY_NAME);
            MailAddress to = new MailAddress(toEmail);

            MailMessage mailMessage = new MailMessage(from, to)
            {
                Subject = "Some subject...",
                Body = "<h1>Hello world!</h1>",
                IsBodyHtml = true,
            };

            string path = Path.Combine(Directory.GetCurrentDirectory(), "gitlink.txt");

            mailMessage.Attachments.Add(new Attachment(path));

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(FROM_EMAIL, PASSWORD),
                EnableSsl = true,
            };

            smtp.Send(mailMessage);

            Console.WriteLine("Hello World!");
        }
    }
}
