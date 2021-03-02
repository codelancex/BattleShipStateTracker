using System.Collections.Generic;


namespace BattleShipStateTracker.Response
{
    /// <summary>
    /// General Error response
    /// </summary>
    public class ErrorResponse
    {
        /// <summary>
        /// List of error messages
        /// </summary>
        public List<string> ErrorMessages { get; set; }
    }
}
