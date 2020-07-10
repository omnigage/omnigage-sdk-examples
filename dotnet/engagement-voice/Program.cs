using System.Collections.Generic;
using System.Threading.Tasks;
using Omnigage;
using Omnigage.Resource;

namespace engagement_voice
{
    public class Program
    {
        /// <summary>
        /// To run this application, the following is required:
        ///
        /// - API token key/secret from Account -> Developer -> API Tokens
        /// - Two audio files (either wav or mp3)
        /// - A Caller ID UUID from Account -> Telephony -> Caller IDs -> Edit (in the URI)
        /// </summary>
        /// <param name="args"></param>
        async static Task Main(string[] args)
        {
            string tokenKey = "";
            string tokenSecret = "";

            // Set the Caller ID (e.g., yL9vQaWrSqg5W8EFEpE6xZ)
            string callerId = "";

            // Sample contact information to call
            string firstName = "";
            string lastName = "";
            string phoneNumber = ""; // In E.164 format with country code (e.g., +11235551234)

            // Audio path for when human is detected
            string audioFilePath1 = ""; // Full path to audio file (e.g., /Users/Shared/piano.wav on Mac)

            // Audio path for machine detection
            string audioFilePath2 = ""; // Full path to audio file (e.g., /Users/Shared/nimoy_spock.wav on Mac)

            // Initialize SDK
            OmnigageClient.Init(tokenKey, tokenSecret);

            EngagementResource engagement = new EngagementResource();
            engagement.Name = "Example Voice Blast";
            engagement.Direction = "outbound";
            await engagement.Create();

            ActivityResource activity = new ActivityResource();
            activity.Name = "Voice Blast";
            activity.Kind = ActivityKind.Voice;
            activity.Engagement = engagement;
            activity.CallerId = new CallerIdResource
            {
                Id = callerId
            };
            await activity.Create();

            UploadResource upload1 = new UploadResource
            {
                FilePath = audioFilePath1
            };
            await upload1.Create();

            UploadResource upload2 = new UploadResource
            {
                FilePath = audioFilePath2
            };
            await upload2.Create();

            VoiceTemplateResource humanRecording = new VoiceTemplateResource();
            humanRecording.Name = "Human Recording";
            humanRecording.Kind = "audio";
            humanRecording.Upload = upload1;
            await humanRecording.Create();

            VoiceTemplateResource machineRecording = new VoiceTemplateResource();
            machineRecording.Name = "Machine Recording";
            machineRecording.Kind = "audio";
            machineRecording.Upload = upload2;
            await machineRecording.Create();

            // Define human trigger
            TriggerResource triggerHumanInstance = new TriggerResource();
            triggerHumanInstance.Kind = "play";
            triggerHumanInstance.OnEvent = "voice-human";
            triggerHumanInstance.Activity = activity;
            triggerHumanInstance.VoiceTemplate = humanRecording;
            await triggerHumanInstance.Create();

            // Define machine trigger
            TriggerResource triggerMachineInstance = new TriggerResource();
            triggerMachineInstance.Kind = TriggerKind.Play;
            triggerMachineInstance.OnEvent = TriggerOnEvent.VoiceMachine;
            triggerMachineInstance.Activity = activity;
            triggerMachineInstance.VoiceTemplate = machineRecording;
            await triggerMachineInstance.Create();

            EnvelopeResource envelope = new EnvelopeResource();
            envelope.PhoneNumber = phoneNumber;
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
            await OmnigageClient.PostBulkRequest("envelopes", EnvelopeResource.SerializeBulk(envelopes));

            // Schedule engagement for processing
            engagement.Status = "scheduled";
            await engagement.Update();
        }
    }
}