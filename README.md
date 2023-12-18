# HackerTopNews

## Summary 

This applicatiuon when launched can be used to fetch the TOP N ranked news stories using the 
Hacker news URL https://hacker-news.firebaseio.com/v0.  The top ID list is resolved along with
all stories returned within the ID list.  These fetches are cached to prevent overloading the
Hacker data source. 

## Assumptions

It has been assumed that the kids returned from a given story
are all comments, not filter is applied which would require recursively resolving the graph.

### Effort 
There has been around 20-25 hrs of development time to build this applicaton. It was based on the skeleton VS core ASP
service using VS2022

## Running
Once built, should be possible to launch in usual way (e.g. F5) from Visual Studio - A console is expected hosting the
service and in debug a Swagger web page http://localhost:5110/swagger/index.html should be launched.

the console will show output such as below :-

```txt
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:5110
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Users\me\dev\cs\HackerTopNews\HackerTopNews
```

use the Swagger page for example HackerApi->Get->Execute

should see a JSON ID list of 200 IDs.

```json
[
  38655066,
  38647484,
  38657126,
  38657577,
  38673292,
  38677124,
  38675616,
  38666032,
  38652619,
  38653456
]
```

the console is expected to show output resolving the ID list

```txt
info: HackerNewsWebService[0]
      rootUrl = https://hacker-news.firebaseio.com/v0, getTopStoriesUrl = https://hacker-news.firebaseio.com/v0/beststories.json
info: HackerTopNews.Services.Cache.NewsStoryCache[0]
      TopStoryCache expiry lifetime = 00:00:20
info: HackerNewsWebService[0]
      rootUrl = https://hacker-news.firebaseio.com/v0, getTopStoriesUrl = https://hacker-news.firebaseio.com/v0/beststories.json
info: HackerTopNews.Services.Cache.NewsStoryCache[0]
      NewsStoryCache expiry lifetime = 00:01:20
info: HackerTopNews.Controllers.HackerTopNewsController[0]
      Get all ids
info: HackerNewsWebService[0]
      GetTopStoryID [https://hacker-news.firebaseio.com/v0/beststories.json]
info: HackerNewsWebService[0]
      GetResponse url = https://hacker-news.firebaseio.com/v0/beststories.json?print=pretty
info: System.Net.Http.HttpClient.IHackerNewsService.LogicalHandler[100]
      Start processing HTTP request GET https://hacker-news.firebaseio.com/v0/beststories.json?print=pretty
info: System.Net.Http.HttpClient.IHackerNewsService.ClientHandler[100]
      Sending HTTP request GET https://hacker-news.firebaseio.com/v0/beststories.json?print=pretty
info: System.Net.Http.HttpClient.IHackerNewsService.ClientHandler[101]
      Received HTTP response headers after 358.0862ms - 200
info: System.Net.Http.HttpClient.IHackerNewsService.LogicalHandler[101]
      End processing HTTP request after 381.3149ms - 200
info: HackerNewsWebService[0]
      GetTopStories url = https://hacker-news.firebaseio.com/v0/beststories.json?print=pretty, response len = 2003, list len = 200
```

## Cache
Note that the cache used within this application can be configures with setttings in appsettings.json

```json
  "NewsCache": {
    "TopNewsExpireSeconds": 20,
    "NewsStoryExpireSeconds": 80,
    "CullFrequency": 5
  }#
```

  The first call made when resolving top ranked score stories is to fetch the list of IDs.  This cache
  is currently configured to expire at a faster rate than the stories resolved from it.  Hence new IDs will be
  resolved and added into the story cache which expires at 4 x age by default. Over time old stories will
  expire and at all times all stories making up the top score will be cached however some may be out of date
  until they also expire and are re-fetched.

  This is done to limit the number of calls made to the Hacker API - there is a deep discussion to be made
  on how such a cache is best implemented and indeed the integrity of the data as one may find a cache story receives
  a lot of comments whilst cached which would mean its score is not reflected when invoking the service.  However,
  given an upper limit on how long an item lives in the cache means this would eventually resolve.
  
  a very simple cache implementation is used AgedCache see Services/Cache

# REST APIS
  There are 2 REST apis

  ## HackerApi

  This is a simple wrapper around the Hacker API returning the raw data as returned by their API - this allows 
  the service to be visualised.  Note that the service is also using the cache so repeatedly pressing the Swagger
  Execute button will fetch caches results and eventually go back and fetch from Hacker API.

  The first top call returns a JSON list of IDs, one of these can be copied onto the second call which takes that
  ID and returns the story it belongs too.  Note that theses stories are all cached based on json settings.

  the second should return JSON such as below

  ```json
  {
  "url": "https://minnesotareformer.com/2022/12/15/toxic-3m-knew-its-chemicals-were-harmful-decades-ago-but-didnt-tell-the-public-government/",
  "type": "story",
  "title": "3M knew its chemicals were harmful decades ago, but didn't tell the public",
  "time": 1702841447,
  "score": 432,
  "id": 38675616,
  "by": "Jimmc414",
  "kids": [
    38675761,
    38676586,
    38675804,
    38675835,
    38676398,
    38676345,
    38676117,
    38676619,
    38676549,
    38677423
  }
  ```

  GET
  /HackerApi

  GET
  /HackerApi/{id}


  ## HackerTopNews

  This is main service making use of above to resolve the top N ranked by score stories based on RankedNewsStory definition shown 
  below.  Note this service is using the above described caches and hence is not aware from where the data is being sourced
  - if the execute button is pressed, after the expiry period the rankings are re-resolved and re-ordered.

  GET
  /HackerTopNews/{n}


```cs
    public class RankedNewsStory
    {
        public string Title { get; }
        public string Uri { get; }
        public string PostedBy { get; }
        public DateTime Time { get; }
        public int Score { get; }
        public int CommentCount { get; }
    }
```

  ## Main classes of Note

  | name  | summary  |
  |---|---|
  | AppBuilder          | set up the DI container with services for this application  |
  | AgedCache            |  used to cache ID fetches and stories based on an expiry time |
  | ScoreRankedNews      |  to resolve and compute top N stories by score |
  | HackerTopNewsController  |  the entry REST point for resolving stories |
  
  ## Unit Tests
   
  There are some unit tests which can be run, these include a test time service so the time
  can be eplicitely set rather than using system time.  There is also a Moq using real life
  data of the Hacker API.  This is done with DI, see TestNews/Support/TestAppBuilder.

  ## Test coverage

  Using R# (which was only used for this purpose), coverage stats show ~80% coverage of the application