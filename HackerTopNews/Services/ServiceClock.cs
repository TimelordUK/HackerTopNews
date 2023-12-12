namespace HackerTopNews.Services
{
    public class ServiceClock : IServiceClock
    {
        public DateTime CurrentTime { get => DateTime.Now; set => throw new NotImplementedException(); }
        public override string ToString()
        {
            return $"ServiceClock: {CurrentTime}";
        }
    }
}

