using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class Призывники
    {
        [Key]
        public int ID_призывника { get; set; }

        [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения")]
        [Display(Name = "Имя")]
        public string Имя { get; set; }

        [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения")]
        [Display(Name = "Фамилия")]
        public string Фамилия { get; set; }

        [Display(Name = "Отчество")]
        public string? Отчество { get; set; }

        [Required(ErrorMessage = "Поле 'Дата рождения' обязательно для заполнения")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Дата_рождения { get; set; }

        [Display(Name = "Номер телефона")]
        [RegularExpression(@"^(\+7|8)\d{10}$", ErrorMessage = "Неправильный формат номера телефона")]
        public string? Номер_телефона { get; set; }

        // Навигационное свойство
        public Полы? Пол {  get; set; }

        // Внешний ключ для связи с таблицей Полы
        [ForeignKey("Пол")]
        [Display(Name = "Пол")]
        public int ID_пола { get; set; }

        [NotMapped]
        public string ПризывникФИО => $"{Фамилия} {Имя} {Отчество}";
    }
}