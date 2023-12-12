namespace HackerTopNews.Services
{
    public class TestClock : IServiceClock
    {
        public DateTime CurrentTime { get; set; } = DateTime.Now;
        public override string ToString()
        {
            return $"TestClock: {CurrentTime}";
        }
    }
}

