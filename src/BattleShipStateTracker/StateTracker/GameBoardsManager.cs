using System;
using System.Collections.Generic;


namespace BattleShipStateTracker.StateTracker
{
    /// <summary>
    /// Game boards manager
    /// </summary>
    public class GameBoardsManager : IBoardsManager
    {
        private readonly Dictionary<string, IBoard> _idToGameBoardDict = new Dictionary<string, IBoard>();

        /// <summary>
        /// Add a board
        /// </summary>
        /// <param name="board">The board to be added</param>
        /// <returns>The board ID</returns>
        public string AddBoard(IBoard board)
        {
            var guid = Guid.NewGuid();
            _idToGameBoardDict.Add(guid.ToString(), new GameBoard());

            return guid.ToString();
        }

        /// <summary>
        /// Check if a board exist
        /// </summary>
        /// <param name="id">Board ID</param>
        /// <returns></returns>
        public bool BoardExist(string id)
        {
            return _idToGameBoardDict.ContainsKey(id);
        }

        /// <summary>
        /// Delete a game board
        /// </summary>
        /// <param name="id">Board ID</param>
        public void DeleteBoard(string id)
        {
            if (_idToGameBoardDict.ContainsKey(id))
            {
                _idToGameBoardDict.Remove(id);
            }
            else
            {
                throw new Exception($"The board {id} doesn't exist.");
            }
        }

        /// <summary>
        /// Get board
        /// </summary>
        /// <param name="id">Board ID</param>
        /// <returns>The board object</returns>
        public IBoard GetBoard(string id)
        {
            return _idToGameBoardDict.TryGetValue(id, out var board) ? board : null;
        }

        /// <summary>
        /// Get all of the boards
        /// </summary>
        /// <returns>All of the board keys</returns>
        public IEnumerable<string> GetBoards()
        {
            return _idToGameBoardDict.Keys;
        }
    }
}
