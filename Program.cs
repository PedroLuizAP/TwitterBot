using TwitterBot.Helper;
using TwitterBot.Helpers;
using TwitterBot.Service;

ClientService clientService = new(args[0], args[1], args[2], args[3]);

long sinceId = args.Length > 4 && !string.IsNullOrEmpty(args[4]) ? long.Parse(args[4]) : 0;

var terms = ParametersHelper.CreateTerms();

var query = terms.CreateQuery();

var parameters = query.CreateParameters(sinceId);

var mentionParameter = ParametersHelper.CreateMentionsParameters(sinceId);

TweetService tweetService = new(clientService, sinceId);

UserService userService = new(clientService);

await userService.GetUserInfo();

RetweetService retweetService = new(clientService);

var userId = userService.GetUserId();

var userScreenName = userService.GetUserScreenName();

var blockedHelper = new BlockedHelper();

RetweetHelper retweetHelper = new(retweetService, userId, blockedHelper, clientService);

do
{
    try
    {
#if DEBUG
        var searchResponse = await tweetService.FindByParameters(parameters);

        var mentionResponse = await tweetService.FindMentions(mentionParameter);

        var resultTweets = searchResponse.FilterTweets();

        if (resultTweets?.Count > 0) await retweetHelper.RetweetTweets(resultTweets);

        var resultMentions = mentionResponse.FilterTweets(true, userScreenName);

        if (resultMentions?.Count > 0) await retweetHelper.RetweetTweets(resultMentions);

#else
        BlockedHelper.Start();

        BlockedHelper.Filter();
        
        BlockedHelper.Find();

        BlockedHelper.Favorite();

        BlockedHelper.Retweet();
#endif

        if (resultMentions?.Count == 0 && searchResponse?.Length > 0) tweetService.SetMaxMentionId(searchResponse);

        var valueTimeout = resultTweets?.Count > 0 ? 10000 : 40000;

        Thread.Sleep(valueTimeout);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }
    finally
    {
#if !DEBUG
        BlockedHelper.Clean();
#endif
    }
} while (true);
