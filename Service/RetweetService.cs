using Tweetinvi;
using Tweetinvi.Models;

namespace TwitterBot.Service
{
    internal class RetweetService
    {
        protected TwitterClient _client { get; set; }

        public RetweetService(TwitterClient client)
        {
            _client = client;
        }

        public async Task<ITweet[]> GetAllRetweetUsers(ITweet tweet)
        {
            return await tweet.GetRetweetsAsync();
        }

        public bool RetweetedByMe(ITweet[] tweets, long userId)
        {
            return tweets.Any(x => x.CreatedBy.Id == userId);
        }
    }
}
