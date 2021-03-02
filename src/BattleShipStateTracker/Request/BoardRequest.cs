using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BattleShipStateTracker.Request
{
    public class BoardRequest
    {
        [Required]
        public string BoardId { get; set; }
    }
}
