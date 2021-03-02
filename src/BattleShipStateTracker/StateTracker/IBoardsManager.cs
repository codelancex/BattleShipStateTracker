using System.Collections.Generic;


namespace BattleShipStateTracker.StateTracker
{
    /// <summary>
    /// Board manager interface
    /// </summary>
    public interface IBoardsManager
    {
        /// <summary>
        /// Add a board
        /// </summary>
        /// <param name="board">Board object</param>
        /// <returns>Board ID</returns>
        string AddBoard(IBoard board);

        /// <summary>
        /// Check if a board exists
        /// </summary>
        /// <param name="id">Board ID to check</param>
        /// <returns>Boolean value that indicates whether board exists</returns>
        bool BoardExist(string id);

        /// <summary>
        /// Delete a board
        /// </summary>
        /// <param name="id">Board to delete</param>
        void DeleteBoard(string id);

        /// <summary>
        /// Get the board object with the given ID
        /// </summary>
        /// <param name="id">Board ID</param>
        /// <returns>Board object</returns>
        IBoard GetBoard(string id);

        /// <summary>
        /// Get all boards
        /// </summary>
        /// <returns>A collection of board IDs</returns>
        IEnumerable<string> GetBoards();
    }
}
