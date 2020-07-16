using System.Threading.Tasks;
using Omnigage.Resource;

namespace call_number
{
    public class Program
    {
        /// <summary>
        /// To run this application, the following is required:
        ///
        /// - API token key/secret from Account -> Developer -> API Tokens
        /// - A verified Caller ID UUID from Account -> Telephony -> Caller IDs -> Edit (in the URI)
        /// - Fill in variables to run example
        /// </summary>
        /// <param name="args"></param>
        async static Task Main(string[] args)
        {
            // Initialize `OmnigageClient` using settings from "appsettings.json"
            // Alternatively, this line can be commented out and use `OmnigageClient.Init()`
            // to set config
            Util.Init();

            // Set the Caller ID (e.g., NeSB5RN8gbT6ZtmZYdYWpH)
            var callerId = "";

            // Recipient phone number
            var to = ""; // In E.164 format (such as +1xxxxxxxxx)

            var call = new CallResource
            {
                To = to,
                Action = CallAction.Dial,
                CallerId = new CallerIdResource
                {
                    Id = callerId
                }
            };

            await call.Create();
        }
    }
}