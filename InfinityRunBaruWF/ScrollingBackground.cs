using System.Drawing;
using System.Windows.Forms;

namespace InfinityRun
{
    public class ScrollingBackground
    {
        private Image _backgroundImage;
        private int _scrollSpeed;
        private int _position1;
        private int _position2;

        public ScrollingBackground(string imagePath, int scrollSpeed, Size clientSize)
        {
            _backgroundImage = Image.FromFile(imagePath);
            _scrollSpeed = scrollSpeed;
            _position1 = 0;
            _position2 = -clientSize.Height; // Posisi gambar kedua langsung di bawah gambar pertama
        }

        public void Update()
        {
            _position1 += _scrollSpeed;
            _position2 += _scrollSpeed;

            // Reset posisi jika gambar keluar dari layar
            if (_position1 >= _backgroundImage.Height)
                _position1 = _position2 - _backgroundImage.Height;
            if (_position2 >= _backgroundImage.Height)
                _position2 = _position1 - _backgroundImage.Height;
        }

        public void Draw(Graphics g, Size clientSize)
        {
            g.DrawImage(_backgroundImage, 0, _position1, clientSize.Width, clientSize.Height);
            g.DrawImage(_backgroundImage, 0, _position2, clientSize.Width, clientSize.Height);
        }
    }
}
