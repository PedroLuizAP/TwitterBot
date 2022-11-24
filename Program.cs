using Tweetinvi;
using Tweetinvi.Core.Events;
using Tweetinvi.Core.Extensions;
using Tweetinvi.Core.Models;
using Tweetinvi.Parameters;
using TwitterBot.Helper;

//var userHelper = new TwitterHelper(args[0], args[1], args[2], args[3]);

var userClient = new TwitterClient(args[0], args[1], args[2], args[3]);

var user = await userClient.Users.GetAuthenticatedUserAsync();

string[] searchTerms = new[]
   {
        "@sseraphini",
        "#paneladev",
        "#bolhadev"
    };

var dateSince = DateTime.Now;
//(bolhadev OR paneladev OR sseraphini) lang:pt since:2022-11-22 -filter:replies

var filter = $"({string.Join(" OR ", searchTerms)}) -filter:replies";

SearchTweetsParameters parameters = new(filter)
{
    Since = DateTime.Now,
    SearchType = Tweetinvi.Models.SearchResultType.Recent,
    Lang = Tweetinvi.Models.LanguageFilter.Portuguese,    
};
var searchResponse = await userClient.Search.SearchTweetsAsync(parameters);

var resultTweets = searchResponse.Where(tweet => !tweet.IsRetweet);

var tweets = resultTweets.OrderByDescending(t => t.CreatedAt).ToList();

foreach (var tw in tweets)
{
    try
    {
        var allRetweets = await tw.GetRetweetsAsync();

        if (!allRetweets.Any(x => x.CreatedBy.Id == user.Id)) // probably not necessary already treated in the query inside the parameters
            await userClient.Tweets.PublishRetweetAsync(tw);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"error in Tweet {tw.Id} \n {ex.Message}");
    }
}


