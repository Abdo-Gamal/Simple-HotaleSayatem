using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotalSystem.ViewModel
{
    public class BookingdataViewModel
    {
        public string Username { get; set; }
        public string UserNationalID { get; set; }
        public string UserPhone { get; set; }
        public DateTime UserBarthDate { get; set; }

        public DateTime reservationDate { get; set; }
        public int NumberOfRoom { get; set; }
        public int NumberOfDays { get; set; }
        //public double BillOfRoom { get; set; }
        public int CatagoryId { get; set; }
        public string UserPassword { get; set; }
    }
}
