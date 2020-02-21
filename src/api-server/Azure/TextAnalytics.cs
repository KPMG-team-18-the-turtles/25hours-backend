using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using OpenTextSummarizer;
using OpenTextSummarizer.Interfaces;

namespace TwentyFiveHours.API.Azure
{
    internal class TextAnalyticsWrapper : IDisposable
    {
        private class StringProvider : IContentProvider
        {
            public string Content { get; set; }
        }

        private readonly TextAnalyticsClient _client;

        public TextAnalyticsWrapper(string key, string endpoint)
        {
            var credential = new APIKeyServiceClientCredentials(key);
            this._client = new TextAnalyticsClient(credential)
            {
                Endpoint = endpoint
            };
        }

        public string[] GetKeyPhrasesFromFile(string path)
        {
            var text = new StreamReader(path);
            var result = this._client.KeyPhrases(text.ReadToEnd());
            text.Dispose();
            return result.KeyPhrases.ToArray();
        }

        public string[] GetSummariesFromFile(string path, int max)
        {
            var text = new StreamReader(path);
            var summary = Summarizer.Summarize(
                new StringProvider { Content = text.ReadToEnd() },
                new SummarizerArguments { Language = "en", MaxSummarySentences = max }
            );
            text.Dispose();
            return summary.Sentences.ToArray();
        }

        public void Dispose()
        {
            this._client.Dispose();
        }
    }
}
