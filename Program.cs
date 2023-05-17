using TwitterBot.Helper;
using TwitterBot.Helpers;
using TwitterBot.Service;

ClientService clientService = new(args[0], args[1], args[2], args[3]);

var terms = ParametersHelper.CreateTerms();

var query = terms.CreateQuery();

var parameters = query.CreateParameters();

var mentionParameter = ParametersHelper.CreateMentionsParameters();

TweetService tweetService = new(clientService);

UserService userService = new(clientService);

await userService.GetUserInfo();
  
RetweetService retweetService = new(clientService);

var userId = userService.GetUserId();

var userScreenName = userService.GetUserScreenName();

do
{
    try
    {
        var searchResponse = await tweetService.FindByParameters(parameters);

        var mentionResponse = await tweetService.FindMentions(mentionParameter);

        var resultTweets = searchResponse.FilterTweets();

        if (resultTweets?.Count > 0) await resultTweets.RetweetTweets(retweetService, userId);

        var resultMentions = mentionResponse.FilterTweets(true, userScreenName);

        if (resultMentions?.Count > 0) await resultMentions.RetweetTweets(retweetService, userId);

        var valueTimeout = resultMentions?.Count > 0 || resultTweets?.Count > 0 ? 10000 : 30000;

        Thread.Sleep(valueTimeout);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
} while (true);
