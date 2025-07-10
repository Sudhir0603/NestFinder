using System;
using System.Linq;
using System.Web.Mvc;
using NestFinder.Models;
using System.Data.Entity;
using System.Net.Mail;
using System.Net;

namespace NestFinder.Controllers
{
    public class ContactController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ✅ Show Contact Form
        public ActionResult ContactForm()
        {
            return View();
        }

        // ✅ Handle Contact Form Submission
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactForm(Contact model)
        {
            if (ModelState.IsValid)
            {
                db.Contacts.Add(model);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Your query has been submitted successfully!";
                return RedirectToAction("ContactForm");
            }
            return View(model);
        }

        // ✅ Admin: View All Queries
        [Authorize(Roles = "Admin")]
        public ActionResult ViewQueries()
        {
            var queries = db.Contacts.OrderByDescending(q => q.SentDate).ToList();
            return View(queries);
        }

        // ✅ Admin: Delete Query
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteQuery(int id)
        {
            var query = db.Contacts.Find(id);
            if (query != null)
            {
                db.Contacts.Remove(query);
                db.SaveChanges();
            }
            return RedirectToAction("ViewQueries");
        }
        // Send Email to User
        [HttpPost]
        public ActionResult SendEmail(int id, string subject, string message)
        {
            var query = db.Contacts.Find(id);
            if (query == null)
            {
                return HttpNotFound();
            }

            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("kamblesudhir2003@gmail.com");
                mail.To.Add(query.Email);
                mail.Subject = subject;
                mail.Body = message;
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
                smtp.Credentials = new NetworkCredential("kamblesudhir2003@gmail.com", "SudhirHa@123");
                smtp.EnableSsl = true;
                smtp.Send(mail);

                TempData["SuccessMessage"] = "Email sent successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to send email: " + ex.Message;
            }

            return RedirectToAction("ViewQueries");
        }
    }
}
