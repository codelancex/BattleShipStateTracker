using System.ComponentModel.DataAnnotations;


namespace BattleShipStateTracker.Request
{
    /// <summary>
    /// POST request body for Attack api
    /// </summary>
    public class AttackRequest
    {
        /// <summary>
        /// Board ID
        /// </summary>
        [Required]
        public string BoardId { get; set; }
        /// <summary>
        /// Position to attack
        /// </summary>
        [Required]
        public Coordinate Position { get; set; }
    }
}
