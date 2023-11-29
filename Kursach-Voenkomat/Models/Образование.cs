using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class Образование
    {
        [Key]
        public int ID_записи_об_образовании { get; set; }

        [ForeignKey("Призывник")]
        [Display(Name = "Призывник")]
        public int ID_призывника { get; set; }

        [Required(ErrorMessage = "Поле 'Номер документа об образовании' обязательно для заполнения")]
        [Display(Name = "Номер документа об образовании")]
        public string Номер_документа_об_образовании { get; set; }

        [ForeignKey("уровеньОбразования")]
        [Display(Name = "Уровень образования")]
        public int? ID_уровня_образования { get; set; }

        [Required(ErrorMessage = "Поле 'Специальность' обязательно для заполнения")]
        public string Специальность { get; set; }

        [Required(ErrorMessage = "Поле 'Дата окончания' обязательно для заполнения")]
        [Display(Name = "Дата окончания")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Дата_окончания { get; set; }

        [ForeignKey("образовательнаяОрганизация")]
        [Display(Name = "Образовательная организация")]
        public int? ID_образовательной_организации { get; set; }

        // Навигационное свойство
        public Призывники? Призывник { get; set; }

        [Display(Name = "Уровень образования")]
        public УровниОбразования? уровеньОбразования { get; set; }

        [Display(Name = "Образовательная организация")]
        public ОбразовательныеОрганизации? образовательнаяОрганизация { get; set; }

        // Свойство для вывода ФИО призывника
        [NotMapped]
        public string ПризывникФИО => $"{Призывник?.Фамилия} {Призывник?.Имя} {Призывник?.Отчество}";
    }
}