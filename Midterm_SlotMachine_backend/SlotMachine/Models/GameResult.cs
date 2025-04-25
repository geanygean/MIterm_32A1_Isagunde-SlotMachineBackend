using System;

namespace SlotMachine.Models
{
    public class GameResult
    {
        public string StudentNumber { get; set; }
        public string StudentName { get; set; }
        public bool IsWin { get; set; }
        public int Retries { get; set; }
        public DateTime DatePlayed { get; set; } = DateTime.Now;
    }
}
