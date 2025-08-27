using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Common;
using GameCenterProject.ValueObjects;

namespace GameCenterProject.Entities
{
    public class Game : Entity<Guid>, IAggregateRoot
    {
        private readonly List<GameEdition> _editions = new();
        private readonly List<Tag> _tags = new();
        private readonly List<Genre> _genres = new();
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public Money Price { get; private set; } = Money.Zero;
    public string ImageUrl { get; set; } = default!;
    public int ReleaseDate { get; private set; }

        private Game() { }

        public Game(string title, string description, Money price, int releaseDate)
        {
            Title = title;
            Description = description;
            Price = price;
            ReleaseDate = releaseDate;
        }

        public void AddEdition(GameEdition edition)
        {
            if (edition == null) throw new ArgumentNullException(nameof(edition));
            _editions.Add(edition);
        }

        public void AddTag(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            _tags.Add(tag);
        }

        public void AddGenre(Genre genre)
        {
            if (genre == null) throw new ArgumentNullException(nameof(genre));
            _genres.Add(genre);
        }

        public void ChangePrice(Money newPrice)
        {
            if (newPrice == null) throw new ArgumentNullException(nameof(newPrice));
            Price = newPrice;
        }

        public void Rename(string name)
        {
            Title = name;
        }

        public void UpdateDescription(string description)
        {
            Description = description;
        }

        public void UpdateReleaseDate(int releaseDate)
        {
            if (releaseDate <= 0) throw new ArgumentOutOfRangeException(nameof(releaseDate), "Release date must be a positive integer.");
            ReleaseDate = releaseDate;
        }

        public void RemoveTag(Tag tag)
        {
            if (tag == null) throw new ArgumentNullException(nameof(tag));
            _tags.Remove(tag);
        }

        public void RemoveGenre(Genre genre)
        {
            if (genre == null) throw new ArgumentNullException(nameof(genre));
            _genres.Remove(genre);
        }
    }
}