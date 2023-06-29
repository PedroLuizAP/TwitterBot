using Tweetinvi.Core.Models;
using Tweetinvi.Models;
using TwitterBot.Helpers;
using TwitterBot.Service;

namespace TwitterBot.Helper
{
    internal class RetweetHelper
    {
        internal RetweetHelper(RetweetService retweetService, long userId, BlockedHelper blockedHelper, ClientService client)
        {
            this.retweetService = retweetService;

            this.userId = userId;

            this.blockedHelper = blockedHelper;

            _client = client;
        }
        private ClientService _client { get; set; }
        private RetweetService retweetService { get; }
        private long userId { get; }
        private BlockedHelper blockedHelper { get; }

        private async Task<ITweet[]> GetAllRetweet(ITweet tweet) => await tweet.GetRetweetsAsync();
        private async Task<bool> RetweetedByMe(ITweet? tweet)
        {
            var retweeterIdsIterator = await _client.Client!.Tweets.GetRetweeterIdsAsync(tweet);

            return retweeterIdsIterator.Any(x => x == userId);
        }

        public async Task RetweetTweets(List<ITweet> resultMentions)
        {
            foreach (var tw in resultMentions)
            {
                try
                {
                    var verifyBlocked = blockedHelper.Verify(tw);

                    if (tw.RetweetCount == 0 && verifyBlocked)
                    {
                        await retweetService.Retweet(tw);

                        continue;
                    }

                    if (!await RetweetedByMe(tw) && verifyBlocked) await retweetService.Retweet(tw);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"RetweetHelper");

                    Console.WriteLine($"error in Mention {tw.Id} \n {ex.Message}");

                    Thread.Sleep(60000);
                }
            }
        }
    }
}