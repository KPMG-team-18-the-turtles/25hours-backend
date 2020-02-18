using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TwentyFiveHours.API.Models
{
    public abstract class AbstractModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ID { get; set; }
    }
}
