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

            _coinPictureBox = new PictureBox
            {
                Size = new Size(20, 20),
                Location = new Point(_random.Next(0, gameSize.Width - 20), -20),
                BackgroundImage = Image.FromFile(@"assets\coin.png"),
                BackgroundImageLayout = ImageLayout.Stretch
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
