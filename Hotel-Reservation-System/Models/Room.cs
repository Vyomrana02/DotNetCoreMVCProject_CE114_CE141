using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Hotel_Reservation_System.Models
{
    public class Room
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Location { get; set; }
        [Required]
        public string Type { get; set; }
        public bool Isactive { get; set; }
        public DateTime Date { get; set; }
        [NotMapped]
        public string[] Types => new string[] { "Non-AC Room","AC Room","Delux Room","Super Delux Room"};

        [NotMapped]
        public virtual List<Room> Rooms => new ApplicationDbContext().Rooms.ToList();
        [NotMapped]
        public virtual List<Room_Usage> Usages => new ApplicationDbContext().Room_Usage.ToList().Where(x => x.Room_Id == Id).ToList();
    }
}