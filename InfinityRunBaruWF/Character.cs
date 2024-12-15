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

        public void Move(KeyEventArgs e, GameForm gameForm)
        {
            // Prevent adding new instances; only move the existing one
            if (e.KeyCode == Keys.Up)
            {
                if (this.GetPanel().Top > 0)
                {
                    this.GetPanel().Top -= _CharSpeed;
                }
            }
            else if (e.KeyCode == Keys.Left)
            {
                if (this.GetPanel().Left > 0)
                {
                    this.GetPanel().Left -= _CharSpeed;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                if (this.GetPanel().Left < gameForm.ClientSize.Width - this.GetPanel().Width)
                {
                    this.GetPanel().Left += _CharSpeed;
                }
            }

            // Ensure character stays within the game area
            this.GetPanel().Left = Math.Max(0, Math.Min(this.GetPanel().Left, gameForm.ClientSize.Width - this.GetPanel().Width));
            this.GetPanel().Top = Math.Max(0, Math.Min(this.GetPanel().Top, gameForm.ClientSize.Height - this.GetPanel().Height));
        }

        public void CenterCharacter(Form GameForm)
        {
            int centerX = (GameForm.ClientSize.Width - _CharPanel.Width) / 2;

            _CharPanel.Left = centerX;

            _CharPanel.Top = GameForm.ClientSize.Height - _CharPanel.Height - 50; // Slightly above the bottom
        }

        public Panel GetPanel()
        {
            return _CharPanel;
        }
    }
}
