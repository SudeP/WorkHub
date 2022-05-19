using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Api.Models.Entities.Identities
{
    public class Identity : IEntity
    {
        [BsonId]
#pragma warning disable
        public ObjectId _id { get; set; }
#pragma warning restore
        public string Key { get; set; }
        public int Value { get; set; }
    }
}
