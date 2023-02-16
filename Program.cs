using Tweetinvi.Parameters;
using TwitterBot.Helper;
using TwitterBot.Helpers;
using TwitterBot.Service;

ClientService clientService = new(args[0], args[1], args[2], args[3]);

string[] searchTerms = new[]
{
    "sseraphini",
    "#paneladev",
    "#bolhadev"
};

var dateSince = DateTime.Now;

var query = searchTerms.CreateQuery();

SearchTweetsParameters parameters = query.CreateParameters();

TweetService tweetService = new(clientService);

RetweetService retweetService = new(clientService);

var userId = (await clientService.DetailUser()).Id;

while (true)
{
    var searchResponse = await tweetService.FindByParameters(parameters);

    var resultTweets = searchResponse.Where(tweet => !tweet.IsRetweet);

    var tweets = resultTweets.OrderBy(t => t.CreatedAt).ToList();

    if (tweets != null && tweets.Count > 0)
    {
        tweets.ForEach(async tw =>
        {
            try
            {
                var allRetweets = await retweetService.GetAllRetweet(tw);

                if (!retweetService.RetweetedByMe(allRetweets, userId)) await retweetService.Retweet(tw);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error in Tweet {tw.Id} \n {ex.Message}");
            }
        });
    }
    else
    {
        Thread.Sleep(40000);

        continue;
    }

    Thread.Sleep(10000);
}


