using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class ЗаключенияВрачей
    {
        [Key]
        public int ID_заключения { get; set; }

        [ForeignKey("Призывник")]
        [Display(Name = "Призывник")]
        public int ID_призывника { get; set; }

        [Required(ErrorMessage = "Поле 'Дата выписки заключения' обязательно для заполнения")]
        [Display(Name = "Дата выписки заключения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Дата_выписки_заключения { get; set; }

        [Required(ErrorMessage = "Поле 'Врач' обязательно для заполнения")]
        [ForeignKey("Врач")]
        [Display(Name = "Врач")]
        public int ID_врача { get; set; }

        [Required(ErrorMessage = "Поле 'Предварительный диагноз' обязательно для заполнения")]
        [Display(Name = "Предварительный диагноз")]
        public string Предварительный_диагноз { get; set; }

        [Required(ErrorMessage = "Поле 'Категория' обязательно для заполнения")]
        [ForeignKey("Категория")]
        [Display(Name = "Категория")]
        public int ID_категории_годности { get; set; }

        [Required(ErrorMessage = "Поле 'Заключение' обязательно для заполнения")]
        public string Заключение { get; set; }

        // Навигационное свойство
        public Призывники? Призывник { get; set; }
        public Врачи? Врач { get; set; }
        public КатегорииГодности? Категория { get; set; }

        // Свойство для вывода ФИО призывника
        [NotMapped]
        public string ПризывникФИО => $"{Призывник?.Фамилия} {Призывник?.Имя} {Призывник?.Отчество}";

        // Свойство для вывода ФИО врача
        [NotMapped]
        public string ВрачФИО => $"{Врач?.Фамилия} {Врач?.Имя} {Врач?.Отчество}";
    }
}
