using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotalManagmentSystem.Models
{
    /// <summary>
    /// this class discraption info about  who is the  emp  work in this room and time of work
    /// </summary>
    public class RoomAndEmployee
    {

        [Key]//none
        public int RoomAndEmployeeId { get; set; }// just number reprsent the column
        [Required,Column(TypeName ="date")]
        public DateTime dateOfDay { get; set; }//yyyy/mm//dd not take time "hours"
        //[Required]
        //public double StartHoureOfWorkInRoom { get; set; }// the time that the emp enter the room
        //[Required]
        //public Double NumberOfHourTakeInRoom{ get; set; }// number of  take in  room

        /// <summary>
        ///  space for forign key and relationship
        /// </summary>
        [Required, ForeignKey("Employee")]
        public int FK_EmployeeId { get; set; }//just number
        public virtual Employee Employee { get; set; }

        // ////////////////////////////
        [Required, ForeignKey("Room")]
        public int FK_RoomId { get; set; }
        public virtual Room Room { get; set; }
    }
}