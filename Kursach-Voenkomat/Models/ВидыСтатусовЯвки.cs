using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class ВидыСтатусовЯвки
    {
        [Key]
        public int ID_наименования_статуса_явки { get; set; }
        [Display(Name = "Наименование статуса явки")]
        public string Наименование_статуса_явки { get; set; }
    }
}
