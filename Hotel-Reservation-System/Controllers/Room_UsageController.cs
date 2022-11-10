using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Reservation_System.Models;

namespace Hotel_Reservation_System.Controllers
{
    [Authorize]
    public class Room_UsageController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Room_Usage
        public async Task<ActionResult> Index()
        {
            return View(await db.Room_Usage.ToListAsync());
        }

        [Authorize(Roles = "Administrator")]
        // GET: Room_Usage/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Usage room_Usage = await db.Room_Usage.FindAsync(id);
            if (room_Usage == null)
            {
                return HttpNotFound();
            }
            return View(room_Usage);
        }

        // GET: Room_Usage/Create
        public ActionResult Create()
        {
            return View(new Room_Usage());
        }

        // POST: Room_Usage/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Room_Id,Guest_Id,Hours,Checkin")] Room_Usage room_Usage)
        {
            if (room_Usage != null) {
                room_Usage.Date = DateTime.Now;
                room_Usage.Checkin = room_Usage.Checkin;
            }
            if (ModelState.IsValid)
            {
                List<Room_Usage> usages = db.Room_Usage.ToList().Where(x => x.Room_Id == room_Usage.Room_Id).ToList();
                foreach (var item in usages) {
                    if ( (item.Checkin >= room_Usage.Checkin && item.Checkin <= room_Usage.CheckOut) || 
                        item.Checkin.AddHours(room_Usage.Hours) >= room_Usage.Checkin) {
                        ModelState.AddModelError("", "Room is not Available!");
                        return View(room_Usage);
                    }
                }

                db.Room_Usage.Add(room_Usage);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(room_Usage);
        }

        // GET: Room_Usage/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Usage room_Usage = await db.Room_Usage.FindAsync(id);
            if (room_Usage == null)
            {
                return HttpNotFound();
            }
            return View(room_Usage);
        }

        // POST: Room_Usage/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Room_Id,Guest_Id,Isactive,Date")] Room_Usage room_Usage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room_Usage).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(room_Usage);
        }


        // POST: Room_Usage/Rooms/5
        
        [HttpPost]
        public JsonResult Rooms(string Type)
        {
            if (!string.IsNullOrEmpty(Type))
            {
                List<Room> rooms = db.Rooms.ToList().Where(x=>x.Type == Type).ToList();
                return Json(new {Rooms =  rooms });
            }
            return null;
        }
        // GET: Room_Usage/Delete/5

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room_Usage room_Usage = await db.Room_Usage.FindAsync(id);
            if (room_Usage == null)
            {
                return HttpNotFound();
            }
            return View(room_Usage);
        }

        // POST: Room_Usage/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Room_Usage room_Usage = await db.Room_Usage.FindAsync(id);
            db.Room_Usage.Remove(room_Usage);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
