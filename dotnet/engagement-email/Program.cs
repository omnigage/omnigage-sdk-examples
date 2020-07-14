using System.Collections.Generic;
using System.Threading.Tasks;
using Omnigage;
using Omnigage.Resource;
using Omnigage.Runtime;

namespace engagement_email
{
    public class Program
    {
        /// <summary>
        /// To run this application, the following is required:
        ///
        /// - API token key/secret from Account -> Developer -> API Tokens
        /// - A verified Email ID UUID from Account -> Email -> Email IDs -> Edit (in the URI)
        /// - Fill in variables to run example
        /// </summary>
        /// <param name="args"></param>
        async static Task Main(string[] args)
        {
            var tokenKey = "";
            var tokenSecret = "";

            // Set the Email ID (e.g., NbXW9TCHax9zfAeDhaY2bG)
            var emailId = "";

            // Sample contact information to call
            var firstName = "";
            var lastName = "";
            var emailAddress = "";

            // Initialize SDK
            OmnigageClient.Init(tokenKey, tokenSecret);

            var engagement = new EngagementResource();
            engagement.Name = "Example Email Blast";
            engagement.Direction = "outbound";
            await engagement.Create();

            var emailTemplate = new EmailTemplateResource();
            emailTemplate.Subject = "Ahoy";
            emailTemplate.Body = "Sample body";

            await emailTemplate.Create();

            var activity = new ActivityResource();
            activity.Name = "Email Blast";
            activity.Kind = ActivityKind.Email;
            activity.Engagement = engagement;
            activity.EmailTemplate = emailTemplate;
            activity.EmailId = new EmailIdResource
            {
                Id = emailId
            };
            await activity.Create();

            var envelope = new EnvelopeResource();
            envelope.EmailAddress = emailAddress;
            envelope.Engagement = engagement;
            envelope.Meta = new Dictionary<string, string>
            {
                { "first-name", firstName },
                { "last-name", lastName }
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