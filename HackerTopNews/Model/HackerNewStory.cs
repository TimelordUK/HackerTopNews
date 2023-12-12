namespace HackerTopNews.Model
{
    public class HackerNewStory
    {
        public string Url { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public long Time { get; set; }
        public int Score { get; set; }
        public int Id { get; set; }
        public string By { get; set; }
        public List<int> Kids { get; set; }
        public override string ToString()
        {
            return $"HackerNewStory Url = {Url}, Type = {Type}, Title = '{Title}', Time = {Time}, Score = {Score}, Id = {Id}, By = {By}, Kids = {(Kids != null ? string.Join(",", Kids) : "")}";
        }
    }
}
