using DocStorage.Data.DataModels;
using System.Security.Cryptography;
using System.Text;

namespace DocStorage.Service
{
    public class Authentication
    {
        private static User currentUser { get; set; }
        
        public static User GetCurrentUser()
        {
            return currentUser;
        }

        public static bool Authenticate(string login, string password, out string responseMessage)
        {
            User user = Extensions.ExtensionFactory.UserContext.GetUser(login);
            if (user == null)
                responseMessage = "Неверный логин";
            else
            {
                if (ValidateUser(user, password))
                {
                    currentUser = user;
                    responseMessage = "";
                    return true;
                }
                else
                    responseMessage = "Неверный пароль";
            }
            return false;
        }

        public static void SignOut()
        {
            currentUser = null;
        }

        private static bool ValidateUser(User user, string password)
        {
            if (user != null) 
                return user.password.Equals(CalculateMD5Hash(password));
            return false;
        }

        private static string CalculateMD5Hash(string input)
        {
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString());
            }
            return sb.ToString();
        }
    }
}
