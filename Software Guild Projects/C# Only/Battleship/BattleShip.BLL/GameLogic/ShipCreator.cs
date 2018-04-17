using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BattleShip.BLL.Ships;

namespace BattleShip.BLL.GameLogic
{
    public class ShipCreator
    {
        public static Ship CreateShip(ShipType type)
        {
            switch (type)
            {
                case ShipType.XWing:
                    return new Ship(ShipType.XWing, 2);
                case ShipType.AWing:
                    return new Ship(ShipType.AWing, 3);
                case ShipType.JediStarfighter:
                    return new Ship(ShipType.JediStarfighter, 3);
                case ShipType.MilleniumFalcon:
                    return new Ship(ShipType.MilleniumFalcon, 4);
                case ShipType.JediStarDestroyer:
                    return new Ship(ShipType.JediStarDestroyer, 5);
                case ShipType.TieFighter:
                    return new Ship(ShipType.TieFighter, 2);
                case ShipType.TieBomber:
                    return new Ship(ShipType.TieBomber, 3);
                case ShipType.TieInterceptor:
                    return new Ship(ShipType.TieInterceptor, 3);
                case ShipType.ImperialInterceptor:
                    return new Ship(ShipType.ImperialInterceptor, 4);
                default:
                    return new Ship(ShipType.SithStarDestroyer, 5);
            }
        }
    }
}
