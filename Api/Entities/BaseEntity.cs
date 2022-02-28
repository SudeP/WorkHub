using Api.ASPNET.Service.Inheritance;
using Api.Models.ResponseModel.Models;
using AutoMapper;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Api.Entities
{
    public class BaseEntity : IHaveCustomMapping
    {
        [BsonId]
#pragma warning disable
        public ObjectId _id { get; set; }
#pragma warning restore
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public bool IsDelete { get; set; }
        public int CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public int UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual void CreateMappings(Profile configuration)
        {
            configuration.CreateMap<BaseEntity, ResultCreate>();
        }
    }
}
