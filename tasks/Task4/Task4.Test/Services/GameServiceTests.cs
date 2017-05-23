using System;
using System.Linq;
using FluentAssertions;
using Moq;
using Task4.Entities;
using Task4.Repositories;
using Task4.Services;
using Task4.Test.Xunit;
using Xunit;

namespace Task4.Test.Services
{
    [TestCaseOrderer("Task4.Test.Xunit.PriorityOrderer", "Task4.Test")]
    public class GameServiceTests
    {
        private readonly INewsService _newsService;
        private readonly IGameRepository _gameRepository;

        public GameServiceTests()
        {
            _newsService = new Mock<INewsService>().Object;
            _gameRepository = new Mock<IGameRepository>().Object;
        }

        [Fact, TestPriority(1)]
        public void GetGamesByGenre_GamesWithDifferentGenreAvailable_ReturnsOnlyGamesWithExpectedGenre()
        {
            // Arrange
            const Genre expectedGenre = Genre.CityBuilder;

            var gameRepositoryMock = new Mock<IGameRepository>();
            gameRepositoryMock.Setup(svc => svc.GetAllGames()).Returns(TestData.GameMocks.AllGamesMock);

            var service = new GameService(_newsService, gameRepositoryMock.Object);

            // Act
            var ret = service.GetGamesByGenre(expectedGenre).ToList();

            // Assert
            ret.Should().HaveCount(2);
            ret.All(g => g.Genre.HasFlag(expectedGenre)).Should().BeTrue();
        }

        [Theory, TestPriority(1)]
        [MemberData(nameof(TestData.GameMocks.GetInvalidGameMocks), MemberType = typeof(TestData.GameMocks))]
        public void AddGame_InvalidGameGiven_ThrowsArgumentException(Game invalidGame)
        {
            //Arrange
            var gameRepoMock = new Mock<IGameRepository>();
            gameRepoMock.Setup(ret => ret.GetAllGames()).Returns(TestData.GameMocks.AllGamesMock);

            var service = new GameService(_newsService, gameRepoMock.Object);

            //Act
            Action action = () => { service.AddGame(invalidGame); };


            //Assert
            action.ShouldThrowExactly<ArgumentException>("Because the games are not valid");
        }

        [Fact, TestPriority(-1)]
        public void UpdateGameInfo_GameToUpdateExists_UpdatesGameInfo()
        {
            //Arrange
            const string newTitle = "UPDATED!";

            var gameRepoMock = new Mock<IGameRepository>();
            gameRepoMock.Setup(ret => ret.GetAllGames()).Returns(TestData.GameMocks.AllGamesMock);

            var service = new GameService(_newsService, gameRepoMock.Object);
            var gameExists = TestData.GameMocks.AllGamesMock.FirstOrDefault();

            //Act
            gameExists.Title = newTitle;

            service.UpdateGameInfo(gameExists);
            var allGamesAfterUpdate = service.GetAllGames();
            var gameAfterUpdate = allGamesAfterUpdate.FirstOrDefault(g => g.AppId == gameExists.AppId);

            //Assert
            allGamesAfterUpdate.Should().HaveCount(TestData.GameMocks.AllGamesMock.Count());

            gameAfterUpdate.Title.Should().BeEquivalentTo(newTitle, "Because the game was successfully updated");
        }

        #region DeleteGame

        [Theory, TestPriority(2)]
        [MemberData(nameof(TestData.GameMocks.GetGamesThatDoNotExist), MemberType = typeof(TestData.GameMocks))]
        public void DeleteGame_GamesToDeleteDoNotExist_ThrowsNoException(Game gameDoesNotExist)
        {
            //Arrange
            var gameRepoMock = new Mock<IGameRepository>();
            gameRepoMock.Setup(ret => ret.GetAllGames()).Returns(TestData.GameMocks.AllGamesMock);

            var service = new GameService(_newsService, gameRepoMock.Object);

            //Act
            Action action = () => { service.DeleteGame(gameDoesNotExist); };


            //Assert
            action.ShouldNotThrow("Because the games are not valid");
        }

        [Fact, TestPriority(2)]
        public void DeleteGame_GameToDeleteExists_ThrowsNoException()
        {
            //Arrange
            var gameRepoMock = new Mock<IGameRepository>();
            gameRepoMock.Setup(ret => ret.GetAllGames()).Returns(TestData.GameMocks.AllGamesMock);

            var service = new GameService(_newsService, gameRepoMock.Object);

            var gameExists = TestData.GameMocks.AllGamesMock.FirstOrDefault();

            //Act
            service.DeleteGame(gameExists);
            var allGamesAfterDelete = service.GetAllGames();

            //Assert
            allGamesAfterDelete.Should().HaveCount(TestData.GameMocks.AllGamesMock.Count() - 1, "Because one game was successfully deleted");
            allGamesAfterDelete.Any(g => g.AppId == gameExists?.AppId).Should().BeFalse("Because the game with the given AppId was successfully deleted");
        }
        #endregion
        
    }
}
