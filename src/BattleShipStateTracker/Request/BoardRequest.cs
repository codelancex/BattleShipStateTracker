using System.ComponentModel.DataAnnotations;


namespace BattleShipStateTracker.Request
{
    /// <summary>
    /// Request body for the APIs that need board info
    /// </summary>
    public class BoardRequest
    {
        /// <summary>
        /// Board ID
        /// </summary>
        [Required]
        public string BoardId { get; set; }
    }
}
