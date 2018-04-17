using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.UI
{
    class GameState
    {
        public Player P1 { get; set; }
        public Player P2 { get; set; }

        public GameState(Player p1, Player p2)
        {
            P1 = p1;
            P2 = p2;
        }
    }
}
