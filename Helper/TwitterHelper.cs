using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterBot.Helper
{
    internal class TwitterHelper
    {
        private string _consumerKey { get; set; }
        private string _consumerSecret { get; set; }
        private string _accessToken { get; set; }
        private string _accessSecret { get; set; }
        private TwitterClient _client { get; set; }

        public void createClient() => _client = new(_consumerKey, _consumerSecret, _accessToken, _accessSecret);


        public TwitterHelper(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _accessToken = accessToken;
            _accessSecret = accessSecret;

            createClient();
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

        internal string CreateQuery(string[] terms)
        {
            return string.Join(" OR ", terms);
        }

        internal SearchTweetsParameters CreateParameters(string query)
        {
            SearchTweetsParameters parameters = new(string.Join(" OR ", query))
            {
                Since = dateSince,
                SearchType = Tweetinvi.Models.SearchResultType.Recent,
                Filters = Tweetinvi.Parameters.Enum.TweetSearchFilters.Safe,
                Locale = "br",
            };
        }
    }
}
