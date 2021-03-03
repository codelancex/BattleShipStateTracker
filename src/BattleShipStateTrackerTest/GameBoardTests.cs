using System;
using BattleShipStateTracker.StateTracker;
using FakeItEasy;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace BattleShipStateTrackerTest
{
    [TestClass]
    public class GameBoardTests
    {
        [TestMethod]
        public void TestAddShip_Success()
        {
            // Arrange
            BattleShip ship = new BattleShip(new Coordinate(5, 4), new Coordinate(6, 4));
            var gameBoard = new GameBoard();

            // Act
            var result = gameBoard.AddShip(ship);

            // Assert
            Assert.AreEqual(Result.Success, result);
        }

        [TestMethod]
        public void TestAddShip_NotWithinBoard_Failed()
        {
            // Arrange
            BattleShip ship = new BattleShip(new Coordinate(5, 10), new Coordinate(6, 10));
            var gameBoard = new GameBoard();

            // Act
            var result = gameBoard.AddShip(ship);

            // Assert
            Assert.AreEqual(Result.NotWithinBoard, result);
        }

        [TestMethod]
        public void TestAddShip_NotVerticalOrHorizontal_Failed()
        {
            // Arrange
            var ship = A.Fake<IShip>();
            A.CallTo(() => ship.Head).Returns(new Coordinate(5, 7));
            A.CallTo(() => ship.Head).Returns(new Coordinate(6, 8));
            var gameBoard = new GameBoard();

            // Act
            var result = gameBoard.AddShip(ship);

            // Assert
            Assert.AreEqual(Result.NotVerticalOrHorizontal, result);
        }

        [TestMethod]
        public void TestAddShip_PositionTaken_Failed1()
        {
            // Arrange
            BattleShip ship1 = new BattleShip(new Coordinate(5, 7), new Coordinate(5, 3));
            var gameBoard = new GameBoard();
            gameBoard.AddShip(ship1);
            BattleShip ship2 = new BattleShip(new Coordinate(4, 6), new Coordinate(6, 6));

            // Act
            var result = gameBoard.AddShip(ship2);

            // Assert
            Assert.AreEqual(Result.PositionsTaken, result);
        }

        [TestMethod]
        public void TestAddShip_PositionTaken_Failed2()
        {
            // Arrange
            BattleShip ship1 = new BattleShip(new Coordinate(4, 6), new Coordinate(6, 6));
            var gameBoard = new GameBoard();
            gameBoard.AddShip(ship1);
            BattleShip ship2 = new BattleShip(new Coordinate(5, 7), new Coordinate(5, 3));

            // Act
            var result = gameBoard.AddShip(ship2);

            // Assert
            Assert.AreEqual(Result.PositionsTaken, result);
        }

        [TestMethod]
        public void TestAttack_Hit()
        {
            // Arrange
            BattleShip ship = new BattleShip(new Coordinate(5, 4), new Coordinate(6, 4));
            var gameBoard = new GameBoard();
            gameBoard.AddShip(ship);

            // Act
            var result = gameBoard.Attack(new Coordinate(5, 4));

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void TestAttack_NotHit()
        {
            // Arrange
            BattleShip ship = new BattleShip(new Coordinate(5, 4), new Coordinate(6, 4));
            var gameBoard = new GameBoard();
            gameBoard.AddShip(ship);

            // Act
            var result = gameBoard.Attack(new Coordinate(7, 4));

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void TestAttack_AttackMoreThanOnce_Exception()
        {
            // Arrange
            BattleShip ship = new BattleShip(new Coordinate(5, 4), new Coordinate(6, 4));
            var gameBoard = new GameBoard();
            gameBoard.AddShip(ship);
            var result = gameBoard.Attack(new Coordinate(5, 4));

            // Act
            var exception = Assert.ThrowsException<Exception>(() => gameBoard.Attack(new Coordinate(5, 4)));

            // Assert
            Assert.AreEqual("The position has been hit before.", exception.Message);
        }

        [TestMethod]
        public void TestAllShipsSink_True()
        {
            // Arrange
            BattleShip ship1 = new BattleShip(new Coordinate(5, 4), new Coordinate(6, 4));
            BattleShip ship2 = new BattleShip(new Coordinate(7, 3), new Coordinate(7, 4));
            var gameBoard = new GameBoard();
            gameBoard.AddShip(ship1);
            gameBoard.AddShip(ship2);

            // Act
            gameBoard.Attack(new Coordinate(5, 4));
            gameBoard.Attack(new Coordinate(6, 4));
            gameBoard.Attack(new Coordinate(7, 3));
            gameBoard.Attack(new Coordinate(7, 4));
            var allShipsSink = gameBoard.AllShipsSink();

            // Assert
            Assert.AreEqual(true, allShipsSink);
        }

        [TestMethod]
        public void TestAllShipsSink_False()
        {
            // Arrange
            BattleShip ship1 = new BattleShip(new Coordinate(5, 4), new Coordinate(6, 4));
            BattleShip ship2 = new BattleShip(new Coordinate(7, 3), new Coordinate(7, 4));
            var gameBoard = new GameBoard();
            gameBoard.AddShip(ship1);
            gameBoard.AddShip(ship2);

            // Act
            gameBoard.Attack(new Coordinate(5, 4));
            gameBoard.Attack(new Coordinate(6, 4));
            gameBoard.Attack(new Coordinate(7, 3));
            gameBoard.Attack(new Coordinate(7, 2));
            var allShipsSink = gameBoard.AllShipsSink();

            // Assert
            Assert.AreEqual(false, allShipsSink);
        }
    }
}
