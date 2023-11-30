using Microsoft.Extensions.Primitives;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kursach_Voenkomat.Models
{
    public class Log
    {
        [Key]
        public int ID_записи { get; set; }

        //[ForeignKey("Пользователь")]
        public string Имя_пользователя { get; set; }

        public string Действие { get; set; }

        public string Таблица { get; set; }

        public DateTime Дата { get; set; }

        public string? Роль {  get; set; }
    }
}