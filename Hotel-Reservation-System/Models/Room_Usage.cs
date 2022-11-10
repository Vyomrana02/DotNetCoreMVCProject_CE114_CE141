using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hotel_Reservation_System.Models
{
    public class Room_Usage
    {
        public int Id { get; set; }
        [Required]
        public int Room_Id { get; set; }
        [Required]
        public string Guest_Id { get; set; }
        [Required]
        public double Hours { get; set; }
        [Required]
        public DateTime Checkin { get; set; }
        public DateTime Date { get; set; }
        //public virtual Room Room => new ApplicationDbContext().Rooms.Find(Room_Id);
        private List<Room> getAllRooms()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<Room> allrooms = db.Rooms.ToList();
            return allrooms;
        }
        [NotMapped]
        public List<Room> Room => getAllRooms();
        [NotMapped]
        public virtual ApplicationUser Guest => new ApplicationDbContext().Users.Find(Guest_Id);
        [NotMapped]
        public string[] Types => new string[] { "Non-AC Room", "AC Room", "Delux Room", "Super Delux Room" };
        [NotMapped]
        public List<ApplicationUser> Guests => getGuestUsers();

        private List<ApplicationUser> getGuestUsers()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            List<ApplicationUser> users = db.Users.ToList();
            List<ApplicationUser> newUsers = new List<ApplicationUser>();

            var role = db.Roles.Where(m => m.Name == "Guest").FirstOrDefault();

            foreach (var item in users)
            {
                if (item.Roles.FirstOrDefault().RoleId == role.Id)
                {
                    newUsers.Add(item);
                }
            }
            return newUsers;

        }
        [NotMapped]
        public DateTime CheckOut => Checkin.AddHours(Hours);
    }
}