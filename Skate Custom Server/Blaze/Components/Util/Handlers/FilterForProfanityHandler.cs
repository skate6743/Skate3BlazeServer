using Blaze.Components.Util.Commands;
using Org.BouncyCastle.Crypto.Prng;
using Servers;
using Servers.Blaze.Models;

namespace Blaze.Components.Util.Handlers
{
    public class FilterForProfanityHandler
    {
        public static async Task HandleRequest(User user, byte[] packetBytes)
        {
            var req = BlazeMessage.CreateModelFromRequest<FilterForProfanityResponse>(packetBytes);

            var responseModel = new FilterForProfanityResponse
            {
                FilteredTextList = new List<Models.FilteredUserText>
                {
                    new Models.FilteredUserText
                    {
                        Result = 0,
                        FilteredText = req.FilteredTextList.First().FilteredText
                    }
                }
            };

            BlazeMessage response = BlazeMessage.CreateResponseFromModel(packetBytes, responseModel);

            await user.Stream.WriteAsync(response.Serialize());
            
        }
    }
}
