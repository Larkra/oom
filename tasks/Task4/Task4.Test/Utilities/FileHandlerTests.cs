using System;
using FluentAssertions;
using Task4.Utilities;
using Xunit;

namespace Task4.Test.Utilities
{
    public class FileHandlerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        [InlineData("DoesNotExist.xxx")]
        public void ReadFromFile_FileDoesNotExist_ReturnsEmptyString(string fileName)
        {
            //Arrange
            var service = new FileHandler();

            //Act
            Action action = () => { service.ReadFromFile(fileName); };

            //Assert
            action.ShouldNotThrow("Because having no file should not raise an exception.");
        }

    }
}
