using System;
using System.IO;
using Xunit;

using Dogged;

namespace Dogged.Tests
{
    /// <summary>
    /// Tests for Object ID manipulation.
    /// </summary>
    public class ObjectIdTest : TestBase
    {
        [Theory]
        [InlineData("0123456789012345678901234567890123456789")]
        [InlineData("deadbeefdeadbeefdeadbeefdeadbeefdeadbeef")]
        [InlineData("cafebabecafebabecafebabecafebabecafebabe")]
        [InlineData("DeadBeefDeadBeefDeadBeefDeadBeefDeadBeef")]
        public void CanCreateFromString(string hex)
        {
            new ObjectId(hex);
        }

        [Theory]
        [InlineData("")]
        [InlineData("42")]
        [InlineData("012345678901234567890123456789012345678")]
        [InlineData("01234567890123456789012345678901234567890")]
        [InlineData("012345678901234567890123456789012345678z")]
        [InlineData("0123456789012345678901234567890123456789z")]
        public void ThrowsOnInvalidInput(string input)
        {
            Assert.Throws<ArgumentException>(() => new ObjectId(input));
        }

        [Theory]
        [InlineData("0000000000000000000000000000000000000000")]
        [InlineData("0123456789012345678901234567890123456789")]
        public void CanConvertToString(string id)
        {
            Assert.Equal(id, new ObjectId(id).ToString());
        }

        [Theory]
        [InlineData("0000000000000000000000000000000000000000", "0000000000000000000000000000000000000000")]
        [InlineData("DeadBeefDeadBeefDeadBeefDeadBeefDeadBeef", "deadbeefdeadbeefdeadbeefdeadbeefdeadbeef")]
        public void HexStringIsLowercase(string given, string expected)
        {
            Assert.Equal(expected, new ObjectId(given).ToString());
        }

        [Theory]
        [InlineData("0000000000000000000000000000000000000000", "0000000000000000000000000000000000000000")]
        [InlineData("DeadBeefDeadBeefDeadBeefDeadBeefDeadBeef", "deadbeefdeadbeefdeadbeefdeadbeefdeadbeef")]
        [InlineData("0123456789012345678901234567890123456789", "0123456789012345678901234567890123456789")]
        public void CanCompareObjectIdsForEquality(string one, string two)
        {
            Assert.Equal(new ObjectId(one), new ObjectId(two));
        }

        [Theory]
        [InlineData("0000000000000000000000000000000000000000", "0000000000000000000000000000000000000001")]
        [InlineData("0123456789012345678901234567890123456789", "0123456789012345678901234567890123456788")]
        [InlineData("cafebabecafebabecafebabecafebabecafebabe", "deadbeefdeadbeefdeadbeefdeadbeefdeadbeef")]
        public void CanCompareObjectIdsForInequality(string one, string two)
        {
            Assert.NotEqual(new ObjectId(one), new ObjectId(two));
        }
    }
}
