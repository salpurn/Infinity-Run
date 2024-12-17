using System;

namespace InfinityRun
{
    public class LeaderboardEntry
    {
        public int Score { get; set; }
        public int GameNumber { get; set; }
        public DateTime Timestamp { get; set; }

        public LeaderboardEntry(int score, int gameNumber)
        {
            Score = score;
            GameNumber = gameNumber;
            Timestamp = DateTime.Now;
        }
    }
}
