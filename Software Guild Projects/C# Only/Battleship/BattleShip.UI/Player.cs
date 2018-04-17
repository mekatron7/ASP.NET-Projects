using BattleShip.BLL.GameLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.UI
{
    class Player
    {
        public string Name { get; }
        public int PNumber { get; }
        public string Side { get; }
        public Board PlayerBoard { get; set; }

        public Player(string name, string side, Board board)
        {
            Name = name;
            Side = side;
            PlayerBoard = board;
        }

    }
}
