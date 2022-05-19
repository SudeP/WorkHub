using Api.Models.ResponseModel.Models;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Api.Models.Structs
{
    public class Entity : IHaveCustomMapping
    {
#pragma warning disable
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
#pragma warning restore
        public int Identity { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<Entity, ResultCreate>();
        }
    }
}
