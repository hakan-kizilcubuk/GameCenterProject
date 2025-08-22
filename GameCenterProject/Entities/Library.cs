using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Common;

namespace GameCenterProject.Entities
{
    public class Library
    {
        public string UserId { get; private set; }
        public List<Game> OwnedGames { get; private set; } = new List<Game>();
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public Library(string userId)
        {
            UserId = userId;
        }

        public void AddGame(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            OwnedGames.Add(game);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveGame(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            OwnedGames.Remove(game);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}