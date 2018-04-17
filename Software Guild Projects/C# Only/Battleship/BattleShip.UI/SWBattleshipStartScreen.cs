using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BattleShip.UI
{
    static class SWBattleshipStartScreen
    {
        public static void SplashScreen()
        {
            Console.SetWindowSize(70, 64);
            Console.WriteLine(@"DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDN$ 
DDNDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDND$ 
DDD......... . ........................  .      .      ZDD$ 
DDD.  .... . .  ..  ..  .   . . .  . .... .  .  .  ....$DD$ 
DDD.~ND..DN$  DN.?DDDD~D?.DD.NDDI.DDND... DD DNDD .....$DD$ 
DDD.~NN7.ND$.=ND8 .DD..D?.DD.D~~N.NN. .. DDD. .D8  N...$DD$ 
DDD ~DID?NN$ DD7D  ND. D?.DD.DDN8 NNND   .ND  .D~=?N??.$DD$ 
DDD.~D.DDID$ NO$D~.ND. NI.DD.N~7D.DD...  .NN..ZD.+?N??.$DD$ 
DDD.~D.DD.D$+D..DD NN..DO N$.D~~N.DD.   ..ND..D$.. N...$DD$ 
DDD.:$ I$.$I$$..I$ $Z . $D?. $:.$:$$$$ ...Z$ :$ . .....$DD$ 
DDD                                                    $DD$ 
DDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDND$ 
DNDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDND$ 
DDD~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ZND$ 
DND                                                    $DD$ 
DDD                            . . .  ....+7.          $DD$ 
DDD                            ... .:$DDDDDD  .        $DD$ 
DDD                 ...    ..=8DNNNDDDDDDDDD?.         $DD$ 
DDD                . .     +DNDDDDNDDDDDDDDDN          $DD$ 
DDD              .?NN=.    .DNDDDDNDDDDDDDDDN,         $DD$ 
DDD..  . . .~ZNDNDDNNN.    .DNDDDDDDDDDDDDDDN8.        $DD$ 
DDD.  ,IDDNDDNDNDNDNNNO    .NDNDDDDDDDDDDDDDDN         $DD$ 
DDD  $DDDDDDDDDDDDDDDDD7... ONNDDDDDDNDDDDDNNDZ .      $DD$ 
DDD  .DDDDDDDDDDDDDDDDDD.  .?DDDDDDNNDDDDDDDDND  .     $DD$ 
DDD...DDDDDDDDDDDDDDDDDND   :NNDDDDNDNDDDDDDDNN=       $DD$ 
DDD  .~NDDDDDDDDDDDDDDDDN+ ..DNDDDDDDDDDDDDDDDDN..     $DD$ 
DDD    NDDDDDDDDDDDDDDDDNN ..DDDDDDDN:DDDDDDDDDN.      $DD$ 
DDD    $NNDDDDDNDNNDNNDNND8..NDDDDDDD.NDDDDDDDDNO .    $DD$ 
DDD    .DNDDDDDNDN?DDDDDDDD$.IDDDDDDD=~DDDDDDDDND      $DD$ 
DDD    .ZDNDDDDNNNN+DNDDNNDN,~NDDDDDN$ DDDDDDDDDNI     $DD$ 
DDD    .,DNDDDDDDNN,DNDDDDDDN.DDDDDDND.$NDDDDDDNDN.    $DD$ 
DDD     .NDDDDDNDDD8.DDDDDDDD?NDDDDDND .NDDDDDDDDD~    $DD$ 
DDD    . +DDDDDDDDDD.+NDDDDDDDDDDDDDDN..NDNDDDDDDDD    $DD$ 
DDD     ..DDDDDDDDDNI.DDDDDDDDDDDDDNNN..=NDDDDDDNND.  .$DD$ 
DDD      .IDDDDDDDDNN..DDDDDDDDDDDDDNN+..NDDDDDDDDDO . $DD$ 
DDD     .. DDNNDDDDDN=.,DDDDDDDDDDDDDN$..+DDDDDDDDDN.  $DD$ 
DDD    .   DDDDDDDDDNN .$DDDDDDDDDDDDDD  .DDDDDDDDDD?. $DD$ 
DDD        :NDDDDDDNDD.. NDDDDDDDDDDDND   ODDDDDDNDDN. $DD$ 
DDD        .NDNDDDDDDDZ  :NDDDDDDDDDNDN ..,DDDDDDDDND, $DD$ 
DDD        .=NNDDDDDNNN  .+DDDDDDDDNNND, . DDDDDO~...  $DD$ 
DDD        ..NNDDDDDDDD7 . 8DDDDDNDNDND? ..:..  . .    $DD$ 
DDD        ..$DDDDDDDDDN.. .NNNNDDNDN$:... . .    .    $DD$ 
DDD        . .DDDNDDDDDN~ . +DND+ .            .   .. .$DD$ 
DDD          .DDNDNDDNDDD ...    .              ,O.:8 .$DD$ 
DDD        . .~DDDNNO=..  .    .. .            .N~ID.I $DD$ 
DDN        .    .                              .~I.:8. $ND$ 
DDD.......... .   .............................   .  . $DD$ 
DDDDDDDDDDDNDDDDDDDDNDDDNDDDDDDDDDDNDDDDNDDNDNDDNDDDNDDNDD$ 
DDDDDDDDDDDNDNNNDNNDNDNNDDDDDNNDDDDDNDDDNDDNDNDDNDDNDNNNDD$ 
DDD........ ..NDN?..   .:8DND. .......~7NDD?  .. . .~7DNDD$ 
DDD    ~??????DD  . 8N=. ..DD   .~??~   .DD? ...??= .. DNN$ 
DDD    ..   .DNNN=...     ~DD  . ..  ..=DND? ..    . .=NDD$ 
DDD    +$$$$$$N+ ...8D8 .  DD .. ?DN8....NN?.  .$$7..  ZNN$ 
DDD     . ....DDN=.......+NND..  ?NND.   ZD? .  .  ..+DDND$ 
DDDDDDDNNDDDDNDDDDDNDDNNDDDDDNDDDDDDDDDDDNDDDNNNNDNDNNDDNN$ 
NDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDDND$ 
...                                                     ... 
                                             ");
            Console.WriteLine();
            Console.WriteLine("Star Wars Battleship has been rated M for Mature by ESRB due \nto Mace Windu appearing in the game.");
            Console.WriteLine("\n");
            Console.WriteLine("Press Enter to continue");
            Console.ReadLine();
            Console.Clear();

            Console.SetWindowSize(110, 30);
            Console.WriteLine(@"     _______.___________.    ___      .______         ____    __    ____  ___      .______          _______.
    /       |           |   /   \     |   _  \        \   \  /  \  /   / /   \     |   _  \        /       |
   |   (----`---|  |----`  /  ^  \    |  |_)  |        \   \/    \/   / /  ^  \    |  |_)  |      |   (----`
    \   \       |  |      /  /_\  \   |      /          \            / /  /_\  \   |      /        \   \    
.----)   |      |  |     /  _____  \  |  |\  \----.      \    /\    / /  _____  \  |  |\  \----.----)   |   
|_______/       |__|    /__/     \__\ | _| `._____|       \__/  \__/ /__/     \__\ | _| `._____|_______/    
                                                                                                            
.______        ___   .___________.___________. __       _______     _______. __    __   __  .______         
|   _  \      /   \  |           |           ||  |     |   ____|   /       ||  |  |  | |  | |   _  \        
|  |_)  |    /  ^  \ `---|  |----`---|  |----`|  |     |  |__     |   (----`|  |__|  | |  | |  |_)  |       
|   _  <    /  /_\  \    |  |        |  |     |  |     |   __|     \   \    |   __   | |  | |   ___/        
|  |_)  |  /  _____  \   |  |        |  |     |  `----.|  |____.----)   |   |  |  |  | |  | |  |            
|______/  /__/     \__\  |__|        |__|     |_______||_______|_______/    |__|  |__| |__| | _|            
                                                                                                            ");
            Console.WriteLine("*Not to be confused with the disappointing, abysmal, and less-fun Star Wars Battlefront");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("By pressing Enter to continue, you are stating that you are 17 years of age or older.");
            Console.ReadLine();
        }
    }
}
