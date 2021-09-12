using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace HotalManagmentSystem.Models
{
    /// <summary>
    ///  this class represent   registartion employee only
    /// </summary>
 
   
    public class Admin
    {
        // defult  for int =>DatabaseGenerated(DatabaseGeneratedOption.Identity) 
        //Required not  allow nullable
        //ForeignKey("nue of migration")
        //MaxLength(100)
        //MinLength(8)
        //Column(TypeName="data")  type in  sql server
        // enum ans strudt not allow null by default but refrance type is allow
        //  [InverseProperty("RoomCategorys")] is there are more than relation
        //DataType(DataType.Password), ErrorMessage =""]
        //RegularExpression(@"[a-zA-z],{8}")
        //rande(20,70)
        [Key,Required]
        public int Admin_ID { get; set; }
        [Required, MaxLength(100), DataType(DataType.EmailAddress)]
        public string AdminEmail { get; set; }
        [Required,DataType(DataType.Password)]

        public string AdminPassward { get; set; }

        /// <summary>
        ///  space for forign key and relationship
        /// </summary>       
         [ForeignKey("Employee")]
        public int emb_ID { get; set; }
        public Employee Employee { get; set; }

    }
}