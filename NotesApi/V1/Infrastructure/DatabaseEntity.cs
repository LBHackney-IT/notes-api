using Amazon.DynamoDBv2.DataModel;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace NotesApi.V1.Infrastructure
{
    //TODO: rename table and add needed fields relating to the table columns.
    // There's an example of this in the wiki https://github.com/LBHackney-IT/lbh-base-api/wiki/DatabaseContext
    [Table("example_table")]
    [DynamoDBTable("example_table", LowerCamelCaseProperties = true)]
    public class DatabaseEntity
    {
        [Column("id")]
        [DynamoDBHashKey]
        public int Id { get; set; }

        [Column("created_at")]
        [DynamoDBProperty]
        public DateTime CreatedAt { get; set; }

    }
}
