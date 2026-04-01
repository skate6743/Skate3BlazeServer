using Blaze.Components.Authentication.Models;
using Blaze.Components.AssociationLists.Models;
using Servers.Blaze.Models;

namespace Blaze.Components.AssociationLists
{
    public class GetLists
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            var responseModel = new AssociationListCollectionInfo
            {
                AssociationListInfoByNameMap = new Dictionary<string, AssociationListInfo>()
            };

            BlazeMessage response = BlazeMessage.CreateResponseFromModel(
                packetBytes,
                responseModel);

            await user.Stream.WriteAsync(response.Serialize());
        }
    }
}