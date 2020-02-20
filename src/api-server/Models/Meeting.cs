using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TwentyFiveHours.API.Models
{
    public class MeetingModel : AbstractModel
    {
        public long Index { get; set; }

        public DateTime Date { get; set; }

        public IList<string> Keywords { get; set; }

        public IList<string> Summary { get; set; }
    }
}
