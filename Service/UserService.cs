using Tweetinvi.Models.DTO;
using TwitterBot.Helpers;

namespace TwitterBot.Service
{
    internal class UserService
    {
        protected ClientService _client { get; set; }
        private IUserDTO? _user { get; set; }
        public UserService(ClientService client) => _client = client;            
        

        public async Task GetUserInfo()
        {
            var user = await _client.Client.Users.GetAuthenticatedUserAsync();
            
            _user = user.UserDTO;
        }

        public long GetUserId() =>  _user!.Id;
        public string GetUserScreenName() =>  _user!.ScreenName;
        
    }
}
