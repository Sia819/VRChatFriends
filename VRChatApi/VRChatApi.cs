using System;
using System.Net.Http;
using System.Text;
using VRChatApi.Endpoints;
using VRChatApi.Logging;

namespace VRChatApi
{
    public class VRChatApi
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public RemoteConfig RemoteConfig { get; set; }
        public UserApi UserApi { get; set; }
        public FriendsApi FriendsApi { get; set; }
        public WorldApi WorldApi { get; set; }
        public ModerationsApi ModerationsApi { get; set; }
        public AvatarApi AvatarApi { get; set; }
        public FavouriteApi FavouriteApi { get; set; }

        public VRChatApi(string username, string password)
        {
            Logger.Trace(() => $"Entering {nameof(VRChatApi)} constructor");                            /// Log : VRChatApi 클래스 생성자의 진입점입니다.
            Logger.Debug(() => $"Using username {username}");                                           /// Log : VRChat ID는 {username} 입니다.

            /// initialize endpoint classes // API endpoint 클래스들을 초기화하여 새로 생성합니다.
            RemoteConfig = new RemoteConfig();
            UserApi = new UserApi(username, password);
            FriendsApi = new FriendsApi();
            WorldApi = new WorldApi();
            ModerationsApi = new ModerationsApi();
            AvatarApi = new AvatarApi();
            FavouriteApi = new FavouriteApi();

            /// initialize http client // static class인 Global의, HttpClient멤버가 처음 사용될 때, 이 멤버를 초기화합니다.
            // TODO: use the auth cookie
            if (Global.HttpClient == null)
            {
                Logger.Trace(() => $"Instantiating {nameof(HttpClient)}");                              /// Log : Global.HttpClient 인스턴스 생성 중
                Global.HttpClient = new HttpClient();
                Global.HttpClient.BaseAddress = new Uri("https://api.vrchat.cloud/api/1/");
                Logger.Info(() => $"VRChat API base address set to {Global.HttpClient.BaseAddress}");   /// Log : VRChat의 기본 API 주소는 {Global.HttpClient.BaseAddress} 로 설정되었습니다.
            }

            /// api요청으로 응답대상 username, password를 해당사이트에서 지원하는 형식인 Base64방식으로 인코딩합니다.
            string authEncoded = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{UserApi.Username}:{UserApi.Password}"));

            /// 편의성을 위한 임시변수입니다. class 변수는 참조형이므로 이 경우,
            /// 참고, header가 수정되면 static변수인 Global.HttpClient.DefaultRequestHeaders도 같이 수정됩니다.
            System.Net.Http.Headers.HttpRequestHeaders requestHeader = Global.HttpClient.DefaultRequestHeaders;

            if (requestHeader.Contains("Authorization"))
            {
                Logger.Debug(() => "Removing existing Authorization header");
                requestHeader.Remove("Authorization");
            }

            /// Global.HttpClient의 DefaultRequestHeaders에 ID / PW를 추가합니다.
            /// static변수에 적용함으로써, HttpRequest의 요청으로 ID와 PW를 전송하는데에 사용됩니다.
            requestHeader.Add("Authorization", $"Basic {authEncoded}");

            Logger.Trace(() => $"Added new Authorization header");                                      /// Log : Global.HttpClient에 권한부여 헤더가 추가되었습니다.
        }
    }
}
