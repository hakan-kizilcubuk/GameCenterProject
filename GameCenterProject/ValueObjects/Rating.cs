using System;
using System.Collections.Generic;
using System.Text;

namespace GameCenterProject.ValueObjects
{
    public sealed record Rating
    {
        public string GameId { get; }
        public int Rate { get; }

        public Rating(string gameId, int rate)
        {
            GameId = gameId;
            Rate = rate;
        }
    }
}