using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TwentyFiveHours.API.Models
{
    public class MeetingModel : AbstractModel
    {
        public class SentimentalSentence
        {
            public string Positive { get; set; }
            
            public string Negative { get; set; }
        }

        public long Index { get; set; }

        public DateTime Date { get; set; }

        public IList<string> Keywords { get; set; } = new List<string>();

        public IList<string> Summary { get; set; } = new List<string>();

        public SentimentalSentence Emotives { get; set; } = new SentimentalSentence { Positive = string.Empty, Negative = string.Empty };

        public string RawTextLocation { get; set; } = string.Empty;

        public string RawAudioLocation { get; set; } = string.Empty;
    }
}
