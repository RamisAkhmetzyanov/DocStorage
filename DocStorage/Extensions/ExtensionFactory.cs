using DocStorage.Data;

namespace DocStorage.Extensions
{
    public static class ExtensionFactory
    {
        private static DbContextEx dataContext;
        private static UserContextEx userContext;

        public static DbContextEx DataContext
        {
            get { return dataContext ?? (dataContext = new DbContextEx()); }
        }

        public static UserContextEx UserContext
        {
            get { return userContext ?? (userContext = new UserContextEx()); }
        }
    }
}