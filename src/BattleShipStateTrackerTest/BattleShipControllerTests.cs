using System;
using BattleShipStateTracker.Controllers;
using BattleShipStateTracker.Request;
using BattleShipStateTracker.Response;
using BattleShipStateTracker.StateTracker;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BattleShipStateTrackerTest
{
    [TestClass]
    public class BattleShipControllerTests
    {
        [TestMethod]
        public void TestGetBoards_Success()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);

            // Act
            var result = battleShipController.GetBoards() as JsonResult;

            // Arrange
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status200OK, result.StatusCode);
            var value = result.Value as BoardResponse[];
            Assert.IsNotNull(value);
            Assert.AreEqual(0, value.Length);
        }

        [TestMethod]
        public void TestAddBoard_Success()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);

            // Act
            var result = battleShipController.AddBoard() as JsonResult;

            // Arrange
            Assert.IsNotNull(result);
            Assert.AreEqual(StatusCodes.Status201Created, result.StatusCode);
            var value = result.Value as BoardResponse;
            Assert.IsNotNull(value);
            Assert.IsTrue(Guid.TryParse(value.BoardId, out _));
        }

        [TestMethod]
        public void TestAddShip_Success()
        {  
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            var result = battleShipController.AddBoard() as JsonResult;
            var boardId = (result.Value as BoardResponse).BoardId;

            // Act
            var addShipResult = battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = boardId, HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() {X = 5, Y = 6},
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() {X = 6, Y = 6}
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(addShipResult);
            Assert.AreEqual(StatusCodes.Status201Created, addShipResult.StatusCode);
            var value = addShipResult.Value as SuccessResponse;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void TestAddShip_BoardNotExist_ErrorResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);

            // Act
            var addShipResult = battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = "fake id",
                HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 },
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 6, Y = 6 }
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(addShipResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, addShipResult.StatusCode);
            var value = addShipResult.Value as ErrorResponse;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void TestAddShip_NotVerticalOrHorizontal_ErrorResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            var result = battleShipController.AddBoard() as JsonResult;
            var boardId = (result.Value as BoardResponse).BoardId;

            // Act
            var addShipResult = battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = boardId,
                HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 },
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 7, Y = 8 }
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(addShipResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, addShipResult.StatusCode);
            var value = addShipResult.Value as ErrorResponse;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void TestAddShip_NotWithinBoard_ErrorResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            var result = battleShipController.AddBoard() as JsonResult;
            var boardId = (result.Value as BoardResponse).BoardId;

            // Act
            var addShipResult = battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = boardId,
                HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 10 },
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 6, Y = 10 }
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(addShipResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, addShipResult.StatusCode);
            var value = addShipResult.Value as ErrorResponse;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void TestAddShip_PositionTaken_ErrorResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            var result = battleShipController.AddBoard() as JsonResult;
            var boardId = (result.Value as BoardResponse).BoardId;
            battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = boardId,
                HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 },
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 6, Y = 6 }
            });

            // Act
            var addShipResult = battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = boardId,
                HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 },
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 7 }
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(addShipResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, addShipResult.StatusCode);
            var value = addShipResult.Value as ErrorResponse;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void TestAttack_BoardNotExist_ErrorResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);

            // Act
            var attackResult = battleShipController.Attack(new AttackRequest()
            {
                BoardId = "fake id",
                Position = new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 }
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(attackResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, attackResult.StatusCode);
            var value = attackResult.Value as ErrorResponse;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void TestAttack_HitResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            var result = battleShipController.AddBoard() as JsonResult;
            var boardId = (result.Value as BoardResponse).BoardId;
            battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = boardId,
                HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 },
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 6, Y = 6 }
            });

            // Act
            var attackResult = battleShipController.Attack(new AttackRequest()
            {
                BoardId = boardId,
                Position = new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 }
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(attackResult);
            Assert.AreEqual(StatusCodes.Status200OK, attackResult.StatusCode);
            var value = attackResult.Value as HitResponse;
            Assert.IsNotNull(value);
            Assert.IsTrue(value.Hit);
        }

        [TestMethod]
        public void TestAttack_WasAttacked_ErrorResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            var result = battleShipController.AddBoard() as JsonResult;
            var boardId = (result.Value as BoardResponse).BoardId;
            battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = boardId,
                HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 },
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 6, Y = 6 }
            });

             battleShipController.Attack(new AttackRequest()
            {
                BoardId = boardId,
                Position = new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 }
            });

            // Act
            var attackResult = battleShipController.Attack(new AttackRequest()
            {
                BoardId = boardId,
                Position = new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 }
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(attackResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, attackResult.StatusCode);
            var value = attackResult.Value as ErrorResponse;
            Assert.IsNotNull(value);
            Assert.AreEqual("The position (5,6) has been attacked before.", value.ErrorMessages[0]);
        }

        [TestMethod]
        public void TestAllShipsSink_AllShipsSinkResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            var result = battleShipController.AddBoard() as JsonResult;
            var boardId = (result.Value as BoardResponse).BoardId;
            battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = boardId,
                HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 },
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 6, Y = 6 }
            });

            battleShipController.AddShip(new AddShipRequest()
            {
                BoardId = boardId,
                HeadPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 7, Y = 7 },
                TailPosition =
                    new BattleShipStateTracker.Request.Coordinate() { X = 7, Y = 6 }
            });

            battleShipController.Attack(new AttackRequest()
            {
                BoardId = boardId,
                Position = new BattleShipStateTracker.Request.Coordinate() { X = 5, Y = 6 }
            });

            battleShipController.Attack(new AttackRequest()
            {
                BoardId = boardId,
                Position = new BattleShipStateTracker.Request.Coordinate() { X = 6, Y = 6 }
            });

            battleShipController.Attack(new AttackRequest()
            {
                BoardId = boardId,
                Position = new BattleShipStateTracker.Request.Coordinate() { X = 7, Y = 7 }
            });

            battleShipController.Attack(new AttackRequest()
            {
                BoardId = boardId,
                Position = new BattleShipStateTracker.Request.Coordinate() { X = 7, Y = 6 }
            });

            // Act
            var allShipsSinkResult = battleShipController.AllShipsSink(new BoardRequest()
            {
                BoardId = boardId
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(allShipsSinkResult);
            Assert.AreEqual(StatusCodes.Status200OK, allShipsSinkResult.StatusCode);
            var value = allShipsSinkResult.Value as AllShipsSinkResponse;
            Assert.IsNotNull(value);
            Assert.IsTrue(value.AllShipsSink);
        }

        [TestMethod]
        public void TestAllShipsSink_BoardNotExist_ErrorResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            battleShipController.AddBoard();

            // Act
            var allShipsSinkResult = battleShipController.AllShipsSink(new BoardRequest()
            {
                BoardId = "fake id"
            }) as JsonResult;

            // Assert
            Assert.IsNotNull(allShipsSinkResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, allShipsSinkResult.StatusCode);
            var value = allShipsSinkResult.Value as ErrorResponse;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void TestDeleteBoard_Success()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            var result = battleShipController.AddBoard() as JsonResult;
            var boardId = (result.Value as BoardResponse).BoardId;

            // Act
            var deleteBoardResult = battleShipController.DeleteBoard(new BoardRequest()
            {
                BoardId = boardId,
            }) as JsonResult;

            // Arrange
            Assert.IsNotNull(deleteBoardResult);
            Assert.AreEqual(StatusCodes.Status200OK, deleteBoardResult.StatusCode);
            var value = deleteBoardResult.Value as SuccessResponse;
            Assert.IsNotNull(value);
        }

        [TestMethod]
        public void TestDeleteBoard_BoardNotExist_ErrorResponse()
        {
            // Arrange
            var boardsManager = new GameBoardsManager();
            var battleShipController = new BattleShipController(boardsManager);
            battleShipController.AddBoard();

            // Act
            var deleteBoardResult = battleShipController.DeleteBoard(new BoardRequest()
            {
                BoardId = "fake id",
            }) as JsonResult;

            // Arrange
            Assert.IsNotNull(deleteBoardResult);
            Assert.AreEqual(StatusCodes.Status400BadRequest, deleteBoardResult.StatusCode);
            var value = deleteBoardResult.Value as ErrorResponse;
            Assert.IsNotNull(value);
        }
    }
}
