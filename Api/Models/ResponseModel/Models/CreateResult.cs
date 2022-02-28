using MongoDB.Bson;

namespace Api.Models.ResponseModel.Models
{
    public class ResultCreate
    {
        public int Id { get; set; }
        public ObjectId _id { get; set; }
    }
}
