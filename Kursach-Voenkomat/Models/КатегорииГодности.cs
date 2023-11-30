using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class КатегорииГодности
    {
        [Key]
        public int ID_категории_годности { get; set; }
        [Display(Name = "Наименование категории")]
        public char Наименование_категории { get; set; }
    }

}
