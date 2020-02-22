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
            var entire = text.ReadToEnd();
            var result = new Dictionary<string, int>();
            System.Diagnostics.Debug.WriteLine(entire);

            foreach (var sentence in entire.Split('.', '?', '!'))
            {
                try
                {
                    var keywords = this._client.KeyPhrases(sentence);
                    foreach (var keyword in keywords.KeyPhrases)
                    {
                        if (result.ContainsKey(keyword))
                            result[keyword] += 1;
                        else
                            result.Add(keyword, 0);
                        System.Diagnostics.Debug.WriteLine(keyword);
                    }
                }
                catch (Exception e)
                {
                    // Do nothing
                }
            }

            text.Dispose();

            return (from item in result
                    orderby item.Value descending
                    select item.Key).Take(15).ToArray();
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
