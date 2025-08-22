using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Common;
using GameCenterProject.ValueObjects;

namespace GameCenterProject.Entities
{
    public class GameEdition : Entity<Guid>
    {
        public string Name { get; private set; } = default!;
        public string Description { get; private set; } = default!;
        public Money Price { get; private set; } = Money.Zero;

        private GameEdition() { }

        public GameEdition(string name, string description, Money price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
    }
}