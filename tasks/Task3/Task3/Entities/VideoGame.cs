﻿using System;
using Task3.Enums;
using Task3.Extensions;
using Task3.Interfaces;

namespace Task3.Entities
{
    public class VideoGame : IGame
    {
        private string _title;
        private int _playThroughs;

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
        public Platform Platform { get; }

        public int PlayThroughs => _playThroughs;
        #endregion
        
        /// <summary>
        /// Creates a new game object
        /// </summary>
        /// <param name="title">The title of the game</param>
        /// <param name="genre">The genre of the game</param>
        /// <param name="platform">All platforms the game is available on</param>
        public VideoGame(string title, Genre genre, Platform platform)
        {
            Title = title;
            Genre = genre;
            Platform = platform;
        }
        
        public string GetDescription()
        {
            return $"Title: {Title}\nGenre: {Genre.GetDisplayName()}\nPlatform: {Platform}\nPlayThroughs: {PlayThroughs}";
        }

        public void RaisePlayThroughs()
        {
            _playThroughs++;
        }

    }
}
