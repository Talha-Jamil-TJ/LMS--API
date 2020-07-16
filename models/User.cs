using System.ComponentModel.DataAnnotations;

namespace ShopManagement.models
{
    public class User
    {
        public int Id { get; set; }

        [Required] [MaxLength(255)] public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        public byte[] PasswordSalt { get; set; }
    }
}