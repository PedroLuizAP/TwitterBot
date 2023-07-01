using Tweetinvi.Exceptions;
using Tweetinvi.Models;
using Tweetinvi.Parameters;
using TwitterBot.Helpers;

namespace TwitterBot.Service
{
    internal class TweetService
    {
        public TweetService(ClientService client, long sinceId)
        {
            _maxId = sinceId;

            _maxMentionId = sinceId;
            
            _client = client;
        }
        private long _maxId { get; set; }
        private long _maxMentionId { get; set; }
        protected ClientService _client { get; set; }
        private void UseMaxId(SearchTweetsParameters parameters) => parameters.SinceId = _maxId;
        private void UseMaxId(GetMentionsTimelineParameters parameters) => parameters.SinceId = _maxMentionId;
        private void SetMaxId(ITweet[] tweets) => _maxId = tweets.Max(tweet => tweet.Id);
        public void SetMaxMentionId(ITweet[] tweets) => _maxMentionId = tweets.Max(tweet => tweet.Id);

        internal async Task<ITweet[]> FindByQuery(string query, bool updateMaxId = true)
        {
            var tweets = await _client.Client!.Search.SearchTweetsAsync(query);

            if (updateMaxId && tweets.Length > 0) SetMaxId(tweets);

            return tweets;
        }
        internal async Task<ITweet[]?> FindByParameters(SearchTweetsParameters parameters, bool updateMaxId = true)
        {
            try
            {
                UseMaxId(parameters);

                var tweets = await _client.Client!.Search.SearchTweetsAsync(parameters);

                if (updateMaxId && tweets.Length > 0) SetMaxId(tweets);

                return tweets;
            }
            catch (TwitterTimeoutException)
            {
                Console.WriteLine("Timeout");

                Thread.Sleep(60000);

                return null;
            }
        }

        internal async Task<ITweet[]?> FindMentions(GetMentionsTimelineParameters parameters, bool updateMaxId = true)
        {
            try
            {
                UseMaxId(parameters);

                var response = await _client.Client!.Timelines.GetMentionsTimelineAsync(parameters);

                List<ITweet> tweets = new();

                foreach (var tweet in response)
                {
                    tweets.Add(tweet);

                    if (tweet.InReplyToStatusId == null) continue;

                    var tweetToRetweet = await FindById(tweet.InReplyToStatusId);

                    if (tweetToRetweet != null) tweets.Add(tweetToRetweet);
                }

                if (updateMaxId && tweets.Count > 0) SetMaxMentionId(response);

                return tweets.ToArray();
            }
            catch(HttpRequestException ex)
            {
                Console.WriteLine("Host bug");

                Thread.Sleep(60000);

                return null;
            }
            catch (TwitterTimeoutException ex)
            {
                Console.WriteLine("Timeout" + ex.Message);

                Thread.Sleep(60000);

                return null;
            }
        }

        private async Task<ITweet?> FindById(long? id)
        {
            try
            {
                return await _client.Client!.Tweets.GetTweetAsync((long)id!);
            }
            catch
            {
                Console.WriteLine("Tweet not found");

                Thread.Sleep(60000);

                return null;
            }
        }
    }
}
