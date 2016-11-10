using System.Data.Entity;
using DocStorage.Data.DataModels;

namespace DocStorage.Data
{
    public class DbContextEx : DbContext
    {
        public DbSet<Document> Documents { get; set; }

        public DbContextEx()
        {
            if (!Database.Exists())
                Database.Create();
            Database.SetInitializer(new DropCreateDatabaseIfModelChanges<DbContextEx>());
        }
        
        public bool SaveDocument(Document document)
        {
            if (document != null)
                Documents.Add(document);
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
        
    }

}