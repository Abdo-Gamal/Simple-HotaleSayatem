using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotalSystem.AdminServices
{


    public interface IAdminService
    {
        bool login(string Email,String password);
        bool changePassword(string Email, String password);
        bool forgetPassword(string Email);
    }
    public class AdminService : IAdminService
    {
        public Model Context { get; set; }
        public AdminService()
        {
            Context = new Model();
        }
        public bool login(string Email, string password)
        {
           
            return Context.Admins.Where(
                a => a.AdminEmail == Email && a.AdminPassward == password).Any();
            //var query = Context.Admins.ToList();
            //foreach(var it in query)
            //{
            //    if (it.AdminEmail == Email && it.AdminPassward == password)
            //        return true;
            //}
            //return false;
            // It should return TRUE when this above statement matches all these conditions

        }

        public bool changePassword(string Email, string password)
        {
            throw new NotImplementedException();
        }

        public bool forgetPassword(string Email)
        {
            throw new NotImplementedException();
        }

      
    }
}