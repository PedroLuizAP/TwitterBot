using Tweetinvi;
using Tweetinvi.Parameters;

long maxId = 0;

var userClient = new TwitterClient(args[0], args[1], args[2], args[3]);

var user = await userClient.Users.GetAuthenticatedUserAsync();

string[] searchTerms = new[]
   {
        "sseraphini",
        "#paneladev",
        "#bolhadev"
    };

var dateSince = DateTime.Now;

var filter = $"({string.Join(" OR ", searchTerms)}) -filter:replies";

SearchTweetsParameters parameters = new(filter)
{
    Since = DateTime.Now,
    SearchType = Tweetinvi.Models.SearchResultType.Recent,
};

while (true)
{
    parameters.SinceId = maxId;

    var searchResponse = await userClient.Search.SearchTweetsAsync(parameters);

    var resultTweets = searchResponse.Where(tweet => !tweet.IsRetweet);

    var tweets = resultTweets.OrderBy(t => t.CreatedAt).ToList();

    if (tweets != null && tweets.Count > 0)
    {
        foreach (var tw in tweets)
        {
            try
            {
                var allRetweets = await tw.GetRetweetsAsync();

                if (!allRetweets.Any(x => x.CreatedBy.Id == user.Id))
                    await userClient.Tweets.PublishRetweetAsync(tw);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error in Tweet {tw.Id} \n {ex.Message}");
            }
        }

        maxId = tweets.Max(tweet => tweet.Id);
    }

    Thread.Sleep(10000);
}


