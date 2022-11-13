using Tweetinvi;

var userClient = new TwitterClient(args[0], args[1], args[2], args[3]);

var user = await userClient.Users.GetAuthenticatedUserAsync();

var tweet = await userClient.Tweets.PublishTweetAsync("Teste Bot");
