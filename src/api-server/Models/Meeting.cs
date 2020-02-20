using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TwentyFiveHours.API.Models
{
    public class MeetingModel : AbstractModel
    {
        public long Index { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;

        public IList<string> Keywords { get; set; } = new List<string>();

        public IList<string> Summary { get; set; } = new List<string>();

        public string RawTextLocation { get; set; } = string.Empty;

        public string RawAudioLocation { get; set; } = string.Empty;
    }
}
