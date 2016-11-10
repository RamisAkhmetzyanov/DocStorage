using System.ComponentModel.DataAnnotations;

namespace DocStorage.Data.DataModels
{
    public class User
    {
        public int Id { get; private set; }
        
        [Required]
        public string login { get; set; }

        [Required]
        public string name { get; set; }

        [Required]
        public string password { get; set; }


    }
}
