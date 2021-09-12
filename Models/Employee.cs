using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;


namespace HotalManagmentSystem.Models
{

    /// <summary>
    ///  this class represent  employee like cleaner not  registartion employee
    /// </summary>
    
    public class Employee
    {
        [Key, Required]
        public int Employee_ID { get; set; }
        [ Required, MinLength(14)]
        public string EmployeeNationalID { get; set; }// represent national id

        [Required, RegularExpression(@"[a-zA-z]{3,}")]
        public string EmployeeName { get; set; }
        [Required,DataType(DataType.PhoneNumber)]
        public string EmployeePhone { get; set; }//one phone
        [Required]
        public double EmployeeSalary { get; set; }

        [Required,Column(TypeName="date")]
        public DateTime EmployeeBarthDate { get; set; }// yyyy/mm/dd  not take hours
        public string EmployeeImage { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        /// <summary>
        ///  space for forign key and relationship
        /// </summary>
        public  ICollection<RoomAndEmployee>RoomAndEmployees { get; set; }

  

    }
}