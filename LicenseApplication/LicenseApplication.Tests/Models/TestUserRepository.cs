using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LicenseApplication.Models;
using LicenseApplication.Tests.Controllers;
using LicenseApplication.Controllers;

namespace LicenseApplication.Tests.Models
{
    public class TestUserRepository : IUserRepository
    {
        public static List<User> _db = new List<User>();
        public Exception ExceptionToThrow { get; set; }

        public TestUserRepository()
        {
            User user = new User() { Id = 0, First_Name = "f_name", Last_Name = "l_name", EmailID = "user@gmail.com", Password = "user", Last_Login = DateTime.Now, Joined_Date = DateTime.Now };
            user = new User() { Id = 1, First_Name = "f_name1", Last_Name = "l_name1", EmailID = "user1@gmail.com", Password = "user1", Last_Login = DateTime.Now, Joined_Date = DateTime.Now };
            _db.Add(user);
        }
        
        public IEnumerable<User> GetAllTestUsers()
        {
            return _db.ToList();
        }

        public bool RegisterUser(User userToRegister)
        {
            bool status = false;
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;

            _db.Add(userToRegister);
            if (SaveChanges())
                status = true;

            return status;
        }
                              
        public bool SaveChanges()
        {
            return true;
        }

        public bool Login(User user)
        {
            User us = _db.SingleOrDefault(m => m.EmailID == user.EmailID);
            if (us != null)
            {
                if (us.Password != user.Password)
                {
                    UserRepository.count++;
                }

            }
            return false;
        }

        public int IsUserExist(User userToCheck)
        {
            return 1;
        }

        public User GetUserDetails(int userId)
        {
            User user = _db.FirstOrDefault(u => u.Id == userId);
            return user;
        }        

        public void EditUser(User userToEdit)
        {
              
            int id = userToEdit.Id;
            User userToUpadate = _db.SingleOrDefault(b => b.Id == id);
            _db.Remove(userToUpadate);
            _db.Add(userToEdit);

       
        }

        public bool DeleteUser(User userToDelete)
        {
            bool status = false;
            TestUserController controller = new TestUserController();
            _db.Remove(userToDelete);
            if(SaveChanges())
                status = true;

            return status;
        }

        public bool UpdateLastLogin(User userToUpdate)
        {
            if (userToUpdate.Last_Login == DateTime.Now.Date)
                return true;
            return false;
        }

        public bool UpdatePassword(User user, string newPassword)
        {
            return true;
        }
        
        public string Encode(string password)
        {
            UserRepository repo = new UserRepository();
            return repo.Encode(password);            
        }
    }
}
