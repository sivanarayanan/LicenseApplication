using LicenseApplication.Controllers;
using LicenseApplication.Models;
using LicenseApplication.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace LicenseApplication.Tests.Controllers
{
    [TestClass]
    public class TestUserController
    {
        private static UserController GetUserController(IUserRepository userRepository)
        {
            UserController userController = new UserController(userRepository);
            userController.ControllerContext = new ControllerContext()
            {
                Controller=userController,
                RequestContext=new RequestContext(new MockHttpContext(), new RouteData())
            };
            return userController;
        }

        public User GetTestUser()
        {
            User testUser = new User();
            testUser.Id = 1;
            testUser.EmailID = "test@mail.com";
            testUser.Password = "test@123";
            testUser.First_Name = "test";
            testUser.Last_Name = "user";
            testUser.Display_Name = "Test User";
            testUser.Joined_Date = new DateTime(2015, 10, 10, 12, 00, 00);
            testUser.Last_Login = new DateTime(2015, 10, 10, 12, 00, 00);
            return testUser;
        }

        [TestMethod]
        public void GetRegisterTest()
        {
            var controller = GetUserController(new TestUserRepository());
            ViewResult result = controller.Register() as ViewResult;
            Assert.AreEqual("Register", result.ViewName);
        }

        [TestMethod]
        public void PostRegisterTest()
        {
            TestUserRepository repository = new TestUserRepository();
            UserController controller = new UserController(repository);
            User user = GetTestUser();
            controller.Register(user);
            IEnumerable<User> users = repository.GetAllTestUsers();
            Assert.IsTrue(users.Contains(user));
        }

        [TestMethod]
        public void GetLoginTest()
        {
            var controller = GetUserController(new TestUserRepository());
            ViewResult result = controller.Login() as ViewResult;
            Assert.AreEqual("Login", result.ViewName);
        }

        [TestMethod]
        public void PostLoginTest()
        {
            TestUserRepository testRepository = new TestUserRepository();
            UserController usercontroller = GetUserController(testRepository);
            User user = new User() { First_Name = "f_name", Last_Name = "l_name", EmailID = "user@gmail.com", Password = "user", Last_Login = DateTime.Now, Joined_Date = DateTime.Now };
            usercontroller.Login(user);
            int actual = UserRepository.count;
            Assert.AreEqual(0, actual);
        }

        [TestMethod]
        public void GetDetailsTest()
        {
            TestUserRepository repository = new TestUserRepository();
            var controller = GetUserController(repository);
            User user = GetTestUser();
            ViewResult result = (ViewResult)controller.Details(user.Id);
            Assert.AreEqual("Details", result.ViewName);
        }

        [TestMethod]
        public void GetEditTest()
        {
            TestUserRepository repository = new TestUserRepository();
            var controller = GetUserController(repository);
            User user = GetTestUser();
            ViewResult result = (ViewResult)controller.Edit(user.Id);
            Assert.AreEqual("Edit", result.ViewName);
        }

        [TestMethod]
        public void PostEditTest()
        {
            User user = new User();
            TestUserRepository testRepository = new TestUserRepository();
            UserController usercontroller = GetUserController(testRepository);
            user = GetTestUser();
            usercontroller.Edit(user);
            IEnumerable<User> users = testRepository.GetAllTestUsers();
            Assert.IsTrue(users.Contains(user));
        }

        [TestMethod]
        public void GetDeleteTest()
        {
            TestUserRepository repository = new TestUserRepository();
            var controller = GetUserController(repository);
            User user = GetTestUser();
            controller.Register(user);
            ViewResult result = (ViewResult)controller.Delete(user.Id);
            Assert.AreEqual("Delete", result.ViewName);
        }

        [TestMethod]
        public void PostDeleteTest()
        {
            TestUserRepository testRepository = new TestUserRepository();
            UserController usercontroller = GetUserController(testRepository);
            int id = 1;
            User user = TestUserRepository._db.SingleOrDefault(b => b.Id == id);
            usercontroller.PostDelete(id);
            IEnumerable<User> users = testRepository.GetAllTestUsers();
            Assert.IsFalse(users.Contains(user));
        }

        [TestMethod]
        public void UpdateLastLoginTest()
        {
            TestUserRepository repository = new TestUserRepository();
            User user = GetTestUser();
            user.Last_Login = DateTime.Now.Date;
            bool result = repository.UpdateLastLogin(user);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void UpdatePasswordTest()
        {
        }        
    }
}