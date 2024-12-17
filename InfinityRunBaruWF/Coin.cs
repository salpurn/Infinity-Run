using System;
using System.Drawing;
using System.Windows.Forms;

namespace InfinityRun
{
    public class Coin
    {
        private PictureBox _coinPictureBox;
        private Random _random;
        private int _baseSpeed;

        public Coin(Size gameSize)
        {
            _random = new Random();
            _baseSpeed = 3;

            // Calculate the center of the game window and define spawn positions
            int centerX = (gameSize.Width - 20) / 2; // Adjust for coin width (20px)
            int[] spawnPositions = new int[] {
                centerX - 240, // 240 pixels to the left of center
                centerX,       // Center
                centerX + 240  // 240 pixels to the right of center
            };

            // Initialize the coin's PictureBox with proper settings
            _coinPictureBox = new PictureBox
            {
                Size = new Size(20, 20), // Coin size
                Location = new Point(spawnPositions[_random.Next(0, spawnPositions.Length)], -20), // Random spawn position
                BackgroundImage = Image.FromFile(@"assets\coin.png"),
                BackgroundImageLayout = ImageLayout.Stretch,
                BackColor = Color.Transparent // Make the background transparent
            };
        }

        public PictureBox GetPictureBox()
        {
            return _coinPictureBox;
        }

        public void Fall(int score)
        {
            double speedMultiplier = 1 + Math.Log10(1 + score / 15.0); // Gradual increase of speed every 15 points
            _coinPictureBox.Top += (int)(_baseSpeed * speedMultiplier);
        }

        public bool IsOutOfBounds(Size gameSize)
        {
            return _coinPictureBox.Top > gameSize.Height;
        }
    }
}
