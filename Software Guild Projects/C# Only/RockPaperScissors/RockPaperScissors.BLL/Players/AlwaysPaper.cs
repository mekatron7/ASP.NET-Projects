using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RockPaperScissors.BLL.Players
{
    public class AlwaysPaper : IChoiceGetter
    {
        public RPSChoice GetChoice()
        {
            return RPSChoice.Paper;
        }
    }
}
