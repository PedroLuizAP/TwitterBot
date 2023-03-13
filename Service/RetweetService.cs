using Tweetinvi.Models;
using TwitterBot.Helpers;

namespace TwitterBot.Service
{
    internal class RetweetService
    {
        protected ClientService _client { get; set; }
        public RetweetService(ClientService client) => _client = client;
        public async Task Retweet(ITweet tweet) => await _client.Client.Tweets.PublishRetweetAsync(tweet);
    }
}
