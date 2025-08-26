using System;
using System.Collections.Generic;
using System.Text;

namespace GameCenterProject.Common
{
    public abstract class Entity<TId>
    {
        public TId Id { get; private set; } = default!;

        protected Entity(TId id)
        {
            Id = id;
        }
        protected Entity() { }
    }
}