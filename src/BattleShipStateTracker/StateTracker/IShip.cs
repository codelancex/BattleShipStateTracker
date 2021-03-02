using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipStateTracker.StateTracker
{
    /// <summary>
    /// Ship interface
    /// </summary>
    public interface IShip
    {
        /// <summary>
        /// Ship head position
        /// </summary>
        Coordinate Head { get; }

        /// <summary>
        /// Ship tail position
        /// </summary>
        Coordinate Tail { get; }

        /// <summary>
        /// Hit the ship
        /// </summary>
        void Hit();

        /// <summary>
        /// Check if the ship sinks
        /// </summary>
        /// <returns>Boolean value that indicates whether the ship sinks</returns>
        bool IsSink();
    }
}
