using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace TwitterBot.Service
{
    internal class TweetService
    {
        protected TwitterClient _client { get; set; }
        public TweetService(TwitterClient client)
        {
            _client = client;
        }

        internal async Task<List<ITweet>> FindByQuery(string query)
        {
            var tweets = await _client.Search.SearchTweetsAsync(query);

            return tweets.AsEnumerable().ToList();
        }

        internal async Task<List<ITweet>> FindByParameters(string query)
        {
            var tweets = await _client.Search.SearchTweetsAsync(query);

            return tweets.AsEnumerable().ToList();
        }
    }
}
