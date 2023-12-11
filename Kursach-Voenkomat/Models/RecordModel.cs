using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class RecordModel
    {
        [Key]
        public int ID_посещения { get; set; }
        [Required(ErrorMessage = "Поле 'Фамилия' обязательно для заполнения")]
        public string Фамилия { get; set; }
        [Required(ErrorMessage = "Поле 'Имя' обязательно для заполнения")]
        public string Имя { get; set; }
        public string? Отчество { get; set; }

        [Required(ErrorMessage = "Поле 'Дата рождения' обязательно для заполнения")]
        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Дата_рождения { get; set; }

        [Required(ErrorMessage = "Поле 'Дата посещения' обязательно для заполнения")]
        [Display(Name = "Дата посещения")]
        public DateTime Дата_посещения { get; set; }
    }
}