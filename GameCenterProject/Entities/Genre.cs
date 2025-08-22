using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Common;

namespace GameCenterProject.Entities
{
    public class Genre : Entity<int>
    {
        public string Name { get; private set; } = default!;

        private Genre() { }
        public Genre(string name)
        {
            Name = name;
        }
    }
}