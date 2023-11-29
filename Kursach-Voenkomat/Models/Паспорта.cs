using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class Паспорта
    {
        [Key]
        public int ID_паспорта { get; set; }

        [Required(ErrorMessage = "Поле 'Серия' обязательно для заполнения")]
        [MaxLength(4, ErrorMessage = "Поле 'Серия' должно содержать не более 4 символов")]
        public int Серия { get; set; }

        [Required(ErrorMessage = "Поле 'Номер' обязательно для заполнения")]
        [MaxLength(6, ErrorMessage = "Поле 'Номер' должно содержать не более 6 символов")]
        public int Номер { get; set; }

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
        public string ПризывникФИО => $"{Призывник?.Фамилия} {Призывник?.Имя} {Призывник?.Отчество}";
    }
}