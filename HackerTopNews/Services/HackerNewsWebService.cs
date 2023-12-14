﻿using HackerTopNews;
using HackerTopNews.Controllers;
using HackerTopNews.Services;
using System;
using System.Collections.Concurrent;
using System.Text.Json;
using HackerTopNews.Model;
using Microsoft.Extensions.Logging;

public class HackerNewsWebService : IHackerNewsService
{
    private static readonly string RootUrl = "https://hacker-news.firebaseio.com/v0";
    private static readonly string GetTopStoriesUrl = $"{RootUrl}/beststories.json";
    private readonly HttpClient _httpClient;
    private static readonly JsonSerializerOptions IgnoreCase = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    private readonly ILogger<HackerNewsWebService> _logger;
    private readonly ConcurrentDictionary<string, Task<HttpResponseMessage>> _responses = new ConcurrentDictionary<string, Task<HttpResponseMessage>>();

    public HackerNewsWebService(ILogger<HackerNewsWebService> logger, HttpClient httpClient)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    // do not have more than one outstanding request out on the same URL
    private async Task<string> GetJson(string url)
    {
        _logger.LogInformation($"GetResponse url = {url}");
        var response = await _responses.GetOrAdd(url, u =>
        {
            return _httpClient.GetAsync(u);
        });
        _responses.TryRemove(url, out _);
        if (!response.IsSuccessStatusCode) return null;
        string responseBody = await response.Content.ReadAsStringAsync();
        return responseBody;
    }

    // https://hacker-news.firebaseio.com/v0/item/38544729.json
    public async Task<HackerNewStory> GetStory(int id)
    {
        try
        {
            string url = $"{RootUrl}/item/{id}.json?print=pretty";
            string jsonString = await GetJson(url);
            if (jsonString != null)
            {
                var story = JsonSerializer.Deserialize<HackerNewStory>(jsonString, IgnoreCase);
                _logger.LogInformation($"GetStory id = {id}, url = {url}, response len = {jsonString.Length}");
                _logger.LogDebug($"GetStory id = {id}, url = ${url}, response = {jsonString}, r = {story}");
                if (story != null)
                {
                    return story;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetStory error " + ex.Message);
        }
        throw new BadHttpRequestException($"Error fetching {id}");
    }

    public async Task<IReadOnlyList<int>> GetTopStories()
    {
        try
        {
            _logger.LogInformation($"GetTopStoryID [{GetTopStoriesUrl}]");
            var url = $"{GetTopStoriesUrl}?print=pretty";
            var jsonString = await GetJson(url);
            if (jsonString != null)
            {
                var listInts = JsonSerializer.Deserialize<List<int>>(jsonString);
                _logger.LogInformation($"GetTopStories url = {url}, response len = {jsonString.Length}, list len = {listInts.Count}");
                _logger.LogDebug($"GetTopStories url = {url}, response = {jsonString}, list len = {listInts.Count}");
                return listInts;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GetTopStoryID error " + ex.Message);
            throw new BadHttpRequestException($"Error fetching top stories");
        }
        return new List<int>();
    }
}
