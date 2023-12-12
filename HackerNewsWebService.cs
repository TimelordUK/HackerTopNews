using System;

public class HackerNewsWebService
{
	private static readonly string GetStories = "https://hacker-news.firebaseio.com/v0/beststories.json";

    public async Task<IEnumerable<Recipe>> GetTopStoryID()
    {
        List<Recipe> newsIds = new List<News>();

        var url = GetStories;
        HttpClient client = new HttpClient();
        client.BaseAddress = new Uri(url);
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

        if (response.IsSuccessStatusCode)
        {
            var jsonString = await response.Content.ReadAsStringAsync();
            var recipeList = JsonConvert.DeserializeObject<NewsIDList>(jsonString);

            if (recipeList != null)
            {
                recipes.AddRange(recipeList.Recipes);
            }
        }

        return recipes;
    }
}
