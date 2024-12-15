using System;
using System.Windows.Forms;

namespace InfinityRun
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Set the application to use Windows Forms.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Run the main game form.
            Application.Run(new GameForm());
        }
    }
}
