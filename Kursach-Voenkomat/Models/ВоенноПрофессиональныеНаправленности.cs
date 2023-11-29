using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class ВоенноПрофессиональныеНаправленности
    {
        [Key]
        public int ID_военно_профессиональной_направленности { get; set; }
        [Display(Name = "Наименование военно профессиональной направленности")]
        public string Наименование_военно_профессиональной_направленности { get; set; }
    }
}
