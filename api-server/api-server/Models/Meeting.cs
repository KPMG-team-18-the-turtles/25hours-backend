using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TwentyFiveHours.API.Models
{
    public class MeetingModel : AbstractModel
    {
        public DateTime Date { get; set; }

        public IList<string> Keywords { get; set; }

        public IList<string> Summary { get; set; }

        public MeetingModel() { }
    }

    public class MeetingDatabaseSettings : IMongoDatabaseSettings
    {
        public string ModelCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
