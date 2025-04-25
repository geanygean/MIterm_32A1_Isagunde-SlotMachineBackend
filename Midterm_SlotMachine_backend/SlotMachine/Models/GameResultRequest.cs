namespace SlotMachine.Models
{
    public class GameResultRequest
    {
        public string StudentNumber { get; set; }
        public bool IsWin { get; set; }
        public int Retries { get; set; }
    }
}
