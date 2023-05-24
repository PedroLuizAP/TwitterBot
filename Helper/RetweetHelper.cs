using Tweetinvi.Models;
using TwitterBot.Service;

namespace TwitterBot.Helper
{
    internal static class RetweetHelper
    {
        private static async Task<ITweet[]> GetAllRetweet(this ITweet tweet) => await tweet.GetRetweetsAsync();
        private static bool RetweetedByMe(this ITweet[] tweets, long userId) => tweets.Any(x => x.CreatedBy.Id == userId);

        public static async Task RetweetTweets(this List<ITweet> resultMentions, RetweetService retweetService, long userId, BlockedHelper blockedHelper)
        {
            foreach (var tw in resultMentions)
            {
                try
                {
                    var allRetweets = await tw.GetRetweetsAsync();

                    if (!allRetweets.RetweetedByMe(userId) && blockedHelper.Verify(tw)) await retweetService.Retweet(tw);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"RetweetHelper");

                    Console.WriteLine($"error in Mention {tw.Id} \n {ex.Message}");
                }
            }
        }
    }
}