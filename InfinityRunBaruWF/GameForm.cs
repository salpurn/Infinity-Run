using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace InfinityRun
{
    public class GameForm : Form
    {
        private Timer _gameTimer;
        private Character _character;
        private List<Coin> _coins;
        private List<Obstacle> _obstacles;
        private PictureBox _background1;
        private PictureBox _background2;
        private int _baseBackgroundSpeed = 3;
        private int _backgroundSpeed;
        private Random _random;
        private int _coinSpawnCounter;
        private int _obstacleSpawnCounter;
        private int _score;

        public GameForm()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.ClientSize = new Size(800, 600);
            this.Text = "Infinity Run";
            this.DoubleBuffered = true; // Prevent flickering
            this.BackColor = Color.LightBlue;

            // Initialize background logic
            _background1 = new PictureBox
            {
                Size = new Size(800, 600),
                Location = new Point(0, 0),
                Image = Image.FromFile(@"assets\path.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            _background2 = new PictureBox
            {
                Size = new Size(800, 600),
                Location = new Point(0, -600), // Positioned directly above the first background
                Image = Image.FromFile(@"assets\path.png"),
                SizeMode = PictureBoxSizeMode.StretchImage
            };

            this.Controls.Add(_background1);
            this.Controls.Add(_background2);

            _random = new Random();
            _coins = new List<Coin>();
            _obstacles = new List<Obstacle>();
            _character = new Character(this);
            _score = 0;

            _gameTimer = new Timer
            {
                Interval = 8 // Roughly 120 FPS
            };
            _gameTimer.Tick += GameTick;
            _gameTimer.Start();

            this.KeyDown += OnKeyDown;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            _character.Move(e, this);
        }

        private void GameTick(object sender, EventArgs e)
        {
            UpdateBackgroundSpeed();
            ScrollBackground();

            // Spawn coins periodically
            _coinSpawnCounter++;
            if (_coinSpawnCounter >= 600)
            {
                var coin = new Coin(this.ClientSize);
                _coins.Add(coin);
                this.Controls.Add(coin.GetPictureBox());
                _coinSpawnCounter = 0;
            }

            // Spawn obstacles periodically
            _obstacleSpawnCounter++;
            if (_obstacleSpawnCounter >= 500)
            {
                var obstacle = new Obstacle(this.ClientSize);
                _obstacles.Add(obstacle);
                this.Controls.Add(obstacle.GetPictureBox());
                _obstacleSpawnCounter = 0;
            }

            // Handle coin logic
            for (int i = _coins.Count - 1; i >= 0; i--)
            {
                _coins[i].Fall(_score); // Pass the score to adjust speed
                if (_coins[i].IsOutOfBounds(this.ClientSize))
                {
                    this.Controls.Remove(_coins[i].GetPictureBox());
                    _coins.RemoveAt(i);
                }
                else if (_coins[i].GetPictureBox().Bounds.IntersectsWith(_character.GetPanel().Bounds))
                {
                    // Coin collected
                    this.Controls.Remove(_coins[i].GetPictureBox());
                    _coins.RemoveAt(i);
                    _score += 5;
                }
            }

            // Handle obstacle logic
            for (int i = _obstacles.Count - 1; i >= 0; i--)
            {
                _obstacles[i].Fall(_score); // Pass the score to adjust speed
                if (_obstacles[i].IsOutOfBounds(this.ClientSize))
                {
                    this.Controls.Remove(_obstacles[i].GetPictureBox());
                    _obstacles.RemoveAt(i);
                }
                else if (_obstacles[i].GetPictureBox().Bounds.IntersectsWith(_character.GetPanel().Bounds))
                {
                    // Obstacle hit
                    this.Controls.Remove(_obstacles[i].GetPictureBox());
                    _obstacles.RemoveAt(i);
                    _score -= 5;

                    if (_score <= 0)
                    {
                        GameOver();
                        return; // Exit GameTick after game over
                    }
                }
            }

            this.Text = $"Infinity Run - Current Score: {_score}";
        }

        private void UpdateBackgroundSpeed()
        {
            double speedMultiplier = 1 + Math.Log10(1 + _score / 15.0); // Gradual increase of speed every 15 points
            _backgroundSpeed = (int)(_baseBackgroundSpeed * speedMultiplier);
        }
        private void ScrollBackground()
        {
            // Move the backgrounds downward with updated speed
            _background1.Top += _backgroundSpeed;
            _background2.Top += _backgroundSpeed;

            // Reset positions when they move out of view
            if (_background1.Top >= this.ClientSize.Height)
                _background1.Top = _background2.Top - _background1.Height;

            if (_background2.Top >= this.ClientSize.Height)
                _background2.Top = _background1.Top - _background2.Height;
        }
        private void GameOver()
        {
            _gameTimer.Stop();
            MessageBox.Show($"Game Over! Better luck next time!", "Infinity Run", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Application.Exit();
        }

    }
}
