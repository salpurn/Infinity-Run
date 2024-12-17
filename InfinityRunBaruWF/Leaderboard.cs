using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace InfinityRun
{
    public class Leaderboard
    {
        private readonly string _filePath;

        public Leaderboard(string filePath)
        {
            _filePath = filePath;
            EnsureFileExists();
        }

        private void EnsureFileExists()
        {
            if (!File.Exists(_filePath))
            {
                var defaultEntries = new List<LeaderboardEntry>();
                SaveScores(defaultEntries); // Create the file with an empty list of entries
            }
        }

        public List<LeaderboardEntry> LoadScores()
        {
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<LeaderboardEntry>>(json) ?? new List<LeaderboardEntry>();
        }

        public void SaveScores(List<LeaderboardEntry> entries)
        {
            var json = JsonConvert.SerializeObject(entries, Newtonsoft.Json.Formatting.Indented);  // Use Formatting.Indented directly here
            File.WriteAllText(_filePath, json);
        }

        public int GetHighestScore()
        {
            var entries = LoadScores();
            return entries.Any() ? entries.Max(e => e.Score) : 0;
        }

        public void AddScore(int score)
        {
            var entries = LoadScores();
            int gameNumber = entries.Count > 0 ? entries.Max(e => e.GameNumber) + 1 : 1; // Increment the game number

            var entry = new LeaderboardEntry(score, gameNumber);
            entries.Add(entry);
            SaveScores(entries);
        }

        public string GetLeaderboardSummary()
        {
            var entries = LoadScores();
            var summary = entries.OrderByDescending(e => e.Score)
                                 .Take(10)
                                 .Select(e => $"{e.GameNumber} - Score: {e.Score}, Date: {e.Timestamp:yyyy-MM-dd HH:mm:ss}")
                                 .ToList();

            return string.Join("\n", summary);
        }
    }
}
