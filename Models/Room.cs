using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotalManagmentSystem.Models
{
    /// <summary>
    ///  this class represent room  and status  only
    /// </summary>
    public class Room
    {
        [Key]
        public int RoomID { get; set; }// like 404 ,33,43...
        [Required]
        public bool? IsReseved { get; set; } //true or flse


        /// <summary>
        ///  space for forign key and relationship
        /// </summary>
        
         [Required,ForeignKey("RoomCategorys")]
        public int FK_CategoryID { get; set; }
        public virtual RoomCategory RoomCategorys { get; set; }
        /// //////////////////////////////////
      
      
        public ICollection<RoomAndUser>RoomAndUsers { get; set; }

        public ICollection<RoomAndEmployee> RoomAndEmployees { get; set; }
    }
}