using System;
using System.Collections.Generic;
using System.Linq;


namespace BattleShipStateTracker.StateTracker
{
    /// <summary>
    /// Game board
    /// </summary>
    public class GameBoard : IBoard
    {
        private const short BoardSize = 10;
        // _shipMap can link a position with a ship, so it will
        // be easy to check if a ship is hit
        private readonly IShip[,] _shipMap = new IShip[BoardSize, BoardSize];

        // _attackMap records whether a position with ship has been attacked before. This
        // can help avoid that a ship is hit at the same position multiple times.
        private readonly bool [,] _attackMap = new bool[BoardSize, BoardSize];

        private readonly List<IShip> _ships = new List<IShip>();

        /// <summary>
        /// Attack a position
        /// </summary>
        /// <param name="position">The position to attack</param>
        /// <returns>Boolean value that indicates whether a ship is hit</returns>
        /// <exception cref="Exception">The position has been attacked before</exception>
        public bool Attack(Coordinate position)
        {
            if (WasAttacked(position))
            {
                throw new Exception("The position has been hit before.");
            }

            var ship = _shipMap[position.X, position.Y];

            if (ship == null) // No ship at this position, so no hit
            {
                return false;
            }

            _attackMap[position.X, position.Y] = true;
            ship.Hit();

            return true;
        }

        /// <summary>
        /// Check if a position was attacked before.
        /// </summary>
        /// <param name="position">position to check</param>
        /// <returns></returns>
        public bool WasAttacked(Coordinate position)
        {
            return _attackMap[position.X, position.Y];
        }

        /// <summary>
        /// Add a ship to the game board
        /// </summary>
        /// <param name="ship">ship</param>
        public Result AddShip(IShip ship)
        {
            if (ship.Head.X >= BoardSize || ship.Head.Y >= BoardSize ||
                ship.Tail.X >= BoardSize || ship.Tail.Y >= BoardSize)
            {
                return Result.NotWithinBoard;
            }

            if (ship.Head.X != ship.Tail.X && ship.Head.Y != ship.Tail.Y)
            {
                return Result.NotVerticalOrHorizontal;
            }

            if (CheckIfAnyPositionTakenByOtherShip(ship))
            {
                return Result.PositionsTaken;
            }

            // Link a position with a ship
            if (ship.Head.X == ship.Tail.X)
            {
                var min = Math.Min(ship.Head.Y, ship.Tail.Y);
                var max = Math.Max(ship.Head.Y, ship.Tail.Y);
                for (int i = min; i <= max; i++)
                {
                    _shipMap[ship.Head.X, i] = ship;
                }
            }
            else
            {
                var min = Math.Min(ship.Head.X, ship.Tail.X);
                var max = Math.Max(ship.Head.X, ship.Tail.X);
                for (int i = min; i <= max; i++)
                {
                    _shipMap[i, ship.Head.Y] = ship;
                }
            }

            _ships.Add(ship);

            return Result.Success;
        }

        /// <summary>
        /// Check if all ships sink
        /// </summary>
        /// <returns>Boolean value that indicates if all ships sink</returns>
        public bool AllShipsSink()
        {
            return _ships.All(s => s.IsSink());
        }

        private bool CheckIfAnyPositionTakenByOtherShip(IShip ship)
        {
            if (ship.Head.X == ship.Tail.X)
            {
                var min = Math.Min(ship.Head.Y, ship.Tail.Y);
                var max = Math.Max(ship.Head.Y, ship.Tail.Y);

                for (int i = min; i <= max; i++)
                {
                    if (_shipMap[ship.Head.X, i] != null)
                    {
                        return true;
                    }
                }
            }
            else
            {
                var min = Math.Min(ship.Head.X, ship.Tail.X);
                var max = Math.Max(ship.Head.X, ship.Tail.X);

                for (int i = min; i <= max; i++)
                {
                    if (_shipMap[i, ship.Head.Y] != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        
    }
}
