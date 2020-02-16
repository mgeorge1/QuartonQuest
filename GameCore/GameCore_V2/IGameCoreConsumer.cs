using System;
using System.Collections.Generic;
using System.Text;

namespace GameCore_V2
{
    interface IGameCoreConsumer
    {
       public TurnInfo TransferMoves(TurnInfo move)
        {

            return move;
        }


    }
}
