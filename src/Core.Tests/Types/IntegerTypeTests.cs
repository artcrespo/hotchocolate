using HotChocolate.Language;
using Xunit;

namespace HotChocolate.Types
{
    public class IntegerTypeTests
    {
        [Fact]
        public void ParseLiteral()
        {
            // arrange
            IntValueNode literal = new IntValueNode(null, "12345");

            // act
            IntType integerType = new IntType();
            object result = integerType.ParseLiteral(literal);

            // assert
            Assert.IsType<int>(result);
            Assert.Equal(12345, result);
        }

        [Fact]
        public void IsInstanceOfType()
        {
            // arrange
            IntValueNode intLiteral = new IntValueNode(null, "12345");
            StringValueNode stringLiteral = new StringValueNode(null, "12345", false);
            NullValueNode nullLiteral = new NullValueNode(null);

            // act
            IntType integerType = new IntType();
            bool isIntLiteralInstanceOf = integerType.IsInstanceOfType(intLiteral);
            bool isStringLiteralInstanceOf = integerType.IsInstanceOfType(stringLiteral);
            bool isNullLiteralInstanceOf = integerType.IsInstanceOfType(nullLiteral);

            // assert
            Assert.True(isIntLiteralInstanceOf);
            Assert.False(isStringLiteralInstanceOf);
            Assert.True(isNullLiteralInstanceOf);
        }

        [Fact]
        public void EnsureIntTypeKindIsCorret()
        {
            // arrange
            IntType type = new IntType();

            // act
            TypeKind kind = type.Kind;

            // assert
            Assert.Equal(TypeKind.Scalar, type.Kind);
        }
    }
}
