using System;


namespace BattleShipStateTracker.StateTracker
{
    /// <summary>
    /// Coordinate
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        public Coordinate(ushort x, ushort y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        public ushort X { get; }

        /// <summary>
        /// 
        /// </summary>
        public ushort Y { get; }

        /// <summary>
        /// Equal function
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if ((obj == null) || this.GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                Coordinate p = (Coordinate)obj;
                return (X == p.X) && (Y == p.Y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }
    }
}
