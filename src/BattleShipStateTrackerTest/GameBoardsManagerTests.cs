using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BattleShipStateTracker.StateTracker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleShipStateTrackerTest
{
    [TestClass]
    public class GameBoardsManagerTests
    {
        [TestMethod]
        public void TestAddBoard_Success()
        {
            // Arrange
            var gameBoardsManager = new GameBoardsManager();
            var board = new GameBoard();

            // Act
            var id = gameBoardsManager.AddBoard(board);

            // Assert
            var isValid = Guid.TryParse(id, out _);
            Assert.IsTrue(isValid);
        }

        [TestMethod]
        public void TestBoardExist_True()
        {
            // Arrange
            var gameBoardsManager = new GameBoardsManager();
            var board = new GameBoard();
            var id = gameBoardsManager.AddBoard(board);

            // Act
            var exist = gameBoardsManager.BoardExist(id);

            // Assert
            Assert.IsTrue(exist);
        }

        [TestMethod]
        public void TestBoardExist_False()
        {
            // Arrange
            var gameBoardsManager = new GameBoardsManager();

            // Act
            var exist = gameBoardsManager.BoardExist("fake id");

            // Assert
            Assert.IsFalse(exist);
        }

        [TestMethod]
        public void TestDeleteBoard_Success()
        {
            // Arrange
            var gameBoardsManager = new GameBoardsManager();
            var board = new GameBoard();
            var id = gameBoardsManager.AddBoard(board);

            // Act
            gameBoardsManager.DeleteBoard(id);

            // Assert
            var exist = gameBoardsManager.BoardExist(id);
            Assert.IsFalse(exist);
        }

        [TestMethod]
        public void TestDeleteBoard_Failed()
        {
            // Arrange
            var gameBoardsManager = new GameBoardsManager();

            // Act
            var exception = Assert.ThrowsException<Exception>(() => gameBoardsManager.DeleteBoard("fake id"));

            // Assert
            Assert.AreEqual("The board fake id doesn't exist.", exception.Message);
        }

        [TestMethod]
        public void TestGetBoard_Success()
        {
            // Arrange
            var gameBoardsManager = new GameBoardsManager();
            IBoard board = new GameBoard();
            var id = gameBoardsManager.AddBoard(board);

            // Act
            board = gameBoardsManager.GetBoard(id);

            // Assert
            Assert.IsNotNull(board);
        }

        [TestMethod]
        public void TestGetBoard_Fail()
        {
            // Arrange
            var gameBoardsManager = new GameBoardsManager();

            // Act
            var board = gameBoardsManager.GetBoard("fake id");

            // Assert
            Assert.IsNull(board);
        }

        [TestMethod]
        public void TestGetBoards_Success()
        {
            // Arrange
            var gameBoardsManager = new GameBoardsManager();
            var id1 = gameBoardsManager.AddBoard(new GameBoard());
            var id2 = gameBoardsManager.AddBoard(new GameBoard());

            // Act
            var boards = gameBoardsManager.GetBoards();

            // Assert
            var enumerable = boards as string[] ?? boards.ToArray();
            Assert.AreEqual(2, enumerable.Count());
            Assert.IsTrue(enumerable.Contains(id1));
            Assert.IsTrue(enumerable.Contains(id2));
        }
    }
}
