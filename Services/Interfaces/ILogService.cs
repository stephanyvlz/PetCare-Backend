public interface ILogService
{
    Task LogInfo(string message, string? userId = null);
    Task LogError(string message, string? userId = null);
}