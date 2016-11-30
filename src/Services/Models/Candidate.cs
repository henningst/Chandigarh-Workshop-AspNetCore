using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Services.Models
{
    public class Candidate
    {
        [BsonId]
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string FavoriteColor { get; set; }
    }
}
