using System;
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
        ImageMenu MenuImg;   
        NavierStokesInpaint navierStokes;
        AlexandruTeleaInpaint alexandruTelea;
        Noise NoiseImg;

        public MainWindow()
        {
            InitializeComponent();
            MenuImg = new ImageMenu();
            navierStokes = new NavierStokesInpaint();
            alexandruTelea = new AlexandruTeleaInpaint();
            NoiseImg = new Noise(MenuImg);
        }

        private void BtLoadImg_Click(object sender, EventArgs e)
        {
            MenuImg.LoadImage();
            try
            {
                PicBox1OrgImg.Image = MenuImg.Img.ToBitmap();
            }
            catch
            {
            }
        }

        private void TrBarNoiseController_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                NoiseImg = new Noise(MenuImg);
                NoiseImg.MaskLoaded = null;
                NoiseMethod(TrBarRangeNoise.Value);
                PicBox2Editedmg.Image = NoiseImg.ImageOut.ToBitmap();
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
        private void BtSquare_CheckedChanged(object sender, EventArgs e)
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
            if (BtStarNoise.Checked == true)
            {
                BtScratches.Checked = false;
                BtSquare.Checked = false;
                int val = TrBarNoiseController.Value;
                PicBox2Editedmg.Image = NoiseImg.MakeStarsNoise(val, -range).ToBitmap();
            }
            if (BtScratches.Checked == true)
            {
                BtStarNoise.Checked = false;
                BtSquare.Checked = false;
                int val = TrBarNoiseController.Value;
                PicBox2Editedmg.Image = NoiseImg.VerticalScratches(val, -range).ToBitmap();
            }
            if (BtSquare.Checked == true)
            {
                BtScratches.Checked = false;
                BtStarNoise.Checked = false;
                PicBox2Editedmg.Image = NoiseImg.Square(-range).ToBitmap();
            }
        }

        private void BtSaveImg_Click(object sender, EventArgs e)
        {
            Image<Rgb, byte> img = new Image<Rgb, byte>(MenuImg.Img.ToBitmap());
            if (alexandruTelea != null)
                img = alexandruTelea.ImageOutTelea;
            if (navierStokes != null)
                img = navierStokes.ImageOutNav;
            try
            {
                MenuImg.SaveImage(img.ToBitmap());
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to save. Error: ");
            }
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
            catch (NullReferenceException)
            {
                MessageBox.Show("Insert images to compare");
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
            catch (NullReferenceException)
            {
                MessageBox.Show("Insert images to compare");
            }
        }

        private void BtLoadMask_Click(object sender, EventArgs e)
        {
            try
            {
                MenuImg.LoadMask();
                PicBox2Editedmg.Image = MenuImg.Mask.ToBitmap();
                NoiseImg.MaskLoaded = MenuImg.Mask;
            }
            catch
            { 
            }
        }
        private void BtInpaintNavierStokes_Click(object sender, EventArgs e)
        {
            try
            {
                alexandruTelea = null;
                navierStokes = new NavierStokesInpaint();
                PicBox3InPainted.Image = navierStokes.InpaintNav(MenuImg, NoiseImg, 1).ToBitmap();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to inpaint");
            }
        }
        private void BtInpaintTelea_Click(object sender, EventArgs e)
        {
            try
            {
                navierStokes = null;
                alexandruTelea = new AlexandruTeleaInpaint();
                PicBox3InPainted.Image = alexandruTelea.InpaintTel(MenuImg, NoiseImg, 1).ToBitmap();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to inpaint");
            }
        }

    }
}
