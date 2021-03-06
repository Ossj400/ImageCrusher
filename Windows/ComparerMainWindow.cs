﻿using System;
using System.Windows.Forms;
using Emgu.CV;
using ExtensionMethodsSpace;
using ImageCrusher.Inpainting;
using ImageCrusher.ImageController;
using Emgu.CV.Structure;
using System.Threading.Tasks;
using Emgu.CV.XPhoto;
using DataGeneratorSpace;

namespace ImageCrusher
{
    public partial class MainWindow : Form
    {
        ImageMenu MenuImg;
        NavierStokesInpaint NavierStokes;
        AlexandruTeleaInpaint AlexandruTelea;
        FrequencySelectiveReconstruction FSR;
        Nans NansAlg;
        Noise NoiseImg;
        DataGenerator data;

        public MainWindow()
        {
            InitializeComponent();
            MenuImg = new ImageMenu();
            NavierStokes = new NavierStokesInpaint();
            AlexandruTelea = new AlexandruTeleaInpaint();
            FSR = new FrequencySelectiveReconstruction();
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
                Image<Bgr, byte> img = MenuImg.Img.Copy();
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
            try
            {
                if (BtSaltNPepperNoise.Checked == true)
                {
                    BtScratches.Checked = false;
                    BtSquare.Checked = false;
                    int val = TrBarNoiseController.Value;
                    PicBox2Editedmg.Image = NoiseImg.SaltAndPepperNoise(val, -range).ToBitmap();
                    //PicBox2Editedmg.Image = NoiseImg.SaltAndPepperNoiseNumbed(40).ToBitmap();
                }
                //if (BtSaltNPepperNoise.Checked == true && NoiseAmountInput.Text != null)
                //{
                //    BtScratches.Checked = false;
                //    BtSquare.Checked = false;
                //    int val = Convert.ToInt32(NoiseAmountInput.Text);
                //    PicBox2Editedmg.Image = NoiseImg.SaltAndPepperNoiseNumbed(val).ToBitmap();
                //}

                if (BtScratches.Checked == true)
                {
                    BtSaltNPepperNoise.Checked = false;
                    BtSquare.Checked = false;
                    int val = TrBarNoiseController.Value;
                    PicBox2Editedmg.Image = NoiseImg.DiagonalScratches(val, -range).ToBitmap();
                }
                if (BtSquare.Checked == true)
                {
                    BtScratches.Checked = false;
                    BtSaltNPepperNoise.Checked = false;
                    PicBox2Editedmg.Image = NoiseImg.Square(-range).ToBitmap();
                }
            }
            catch
            {
            }
        }
        private void BtInpaintNavierStokes_Click(object sender, EventArgs e)
        {
            try
            {
                AlexandruTelea = null;
                NansAlg = null;
                FSR = null;
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
                FSR = null;
                AlexandruTelea = new AlexandruTeleaInpaint();
                PicBox3InPainted.Image = AlexandruTelea.InpaintTel(MenuImg, NoiseImg, 1).ToBitmap();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to inpaint.");
            }
        }  

        private void BtInpaintNans_Click(object sender, EventArgs e)
        {
            try
            {
                NavierStokes = null;
                AlexandruTelea = null;
                FSR = null;

                if (MenuImg.ImageOut == null)
                    NansAlg = new Nans(MenuImg, NoiseImg);
                if (MenuImg.ImageOut != null)
                    NansAlg = new Nans(MenuImg);

                var tasks = new[]
                {
                    Task.Factory.StartNew(() => NansAlg.Compute(0)),
                    Task.Factory.StartNew(() => NansAlg.Compute(1)),
                    Task.Factory.StartNew(() => NansAlg.Compute(2))
                };
                Task.WaitAll(tasks);
                PicBox3InPainted.Image = NansAlg.ImageOutNans.ToBitmap();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to inpaint.");
            }
        }

