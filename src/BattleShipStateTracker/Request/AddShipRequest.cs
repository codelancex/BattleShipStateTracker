using System.ComponentModel.DataAnnotations;


namespace BattleShipStateTracker.Request
{
    /// <summary>
    /// AddShipRequest POST request body.
    /// In the request, head and tail positions
    /// are used for locating a ship on the board.
    /// </summary>
    public class AddShipRequest
    {
        /// <summary>
        /// Board Id
        /// </summary>
        [Required]
        public string BoardId { get; set; }

        /// <summary>
        /// Ship head position
        /// </summary>
        [Required]
        public Coordinate HeadPosition { get; set; }

        /// <summary>
        /// Ship tail position
        /// </summary>
        [Required]
        public Coordinate TailPosition { get; set; }
    }

    /// <summary>
    /// Coordinate used in the POST request.
    /// X and Y need to be nullable to do the
    /// Required validation
    /// </summary>
    public class Coordinate
    {
        /// <summary>
        /// X
        /// </summary>
        [Required]
        [Range(0, 9)]
        public ushort? X { get; set; }

        /// <summary>
        /// Y
        /// </summary>
        [Required]
        [Range(0, 9)]
        public ushort? Y { get; set; }

        /// <summary>
        /// ToString override
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"({X},{Y})";
        }
    }
}
