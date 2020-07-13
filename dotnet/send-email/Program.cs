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
        /// - A verified Phone Number UUID from Account -> Email -> Email IDs -> Edit (in the URI)
        /// - Fill in variables to run example
        /// </summary>
        /// <param name="args"></param>
        async static Task Main(string[] args)
        {
            var tokenKey = "";
            var tokenSecret = "";

            // Set the Email ID (e.g., NbXW9TCHax9zfAeDhaY2bG)
            var emailId = "";

            // Recipient email address
            var to = "";

            // Initialize SDK
            OmnigageClient.Init(tokenKey, tokenSecret);

            // Create email message
            var message = new EmailMessageResource
            {
                Subject = "Ahoy",
                Body = "Sample body"
            };
            await message.Create();

            // Send email
            var email = new EmailResource
            {
                To = to,
                EmailMessage = message,
                EmailId = new EmailIdResource
                {
                    Id = emailId
                }
            };
            await email.Create();
        }
    }
}