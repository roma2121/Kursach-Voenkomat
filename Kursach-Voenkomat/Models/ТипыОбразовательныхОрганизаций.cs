using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class ТипыОбразовательныхОрганизаций
    {
        [Key]
        public int ID_типа_образовательной_организации { get; set; }
        [Display(Name = "Наименование типа образовательной организации")]
        public string Наименование_типа_образовательной_организации { get; set; }
    }
}
