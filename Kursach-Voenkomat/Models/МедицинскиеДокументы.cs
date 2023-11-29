using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class МедицинскиеДокументы
    {
        [Key]
        public int ID_медицинского_документа { get; set; }

        // Внешний ключ для связи с таблицей Призывники
        [ForeignKey("Призывник")]
        [Display(Name = "Призывник")]
        public int ID_призывника { get; set; }

        public string Диагноз { get; set; }

        [Display(Name = "Наименование медицинской организации")]
        public string Наименование_медицинской_организации { get; set; }

        [Display(Name = "Дата выписки медицинского заключения")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Дата_выписки_медицинского_заключения { get; set; }

        // Навигационное свойство
        public Призывники? Призывник { get; set; }

        // Свойство для вывода ФИО призывника
        [NotMapped]
        public string ПризывникФИО => $"{Призывник?.Фамилия} {Призывник?.Имя} {Призывник?.Отчество}";
    }
}