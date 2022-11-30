using Tweetinvi;
using Tweetinvi.Models;

namespace TwitterBot.Helpers
{
    internal class ClientService
    {
        private string _consumerKey { get; set; }
        private string _consumerSecret { get; set; }
        private string _accessToken { get; set; }
        private string _accessSecret { get; set; }
        public TwitterClient Client { get; set; }

        private void CreateClient() => Client = new(_consumerKey, _consumerSecret, _accessToken, _accessSecret);

        public ClientService(string consumerKey, string consumerSecret, string accessToken, string accessSecret)
        {
            _consumerKey = consumerKey;
            _consumerSecret = consumerSecret;
            _accessToken = accessToken;
            _accessSecret = accessSecret;

            CreateClient();
        }

        public async Task<IAuthenticatedUser> DetailUser() => await Client.Users.GetAuthenticatedUserAsync();

    }
}
