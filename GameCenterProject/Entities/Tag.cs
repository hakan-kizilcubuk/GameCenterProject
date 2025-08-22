using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Common;

namespace GameCenterProject.Entities
{
    public class Tag : Entity<int>
    {
        public string Name { get; private set; } = default!;

        private Tag() { }
        public Tag(string name)
        {
            Name = name;
        }
    }
}