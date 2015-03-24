using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.Entity;

namespace LicenseApplication.Models
{
    public class UserRepository : IUserRepository
    {
        private LicenseEntities db = new LicenseEntities();

        public static int count = 0;

        public bool RegisterUser(User userToRegister)
        {
            bool status = false;
            User user = userToRegister;
            user.Password = Encode(userToRegister.Password);
            db.Users.Add(user);
            if (SaveChanges())
                status = true;

            return status;
        }

        public void EditUser(User userToEdit)
        {
            db.Entry(userToEdit).State = EntityState.Modified;
            db.SaveChanges();
        }

        public bool SaveChanges()
        {
            bool status = false;
            if (db.SaveChanges() == 1)
                status = true;

            return status;
        }

        public int IsUserExist(User userToCheck)
        {
            User user = db.Users.SingleOrDefault(u => u.EmailID == userToCheck.EmailID);
            int id = 0;

            if (user != null)
            {
                id = (from j in db.Users.Where(m => m.EmailID == userToCheck.EmailID)
                      select j.Id).SingleOrDefault<System.Int32>();
                return id;
            }
            return id;
        }
        
        public bool Login(User userToLogin)
        {
            return true;
        }
        
        public User GetUserDetails(int userId)
        {
            User user = db.Users.SingleOrDefault(u => u.Id == userId);

            return user;
        }

        public bool UpdateLastLogin(User userToUpdate)
        {
            bool status = false;
            User user = db.Users.Where(u => u.Id == userToUpdate.Id).SingleOrDefault<User>();            
            if (user != null)
            {
                user.Last_Login = DateTime.Now;
                db.Entry(user).State = EntityState.Modified;
                if (db.SaveChanges() == 1)
                    status = true;
            }                      
            return status;
        }

        public bool UpdatePassword(User userToUpdate, string newPassword)
        {
            bool status = false;
            User user = db.Users.Where(u => u.Id == userToUpdate.Id).SingleOrDefault<User>();
            if (user != null)
            {
                user.Password = newPassword;
                db.Entry(user).State = EntityState.Modified;
                if (db.SaveChanges() == 1)
                    status = true;
            }         
            return status;
        }

        public bool DeleteUser(User userToDelete)
        {
            bool status = false;
            db.Users.Remove(userToDelete);
            if (SaveChanges())
                status = true;

            return status;               
        }

        public string Encode(string value)
        {
            var hash = System.Security.Cryptography.SHA1.Create();
            var encoder = new System.Text.ASCIIEncoding();
            var combined = encoder.GetBytes(value ?? "");
            string EncodedString = BitConverter.ToString(hash.ComputeHash(combined)).ToLower().Replace("-", "");
            return EncodedString;
        }
    }
}