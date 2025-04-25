using System;

namespace SlotMachine.Models
{
    public class Player
    {
        public string StudentNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime LastPlayed { get; set; } = DateTime.MinValue;
    }
}
