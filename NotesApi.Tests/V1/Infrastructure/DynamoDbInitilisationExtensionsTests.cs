using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NotesApi.V1.Infrastructure;
using NUnit.Framework;
using System;
using System.Linq;

namespace NotesApi.Tests.V1.Infrastructure
{
    [TestFixture]
    public class DynamoDbInitilisationExtensionsTests
    {
        [TestCase(null)]
        [TestCase("false")]
        [TestCase("true")]
        public void ConfigureDynamoDBTestNoLocalModeEnvVarUsesAWSService(string localModeEnvVar)
        {
            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", localModeEnvVar);

            ServiceCollection services = new ServiceCollection();
            services.ConfigureDynamoDB();

            services.Any(x => x.ServiceType == typeof(IAmazonDynamoDB)).Should().BeTrue();
            services.Any(x => x.ServiceType == typeof(IDynamoDBContext)).Should().BeTrue();

            Environment.SetEnvironmentVariable("DynamoDb_LocalMode", null);
        }
    }
}
