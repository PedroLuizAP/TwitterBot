using Tweetinvi;
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

var dateSince = DateTime.UtcNow.AddMinutes(-5);

SearchTweetsParameters parameters = new(string.Join(" OR ", searchTerms))
{
    Since = dateSince,
    SearchType = Tweetinvi.Models.SearchResultType.Recent,
    Filters = Tweetinvi.Parameters.Enum.TweetSearchFilters.Safe,
    Locale= "br",   
};

var searchResponse = await userClient.Search.SearchTweetsAsync(parameters);

var tweets = searchResponse.ToList();

tweets.First().PublishRetweetAsync().Wait();
