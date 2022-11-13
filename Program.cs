using Tweetinvi;
using Tweetinvi.Parameters;

var userClient = new TwitterClient(args[0], args[1], args[2], args[3]);

var user = await userClient.Users.GetAuthenticatedUserAsync();

//string[] searchTerms = new[]
//   {
//        "@sseraphini",        
//        "#paneladev",
//        "#bolhadev"
//    };

//SearchTweetsParameters parameters = new(string.Join(" OR ", searchTerms)) 
//{
//     //Since = DateTime.UtcNow
//};

var searchResponse = await userClient.Search.SearchTweetsAsync("pedroluizap");

var tweets = searchResponse.ToList();

tweets.First().PublishRetweetAsync().Wait();
