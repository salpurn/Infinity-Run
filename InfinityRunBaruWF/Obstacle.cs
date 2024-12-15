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

        public Obstacle(Size gameSize)
        {
            _random = new Random();
            _baseSpeed = 3;

            _obstaclePictureBox = new PictureBox
            {
                Size = new Size(30, 30),
                Location = new Point(_random.Next(0, gameSize.Width - 30), -30),
                BackgroundImage = Image.FromFile(@"assets\obstacle.png"),
                BackgroundImageLayout = ImageLayout.Stretch
            };
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
