using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class ОбразовательныеОрганизации
    {
        [Key]
        public int ID_образовательной_организации { get; set; }

        [Required(ErrorMessage = "Поле 'Наименование образовательной организации' обязательно для заполнения")]
        [Display(Name = "Наименование образовательной организации")]
        public string Наименование_образовательной_организации { get; set; }

        [ForeignKey("типОбразовательнойОрганизации")]
        public int ID_типа_образовательной_организации { get; set; }

        [Required(ErrorMessage = "Поле 'Адрес образовательной организации' обязательно для заполнения")]
        [Display(Name = "Адрес образовательной организации")]
        public string Адрес_образовательной_организации { get; set; }

        [Display(Name = "Тип образовательной организации")]
        // Навигационное свойство
        public ТипыОбразовательныхОрганизаций? типОбразовательнойОрганизации { get; set; }
    }
}