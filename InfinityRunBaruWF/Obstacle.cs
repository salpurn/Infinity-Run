using System;
using System.Drawing;
using System.Windows.Forms;

namespace InfinityRun
{
    public class Obstacle
    {
        private PictureBox _obstaclePictureBox;
        private Random _random;
        private int _baseSpeed;

        private int width; // Set obstacle width to 50
        private int height; // Height will be calculated based on the width

        // Define the spawn positions (240 left of center, center, and 240 right of center)
        private int[] _spawnPositions;

        public Obstacle(Size gameSize)
        {
            // Set initial width and calculate height based on width
            width = 50; // Set obstacle width to 50
            height = (int)(228.0 / 248.0 * width); // Calculate height based on the width ratio

            _random = new Random();
            _baseSpeed = 3;

            // Define spawn positions: 240 pixels to the left of center, center, and 240 pixels to the right of center
            int centerX = (gameSize.Width - width) / 2;  // Adjust center to account for the obstacle's width
            _spawnPositions = new int[] {
                centerX - 240, // 240 pixels to the left of center
                centerX, // Center
                centerX + 240 // 240 pixels to the right of center
            };

            // Initialize the obstacle's PictureBox with proper settings
            _obstaclePictureBox = new PictureBox
            {
                Size = new Size(width, height),
                Location = new Point(_spawnPositions[_random.Next(0, _spawnPositions.Length)], -height), // Random spawn position
                Image = Image.FromFile(@"assets\obstacle.png"), // Use Image for proper transparency handling
                SizeMode = PictureBoxSizeMode.StretchImage // Ensure the image stretches to fit the PictureBox
            };

            // Set the PictureBox background to transparent to avoid any unwanted background color
            _obstaclePictureBox.BackColor = Color.Transparent;
        }

        public PictureBox GetPictureBox()
        {
            return _obstaclePictureBox;
        }

        public void Fall(int score)
        {
            double speedMultiplier = 1 + Math.Log10(1 + score / 15.0); // Gradual increase of speed every 15 points
            _obstaclePictureBox.Top += (int)(_baseSpeed * speedMultiplier);
        }

        public bool IsOutOfBounds(Size gameSize)
        {
            return _obstaclePictureBox.Top > gameSize.Height;
        }
    }
}
