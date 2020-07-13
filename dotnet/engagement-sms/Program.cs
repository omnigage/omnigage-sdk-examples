using System.Collections.Generic;
using System.Threading.Tasks;
using Omnigage;
using Omnigage.Resource;
using Omnigage.Runtime;

namespace engagement_sms
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
            var firstName = "";
            var lastName = "";
            var phoneNumber = ""; // In E.164 format (such as +1xxxxxxxxx)

            // Initialize SDK
            OmnigageClient.Init(tokenKey, tokenSecret);

            var engagement = new EngagementResource
            {
                Name = "Example SMS Blast",
                Direction = "outbound"
            };
            await engagement.Create();

            var template = new TextTemplateResource
            {
                Name = "Text Template",
                Body = "Sample body"
            };
            await template.Create();

            var activity = new ActivityResource
            {
                Name = "SMS Blast",
                Kind = ActivityKind.Text,
                Engagement = engagement,
                TextTemplate = template,
                PhoneNumber = new PhoneNumberResource
                {
                    Id = phoneNumberId
                }
            };
            await activity.Create();

            var envelope = new EnvelopeResource
            {
                PhoneNumber = phoneNumber,
                Engagement = engagement,
                Meta = new Dictionary<string, string>
                {
                    { "first-name", firstName },
                    { "last-name", lastName }
                }
            };

            // Push one or more envelopes into list
            List<EnvelopeResource> envelopes = new List<EnvelopeResource> { };
            envelopes.Add(envelope);

            // Populate engagement queue
            await Client.PostBulkRequest("envelopes", EnvelopeResource.SerializeBulk(envelopes));

            // Schedule engagement for processing
            engagement.Status = "scheduled";
            await engagement.Update();
        }
    }
}