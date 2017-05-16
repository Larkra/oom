using Task3.Enums;

namespace Task3.Interfaces
{
    public interface IGame
    {
        string Title { get; }
        Genre Genre { get; }
        
        string GetDescription();
    }
}
