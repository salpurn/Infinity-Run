using System;
using System.Drawing;
using System.Windows.Forms;

namespace InfinityRun
{
    public class Character
    {
        private Panel _CharPanel;
        private int _CharSpeed = 17;

        public Character(Form GameForm)
        {
            _CharPanel = new Panel
            {
                Width = 50,
                Height = 103,
                Location = new Point(375, 500) // Starting position
            };

            try
            {
                _CharPanel.BackgroundImage = Image.FromFile(@"assets/character.png");
                _CharPanel.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading character image: " + e.Message);
            }

            GameForm.Controls.Add(_CharPanel);
        }

        public void Move(KeyEventArgs e, Form GameForm)
        {
            if (e.KeyCode == Keys.Left && _CharPanel.Left > 0)
                _CharPanel.Left -= _CharSpeed;
            else if (e.KeyCode == Keys.Right && _CharPanel.Left < GameForm.ClientSize.Width - _CharPanel.Width)
                _CharPanel.Left += _CharSpeed;
        }

        public Panel GetPanel()
        {
            return _CharPanel;
        }
    }
}
