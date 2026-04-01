using Blaze.Components.UserSessions.Commands;
using Blaze.Components.UserSessions.Models;
using Newtonsoft.Json;
using Servers;
using Servers.Blaze.Models;
using Servers.Database;

namespace Blaze.Components.UserSessions.Handlers
{
    public class LookupUsersHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            LookupUsersRequest request = BlazeMessage.CreateModelFromRequest<LookupUsersRequest>(packetBytes);

            var responseModel = new LookupUsersResponse
            {
                UserDataList = new List<UserData>()
            };

            if ((LookupType)request.LookupType == LookupType.PERSONA_NAME)
            {
                await using var db = new AppDbContext();

                var users = new List<UserData>();
                foreach (UserIdentification userToFind in request.UserIdentificationList)
                {
                    UserDbData? data = db.Users.Where(x => x.DisplayName == userToFind.Name).FirstOrDefault();

                    if (data != null)
                    {
                        string displayName = data.DisplayName;

                        string usernamesPath = Path.Combine(ServerGlobals.BaseDirectory, "spoofed_usernames.json");

                        if (File.Exists(usernamesPath))
                        {
                            string usernamesJson = File.ReadAllText(usernamesPath);
                            var nameSpoofs = JsonConvert.DeserializeObject<Dictionary<string, string>>(usernamesJson);

                            if (nameSpoofs != null && nameSpoofs.ContainsKey(displayName))
                            {
                                displayName = nameSpoofs[displayName];
                            }
                        }

                        users.Add(new UserData
                        {
                            ExtendedData = user.ExtendedData,
                            StatusFlags = 1,
                            UserInfo = new UserIdentification
                            {
                                Name = displayName,
                                BlazeId = data.BlazeId,
                                IsOnline = true,
                                ExternalId = data.PsnId
                            }
                        });
                    }

                    responseModel.UserDataList = users;
                }

                BlazeMessage response = BlazeMessage.CreateResponseFromModel(packetBytes, responseModel);

                await user.Stream.WriteAsync(response.Serialize());
            }
        }
    }
}