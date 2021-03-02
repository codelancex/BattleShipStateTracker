using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BattleShipStateTracker.Request;
using BattleShipStateTracker.Response;
using BattleShipStateTracker.StateTracker;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Coordinate = BattleShipStateTracker.StateTracker.Coordinate;


namespace BattleShipStateTracker.Controllers
{
    /// <summary>
    /// Controller that will handle incoming requests
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Produces("application/json")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class BattleShipController : ControllerBase
    {
        private static readonly ILog Logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IBoardsManager _boardsManager;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="boardsManager">Boards manager</param>
        public BattleShipController(IBoardsManager boardsManager)
        {
            _boardsManager = boardsManager;
        }

        /// <summary>
        /// Get all of the game boards
        /// </summary>
        /// <returns></returns>
        /// <response code="200">All of the game boards returned.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<BoardResponse>), 200)]
        [Route("GetBoards")]
        public IActionResult GetBoards()
        {
            Logger.Info("GetBoards request received.");
            return new JsonResult(_boardsManager.GetBoards().Select(id => new BoardResponse(){ BoardId = id }).ToArray())
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        /// <summary>
        /// Add a game board
        /// </summary>
        /// <returns>Game board ID for future references</returns>
        [HttpPost]
        [Route("AddBoard")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(BoardResponse), 201)]
        public IActionResult AddBoard()
        {
            Logger.Info("AddBoard request received.");
            var board = new GameBoard();
            var boardId = _boardsManager.AddBoard(board);

            Logger.Info($"Board [{boardId}] has been added.");

            return new JsonResult(new BoardResponse(){BoardId = boardId})
            {
                StatusCode = StatusCodes.Status201Created
            };
        }

        /// <summary>
        /// Add a battle ship on the board
        /// Note: This function won't handle the scenario that
        /// the ship is partially or completely placed out of the board
        /// because the request validation will fail the request before
        /// entering this function.
        /// </summary>
        /// <param name="request">request body</param>
        /// <returns>Success or Error response</returns>
        [HttpPost]
        [Route("AddShip")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(SuccessResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public IActionResult AddShip([FromBody]AddShipRequest request)
        {
            Logger.Info("AddShip request received.");
            var board = _boardsManager.GetBoard(request.BoardId);

            if (board == null)
            {
                Logger.Error($"The board [{request.BoardId}] can't be found.");
                return CreateErrorResponse(StatusCodes.Status400BadRequest,
                    $"The board [{request.BoardId}] doesn't exist");
            }

            Result result;
            try
            {
                result = board.AddShip(new BattleShip(
                    new Coordinate(request.HeadPosition.X.Value, request.HeadPosition.Y.Value),
                    new Coordinate(request.TailPosition.X.Value, request.TailPosition.Y.Value)));
            }
            catch (Exception e) // BattleShip constructor will throw exception if the ship is not placed vertically or horizontally
            {
                Logger.Error(e.Message);
                return CreateErrorResponse(StatusCodes.Status400BadRequest,
                    $"The ship {request.HeadPosition}, {request.TailPosition} must be vertical or horizontal");
            }

            if (result == Result.PositionsTaken)
            {
                Logger.Error($"The ship {request.HeadPosition}, {request.TailPosition} can't be placed because some positions have been taken by other ship.");

                return CreateErrorResponse(StatusCodes.Status400BadRequest,
                    $"The ship {request.HeadPosition}, {request.TailPosition} can't be placed because some positions have been taken by other ship");
            }

            Logger.Info("The ship has been successfully added.");

            return CreateSuccessResponse(StatusCodes.Status201Created);
        }

        /// <summary>
        /// Attack a position on the game board
        /// Note: If the position with a ship on it has been attacked before,
        /// Error response will be returned.
        /// </summary>
        /// <param name="request">request body</param>
        /// <returns>Return whether a ship is hit</returns>
        [HttpPost]
        [Route("Attack")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(HitResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public IActionResult Attack([FromBody] AttackRequest request)
        {
            Logger.Info("Attack request received.");
            var board = _boardsManager.GetBoard(request.BoardId);

            if (board == null)
            {
                Logger.Error($"The board [{request.BoardId}] can't be found.");
                return CreateErrorResponse(StatusCodes.Status400BadRequest, $"The board {request.BoardId} doesn't exist");
            }

            var coordinate = new Coordinate(request.Position.X.Value, request.Position.Y.Value);
            var attacked = board.WasAttacked(coordinate);
            if (attacked)
            {
                Logger.Error($"The position {request.Position} has been attached before.");
                return CreateErrorResponse(StatusCodes.Status400BadRequest, $"The position {request.Position} has been attacked before.");
            }

            var hit = board.Attack(coordinate);
            Logger.Info($"A ship is hit: {hit}");

            return new JsonResult(new HitResponse(){ Hit = hit })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        /// <summary>
        /// Check if all ships sink
        /// </summary>
        /// <param name="boardToCheck">The board to check</param>
        /// <returns>Return whether all ships sink</returns>
        [HttpGet]
        [Route("AllShipsSink")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(AllShipsSinkResponse), 200)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public IActionResult AllShipsSink([FromBody] BoardRequest boardToCheck)
        {
            Logger.Info("AllShipsSink request received.");
            var board = _boardsManager.GetBoard(boardToCheck.BoardId);

            if (board == null)
            {
                Logger.Error($"The board [{boardToCheck.BoardId}] can't be found.");
                return CreateErrorResponse(StatusCodes.Status400BadRequest, $"The board {boardToCheck.BoardId} doesn't exist");
            }

            var allShipsSink = board.AllShipsSink();
            Logger.Info($"All ships sink: {allShipsSink}");
            return new JsonResult(new AllShipsSinkResponse(){ AllShipsSink = allShipsSink })
            {
                StatusCode = StatusCodes.Status200OK
            };
        }

        /// <summary>
        /// Delete a board
        /// </summary>
        /// <param name="boardToDelete">The board to delete</param>
        /// <returns>Success or Error response</returns>
        [HttpPost]
        [Route("DeleteBoard")]
        [MapToApiVersion("1.0")]
        [ProducesResponseType(typeof(SuccessResponse), 201)]
        [ProducesResponseType(typeof(ErrorResponse), 400)]
        public IActionResult DeleteBoard([FromBody] BoardRequest boardToDelete)
        {
            Logger.Info("DeleteBoard request received.");
            if (_boardsManager.BoardExist(boardToDelete.BoardId))
            {
                _boardsManager.DeleteBoard(boardToDelete.BoardId);
            }
            else
            {
                Logger.Error($"The board [{boardToDelete.BoardId}] can't be found.");
                return CreateErrorResponse(StatusCodes.Status400BadRequest, $"The board {boardToDelete.BoardId} doesn't exist");
            }
            Logger.Info($"The Board {boardToDelete.BoardId} has been deleted.");
            return CreateSuccessResponse(StatusCodes.Status200OK);
        }

        // Create a general error response which include all the error messages.
        private static JsonResult CreateErrorResponse(int statusCode, string errorMessage)
        {
            var response = new ErrorResponse()
            {
                ErrorMessages = new List<string>()
                {
                    errorMessage
                }
            };
            return new JsonResult(response)
            {
                StatusCode = statusCode
            };
        }

        // Create a general success response.
        private static JsonResult CreateSuccessResponse(int statusCode)
        {
            var successResponse = new SuccessResponse();

            return new JsonResult(successResponse)
            {
                StatusCode = statusCode
            };
        }
    }
}
