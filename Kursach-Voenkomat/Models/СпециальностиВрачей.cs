using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class СпециальностиВрачей
    {
        [Key]
        public int ID_специальности { get; set; }
        [Display(Name = "Наименование специальности")]
        public string Наименование_специальности { get; set; }
    }
}
