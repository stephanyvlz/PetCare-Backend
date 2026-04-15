public class Log
{
    public int Id { get; set; }
    public required string Message { get; set; }
    public required string Level { get; set; }
    public string? UserId { get; set; }
    public string? IpAddress { get; set; }
    public DateTime CreatedAt { get; set; }
}