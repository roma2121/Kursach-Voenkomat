using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class Паспорта
    {
        [Key]
        public int ID_паспорта { get; set; }

        [Required(ErrorMessage = "Поле 'Серия' обязательно для заполнения")]
        [StringLength(4, ErrorMessage = "Серия должна содержать максимум 4 цифры")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Серия должна содержать только цифры")]
        public string Серия { get; set; }

        [Required(ErrorMessage = "Поле 'Номер' обязательно для заполнения")]
        [StringLength(6, ErrorMessage = "Номер должен содержать максимум 6 цифры")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Номер должен содержать только цифры")]
        public string Номер { get; set; }

        [Required(ErrorMessage = "Поле 'Дата выдачи' обязательно для заполнения")]
        [Display(Name = "Дата выдачи")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime Дата_выдачи { get; set; }

        [Required(ErrorMessage = "Поле 'Кем выдан' обязательно для заполнения")]
        [Display(Name = "Кем выдан")]
        public string Кем_выдан { get; set; }

        [Required(ErrorMessage = "Поле 'Место рождения' обязательно для заполнения")]
        [Display(Name = "Место рождения")]
        public string Место_рождения { get; set; }

        [ForeignKey("Призывник")]
        [Display(Name = "Призывник")]
        public int ID_призывника { get; set; }

        [Required(ErrorMessage = "Поле 'Адрес прописки' обязательно для заполнения")]
        [Display(Name = "Адрес прописки")]
        public string Адрес_прописки { get; set; }


        // Навигационное свойство
        public Призывники? Призывник { get; set; }

        // Свойство для вывода ФИО призывника
        [NotMapped]
        public string? ПризывникФИО => $"{Призывник?.Фамилия} {Призывник?.Имя} {Призывник?.Отчество}";
    }
}