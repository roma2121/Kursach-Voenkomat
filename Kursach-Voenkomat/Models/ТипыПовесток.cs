using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class ТипыПовесток
    {
        [Key]
        public int ID_типа_повестки { get; set; }
        [Display(Name = "Тип повестки")]
        public string Тип_повестки { get; set; }
    }
}
