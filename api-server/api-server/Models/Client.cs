using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TwentyFiveHours.API.Models
{
    public class ClientModel : AbstractModel
    {
        public string Name { get; set; }

        public string Contact { get; set; }

        public DateTime LastMeetingDate { get; set; }

        public IList<MeetingModel> Meetings { get; set; } = new List<MeetingModel>();

        public ClientModel() { }
    }

    public class ClientDatabaseSettings : IMongoDatabaseSettings
    {
        public string ModelCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
