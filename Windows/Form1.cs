using System;
using System.Windows.Forms;
using Emgu.CV;
using ExtensionMethodsSpace;
using ImageCrusher.Inpainting;
using ImageCrusher.ImageController;
using Emgu.CV.Structure;
using System.Threading.Tasks;

namespace ImageCrusher
{
    public partial class MainWindow : Form
    {
        ImageMenu MenuImg;
        NavierStokesInpaint NavierStokes;
        AlexandruTeleaInpaint AlexandruTelea;
        Nans NansAlg;
        Noise NoiseImg;

        public MainWindow()
        {
            InitializeComponent();
            MenuImg = new ImageMenu();
            NavierStokes = new NavierStokesInpaint();
            AlexandruTelea = new AlexandruTeleaInpaint();
            NansAlg = new Nans();
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
        private void BtSaveImg_Click(object sender, EventArgs e)
        {
            try
            {
                Image<Rgb, byte> img = new Image<Rgb, byte>(MenuImg.Img.ToBitmap());
                if (AlexandruTelea != null)
                    img = AlexandruTelea.ImageOutTelea;
                if (NavierStokes != null)
                    img = NavierStokes.ImageOutNav;
                if (NansAlg != null)
                    img = NansAlg.ImageOutNans;

                MenuImg.SaveImage(img.ToBitmap());
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to save.");
            }
        }
        private void BtLoadMask_Click(object sender, EventArgs e)
        {
            try
            {
                MenuImg.LoadMaskGray();
                PicBox2Editedmg.Image = MenuImg.Mask.ToBitmap();
                NoiseImg.MaskLoaded = MenuImg.Mask;
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
        private void BtSaltNPepperNoise_CheckedChanged(object sender, EventArgs e)
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
            if (BtSaltNPepperNoise.Checked == true)
            {
                BtScratches.Checked = false;
                BtSquare.Checked = false;
                int val = TrBarNoiseController.Value;
                PicBox2Editedmg.Image = NoiseImg.SaltAndPepperNoise(val, -range).ToBitmap();
            }
            if (BtScratches.Checked == true)
            {
                BtSaltNPepperNoise.Checked = false;
                BtSquare.Checked = false;
                int val = TrBarNoiseController.Value;
                PicBox2Editedmg.Image = NoiseImg.VerticalScratches(val, -range).ToBitmap();
            }
            if (BtSquare.Checked == true)
            {
                BtScratches.Checked = false;
                BtSaltNPepperNoise.Checked = false;
                PicBox2Editedmg.Image = NoiseImg.Square(-range).ToBitmap();
            }
        }
        private void BtInpaintNavierStokes_Click(object sender, EventArgs e)
        {
            try
            {
                AlexandruTelea = null;
                NansAlg = null;
                NavierStokes = new NavierStokesInpaint();
                PicBox3InPainted.Image = NavierStokes.InpaintNav(MenuImg, NoiseImg, 1).ToBitmap();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to inpaint.");
            }
        }
        private void BtInpaintTelea_Click(object sender, EventArgs e)
        {
            try
            {
                NavierStokes = null;
                NansAlg = null;
                AlexandruTelea = new AlexandruTeleaInpaint();
                PicBox3InPainted.Image = AlexandruTelea.InpaintTel(MenuImg, NoiseImg, 1).ToBitmap();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to inpaint.");
            }
        }
        private void BtCalcRMSError_Click(object sender, EventArgs e)
        {
            Indicator indicate = null;
            if (NavierStokes != null)
                indicate = new Indicator(MenuImg, NavierStokes);
            if (AlexandruTelea != null)
                indicate = new Indicator(MenuImg, AlexandruTelea);
            if (NansAlg != null)
                indicate = new Indicator(MenuImg, NansAlg);

            try
            {
                TxtBoxRMSerror.Text = indicate.RMSE().DisplayDouble(3);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Insert images to compare.");
            }
        }
        private void BtCalcPSNRerror_Click(object sender, EventArgs e)
        {
            Indicator indicate = null;
            if (NavierStokes != null)
                indicate = new Indicator(MenuImg, NavierStokes);
            if (AlexandruTelea != null)
                indicate = new Indicator(MenuImg, AlexandruTelea);
            if (NansAlg != null)
                indicate = new Indicator(MenuImg, NansAlg);

            try
            {
                double[] psnr = indicate.PNSR();
                TxtBoxPSNRRed.Text = psnr[0].DisplayDouble(3);
                TxtBoxPSNRGreen.Text = psnr[1].DisplayDouble(3);
                TxtBoxPSNRBlue.Text = psnr[2].DisplayDouble(3);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Insert images to compare.");
            }
        }

        private void BtInpaintNans_Click(object sender, EventArgs e)  // private asnyc void..
        {
            //try
            //{
                NavierStokes = null;
                AlexandruTelea = null;

                if(MenuImg.ImageOut==null)
                    NansAlg = new Nans(MenuImg, NoiseImg);
                if(MenuImg.ImageOut!=null)
                    NansAlg = new Nans(MenuImg);

            //NansAlg.Compute(0);
            //NansAlg.Compute(1);
            //NansAlg.Compute(2);

            var tasks = new[]
            {
                Task.Factory.StartNew(() => NansAlg.Compute(0)),
                Task.Factory.StartNew(() => NansAlg.Compute(1)),
                Task.Factory.StartNew(() => NansAlg.Compute(2))
            };
            Task.WaitAll(tasks);

            PicBox3InPainted.Image = NansAlg.ImageOutNans.ToBitmap();

                //}
                //catch (NullReferenceException)
                //{
                //    MessageBox.Show("Nothing to inpaint.");
                //}
            }

            private void BtSaveMask_Click(object sender, EventArgs e)
        {
            try
            {
                MenuImg.SaveMask(NoiseImg.GetMask());
            }
            catch
            {
            }
        }

        private void BtRGB_Mask_Click(object sender, EventArgs e)    /// THIS BUTTON IS JUST TO TRY SOMETHING AND CAN BE PROBLEMATIC FOR PROJECT
        {
            try
            {
                MenuImg.LoadRGB_ImageAsMask();
                PicBox2Editedmg.Image = MenuImg.ImageOut.ToBitmap();
            }
            catch
            {
            }
        }
        private void BtSaveRGbMask_Click(object sender, EventArgs e)
        {
            try
            {
                MenuImg.SaveImage(NoiseImg.ImageOut.ToBitmap()); 
                
            }
            catch
            {
            }
        }
    }
}
