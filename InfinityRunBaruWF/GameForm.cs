using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Windows.Forms;

namespace InfinityRun
{
    public partial class GameForm : Form
    {
        private Timer _gameTimer;
        private Random _random;
        private Character _character;
        private List<Coin> _coins;
        private List<Obstacle> _obstacles;
        private ScrollingBackground _scrollingBackground;
        private int _coinSpawnCounter;
        private int _obstacleSpawnCounter;
        private int _score;
        private Leaderboard _leaderboard;
        private bool _isGameOver;
        private SoundPlayer _player;

        public int Score
        {
            get => _score; private set => _score = value;
        }

        public GameForm(Leaderboard leaderboard)
        {
            InitializeComponents();

            _leaderboard = leaderboard;
            _scrollingBackground = new ScrollingBackground(@"assets/Canvas.png", 2, this.ClientSize);
        }

        private void InitializeComponents()
        {
            this.ClientSize = new Size(800, 600);
            this.Text = "Infinity Run";
            this.DoubleBuffered = true;
            _random = new Random();
            _coins = new List<Coin>();
            _obstacles = new List<Obstacle>();
            _character = new Character(this);
            Score = 0;
            _isGameOver = false;

            _gameTimer = new Timer
            {
                Interval = 8
            };
            _gameTimer.Tick += GameTick;
            _gameTimer.Start();

            this.KeyDown += (s, e) => _character.OnKeyDown(e);

            this.Resize += OnResize;

            _character.GameOverEvent += GameOver;

            _player = new SoundPlayer();
        }

        private void OnResize(object sender, EventArgs e)
        {
            _character.CenterCharacter(this);
        }

        private void GameTick(object sender, EventArgs e)
        {
            if (_isGameOver) return;

            _scrollingBackground.Update();

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
                _coins[i].Fall(Score); // Pass the score to adjust speed
                if (_coins[i].IsOutOfBounds(this.ClientSize))
                {
                    this.Controls.Remove(_coins[i].GetPictureBox());
                    _coins.RemoveAt(i);
                }
                else if (_coins[i].GetPictureBox().Bounds.IntersectsWith(_character.GetPanel().Bounds))
                {
                    // Coin collected, play sound
/*                    PlaySound("InfinityRun.assets.hitcoin.wav");
*/                    this.Controls.Remove(_coins[i].GetPictureBox());
                    _coins.RemoveAt(i);
                    Score += 5;
                }
            }

            // Handle obstacle logic
            for (int i = _obstacles.Count - 1; i >= 0; i--)
            {
                _obstacles[i].Fall(Score); // Pass the score to adjust speed
                if (_obstacles[i].IsOutOfBounds(this.ClientSize))
                {
                    this.Controls.Remove(_obstacles[i].GetPictureBox());
                    _obstacles.RemoveAt(i);
                }
                else if (_obstacles[i].GetPictureBox().Bounds.IntersectsWith(_character.GetPanel().Bounds))
                {
                    // Obstacle hit, play sound and game over
/*                    PlaySound("InfinityRun.assets.hitobstacle.wav");
*/                    GameOver("");
                    return;
                }
            }

            this.Text = $"Infinity Run - Current Score: {Score}";
            this.Invalidate(); // Force the form to repaint
        }

        private void PlaySound(string resourceName)
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    _player.Stream = stream;
                    _player.Play();
                }
                else
                {
                    Console.WriteLine("Error: Sound resource not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error playing sound: " + ex.Message);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            _scrollingBackground.Draw(e.Graphics, this.ClientSize);
        }

        private void GameOver(string message)
        {
            if (_isGameOver) return;

            _isGameOver = true;
            _gameTimer.Stop();

            _leaderboard.AddScore(Score);

            int highestScore = _leaderboard.GetHighestScore();
            string finalMessage = string.IsNullOrEmpty(message)
                ? (Score == highestScore
                    ? $"Game Over! Highest score yet! You've made it!\nYour Score: {Score}"
                    : $"Game Over! Better luck next time!\nYour Score: {Score}, Highest Score: {highestScore}")
                : message;

            MessageBox.Show(finalMessage, "Infinity Run", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Application.Exit();
        }
    }
}