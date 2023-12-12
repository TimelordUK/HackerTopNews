namespace HackerTopNews.Services.Clock
{
    /// <summary>
    /// provides reference time inject either actual System time or a test clock 
    /// </summary>
    public interface IServiceClock
    {
        DateTime CurrentTime { get; set; }
    }
}

