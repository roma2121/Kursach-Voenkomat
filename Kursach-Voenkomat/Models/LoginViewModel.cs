using System.ComponentModel.DataAnnotations;

namespace Kursach_Voenkomat.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Поле 'Логин' обязательно для заполнения")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Поле 'Пароль' обязательно для заполнения")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
