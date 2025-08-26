using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Common;
using GameCenterProject.ValueObjects;

namespace GameCenterProject.Entities
{
    public class Cart : Entity<Guid>
    {
        public string UserId { get; private set; } = string.Empty;
        public List<Rating> Ratings { get; private set; } = new List<Rating>();
        public Money TotalPrice { get; private set; } = Money.Zero;
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
        public List<Game> Games { get; private set; } = new List<Game>();

        private Cart() { }

        public Cart(string userId)
            : base(Guid.NewGuid())
        {
            UserId = userId;
        }

        public void AddRating(Rating rating)
        {
            if (rating == null) throw new ArgumentNullException(nameof(rating));
            Ratings.Add(rating);
        }

        public void AddGame(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            Games.Add(game);
            TotalPrice = TotalPrice.Add(game.Price);
            UpdatedAt = DateTime.UtcNow;
        }

        public void RemoveGame(Game game)
        {
            if (game == null) throw new ArgumentNullException(nameof(game));
            Games.Remove(game);
            TotalPrice = TotalPrice.Subtract(game.Price);
            UpdatedAt = DateTime.UtcNow;
        }
    }
}