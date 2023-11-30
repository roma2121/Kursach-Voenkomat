using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class Role
    {
        [Key]
        public int Role_id { get; set; }
        public string Name { get; set; }
    }
}
