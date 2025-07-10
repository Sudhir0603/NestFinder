using System;
using System.Linq;
using System.Web.Mvc;
using NestFinder.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity.Core.Metadata.Edm;

namespace NestFinder.Controllers
{
    public class CommentController : Controller
    {
       
        private ApplicationDbContext db = new ApplicationDbContext();

        private const int PageSize = 5; // ✅ Load 5 comments per request

        // ✅ Submit a new comment
        [HttpPost]
        [ValidateAntiForgeryToken] // ✅ Prevent CSRF attacks
        public JsonResult SubmitComment(int propertyId, string content)
        {
            var userId = User.Identity.GetUserId();
            if (userId == null)
                return Json(new { success = false, message = "⚠ You must be logged in to comment!" });

            if (string.IsNullOrWhiteSpace(content))
                return Json(new { success = false, message = "⚠ Comment cannot be empty!" });

            var comment = new Comment
            {
                PropertyId = propertyId,
                UserId = userId,
                Content = content,
                CreatedAt = DateTime.UtcNow
            };

            db.Comments.Add(comment);
            db.SaveChanges();

            return Json(new { success = true });
        }

        [HttpGet]
        public JsonResult GetComments(int propertyId, int page = 1)
        {
            try
            {
                var totalComments = db.Comments.Count(c => c.PropertyId == propertyId); // ✅ Get total comments count
                var userId = User.Identity.GetUserId();

                var comments = db.Comments
                    .Where(c => c.PropertyId == propertyId)
                    .OrderByDescending(c => c.CreatedAt)
                    .Skip((page - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();

                var commentList = comments.Select(c => new
                {
                    id = c.Id,
                    user = db.Users.Find(c.UserId)?.UserName ?? "Unknown User",
                    content = c.Content,
                    date = c.CreatedAt.ToString("MMM dd, yyyy"),
                    canDelete = c.UserId == userId
                }).ToList();

                bool hasMore = (page * PageSize) < totalComments;

                return Json(new { comments = commentList, hasMore, totalComments }, JsonRequestBehavior.AllowGet); // ✅ Include total count
            }
            catch (Exception ex)
            {
                return Json(new { error = "⚠ Error fetching comments: " + ex.Message }, JsonRequestBehavior.AllowGet);
            }
        }


        // ✅ Delete a comment (Only if the logged-in user is the owner)
        [HttpPost]
        public JsonResult DeleteComment(int commentId)
        {
            var userId = User.Identity.GetUserId();
            var comment = db.Comments.FirstOrDefault(c => c.Id == commentId && c.UserId == userId);

            if (comment == null)
                return Json(new { success = false, message = "⚠ You can only delete your own comments!" });

            db.Comments.Remove(comment);
            db.SaveChanges();

            return Json(new { success = true });
        }
    }
}
