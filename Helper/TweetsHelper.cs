using Tweetinvi.Models;

namespace TwitterBot.Helper
{
    public static class TweetsHelper
    {
        public static List<ITweet> FilterTweets(this ITweet[]? tweetsFilter, bool isMentions = false, string userScreenName = "")
        {
            List<ITweet> tweets = new();

            if (tweetsFilter?.Length > 0)
            {
                foreach (var tweet in tweetsFilter)
                {
                    if (isMentions && tweet.Text.Contains($"@{userScreenName}")) tweet.FavoriteAsync().Wait();

                    if (tweet.IsRetweet || tweet.InReplyToStatusId != null) continue;

                    tweets.Add(tweet);
                };
            }

            return tweets.OrderBy(t => t.CreatedAt).ToList();
        }
    }
}