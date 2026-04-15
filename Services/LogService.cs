public class LogService : ILogService
{
    private readonly ILogRepository _repo;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogService(ILogRepository repo, IHttpContextAccessor httpContextAccessor)
    {
        _repo = repo;
        _httpContextAccessor = httpContextAccessor;
    }

   private string? GetIp()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context == null)
                return null;

            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrEmpty(ip))
                return ip;

            return context.Connection.RemoteIpAddress?.ToString();
        }

    public async Task LogError(string message, string? userId = null)
{
    try
    {
        await _repo.AddAsync(new Log
        {
            Message = message,
            Level = "ERROR",
            UserId = userId,
            IpAddress = GetIp(),
            CreatedAt = DateTime.UtcNow // usa UtcNow consistentemente
        });

        await _repo.SaveChangesAsync();
    }
    catch
    {
        // evitar romper la app
    }
}

public async Task LogInfo(string message, string? userId = null)
{
    try
    {
        await _repo.AddAsync(new Log
        {
            Message = message ?? "Sin mensaje",
            Level = "INFO",
            UserId = userId,
            IpAddress = GetIp(),
            CreatedAt = DateTime.UtcNow
        });

        await _repo.SaveChangesAsync(); // 👈 igual aquí
    }
    catch
    {
        // evitar romper la app
    }
}
    
}