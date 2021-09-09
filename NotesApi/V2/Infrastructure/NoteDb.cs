using System;
using Amazon.DynamoDBv2.DataModel;
using Hackney.Core.DynamoDb.Converters;
using NotesApi.V2.Domain;

namespace NotesApi.V2.Infrastructure
{
    [DynamoDBTable("Notes", LowerCamelCaseProperties = true)]
    public class NoteDb
    {
        [DynamoDBRangeKey]
        public Guid Id { get; set; }

        [DynamoDBHashKey]
        public Guid TargetId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbEnumConverter<TargetType>))]
        public TargetType TargetType { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime CreatedAt { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbObjectConverter<Categorisation>))]
        public Categorisation Categorisation { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbObjectConverter<AuthorDetails>))]
        public AuthorDetails Author { get; set; }

        public bool Highlight { get; set; }
    }
}
