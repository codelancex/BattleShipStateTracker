
namespace BattleShipStateTracker.StateTracker
{
    /// <summary>
    /// Board interface
    /// </summary>
    public interface IBoard
    {
        /// <summary>
        /// Attack a given position
        /// </summary>
        /// <param name="position">Position to attack</param>
        /// <returns></returns>
        bool Attack(Coordinate position);

        /// <summary>
        /// Check whether a given position with
        /// ship was attacked before
        /// </summary>
        /// <param name="position">Position to check</param>
        /// <returns></returns>
        bool WasAttacked(Coordinate position);

        /// <summary>
        /// Add a ship to the board
        /// </summary>
        /// <param name="ship">The ship to add</param>
        Result AddShip(IShip ship);

        /// <summary>
        /// Check if all ships sink
        /// </summary>
        /// <returns>Boolean value that indicates whether all ships sink</returns>
        bool AllShipsSink();
    }

    /// <summary>
    /// Add ship result
    /// </summary>
    public enum Result
    {
        /// <summary>
        /// Successfully added
        /// </summary>
        Success,

        /// <summary>
        /// The ship is not completely
        /// within the board
        /// </summary>
        NotWithinBoard,

        /// <summary>
        /// The ship is not placed vertically
        /// or horizontally
        /// </summary>
        NotVerticalOrHorizontal,

        /// <summary>
        /// The position has been taken by
        /// other ship
        /// </summary>
        PositionsTaken
    }
}
