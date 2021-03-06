using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using HotChocolate.Execution;
using HotChocolate.Language;
using HotChocolate.Types;
using Newtonsoft.Json;
using Xunit;

namespace HotChocolate.Types
{
    public class IntrospectionTests
    {
        [Fact]
        public async Task TypeNameIntrospectionOnQuery()
        {
            // arrange
            Schema schema = CreateSchema();
            string query = "{ __typename }";

            // act
            QueryResult result = await schema.ExecuteAsync(query);

            // assert
            Assert.Null(result.Errors);
            Assert.Equal(Snapshot.Current(), Snapshot.New(result));
        }

        [Fact]
        public async Task TypeNameIntrospectionNotOnQuery()
        {
            // arrange
            Schema schema = CreateSchema();
            string query = "{ b { __typename } }";

            // act
            QueryResult result = await schema.ExecuteAsync(query);

            // assert
            Assert.Null(result.Errors);
            Assert.Equal(Snapshot.Current(), Snapshot.New(result));
        }

        [Fact]
        public async Task TypeIntrospectionOnQuery()
        {
            // arrange
            Schema schema = CreateSchema();
            string query = "{ __type (type: \"Foo\") { name } }";

            // act
            QueryResult result = await schema.ExecuteAsync(query);

            // assert
            Assert.Null(result.Errors);
            Assert.Equal(Snapshot.Current(), Snapshot.New(result));
        }

        [Fact]
        public async Task TypeIntrospectionOnQueryWithFields()
        {
            // arrange
            Schema schema = CreateSchema();
            string query =
                "{ __type (type: \"Foo\") " +
                "{ name fields { name type { name } } } }";

            // act
            QueryResult result = await schema.ExecuteAsync(query);

            // assert
            Assert.Null(result.Errors);
            Assert.Equal(Snapshot.Current(), Snapshot.New(result));
        }

        [Fact]
        public async Task ExecuteGraphiQLIntrospectionQuery()
        {
            // arrange
            Schema schema = CreateSchema();
            string query =
                FileResource.Open("IntrospectionQuery.graphql");

            // act
            QueryResult result = await schema.ExecuteAsync(query);

            // assert
            Assert.Null(result.Errors);
            Assert.Equal(Snapshot.Current(), Snapshot.New(result));
        }

        private static Schema CreateSchema()
        {
            return Schema.Create(c => c.RegisterType<Query>());
        }

        private class Query
            : ObjectType
        {
            protected override void Configure(IObjectTypeDescriptor descriptor)
            {
                descriptor.Field("a")
                    .Type<StringType>()
                    .Resolver(() => "a");

                descriptor.Field("b")
                    .Type<Foo>()
                    .Resolver(() => new object());
            }
        }

        private class Foo
            : ObjectType
        {
            protected override void Configure(IObjectTypeDescriptor descriptor)
            {
                descriptor.Field("a")
                    .Type<StringType>()
                    .Resolver(() => "foo.a");
            }
        }
    }
}
