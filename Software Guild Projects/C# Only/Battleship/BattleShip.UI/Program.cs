using BattleShip.BLL.Ships;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BattleShip.UI
{
    class Program
    {
        static void Main(string[] args)
        {
            //Use this later for placing ships
            //for(ShipType s = ShipType.Destroyer; s <= ShipType.Carrier; s++)
            //{
            //    bool success = PlaceShip(s);
            //    if (!success)
            //    {
            //        s--;
            //    }
            //}

            SWBattleshipStartScreen.SplashScreen();
            WorkflowSetup.SetupPlayer();
            WorkflowSetup.StartMatch();

        }
    }
}
