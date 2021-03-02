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
}
