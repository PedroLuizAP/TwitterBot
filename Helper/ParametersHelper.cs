using Tweetinvi.Parameters;

namespace TwitterBot.Helper
{
    internal static class ParametersHelper
    {
        internal static string CreateQuery(this string[] terms, bool withReplies = true) => $"({string.Join(" OR ", terms)})" + (withReplies ? "-filter:replies" : string.Empty);
        internal static SearchTweetsParameters CreateParameters(this string query) => new(query) { Since = DateTime.Now, SearchType = Tweetinvi.Models.SearchResultType.Recent, };
        internal static GetMentionsTimelineParameters CreateMentionsParameters() => new() { };
    }
}
