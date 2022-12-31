public class TimeService : IUtcTimeService
{
    public DateTime CurrentUtcDateTime => DateTime.UtcNow;
}