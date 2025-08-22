using System;
using System.Collections.Generic;
using System.Text;
using GameCenterProject.Common;
using GameCenterProject.Entities;
using GameCenterProject.ValueObjects;

namespace GameCenterProject.Entities
{
    public class CartItem
    {
        public string GameId { get; private set; }
        public string GameEditionId { get; private set; }
        public int Quantity { get; private set; } = 1;
        public Money Price { get; private set; }

        public string GameName { get; private set; }

        public CartItem(string gameId, string gameEditionId, int quantity, Money price, string gameName)
        {
            GameId = gameId;
            GameEditionId = gameEditionId;
            Quantity = quantity;
            Price = price;
            GameName = gameName;
        }
    }
}