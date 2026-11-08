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
        [InlineData("0123456789012345678901234567890123456789", ObjectIdType.SHA1)]
        [InlineData("deadbeefdeadbeefdeadbeefdeadbeefdeadbeef", ObjectIdType.SHA1)]
        [InlineData("cafebabecafebabecafebabecafebabecafebabe", ObjectIdType.SHA1)]
        [InlineData("DeadBeefDeadBeefDeadBeefDeadBeefDeadBeef", ObjectIdType.SHA1)]
        public void CanCreateFromString(string hex, ObjectIdType type)
        {
            new ObjectId(hex, type);
        }

        [Theory]
        [InlineData("", ObjectIdType.SHA1)]
        [InlineData("42", ObjectIdType.SHA1)]
        [InlineData("012345678901234567890123456789012345678", ObjectIdType.SHA1)]
        [InlineData("01234567890123456789012345678901234567890", ObjectIdType.SHA1)]
        [InlineData("012345678901234567890123456789012345678z", ObjectIdType.SHA1)]
        [InlineData("0123456789012345678901234567890123456789z", ObjectIdType.SHA1)]
        public void ThrowsOnInvalidInput(string input, ObjectIdType type)
        {
            Assert.Throws<ArgumentException>(() => new ObjectId(input, type));
        }

        [Theory]
        [InlineData("0000000000000000000000000000000000000000", ObjectIdType.SHA1)]
        [InlineData("0123456789012345678901234567890123456789", ObjectIdType.SHA1)]
        public void CanConvertToString(string id, ObjectIdType type)
        {
            Assert.Equal(id, new ObjectId(id, type).ToString());
        }

        [Theory]
        [InlineData("0000000000000000000000000000000000000000", "0000000000000000000000000000000000000000", ObjectIdType.SHA1)]
        [InlineData("DeadBeefDeadBeefDeadBeefDeadBeefDeadBeef", "deadbeefdeadbeefdeadbeefdeadbeefdeadbeef", ObjectIdType.SHA1)]
        public void HexStringIsLowercase(string given, string expected, ObjectIdType type)
        {
            Assert.Equal(expected, new ObjectId(given, type).ToString());
        }

        [Theory]
        [InlineData("0000000000000000000000000000000000000000", ObjectIdType.SHA1, "0000000000000000000000000000000000000000", ObjectIdType.SHA1)]
        [InlineData("DeadBeefDeadBeefDeadBeefDeadBeefDeadBeef", ObjectIdType.SHA1, "deadbeefdeadbeefdeadbeefdeadbeefdeadbeef", ObjectIdType.SHA1)]
        [InlineData("0123456789012345678901234567890123456789", ObjectIdType.SHA1, "0123456789012345678901234567890123456789", ObjectIdType.SHA1)]
        public void CanCompareObjectIdsForEquality(string one, ObjectIdType oneType, string two, ObjectIdType twoType)
        {
            Assert.Equal(new ObjectId(one, oneType), new ObjectId(two, twoType));
        }

        [Theory]
        [InlineData("0000000000000000000000000000000000000000", ObjectIdType.SHA1, "0000000000000000000000000000000000000001", ObjectIdType.SHA1)]
        [InlineData("0123456789012345678901234567890123456789", ObjectIdType.SHA1, "0123456789012345678901234567890123456788", ObjectIdType.SHA1)]
        [InlineData("cafebabecafebabecafebabecafebabecafebabe", ObjectIdType.SHA1, "deadbeefdeadbeefdeadbeefdeadbeefdeadbeef", ObjectIdType.SHA1)]
        public void CanCompareObjectIdsForInequality(string one, ObjectIdType oneType, string two, ObjectIdType twoType)
        {
            Assert.NotEqual(new ObjectId(one, oneType), new ObjectId(two, twoType));
        }
    }
}
