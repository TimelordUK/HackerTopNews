using System;

namespace HackerTopNews.Model
{
    /*
     * these entries are returned as JSON to the caller of this app service
     */
    public class RankedNewsStory
    {
        public RankedNewsStory(HackerNewStory hackerNewStory)
        {
            Title = hackerNewStory.Title;
            Uri = hackerNewStory.Url;
            PostedBy = hackerNewStory.By;
            Time = DateTimeOffset.FromUnixTimeSeconds(hackerNewStory.Time).DateTime;
            Score = hackerNewStory.Score;
            CommentCount = hackerNewStory.Kids.Count;
        }
        public string Title { get; }
        public string Uri { get; }
        public string PostedBy { get; }
        public DateTime Time { get; }
        public int Score { get; }
        public int CommentCount { get; }

        public override string ToString()
        {
            return $"RankedNewsStory Title = {Title}, Uri = {Uri}, PostedBy = {PostedBy}, Time = {Time}, Score = {Score}, CommentCount = {CommentCount}";
        }
    }
}
