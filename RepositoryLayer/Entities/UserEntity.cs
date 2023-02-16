
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepositoryLayer.Entities
{
    public class UserEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        public string Fullname { get; set; }
        public string EmailID { get; set; }
        public string Password { get; set; }
        public long MobileNumber { get; set; }
    }
}
