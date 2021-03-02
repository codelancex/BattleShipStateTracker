using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipStateTracker.StateTracker
{
    /// <summary>
    /// 
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        bool Attack(Coordinate position);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="coordinate"></param>
        /// <returns></returns>
        bool WasAttacked(Coordinate coordinate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ship"></param>
        Result AddShip(IShip ship);
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        bool AllShipsSink();
    }

    public enum Result
    {
        Success,
        NotWithinBoard,
        NotVerticalOrHorizontal,
        PositionsTaken
    }
}
