using System;

[Serializable]
public class NewsIDList
{
    [JsonProperty("results")]
    public IEnumerable<int> NewsID { get; set; }
}
