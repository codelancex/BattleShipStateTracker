using System.ComponentModel.DataAnnotations;


namespace BattleShipStateTracker.Request
{
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
