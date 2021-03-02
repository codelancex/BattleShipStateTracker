using System;


namespace BattleShipStateTracker.StateTracker
{
    /// <summary>
    /// The BattleShip to be placed on the game board.
    /// </summary>
    public class BattleShip : IShip
    {
        private short _hitNum = 0;
        private readonly short _size = 0;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="head">head position</param>
        /// <param name="tail">tail position</param>
        public BattleShip(Coordinate head, Coordinate tail)
        {
            if (head.X != tail.X && head.Y != tail.Y)
            {
                throw new Exception("The ship is not placed vertically or horizontally");
            }

            Head = head;
            Tail = tail;
            // Size calculation relies on that the ship can only be placed vertically or horizontally.
            _size = (short) Math.Max(Math.Abs(head.X - tail.X) + 1, Math.Abs(head.Y - tail.Y) + 1);
        }

        /// <summary>
        /// Ship head position on the game board.
        /// </summary>
        public Coordinate Head { get; }

        /// <summary>
        /// Ship tail position on the game board.
        /// </summary>
        public Coordinate Tail { get; }

        /// <summary>
        /// Hit the ship.
        /// Note: The client code (i.e., GameBoard) will make sure a position is only hit once.
        /// </summary>
        public void Hit()
        {
            _hitNum++;
        }

        /// <summary>
        /// Check whether the ship sinks
        /// </summary>
        /// <returns>Boolean value that indicate whether the ship sinks</returns>
        public bool IsSink()
        {
            return _hitNum == _size;
        }

        /// <summary>
        /// Object to string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"[({Head.X},{Head.Y}), ({Tail.X},{Tail.Y})]";
        }
    }
}
