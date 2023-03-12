using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using TwitterBot.Helpers;

namespace TwitterBot.Service
{
    internal class TweetService
    {
        private long _maxId { get; set; } = 0;
        private long _maxMentionId { get; set; } = 0;
        protected ClientService _client { get; set; }
        public TweetService(ClientService client) => _client = client;
        private void UseMaxId(SearchTweetsParameters parameters) => parameters.SinceId = _maxId;
        private void UseMaxId(GetMentionsTimelineParameters parameters) => parameters.SinceId = _maxMentionId;
        private void SetMaxId(ITweet[] tweets) => _maxId = tweets.Max(tweet => tweet.Id);
        private void SetMaxMentionId(ITweet[] tweets) => _maxMentionId = tweets.Max(tweet => tweet.Id);

        internal async Task<ITweet[]> FindByQuery(string query, bool updateMaxId = true)
        {
            var tweets = await _client.Client.Search.SearchTweetsAsync(query);

            if (updateMaxId && tweets.Length > 0) SetMaxId(tweets);

            return tweets;
        }

        internal async Task<ITweet[]?> FindByParameters(SearchTweetsParameters parameters, bool updateMaxId = true)
        {
            try
            {
                UseMaxId(parameters);

                var tweets = await _client.Client.Search.SearchTweetsAsync(parameters);

                if (updateMaxId && tweets.Length > 0) SetMaxId(tweets);

                return tweets;
            }
            catch (TwitterTimeoutException)
            {
                return null;
            }
        }

        internal async Task<ITweet[]?> FindMentions(GetMentionsTimelineParameters parameters, bool updateMaxId = true)
        {
            try
            {
                UseMaxId(parameters);

                var tweets = _maxMentionId > 0 ? await _client.Client.Timelines.GetMentionsTimelineAsync(parameters) : await _client.Client.Timelines.GetMentionsTimelineAsync();

                if (updateMaxId && tweets.Length > 0) SetMaxMentionId(tweets);

                return tweets;
            }
            catch (TwitterTimeoutException)
            {
                return null;
            }
        }
    }
}
