using System;
using System.Drawing;
using System.Windows.Forms;
using System.Media;

namespace InfinityRun
{
    public class Character
    {
        private Panel _characterPanel;
        private Timer _jumpTimer;
        private int _jumpHeight = 120;
        private bool _isJumping = false;
        private double _jumpArcProgress = 0; // Progress along the jump arc (0 to 1)
        private const int Margin = 50; // Margin from left and right wall for Game Over
        private const double JumpArcStep = 0.05; // Progress increment for the arc
        private int _jumpStartX; // Starting X position for the jump
        private double _moveDistance = 240;
        private Keys _currentKey;
        public event Action<string> GameOverEvent;
        private SoundPlayer _jumpSoundPlayer;

        public Character(GameForm gameForm)
        {
            int width = 70;
            int height = 113;

            _characterPanel = new Panel
            {
                Width = width,
                Height = height,
                Location = new Point(gameForm.ClientSize.Width / 2 - width / 2, gameForm.ClientSize.Height - height - 50), // Position at the bottom center
                BackColor = Color.Transparent
            };

            try
            {
                _characterPanel.BackgroundImage = Image.FromFile(@"assets/character.png");
                _characterPanel.BackgroundImageLayout = ImageLayout.Stretch;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading character image: " + e.Message);
            }

            gameForm.Controls.Add(_characterPanel);

            _jumpTimer = new Timer { Interval = 30 }; // Jump animation speed
            _jumpTimer.Tick += (s, e) => PerformJump(gameForm);

            // Initialize the SoundPlayer for the jump sound
            _jumpSoundPlayer = new SoundPlayer(@"assets/jump.wav");
        }

        public void OnKeyDown(KeyEventArgs e)
        {
            if (!_isJumping)
            {
                if (e.KeyCode == Keys.Right || e.KeyCode == Keys.Left)
                {
                    _jumpStartX = _characterPanel.Left; // Record starting X position
                    _currentKey = e.KeyCode;
                    _isJumping = true;
                    _jumpTimer.Start();

                    PlayJumpSound();
                }
            }
        }

        private void PerformJump(GameForm gameForm)
        {
            if (_isJumping)
            {
                _jumpArcProgress += 0.05; // Progress along the jump arc (from 0 to 1)

                // Calculate vertical position (arc effect)
                int baseY = gameForm.ClientSize.Height - _characterPanel.Height - 50;
                _characterPanel.Top = baseY - (int)(_jumpHeight * Math.Sin(_jumpArcProgress * Math.PI)); // Sinusoidal jump arc

                if (_currentKey == Keys.Right)
                {
                    _characterPanel.Left = _jumpStartX + (int)(_moveDistance * _jumpArcProgress);
                }
                else if (_currentKey == Keys.Left)
                {
                    _characterPanel.Left = _jumpStartX - (int)(_moveDistance * _jumpArcProgress);
                }

                if (_jumpArcProgress >= 1)
                {
                    _isJumping = false;
                    _jumpTimer.Stop();
                    _jumpArcProgress = 0; // Reset progress for the next jump
                }

                // Check for Game Over conditions (out of path)
                if (_characterPanel.Left <= Margin || _characterPanel.Left >= _characterPanel.Parent.ClientSize.Width - Margin - _characterPanel.Width)
                {
                    GameOverEvent?.Invoke("Game over! You're exceeding the desired path!");
                }
            }
        }

        public void CenterCharacter(GameForm gameForm)
        {
            int centerX = (gameForm.ClientSize.Width - _characterPanel.Width) / 2;
            _characterPanel.Left = centerX;
            _characterPanel.Top = gameForm.ClientSize.Height - _characterPanel.Height - 50; // Position slightly above the bottom of the form
        }

        public Panel GetPanel()
        {
            return _characterPanel;
        }

        private void PlayJumpSound()
        {
            try
            {
                _jumpSoundPlayer.Play();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing jump sound: " + ex.Message);
            }
        }
    }
}
