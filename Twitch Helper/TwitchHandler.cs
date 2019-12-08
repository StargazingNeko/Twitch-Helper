using Newtonsoft.Json.Linq;
using RestSharp;

namespace Twitch_Helper
{
    public class TwitchHandler
    {
        private static int iGrabbedFollowerCount = 0;
        private static string UserData { get; set; }
        private static int UserID { get; set; } = 1;
        private static string Username { get; set; }
        private static readonly string ClientID = "";
        //private static string OAuthToken = "";
        public static bool bHasInitRun = false;
        private static JObject ReadUserData;
        private readonly JsonHandler _JsonHandler = new JsonHandler();

        public void Init()
        {
            Username = (string)_JsonHandler.Read("config.json", true)["Username"];

            if(Username == null || Username == "")
            {
                return;
            }

            UserData = PullUserData(Username).ToString();
            ReadUserData = new JsonHandler().Read(UserData.ToString(), false);
            UserID = (int)ReadUserData["data"][0]["id"];
            bHasInitRun = true;
        }
        public string PullUserData(string username)
        {

            RestClient restClient = new RestClient($"https://api.twitch.tv/helix/users?login={username}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Client-ID", $"{ClientID}");
            //request.AddHeader("Authentication", $"Bearer {OAuthToken}");
            IRestResponse response = restClient.Execute(request);

            return response.Content;
        }

        public int PullFollowerCount()
        {
            if (bHasInitRun == false)
            {
                Init();
            }

            RestClient restClient = new RestClient($"https://api.twitch.tv/helix/users/follows?to_id={UserID}");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Client-ID", $"{ClientID}");
            IRestResponse response = restClient.Execute(request);

            ReadUserData = new JsonHandler().Read(response.Content, false);

            iGrabbedFollowerCount = (int)ReadUserData["total"];
        
            return iGrabbedFollowerCount;
        }
    }
}
