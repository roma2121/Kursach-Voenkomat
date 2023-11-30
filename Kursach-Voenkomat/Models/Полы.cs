using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class Полы
    {
        [Key]
        public int ID_пола { get; set; }
        public string Пол { get; set; }
    }
}