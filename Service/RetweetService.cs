using Tweetinvi;
using Tweetinvi.Models;
using TwitterBot.Helpers;

namespace TwitterBot.Service
{
    internal class RetweetService
    {
        protected ClientService _client { get; set; }
        public RetweetService(ClientService client) => _client = client;
        public async Task<ITweet[]> GetAllRetweet(ITweet tweet) => await tweet.GetRetweetsAsync();
        public bool RetweetedByMe(ITweet[] tweets, long userId) => tweets.Any(x => x.CreatedBy.Id == userId);
        public async Task Retweet(ITweet tweet) => await _client.Client.Tweets.PublishRetweetAsync(tweet);

    }
}
