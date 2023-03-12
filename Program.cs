using Tweetinvi.Models;
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

var parameters = query.CreateParameters();

var mentionParameter = ParametersHelper.CreateMentionsParameters();

TweetService tweetService = new(clientService);

UserService userService = new(clientService);

await userService.GetUserInfo();


var userId = userService.GetUserId();

var userScreenName = userService.GetUserScreenName();

while (true)
{
    var searchResponse = await tweetService.FindByParameters(parameters);

    var mentionResponse = await tweetService.FindMentions(mentionParameter);

    var resultTweets = searchResponse?.Where(tweet => !tweet.IsRetweet).ToList();

    var resultMentions = mentionResponse?.Where(tweet => tweet.Text.Contains($"@{userScreenName}")).ToList();

    if (resultTweets?.Count > 0)
    {
        resultTweets = resultTweets.OrderBy(t => t.CreatedAt).ToList();

        resultTweets.ForEach(async tw =>
        {
            RetweetService retweetService = new(clientService);

            try
            {
                var allRetweets = await retweetService.GetAllRetweet(tw);

                if (!retweetService.RetweetedByMe(allRetweets, userId)) await retweetService.Retweet(tw);
            }
            catch (Exception ex)
            {
                try { await retweetService.Retweet(tw); } catch { }

                Console.WriteLine($"error in Tweet {tw.Id} \n {ex.Message}");
            }
        });
    }

    if (resultMentions?.Count > 0)
    {
        resultMentions = resultMentions.OrderBy(t => t.CreatedAt).ToList();

        resultMentions.ForEach(async tw =>
        {
            RetweetService retweetService = new(clientService);

            try
            {
                var allRetweets = await retweetService.GetAllRetweet(tw);

                if (!retweetService.RetweetedByMe(allRetweets, userId)) await retweetService.Retweet(tw);
            }
            catch (Exception ex)
            {
                try { await retweetService.Retweet(tw); } catch { }

                Console.WriteLine($"error in Mention {tw.Id} \n {ex.Message}");
            }
        });
    }

    if (resultMentions?.Count > 0 || resultTweets?.Count > 0)
    {
        Thread.Sleep(10000);

        continue;
    }

    Thread.Sleep(50000);
}