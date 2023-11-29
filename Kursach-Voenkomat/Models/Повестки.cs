using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class Повестки
    {
        [Key]
        public int ID_повестки { get; set; }

        [ForeignKey("Призывник")]
        [Display(Name = "Призывник")]
        public int ID_призывника { get; set; }

        [ForeignKey("типПовестки")]
        [Display(Name = "Тип повестки")]
        public int ID_типа_повестки { get; set; }

        [Required(ErrorMessage = "Поле 'Дата выписки' обязательно для заполнения")]
        [Display(Name = "Дата выписки")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Дата_выписки { get; set; }

        [ForeignKey("статусЯвки")]
        [Display(Name = "Статус явки")]
        public int? ID_наименования_статуса_явки { get; set; }

        [Required(ErrorMessage = "Поле 'Дата явки' обязательно для заполнения")]
        [Display(Name = "Дата явки")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Дата_явки { get; set; }

        // Навигационное свойство
        public Призывники? Призывник { get; set; }

        [Display(Name = "Тип повестки")]
        public ТипыПовесток? типПовестки { get; set; }

        [Display(Name = "Статус явки")]
        public ВидыСтатусовЯвки? статусЯвки { get; set; }

        // Свойство для вывода ФИО призывника
        [NotMapped]
        public string ПризывникФИО => $"{Призывник?.Фамилия} {Призывник?.Имя} {Призывник?.Отчество}";
    }
}