        private void BtInpaintFSR_Click(object sender, EventArgs e)
        {
            try
            {
                NavierStokes = null;
                AlexandruTelea = null;
                NansAlg = null;
                FSR = new FrequencySelectiveReconstruction();
                FSR.InpaintFSR(MenuImg, NoiseImg, XPhotoInvoke.InpaintType.FsrBest);
                PicBox3InPainted.Image = FSR.ImageOutFSR.ToImage<Bgr, byte>().ToBitmap();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to inpaint.");
            }
        }

        private void BtInpaintFSRFast_Click(object sender, EventArgs e)
        {
            try
            {
                NavierStokes = null;
                AlexandruTelea = null;
                NansAlg = null;
                FSR = new FrequencySelectiveReconstruction();
                FSR.InpaintFSR(MenuImg, NoiseImg, XPhotoInvoke.InpaintType.FsrFast);
                PicBox3InPainted.Image = FSR.ImageOutFSR.ToImage<Bgr, byte>().ToBitmap();
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Nothing to inpaint.");
            }
        }

        private void BtInpaintShiftMap_Click(object sender, EventArgs e)
        {
            try
            {
                NavierStokes = null;
                AlexandruTelea = null;
                NansAlg = null;
                FSR = new FrequencySelectiveReconstruction();
                FSR.InpaintFSR(MenuImg, NoiseImg, XPhotoInvoke.InpaintType.Shiftmap);
                PicBox3InPainted.Image = FSR.ImageOutFSR.ToImage<Bgr, byte>().ToBitmap();
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
            if (FSR != null)
                indicate = new Indicator(MenuImg, FSR);

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
            if (FSR != null)
                indicate = new Indicator(MenuImg, FSR);

            try
            {
                TxtBoxCalcPSNR.Text = indicate.PNSR().DisplayDouble(3);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Insert images to compare.");
            }
        }

        private void BtCalcGMSD_Click(object sender, EventArgs e)
        {
            Indicator indicate = null;
            if (NavierStokes != null)
                indicate = new Indicator(MenuImg, NavierStokes);
            if (AlexandruTelea != null)
                indicate = new Indicator(MenuImg, AlexandruTelea);
            if (NansAlg != null)
                indicate = new Indicator(MenuImg, NansAlg);
            if (FSR != null)
                indicate = new Indicator(MenuImg, FSR);

            try
            {
                TxtBoxCalcGMSD.Text = indicate.GMSD().DisplayDouble(3);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Insert images to compare.");
            }

        }

        private void BtCalcSSIM_Click(object sender, EventArgs e)
        {
            Indicator indicate = null;
            if (NavierStokes != null)
                indicate = new Indicator(MenuImg, NavierStokes);
            if (AlexandruTelea != null)
                indicate = new Indicator(MenuImg, AlexandruTelea);
            if (NansAlg != null)
                indicate = new Indicator(MenuImg, NansAlg);
            if (FSR != null)
                indicate = new Indicator(MenuImg, FSR);

            try
            {
                TxtBoxCalcSSIM.Text = indicate.SSIM().DisplayDouble(3);
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Insert images to compare.");
            }
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

        private void BtRGB_Mask_Click(object sender, EventArgs e)   
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

        private void BtAutomate_Click(object sender, EventArgs e)
        {
                MessageBox.Show("Please choose directory to make automatic tests of algorithms by picking photo from folder.");
                MenuImg.LoadImage();
                PicBox1OrgImg.Image = MenuImg.Img.ToBitmap();
                MessageBox.Show("Please choose directory to make automatic tests of algorithms by picking mask from folder.");
                MenuImg.LoadMaskGray();
                PicBox2Editedmg.Image = MenuImg.Mask.ToBitmap();
                NoiseImg.MaskLoaded = MenuImg.Mask;
            //data = new DataGenerator(MenuImg);
            //data.GenTelea();
            //data = new DataGenerator(MenuImg);
            //data.GenNStokes();
            //data = new DataGenerator(MenuImg);
            //data.GenFSRF();
            //data = new DataGenerator(MenuImg);
            //data.GenShiftM();
            data = new DataGenerator(MenuImg);
            data.GenFsrB();
            //data = new DataGenerator(MenuImg);
            //data.GenNans();
            //data = new DataGenerator(MenuImg);
        }
    }
}
