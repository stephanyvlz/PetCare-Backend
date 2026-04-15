using PetCare.API.Data;

public class LogRepository : ILogRepository
{
    private readonly AppDbContext _context;

    public LogRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Log log)
    {
        await _context.Logs.AddAsync(log);
    }
    public async Task SaveChangesAsync() // 👈 agregar esto
    {
        await _context.SaveChangesAsync();
    }
}
