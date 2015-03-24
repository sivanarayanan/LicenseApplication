namespace LicenseApplication.Models
{
    public interface IUserRepository
    {
        bool RegisterUser(User userToRegister);
        bool SaveChanges();
        bool Login(User userToLogin);
        int IsUserExist(User userToCheck);
        User GetUserDetails(int userID);
        bool UpdateLastLogin(User userToUpdate);
        bool DeleteUser(User userToDelete);
        bool UpdatePassword(User user, string newPassword);
        string Encode(string password);
        void EditUser(User user);

    }
}
