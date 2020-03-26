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
          //  SetImageToMlBuilder();                                   /// Only for ML Builder, delete later.
        }
        public void SetImageToMlBuilder()               /// Only for ML Builder, delete later.            !!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
        {
            byte[,,] Data = Img.Data;
            for (int item = 0; item < img.Height; item++)
            {
                for (int x = 0; x < img.Width+2; x++)
                {
                    if (Data[item, x, 0] == 0)
                    {
                        Data[item, x, 0] = 1;
                    }
                    if (Data[item, x, 1] == 0)
                    {
                        Data[item, x, 1] = 1;
                    }
                    if (Data[item, x, 2] == 0)
                    {
                        Data[item, x, 2] = 1;
                    }
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
