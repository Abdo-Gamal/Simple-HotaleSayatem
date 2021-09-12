using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotalManagmentSystem.Models
{
    /// <summary>
    ///  this class represent info about room and  customer who want reserved this room 
    ///  ans number of day and bill
    /// </summary>
    public class RoomAndUser
    {
       
        [Key]
        public int RoomAndUserId { get; set; }//just numer of column
        [Required, Column(TypeName = "date")]
        public DateTime reservationDate { get; set; }// yyyy/mm/dd not take hours
    
        [Required]
        public int NumberOfDays { get; set; } //1,33,3,455....
      
        /// <summary>
        ///  space for forign key and relationship
        /// </summary>
        [Required,ForeignKey("Room")]
        public int FK_roomId { get; set; }
        public virtual Room Room { get; set; }
        // // //////////
        [Required, ForeignKey("User")]
        public int FK_UserID { get; set; }
        public virtual User User { get; set; }

    }
}