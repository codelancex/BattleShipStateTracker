
namespace BattleShipStateTracker.Response
{
    /// <summary>
    /// Response to tell whether a ship is hit.
    /// It's used in Attack request
    /// </summary>
    public class HitResponse
    {
        /// <summary>
        /// Boolean value that indicate
        /// whether a ship is hit
        /// </summary>
        public bool Hit { get; set; }
    }
}
