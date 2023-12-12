namespace HackerTopNews.Model
{
    public class ApiNewsStory
    {
        public string Title { get; set; }
        public string Uri { get; set; }
        public string PostedBy { get; set; }
        public DateTime Time { get; set; }
        public int Score { get; set; }
        public int CommentCount { get; set; }

        public override string ToString()
        {
            return $"ApiNewsStory Title = {Title}, Uri = {Uri}, PostedBy = {PostedBy}, Time = {Time}, Score = {Score}, CommentCount = {CommentCount}";
        }
    }
}
