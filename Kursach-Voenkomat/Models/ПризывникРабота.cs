using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class ПризывникРабота
    {
        [Key]
        public int ID_призывник_работа { get; set; }

        [ForeignKey("Призывник")]
        [Display(Name = "Призывник")]
        public int ID_призывника { get; set; }
        [ForeignKey("Работа")]
        [Display(Name = "Работа")]
        public int ID_работы { get; set; }

        public Призывники? Призывник { get; set; }
        public Работа? Работа { get; set; }

        // Свойство для вывода ФИО призывника
        [NotMapped]
        public string ПризывникФИО => $"{Призывник?.Фамилия} {Призывник?.Имя} {Призывник?.Отчество}";
    }
}
