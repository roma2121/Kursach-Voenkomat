using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class КатегорииПрофессиональнойПригодности
    {
        [Key]
        public int ID_категории_профессиональной_пригодности { get; set; }
        [Display(Name = "Категории профессиональной пригодности")]
        public string Категории_профессиональной_пригодности { get; set; }
    }
}
