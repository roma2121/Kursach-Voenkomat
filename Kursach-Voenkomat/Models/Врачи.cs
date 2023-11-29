using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class Врачи
    {
        [Key]
        public int ID_врача { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        [ForeignKey("Специальность")]
        [Display(Name = "Специальность")]
        public int ID_специальности { get; set; }

        // Навигационное свойство
        public СпециальностиВрачей? Специальность {  get; set; }

        [NotMapped]
        public string ВрачФИО => $"{Фамилия} {Имя} {Отчество}";
    }
}
