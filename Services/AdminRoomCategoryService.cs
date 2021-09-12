using HotalManagmentSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HotalSystem.Services
{
    public interface IAdminRoomCategoryService
    {
        List<RoomCategory> ReadAll();
        int Create(RoomCategory newCategory);
        RoomCategory ReadbyId(int id);
        int Eidet(RoomCategory data);
        bool  Delete(int id);
    }
    public class AdminRoomCategoryService : IAdminRoomCategoryService
    {
        private readonly Model context;
        public AdminRoomCategoryService()
        {
            context = new Model();
        }
      public  List<RoomCategory> ReadAll()
        {
            return context.RoomCategorys.ToList();
        }

       
        private int CatogryIsFound(RoomCategory newCategory)
        {


            var newNumberOfPerson = newCategory.CategoryNumberOfPerson;
            bool ISNumberOfPersonFound = context.RoomCategorys.Where(
                c => c.CategoryNumberOfPerson == newNumberOfPerson).Any();

            var newDiscraption = newCategory.CategoryDescription.ToLower();
            bool ISDiscraptionFound = context.RoomCategorys.Where(
                c => c.CategoryDescription.ToLower() == newDiscraption).Any();
            int ans = 0;

            if (ISDiscraptionFound == true) ans++;
            if (ISNumberOfPersonFound == true) ans++;
            return ans;
        }

        public int Create(RoomCategory newCategory)
        {
            int ans = CatogryIsFound(newCategory);
            if (ans == 2) return -2;
                newCategory.UnreservedRoomCount = newCategory.RoomTypeCount;
                context.RoomCategorys.Add(newCategory);
               // int 

                Addroom(newCategory.CategoryID, newCategory.RoomTypeCount);
                return  context.SaveChanges(); //return 0 if not save 1 if save 1 object
        }

        public RoomCategory ReadbyId(int id)
        {
           return context.RoomCategorys.Where(c=> c.CategoryID==id).FirstOrDefault();
        }
        private void Addroom(int cat, int x)
        {

            int sum = 0;
            for (int it = 0; it < x; it++)
            {
                Room room = new Room();
                room.IsReseved = false;
                room.FK_CategoryID = cat;
                context.Rooms.Add(room);
                 
            }
            context.SaveChanges();
        }
        public int Eidet(RoomCategory newCategory)
        {
            //int ans = CatogryIsFound(newCategory);
            //if (ans == 2) return -2;
             RoomCategory oldCategory  = context.RoomCategorys.Where(r => r.CategoryID == newCategory.CategoryID).FirstOrDefault();
            if (newCategory.RoomTypeCount == oldCategory.RoomTypeCount)
            {
               
                oldCategory.RoomTypeCount = newCategory.RoomTypeCount;
                oldCategory.UnreservedRoomCount = newCategory.UnreservedRoomCount;
                oldCategory.CategoryPrice = newCategory.CategoryPrice;
                oldCategory.CategoryDescription = newCategory.CategoryDescription;
               
                return context.SaveChanges(); //return 0 or 1 or 2 or 3
            }

           else if (oldCategory.RoomTypeCount < newCategory.RoomTypeCount)
            {
                oldCategory.UnreservedRoomCount += (newCategory.RoomTypeCount - oldCategory.RoomTypeCount);
                Addroom(newCategory.CategoryID, newCategory.RoomTypeCount - oldCategory.RoomTypeCount);
                
                oldCategory.RoomTypeCount = newCategory.RoomTypeCount;
                oldCategory.CategoryPrice = newCategory.CategoryPrice;
                oldCategory.CategoryDescription = newCategory.CategoryDescription;
               
               int x= context.SaveChanges();

                return x; //return 0 or 1 or 2 or 3
            }
            else
            {
                if ((oldCategory.RoomTypeCount-newCategory.RoomTypeCount) > oldCategory.UnreservedRoomCount)
                    return -1;
                
                    List<Room> rooms = context.Rooms.Where(r => r.FK_CategoryID == newCategory.CategoryID&&r.IsReseved==false).ToList();
                oldCategory.UnreservedRoomCount = newCategory.UnreservedRoomCount;
                

                int xx = oldCategory.RoomTypeCount - newCategory.RoomTypeCount;
                oldCategory.RoomTypeCount = newCategory.RoomTypeCount;
                oldCategory.CategoryPrice = newCategory.CategoryPrice;
                oldCategory.CategoryDescription = newCategory.CategoryDescription;
             
              
                foreach (Room it in rooms)
                 {      if (xx == 0) break;
                          xx--;
                        context.Rooms.Remove(it);
                      context.SaveChanges();
                }

               return context.SaveChanges(); //return 0 or 1 or 2 or 3
            }
       }

        public bool Delete(int id)
        {
           
            RoomCategory Category = ReadbyId(id);
            //if(Category.UnreservedRoomCount!= Category.RoomTypeCount)
            //    return false; //can not delete
            if (Category != null)
            {
                context.RoomCategorys.Remove(Category);
                return context.SaveChanges() > 0 ? true : false;
                
            }
            return false;
        }
    }
}