using System;
using BattleShipStateTracker.StateTracker;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleShipStateTrackerTest
{
    [TestClass]
    public class BattleShipTests
    {
        [TestMethod]
        public void TestConstructor_Success()
        {
            // Act
            var battleShip = new BattleShip(new Coordinate(4, 5), new Coordinate(4, 7));

            // Assert
            Assert.AreEqual(new Coordinate(4, 5), battleShip.Head);
            Assert.AreEqual(new Coordinate(4, 7), battleShip.Tail);
        }

        [TestMethod]
        public void TestConstructor_Failed()
        {
            // Act
            var exception = Assert.ThrowsException<Exception>(() => new BattleShip(new Coordinate(4, 5), new Coordinate(6, 7)));

            // Assert
            Assert.AreEqual("The ship is not placed vertically or horizontally", exception.Message);
        }

        [TestMethod]
        public void TestHit_NotSink()
        {
            // Arrange
            var battleShip = new BattleShip(new Coordinate(4, 5), new Coordinate(4, 6));

            // Act
            battleShip.Hit();

            // Assert
            Assert.IsFalse(battleShip.IsSink());
        }

        [TestMethod]
        public void TestHit_Sink()
        {
            // Arrange
            var battleShip = new BattleShip(new Coordinate(4, 5), new Coordinate(4, 6));

            // Act
            battleShip.Hit();
            battleShip.Hit();

            // Assert
            Assert.IsTrue(battleShip.IsSink());
        }
    }
}
