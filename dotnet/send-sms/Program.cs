using System.Threading.Tasks;
using Omnigage;
using Omnigage.Resource;

namespace send_sms
{
    public class Program
    {
        /// <summary>
        /// To run this application, the following is required:
        ///
        /// - API token key/secret from Account -> Developer -> API Tokens
        /// - A verified Phone Number UUID from Account -> Telephony -> Phone Numbers -> Edit (in the URI)
        /// - Fill in variables to run example
        /// </summary>
        /// <param name="args"></param>
        async static Task Main(string[] args)
        {
            var tokenKey = "";
            var tokenSecret = "";

            // Set the Phone Number ID (e.g., GncieHvbCKfMYXmeycoWZm)
            var phoneNumberId = "";

            // Sample contact information to call
            var to = ""; // In E.164 format (such as +1xxxxxxxxx)

            // Initialize SDK
            OmnigageClient.Init(tokenKey, tokenSecret);

            var message = new TextMessageResource
            {
                Body = "Sample body"
            };
            await message.Create();

            var text = new TextResource
            {
                To = to,
                TextMessage = message,
                PhoneNumber = new PhoneNumberResource
                {
                    Id = phoneNumberId
                }
            };

            await text.Create();
        }
    }
}