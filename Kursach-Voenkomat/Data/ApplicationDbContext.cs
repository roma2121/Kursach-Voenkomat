using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Kursach_Voenkomat.Models;

namespace Kursach_Voenkomat.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ВидыСтатусовЯвки>? ВидыСтатусовЯвки { get; set; }
        public DbSet<ВоенноПрофессиональныеНаправленности>? ВоенноПрофессиональныеНаправленности { get; set; }
        public DbSet<Врачи>? Врачи { get; set; }
        public DbSet<ЗаключенияВрачей>? ЗаключенияВрачей { get; set; }
        public DbSet<КатегорииГодности>? КатегорииГодности { get; set; }
        public DbSet<КатегорииПрофессиональнойПригодности>? КатегорииПрофессиональнойПригодности { get; set; }
        public DbSet<МедицинскиеДокументы>? МедицинскиеДокументы { get; set; }
        public DbSet<Образование>? Образование { get; set; }
        public DbSet<ОбразовательныеОрганизации>? ОбразовательныеОрганизации { get; set; }
        public DbSet<Паспорта>? Паспорта { get; set; }
        public DbSet<Повестки>? Повестки { get; set; }
        public DbSet<Полы>? Полы { get; set; }
        public DbSet<Призывники>? Призывники { get; set; }
        public DbSet<ПризывникРабота>? ПризывникРабота { get; set; }
        public DbSet<ПрофессиональноПсихологическиеПоказателиГраждан>? ПрофессиональноПсихологическиеПоказателиГраждан { get; set; }
        public DbSet<Работа>? Работа { get; set; }
        public DbSet<РешенияКомиссии>? РешенияКомиссии { get; set; }
        public DbSet<СпециальностиВрачей>? СпециальностиВрачей { get; set; }
        public DbSet<ТипыОбразовательныхОрганизаций>? ТипыОбразовательныхОрганизаций { get; set; }
        public DbSet<ТипыПовесток>? ТипыПовесток { get; set; }
        public DbSet<УровниОбразования>? УровниОбразования { get; set; }
        public DbSet<Приписные>? Приписные { get; set; }
        public DbSet<Kursach_Voenkomat.Models.RecordModel>? RecordModel { get; set; }
    }
}