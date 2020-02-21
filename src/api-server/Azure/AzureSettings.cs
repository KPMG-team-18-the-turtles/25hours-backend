using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TwentyFiveHours.API.Azure
{
    public class AzureSettings : IEnumerable<string>
    {
        public string TextAnalyticsAPIKey { get; set; }

        public string TextAnalyticsEndpoint { get; set; }

        public string SpeechRecognitionAPIKey { get; set; }

        public string SpeechRecognitionRegion { get; set; }

        public IEnumerator<string> GetEnumerator()
        {
            return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            yield return this.TextAnalyticsAPIKey;
            yield return this.TextAnalyticsEndpoint;
            yield return this.SpeechRecognitionAPIKey;
            yield return this.SpeechRecognitionRegion;
        }
    }
}
