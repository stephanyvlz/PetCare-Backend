public interface ILogRepository
{
    Task AddAsync(Log log);
    Task SaveChangesAsync(); 
}