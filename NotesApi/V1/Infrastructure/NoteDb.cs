using Amazon.DynamoDBv2.DataModel;
using NotesApi.V1.Domain;
using System;

namespace NotesApi.V1.Infrastructure
{
    [DynamoDBTable("Notes", LowerCamelCaseProperties = true)]
    public class NoteDb
    {
        [DynamoDBRangeKey]
        public Guid Id { get; set; }

        [DynamoDBHashKey]
        public Guid TargetId { get; set; }

        public string Description { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbEnumConverter<TargetType>))]
        public TargetType TargetType { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbDateTimeConverter))]
        public DateTime DateTime { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbObjectConverter<Categorisation>))]
        public Categorisation Categorisation { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbObjectConverter<AuthorDetails>))]
        public AuthorDetails Author { get; set; }

        [DynamoDBProperty(Converter = typeof(DynamoDbEnumConverter<Tag>))]
        public Tag Tags { get; set; }
    }
}
