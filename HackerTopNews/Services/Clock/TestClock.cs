namespace HackerTopNews.Services.Clock
{
    /*
     * used for unit testing when cache entries need to be aged based on a test time over
     * real system time
     */ 
    public class TestClock : IServiceClock
    {
        public DateTime CurrentTime { get; set; } = DateTime.Now;
        public override string ToString()
        {
            return $"TestClock: {CurrentTime}";
        }
    }
}

