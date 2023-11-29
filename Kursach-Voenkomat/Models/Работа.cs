using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class Работа
    {
        public string Место_работы { get; set; }
        [Key]
        public int ID_работы { get; set; }
    }
}
