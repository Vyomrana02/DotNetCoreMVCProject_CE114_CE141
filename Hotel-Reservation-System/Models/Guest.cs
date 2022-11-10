using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hotel_Reservation_System.Models
{
    public class Guest
    {
        public int Id { get; set; }
        public string Names { get; set; }
        public string Phone { get; set; }
        public string Address{ get; set; }
        public bool Isactive { get; set; }
        public DateTime Date { get; set; }

        //[NotMapped]
        //public virtual List<Guest> Guests => new ApplicationDbContext().Guests.ToList();
        public virtual List<Room_Usage> Usages => new ApplicationDbContext().Room_Usage.ToList().Where(x => x.Guest_Id == Id).ToList();
    }
}