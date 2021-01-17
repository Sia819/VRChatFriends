using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using VRChatFriends.Common;
using VRChatApi;
using VRChatApi.Classes;

namespace VRChatFriends
{
    class ApiExample
    {
        static async Task Main(string[] args)
        {
            VRChatApi.VRChatApi api = new VRChatApi.VRChatApi("lunasia819@gmail.com", "tjdrb1458");
            VRChatApi.Logging.LogProvider.SetCurrentLogProvider(new ColoredConsoleLogProvider());

            /// Remote Config
            /// VRChatApi 인스턴스의, 프로퍼티 크래스.RemoteConfig class 프로퍼티의 Get함수를 호출합니다.
            /// VRChatApi.Global.ApiKey를 서버에서 받은 값으로 업데이트 합니다.
            /// 이는, public ApiKey가 만료되었을때, 서버에서 새 ApiKey를 받는것을 기대할 수 있습니다.
            ConfigResponse config = await api.RemoteConfig.Get();

            #region UserAPI
            /// 회원가입 API로 새 계정만들기
            // UserResponse userNew = await api.UserApi.Register("someName", "somePassword", "some@email.com");
            /// 로그인하기
            UserResponse user = await api.UserApi.Login();
            /// 유저정보 수정
            //UserResponse userUpdated = await api.UserApi.UpdateInfo(user.id, null, null, null, new List<string>() { "admin_moderator", "admin_scripting_access", "system_avatar_access", "system_world_access" });
            #endregion

            #region Friends
            // 친구 리스트 불러오기
            List<UserBriefResponse> friends = await api.FriendsApi.Get(0, 20, true);
            // 친구 신청요청
            //NotificationResponse friendRequestResponse = await api.FriendsApi.SendRequest("usr_f8220fc0-e6f9-45ab-8d9f-ae00e8491685", api.UserApi.Username);
            // 친구 삭제요청
            //string friendDeletionResponse = await api.FriendsApi.DeleteFriend("usr_f8220fc0-e6f9-45ab-8d9f-ae00e8491685");
            // 친구 승락요청
            //await api.FriendsApi.AcceptFriend("usr_f8220fc0-e6f9-45ab-8d9f-ae00e8491685");
            #endregion

            #region WorldAPI
            // * var worldId = "wrld_b2d24c29-1ded-4990-a90d-dd6dcc440300"; // The great Pug
            // * WorldResponse world = await api.WorldApi.Get(worldId);
            //List<WorldBriefResponse> starWorlds = await api.WorldApi.Search(WorldGroups.Favorite, count: 4);
            // * List<WorldBriefResponse> scaryWorlds = await api.WorldApi.Search(keyword: "Scary", sort: SortOptions.Popularity);
            // * List<WorldBriefResponse> featuredWorlds = await api.WorldApi.Search(featured: true);
            // * WorldMetadataResponse metadata = await api.WorldApi.GetMetadata(worldId);
            // * var instances = new List<WorldInstanceResponse>();
            // * if (world.instances.Count > 0) {
            // *     foreach(WorldInstance instance in world.instances)
            // *     {
            // *         WorldInstanceResponse worldInst = await api.WorldApi.GetInstance(world.id, instance.id);
            // *         instances.Add(worldInst);
            // *     }
            // * }
            #endregion

            // avatar api
            // * AvatarResponse avatar = await api.AvatarApi.GetById("avtr_17036f54-f706-46bf-8a43-0c60564123ff");

            Console.ReadLine();
        }
    }
}
