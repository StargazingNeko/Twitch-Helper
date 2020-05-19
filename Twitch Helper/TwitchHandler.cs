using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Windows;
using System.Windows.Navigation;

namespace Twitch_Helper
{
    public class TwitchHandler
    {
        private static int iGrabbedFollowerCount = 0;
        private static string UserData { get; set; }
        private static string UserID { get; set; }
        private static string Username { get; set; }
        private string OAuthUserData { get; set; }
        public static readonly string ClientID = "2u4r5adco7dubxsnka2hsw73ydbrjs";
        private string _OAuthToken;
        public static bool bHasInitRun = false;
        private static JObject ReadUserData;
        private readonly JsonHandler _JsonHandler = new JsonHandler();

        public void Init()
        {
            _OAuthToken = (string)_JsonHandler.Read("config.json", true)?["OAuthToken"] ?? string.Empty;

            if (string.IsNullOrEmpty(_OAuthToken) && !_OAuthToken.ToLower().Contains("oauth"))
            {
                MessageBoxResult MessageResult = MessageBox.Show("You must provide an OAuth token!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                if (MessageResult == MessageBoxResult.OK)
                {
                    Environment.Exit(0);
                }

                return;
            }

            GetUsername();
            if (string.IsNullOrEmpty(Username))
            {
                return;
            }

            UserData = GetUserData(Username).ToString();
            ReadUserData = new JsonHandler()?.Read(UserData.ToString(), false) ?? JObject.Parse("{ }");

            GetUserId();
            if (string.IsNullOrEmpty(UserID))
            {
                return;
            }

            bHasInitRun = true;
        }

        public void SetOAuthToken(string OAuthToken)
        {
            _OAuthToken = OAuthToken;
        }

        public string OAuthValidation(string OAuth)
        {
            if (string.IsNullOrEmpty(OAuth) && !OAuth.ToLower().Contains("oauth"))
            {
                MessageBox.Show("OAuth Token is not set!", "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return string.Empty;
            }

            RestClient restClient = new RestClient($"https://id.twitch.tv/oauth2/validate");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Client-ID", ClientID);
            request.AddHeader("Authorization", $"OAuth {_OAuthToken}");
            IRestResponse response = restClient.Execute(request);

            if (!response.IsSuccessful)
            {
                return string.Empty;
            }

            OAuthUserData = response.Content ?? string.Empty;

            return OAuthUserData;
        }

        public string GetUserData(string username)
        {

            RestClient restClient = new RestClient($"https://api.twitch.tv/helix/users?login={username}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Client-ID", $"{ClientID}");
            request.AddHeader("Authorization", $"Bearer {_OAuthToken}");
            IRestResponse response = restClient.Execute(request);

            return response.Content ?? "Response was Null";
        }

        public int GetFollowerCount()
        {
            if (bHasInitRun == false)
            {
                Init();
            }

            RestClient restClient = new RestClient($"https://api.twitch.tv/helix/users/follows?to_id={UserID}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Client-ID", ClientID);
            request.AddHeader("Authorization", $"Bearer {_OAuthToken}");
            IRestResponse response = restClient.Execute(request);

            ReadUserData = _JsonHandler?.Read(response.Content, false) ?? JObject.Parse("{ \"total\": 0 }");

            iGrabbedFollowerCount = (int?)ReadUserData?["total"] ?? 0;
        
            return iGrabbedFollowerCount;
        }

        public string GetUsername(string OAuth = "")
        {

            if (string.IsNullOrEmpty(OAuthUserData) || string.IsNullOrEmpty(OAuth))
            {
                if (!string.IsNullOrEmpty(_OAuthToken) && !_OAuthToken.ToLower().Contains("oauth"))
                {
                    OAuth = _OAuthToken;
                }
                else
                {
                    MessageBox.Show("OAuth validation failed!", "Username", MessageBoxButton.OK, MessageBoxImage.Error);
                    return string.Empty;
                }
            }

            OAuthValidation(OAuth);
            Username = (string)_JsonHandler?.Read(OAuthUserData, false)?["login"] ?? "Username";

            return Username;
        }

        public string GetUserId(string OAuth = "")
        {
            if (string.IsNullOrEmpty(OAuthUserData) || string.IsNullOrEmpty(OAuth))
            {
                if (!string.IsNullOrEmpty(_OAuthToken) && !_OAuthToken.ToLower().Contains("oauth"))
                {
                    OAuth = _OAuthToken;
                }
                else
                {
                    MessageBox.Show("OAuth validation failed!", "UserId", MessageBoxButton.OK, MessageBoxImage.Error);
                    return string.Empty;
                }
            }

            OAuthValidation(OAuth);
            UserID = (string)_JsonHandler?.Read(OAuthUserData, false)?["user_id"] ?? "UserId";

            return UserID;
        }
    }
}
