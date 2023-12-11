using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class Приписные
    {
        [Key]
        public int ID_приписного { get; set; }

        [Required(ErrorMessage = "Поле 'Призывник' обязательно для заполнения")]
        [Display(Name = "Призывник")]
        [ForeignKey("Призывник")]
        public int? ID_призывника { get; set; }

        [Required(ErrorMessage = "Поле 'Решение' обязательно для заполнения")]
        [Display(Name = "Решение")]
        [ForeignKey("Решение")]
        public int? ID_решения { get; set; }

        [Display(Name = "Призывник")]
        public Призывники? Призывник { get; set; }

        [Display(Name = "Решение")]
        public РешенияКомиссии? Решение { get; set; }
    }
}
