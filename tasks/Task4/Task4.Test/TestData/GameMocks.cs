using System.Collections.Generic;
using Task4.Entities;

namespace Task4.Test.TestData
{
    public static class GameMocks
    {
        public static IEnumerable<Game> AllGamesMock => new List<Game>
        {
            new Game { AppId = 0, Genre = Genre.CityBuilder     , Title = "Cities Skylines"},
            new Game { AppId = 1, Genre = Genre.GraphicAdventure, Title = "Myst"},
            new Game { AppId = 2, Genre = Genre.Mmorpg          , Title = "Ultima Online"},
            new Game { AppId = 3, Genre = Genre.CityBuilder     , Title = "Sim City 2000"}
        };

        public static IEnumerable<object[]> GetGamesThatDoNotExist()
        {
            yield return new object[] { new Game { AppId = -1 , Genre = Genre.CityBuilder, Title = "Sim City 2000" } };
            yield return new object[] { new Game { AppId = 201, Genre = Genre.CityBuilder, Title = "Sim City 4" } };
            yield return new object[] { new Game { AppId = 202, Genre = Genre.CityBuilder, Title = "SC 5" } };
        }

        public static IEnumerable<object[]> GetInvalidGameMocks()
        {
            yield return new object[] { new Game { AppId = -1, Genre = Genre.CityBuilder, Title = "Sim City 2000" } }; //AppId is negative
            yield return new object[] { new Game { AppId = 900, Title = string.Empty } };                              //Title is empty string
            yield return new object[] { new Game { AppId = 901 } };                                                    //Title is null
            yield return new object[] { new Game { AppId = 3, Genre = Genre.CityBuilder, Title = "SC 5" } };           //Duplicate AppId
        }
        
    }
}
