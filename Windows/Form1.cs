using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using ExtensionMethodsSpace;
using ImageCrusher.Inpainting;
using ImageCrusher.ImageController;
using Emgu.CV.Structure;

namespace ImageCrusher
{
    public partial class MainWindow : Form
    {
        private static ImageMenu MenuImg = new ImageMenu();     
        private static NavierStokesInpaint navierStokes = new NavierStokesInpaint();
        private static AlexandruTeleaInpaint alexandruTelea = new AlexandruTeleaInpaint();
        Noise NoiseImg = new Noise(MenuImg);

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtLoadImg_Click(object sender, EventArgs e)
        {
            try
            {
                PicBox1OrgImg.Image = MenuImg.LoadImage();
            }
            catch
            {
            }
        }

        private void TrBarNoiseController_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                NoiseMethod(TrBarRangeNoise.Value);
                PicBox2Editedmg.Image = MenuImg.GetImageOut().ToBitmap();
            }
            catch
            {
            }
        }

        private void BtScratches_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
            NoiseMethod(TrBarRangeNoise.Value);
            }
            catch
            {
            }
        }
        private void BtStarNoise_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
            NoiseMethod(TrBarRangeNoise.Value);
            }
            catch
            {
            }
        }

        private void NoiseMethod(int range)
        {
            NoiseImg = new Noise(MenuImg);
            if (BtStarNoise.Checked == true)
            {
                BtScratches.Checked = false;
                int val = TrBarNoiseController.Value;
                PicBox2Editedmg.Image = NoiseImg.MakeStarsNoise(val, -range).ToBitmap();
            }
            if (BtScratches.Checked == true)
            {
                BtStarNoise.Checked = false;
                int val = TrBarNoiseController.Value;
                PicBox2Editedmg.Image = NoiseImg.VerticalScratches(val, -range).ToBitmap();
            }
        }

        private void BtSaveImg_Click(object sender, EventArgs e)
        {
            Image<Bgr, byte> img = new Image<Bgr, byte>(MenuImg.GetImageIn().ToBitmap());
            if (alexandruTelea.GetImage()!= null)
                img = alexandruTelea.GetImage();
            if (navierStokes.GetImage()!= null)
                img = navierStokes.GetImage();

            MenuImg.SaveImage(img.ToBitmap());
        }

        private void BtCalcRMSError_Click(object sender, EventArgs e)
        {
            Indicator indicate = null;
            if (navierStokes != null)
                indicate = new Indicator(MenuImg, navierStokes);
            if (alexandruTelea != null)
            {
                indicate = new Indicator(MenuImg, alexandruTelea);
            }
            try
            {
                TxtBoxRMSerror.Text = indicate.RMSE().DisplayDouble(3);
            }
            catch
            {
            }
        }
        private void BtCalcPSNRerror_Click(object sender, EventArgs e)
        {
            Indicator indicate=null;
            if (navierStokes != null)
                 indicate = new Indicator(MenuImg, navierStokes);
            if (alexandruTelea != null)
                indicate = new Indicator(MenuImg, alexandruTelea); 

            try
            {
                double[] psnr = indicate.PNSR();
                TxtBoxPSNRRed.Text = psnr[0].DisplayDouble(3);
                TxtBoxPSNRGreen.Text = psnr[1].DisplayDouble(3);
                TxtBoxPSNRBlue.Text = psnr[2].DisplayDouble(3);
            }
            catch
            {
            }
        }



        private void BtLoadMask_Click(object sender, EventArgs e)
        {
            PicBox2Editedmg.Image = MenuImg.LoadMask();
            PicBox2Editedmg.Image = MenuImg.GetMask().ToBitmap();
        }
        private void BtInpaintNavierStokes_Click(object sender, EventArgs e)
        {
            alexandruTelea = null;
            navierStokes = new NavierStokesInpaint();
            PicBox3InPainted.Image = navierStokes.InpaintNav(MenuImg, NoiseImg).ToBitmap();
        }
        private void BtInpaintTelea_Click(object sender, EventArgs e)
        {
            navierStokes = null;
            alexandruTelea = new AlexandruTeleaInpaint();
            PicBox3InPainted.Image = alexandruTelea.InpaintTel(MenuImg, NoiseImg).ToBitmap();
        }
    }
}
