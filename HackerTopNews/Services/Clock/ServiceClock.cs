namespace HackerTopNews.Services.Clock
{
    /*
     * defsult implementation as used by the real system
     */
    public class ServiceClock : IServiceClock
    {
        public DateTime CurrentTime { get => DateTime.Now; set => throw new NotImplementedException(); }
        public override string ToString()
        {
            return $"ServiceClock: {CurrentTime}";
        }
    }
}

