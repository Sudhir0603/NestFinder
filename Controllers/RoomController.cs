using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NestFinder.Models;

public class RoomController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    // GET: Add Room Page
    public ActionResult AddRoom(int? propertyId)
    {
        if (propertyId == null)
        {
            return RedirectToAction("Index", "Property"); // Redirect if no property ID
        }

        ViewBag.PropertyId = propertyId;
        return View();
    }


    // POST: Save Rooms
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult AddRoom(List<Room> Rooms)
    {
        if (Rooms == null || Rooms.Count == 0)
        {
            return RedirectToAction("Index", "Property");
        }

        foreach (var room in Rooms)
        {
            db.Rooms.Add(room);
        }

        db.SaveChanges();
        return RedirectToAction("Details", "Property", new { id = Rooms.First().PropertyId });
    }
}
