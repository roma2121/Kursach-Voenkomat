using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class УровниОбразования
    {
        [Key]
        public int ID_уровня_образования { get; set; }
        [Display(Name = "Наименование уровня образования")]
        public string Наименование_уровня_образования { get; set; }
    }
}
