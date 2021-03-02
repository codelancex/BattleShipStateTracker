using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipStateTracker.StateTracker
{
    public interface IBoardsManager
    {
        string AddBoard(IBoard board);
        bool BoardExist(string id);
        void DeleteBoard(string id);
        IBoard GetBoard(string id);
        IEnumerable<string> GetBoards();
    }
}
