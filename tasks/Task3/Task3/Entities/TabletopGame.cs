using System;
using Task3.Enums;
using Task3.Extensions;
using Task3.Interfaces;

namespace Task3.Entities
{
    public class TabletopGame : IGame
    {
        private string _title;

        #region properties
        public string Title
        {
            get { return _title; }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Title cannot be null or whitespace.", nameof(value));
                _title = value;
            }

        }
        public Genre Genre { get; }
        #endregion

        /// <summary>
        /// Creates a new tabletop game object
        /// </summary>
        /// <param name="title">The title of the game</param>
        /// <param name="genre">The genre of the game</param>
        public TabletopGame(string title, Genre genre)
        {
            Title = title;
            Genre = genre;
        }

        public string GetDescription()
        {
            return $"Title: {Title}\nGenre: {Genre.GetDisplayName()}";
        }
    }
}
