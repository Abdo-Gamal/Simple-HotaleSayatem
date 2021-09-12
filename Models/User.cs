using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotalManagmentSystem.Models
{
    /// <summary>
    ///  this class represent info about customer info 
    ///  name ,UserBarthDate"age" ,national id
    ///  national id,email,password
    /// </summary>
    public class User
    {

        
        [Key,Required,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required, MinLength(14)]
        public string UserNationalID { get; set; }//national id
        [Required, MaxLength(30)]
        public string UserName{ get; set; }
        [Required, MaxLength(30)]
        public string UserPhone { get; set; }
      
        [Required, Column(TypeName = "date")]
        public DateTime UserBarthDate { get; set; }// yyyy/mm/dd  not take hours
        [Required, DataType(DataType.Password)]
        public string UserPassword { get; set; }
        //user 
        /// <summary>
        ///  space for forign key and relationship
        /// </summary>

        public virtual ICollection<RoomAndUser> RoomAndUsers { get; set; }

    }
}