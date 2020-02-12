using System;
using System.Drawing;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;

/// <summary>
/// inpaint NaNs zaimplementować w C#
/// 
/// zrobić samo MSE i z tego RMSE i PSNR
/// </summary>
namespace ImageCrusher.ImageController
{
    class ImageMenu
    {
        Image<Rgb, byte> img;
        Image<Rgb, byte> imageOut;
        Image<Gray, byte> mask;

        public Image<Rgb, byte> Img { get => img; set => img = value; }
        public Image<Rgb, byte> ImageOut { get => imageOut; set => imageOut = value; }
        public Image<Gray, byte> Mask { get => mask; set => mask = value; }

        public void LoadImage()
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Img = new Image<Rgb, byte>(OpenFile.FileName);
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show("Wrong format. Error: " + '\n' + e.ToString());   ///////////////// ???????
                }
            }
        }
        public void LoadMaskGray()
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    Mask = new Image<Gray, byte>(OpenFile.FileName);
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show("Wrong format. Error: " + '\n' + e.ToString());   ///////////////// ???????
                }
            }
        }

        public void LoadRGB_ImageAsMask()
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    ImageOut = new Image<Rgb, byte>(OpenFile.FileName);
                }
                catch (ArgumentException e)
                {
                    MessageBox.Show("Wrong format. Error: " + '\n' + e.ToString());   ///////////////// ???????
                }
            }
        }

        public void SaveMask(Image<Gray, byte> mask)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Image Files ( *.bmp)| *.bmp";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                mask.Save(saveFile.FileName);
            }
        }

        public void SaveImage(Bitmap img)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Image Files ( *.bmp)| *.bmp";

            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                img.Save(saveFile.FileName);
            }
        }
    }
}
