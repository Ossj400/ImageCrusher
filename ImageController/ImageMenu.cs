using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV.Structure;
using Emgu.CV;
using System.Windows.Forms;
using Emgu.CV.Quality;
using System.Drawing.Imaging;

/// <summary>
///  Podzielic na klasy. 
///  1. Do ładowania obrazu i przechowywania
///  2. Do zaszumiania
///  3. Do oceny 
///  4. Do inpaintaingu
///  
/// SZUM
/// Pojedyncze piksele szum i dziury kwadratowe na objętności %
///  
/// Przetestować te metody inpaintingu (można opisać)
/// 
/// 
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
        public void LoadMask()
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
