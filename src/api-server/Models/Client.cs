using System;
using System.Collections.Generic;
using System.Drawing;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TwentyFiveHours.API.Models
{
    public class ClientModel : AbstractModel
    {
        public class AgendaItem
        {
            public string Content { get; set; }

            public bool Completed { get; set; }
        }

        public string Name { get; set; }

        public string Contact { get; set; }

        public string ProfileImagePath { get; set; }

        public IList<MeetingModel> Meetings { get; set; } = new List<MeetingModel>();

        public IList<AgendaItem> Agenda { get; set; } = new List<AgendaItem>();
    }

    public class ClientDatabaseSettings : IMongoDatabaseSettings
    {
        public string ModelCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
