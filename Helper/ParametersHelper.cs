using Tweetinvi.Parameters;

namespace TwitterBot.Helper
{
    internal class ParametersHelper
    {
        internal string CreateQuery(string[] terms)
        {
            return $"({string.Join(" OR ", terms)}) -filter:replies";
        }

        internal SearchTweetsParameters CreateParameters(string query)
        {
            return new(query)
            {
                Since = DateTime.Now,
                SearchType = Tweetinvi.Models.SearchResultType.Recent,
            };
        }
    }
}
