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
        Image<Bgr, byte> image;
        Image<Bgr, byte> imageOut;
        Image<Gray, byte> mask;  // delete this ?
        public Bitmap LoadImage()
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                image = new Image<Bgr, byte>(OpenFile.FileName);
            }
            return image.ToBitmap();
        }
        public Bitmap LoadMask()
        {
            OpenFileDialog OpenFile = new OpenFileDialog();
            if (OpenFile.ShowDialog() == DialogResult.OK)
            {
                mask = new Image<Gray, byte>(OpenFile.FileName);
            }
            return mask.ToBitmap();
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
        public Image<Bgr, Byte> GetImageOut()
        {
            return imageOut;
        }
        public Image<Bgr, Byte> GetImageIn()
        {
            return image;
        }
        public Image<Gray, Byte> GetMask()
        {
            return mask;
        }
    }
}
