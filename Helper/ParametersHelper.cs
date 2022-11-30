using Tweetinvi.Parameters;

namespace TwitterBot.Helper
{
    internal static class ParametersHelper
    {
        internal static string CreateQuery(this string[] terms, bool withReplies = true)
        {
            return $"({string.Join(" OR ", terms)})" + (withReplies ? "-filter:replies" : string.Empty);
        }

        internal static SearchTweetsParameters CreateParameters(this string query)
        {
            return new(query)
            {
                Since = DateTime.Now,
                SearchType = Tweetinvi.Models.SearchResultType.Recent,
            };
        }
    }
}
