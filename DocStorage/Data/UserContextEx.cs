using System.Data.Entity;
using System.Linq;
using DocStorage.Data.DataModels;
using System.Security.Cryptography;
using System.Text;

namespace DocStorage.Data
{
    public class UserContextEx : DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserContextEx()
        {
            if (!Database.Exists())
                Database.Create();
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContextEx>());
        }

        public void CreateUser(string login, string name, string password, out string responseMessage)
        {
            User user = GetNewUser(login, name, password);
            if (user == null)
            {
                responseMessage = "Такой логин уже есть";
            }
            else
            {
                if (SaveUser(user))
                    responseMessage = "Регистрация прошла успешно";
                else
                    responseMessage = "Не удалось зарегистрироваться";
            }
        }

        private User GetNewUser(string login, string name, string password)
        {
            if (Users != null)
            {
                if (Users.Where(u => u.login.Equals(login) || u.name.Equals(name)).Any())
                    return null;
            }
            var newUser = Users.Create();
            newUser.login = login;
            newUser.name = name;
            newUser.password = CalculateMD5Hash(password);
            return newUser;
        }

        public User GetUser(string login)
        {
            var users = Users.Where(u => u.login.Equals(login));
            if (users != null && users.Count() != 0)
            {
                return Users.Where(u => u.login.Equals(login)).First();
            }
            return null;
        }
        
        private bool SaveUser(User user)
        {
            if (user != null)
                Users.Add(user);
            else
                return false;
            try
            {
                SaveChanges();
                return true;
            }
            catch
            {
                return false;
            }
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