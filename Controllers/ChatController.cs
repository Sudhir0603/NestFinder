using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NestFinder.Models;

namespace NestFinder.Controllers
{
    [Authorize]
    public class ChatController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ✅ Load chat page
        public ActionResult Index(string receiverId)
        {
            string userId = User.Identity.GetUserId();
            var receiver = db.Users.Find(receiverId);

            if (receiver == null || userId == receiverId)
            {
                return RedirectToAction("Index", "Home"); // Prevent self-chat
            }

            ViewBag.CurrentUserId = userId;
            ViewBag.Receiver = receiver;
            return View();
        }

        // ✅ GET: Retrieve Messages
        public JsonResult GetMessages(string receiverId)
        {
            string userId = User.Identity.GetUserId();

            var messages = db.Messages
                .Where(m => (m.SenderId == userId && m.ReceiverId == receiverId) ||
                            (m.SenderId == receiverId && m.ReceiverId == userId))
                .OrderBy(m => m.Timestamp)
                .Select(m => new
                {
                    m.SenderId,
                    m.Content,
                    m.Timestamp
                })
                .ToList();

            return Json(messages, JsonRequestBehavior.AllowGet);
        }

        // ✅ POST: Send Message
        [HttpPost]
        public JsonResult SendMessage(string receiverId, string content)
        {
            string userId = User.Identity.GetUserId();

            if (string.IsNullOrEmpty(content))
            {
                return Json(new { success = false, message = "Message cannot be empty!" });
            }

            var message = new Message
            {
                SenderId = userId,
                ReceiverId = receiverId,
                Content = content,
                Timestamp = DateTime.Now
            };

            db.Messages.Add(message);
            db.SaveChanges();

            return Json(new { success = true, message = "Message sent!" });
        }
    }
}
