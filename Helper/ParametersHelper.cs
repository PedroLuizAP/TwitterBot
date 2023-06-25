using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace TwitterBot.Helper
{
    internal static class ParametersHelper
    {
        internal static string CreateQuery(this string[] terms, bool withReplies = true) => $"({string.Join(" OR ", terms)})" + (withReplies ? "-filter:replies" : string.Empty);
        internal static SearchTweetsParameters CreateParameters(this string query, long sinceId) => new(query) { Since = DateTime.Now, SinceId = sinceId, SearchType = SearchResultType.Recent };
        internal static GetMentionsTimelineParameters CreateMentionsParameters(long sinceId) => new() { PageSize = 5, SinceId = sinceId };
        internal static string[] CreateTerms() => new[] { "sseraphini", "#paneladev", "#bolhadev" };
    }
}
