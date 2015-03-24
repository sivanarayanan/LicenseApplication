using LicenseApplication.Models;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;

namespace LicenseApplication.Controllers
{
    public class UserController : Controller
    {
        LicenseEntities db = new LicenseEntities();

        IUserRepository userRepository;

        public UserController() 
            : this(new UserRepository()) 
        { }

        public UserController(IUserRepository repository)
        {
            userRepository = repository;
        }


        //
        // GET : /User/Register
        public ActionResult Register()
        {
            return View("Register");
        }

        //
        // POST : /User/Register
        [HttpPost]
        public ActionResult Register([Bind(Exclude = "Id")]User userToRegister)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userToRegister.Joined_Date = DateTime.Now;
                    userToRegister.Last_Login = DateTime.Now;
                    if (userRepository.RegisterUser(userToRegister))
                    {
                        return RedirectToAction("Login");
                    }
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                ViewData["RegisterError"] = "Registration Failed. Check Your Details";
            }
            return View("Register");
        }

        //
        // GET : /User/Login
        public ActionResult Login()
        {
            return View("Login");
        }

        //
        // POST : /User/Login
        [HttpPost]
        public ActionResult Login(User userToLogin)
        {
            if (userRepository.Login(userToLogin))
            {
                try
                {
                    User user = db.Users.SingleOrDefault(u => u.EmailID == userToLogin.EmailID);
                    if (user != null)
                    {
                        if (user.Password == userRepository.Encode(userToLogin.Password))
                        {
                            FormsAuthentication.SetAuthCookie(userToLogin.EmailID, false);
                            Session["UserId"] = user.Id;
                            Session["UserName"] = user.Display_Name;
                            if (userRepository.UpdateLastLogin(user))
                                return RedirectToAction("Index", "License");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Email or Password");
                        }
                    }
                    throw new ArgumentException();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    ViewData["LoginError"] = "Login Failed. Check Your Details";
                }
            }
            return View("Login");
        }

        //
        //GET: /Admin/Logout/
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "User");
        } 

        //
        // GET : /User/Profile/<int id>
        public ActionResult Details(int id)
        {
            User user = userRepository.GetUserDetails(id);
            return View("Details", user);
        }

        //
        // GET : /User/Edit/<int id>
        public ActionResult Edit(int id)
        {
            User userToEdit = userRepository.GetUserDetails(id);
            return View("Edit", userToEdit);
        }

        ////
        //// POST : /User/Edit/<int id>
        [HttpPost]
        public ActionResult Edit(User userToEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    userRepository.EditUser(userToEdit);
                    db.SaveChanges();
                    return RedirectToAction("Details/" + userToEdit.Id);
                }
                throw new ArgumentException();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }
        //
        // GET : /User/Delete/<int id>
        public ActionResult Delete(int id)
        {
            User userToDelete = userRepository.GetUserDetails(id);
            return View("Delete", userToDelete);
        }

        //
        // POST : /User/Detele/<int id>
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult PostDelete(int id)
        {
            User userToDelete = userRepository.GetUserDetails(id);
            try
            {
                if (userRepository.DeleteUser(userToDelete))
                {
                    Logout();
                    return RedirectToAction("Login");
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        //
        // GET : /User/Change_Password/<int id>
        public ActionResult ChangePassword(int id)
        {
            User user = userRepository.GetUserDetails(id);
            return View("ChangePassword", user);
        }

        //
        // POST : /User/Change_Password/<int id>
        [HttpPost]
        public ActionResult ChangePassword(int id, FormCollection collection)
        {            
            string oldPassword = Request["Old_Password"].ToString();
            string newPassword = Request["New_Password"].ToString();
            string confirmPassword = Request["Con_Password"].ToString();

            User user = userRepository.GetUserDetails(id);
            if (user != null)
            {
                if (user.Password == (userRepository.Encode(oldPassword)))
                {
                    if (newPassword == confirmPassword)
                    {
                        if (userRepository.UpdatePassword(user, (userRepository.Encode(newPassword))))
                            return RedirectToAction("Details/" + user.Id);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Mismatch Passwords");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Incorrect Password");
                }
            }
            return View();
        }
    }
}
