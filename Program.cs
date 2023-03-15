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

RetweetService retweetService = new(clientService);

var userId = userService.GetUserId();

var userScreenName = userService.GetUserScreenName();

while (true)
{
    var searchResponse = await tweetService.FindByParameters(parameters);

    var mentionResponse = await tweetService.FindMentions(mentionParameter);

    var resultTweets = searchResponse.FilterTweets();

    if (resultTweets?.Count > 0) await resultTweets.RetweetTweets(retweetService, userId);    

    var resultMentions = mentionResponse.FilterTweets(true, userScreenName);

    if (resultMentions?.Count > 0) await resultMentions.RetweetTweets(retweetService, userId);    

    if (resultMentions?.Count > 0 || resultTweets?.Count > 0)
    {
        Thread.Sleep(10000);

        continue;
    }

    Thread.Sleep(30000);
}