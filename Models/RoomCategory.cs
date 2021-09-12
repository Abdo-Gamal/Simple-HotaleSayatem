using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotalManagmentSystem.Models
{
    /// <summary>
    ///  this class represent info about room 
    ///     there discraption"like free food and eleichtric divice " 
    ///  and there prices and number of person in it 
    ///  
    /// </summary>
    public class RoomCategory
    {
        [Key,Required]
        public int CategoryID { get; set; }//just  number for culomn
        [Required] 
        public string CategoryNumberOfPerson { get; set; }//single  or double or third or....
        [Required]
        public string CategoryDescription { get; set; }//service and food ..
        [Required]
        public double CategoryPrice { get; set; }//price of this Category
        [Required]
        public int RoomTypeCount { get; set; }
        [Required]
        public int UnreservedRoomCount { get; set; }
        [Required]
        public string CategoryImage { get; set; }
        /// <summary>
        ///  space for forign key and relationship
        /// </summary>
        public virtual ICollection<Room> Rooms { get; set; }
    }
}