using System;
using GraphQL.Client.Extensions;
using NUnit.Framework;

namespace GraphQLExtensionsTest01
{
    public class QueryTest
    {
        [Test]
        public void AddField_TestInHierarchicalQuery()
        {
            var query = new Query<DummyDto>("dummy")
                .AddField(dto => dto.Key)
                .AddField(dto => dto.Values);

            Assert.That(query.Build(), Is.EqualTo("dummy{Key Values}"));
        }

        [Test]
        public void AddArgument_TestInHierarchicalQuery()
        {
            var query = new Query<DummyDto>("dummy")
                .AddField(dto => dto.Key)
                .AddField(dto => dto.Values, sq => sq.AddArgument("match", "blabla"));

            Assert.That(query.Build(), Is.EqualTo("dummy{Key Values(match:\"blabla\")}"));
        }

        [Test]
        public void AddArgument_TestExceptionInHierarchicalQuery()
        {
            var query = new Query<DummyDto>("dummy")
                .AddField(dto => dto.Key)
                .AddField(dto => dto.SubObject, sq => sq.AddArgument("match", "blabla"));

            Assert.That(() => query.Build(), Throws.Exception.TypeOf<ArgumentException>());
        }


        [Test]
        public void AddArgument_TestInHierarchicalQueryWithSubobject()
        {
            var query = new Query<DummyDto>("dummy")
                .AddField(dto => dto.Key)
                .AddField(dto => dto.SubObject
                    , sq => sq.AddField(dto2=>dto2.Value)
                        .AddArgument("match", "blabla"));

            Assert.That(query.Build(), Is.EqualTo("dummy{Key SubObject(match:\"blabla\"){Value}}"));
        }

        private class DummyDto
        {
            public string Key { get; set; }

            public string[] Values { get; set; }

            public SubDto SubObject { get; set; }
        }

        private class SubDto
        {
            public int Value { get; set; }
        }
    }
}