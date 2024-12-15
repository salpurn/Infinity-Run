using System;
using System.Windows.Forms;
using InfinityRun;
using Newtonsoft.Json;

namespace InfinityRun
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            string leaderboardFilePath = "leaderboard.json";
            var leaderboard = new Leaderboard(leaderboardFilePath);

            GameForm gameForm = new GameForm(leaderboard);

            Application.Run(gameForm);
        }
    }
}
