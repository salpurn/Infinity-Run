using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using Newtonsoft.Json;
using JsonFormatting = Newtonsoft.Json.Formatting;

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
                var defaultScores = new List<int>();
                SaveScores(defaultScores); // Create the file with an empty score list
            }
        }

        public List<int> LoadScores()
        {
            var json = File.ReadAllText(_filePath);
            return JsonConvert.DeserializeObject<List<int>>(json) ?? new List<int>();
        }

        public void SaveScores(List<int> scores)
        {
            var json = JsonConvert.SerializeObject(scores, JsonFormatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        public int GetHighestScore()
        {
            var scores = LoadScores();
            return scores.Any() ? scores.Max() : 0;
        }

        public void AddScore(int score)
        {
            var scores = LoadScores();
            scores.Add(score);
            SaveScores(scores);
        }
    }
}
