using System;
using System.Linq;
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

            // Ensure only one instance of the application
            if (Application.OpenForms.OfType<GameForm>().Any())
            {
                MessageBox.Show("The game is already running.", "Infinity Run", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string leaderboardFilePath = "newleaderboard.json";
            var leaderboard = new Leaderboard(leaderboardFilePath);

            GameForm gameForm = new GameForm(leaderboard);
            Application.Run(gameForm);
        }
    }
}
