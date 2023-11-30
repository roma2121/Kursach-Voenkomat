using Kursach_Voenkomat.Data;

namespace Kursach_Voenkomat.Models
{
    public class AuditService : IAuditService
    {
        private readonly UserContext _dbContext;

        public AuditService(UserContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void LogAction(string userId, string userRole, string action, string tableName)
        {
            var auditLog = new Log
            {
                Имя_пользователя = userId,
                Действие = action,
                Таблица = tableName,
                Дата = DateTime.Now,
                Роль = userRole
            };

            _dbContext.Logs.Add(auditLog);
            _dbContext.SaveChanges();
        }
    }
}