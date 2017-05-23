using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Task4.Repositories;
using Task4.Utilities;
using Xunit;

namespace Task4.Test.Repositories
{
    public class GameRepositoryTests
    {
        [Theory]
        [InlineData("lorem ipsum")]
        [InlineData("xyz")]
        [InlineData("<NotAValidJson>")]
        public void GetAllGames_JsonStringIsInvalid_ThrowsFormatException(string json)
        {
            // Arrange
            var fileHandlerMock = new Mock<IFileHandler>();
            fileHandlerMock.Setup(ret => ret.ReadFromFile(It.IsAny<string>())).Returns(json);

            var service = new GameRepository(fileHandlerMock.Object);
            
            //Act
            Action action = () =>
            {
                service.GetAllGames();
            };

            //Assert
            action.ShouldThrowExactly<FormatException>("Because only valid json can be deserialized.");
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("[]")]
        [InlineData("{}")]
        [InlineData("{ \"Foo\" : \"bar\"}")]
        public void GetAllGames_JsonStringIsValidButEmptyOrNotDeserializable_ReturnsEmptyCollection(string json)
        {
            // Arrange
            var fileHandlerMock = new Mock<IFileHandler>();
            fileHandlerMock.Setup(ret => ret.ReadFromFile(It.IsAny<string>())).Returns(json);

            var service = new GameRepository(fileHandlerMock.Object);

            //Act
            var result = service.GetAllGames().ToList();

            //Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(0);
        }
        
    }
}
