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

namespace ImageCrusher
{
    public partial class Form1 : Form
    {
        ImageController ImageControl = new ImageController();
        public Form1()
        {
            InitializeComponent();
        }

        private void BtLoadImg_Click(object sender, EventArgs e)
        {
            PicBox1OrgImg.Image = ImageControl.LoadImage();
        }

        private void TrBarNoiseController_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                NoiseMethod(TrBarRangeNoise.Value);
                PicBox2Editedmg.Image = ImageControl.GetImage().ToBitmap();
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
            if (BtStarNoise.Checked == true)
            {
                BtScratches.Checked = false;
                int val = TrBarNoiseController.Value;
                PicBox2Editedmg.Image = ImageControl.MakeStarsNoise(val, -range).ToBitmap();
            }
            if (BtScratches.Checked == true)
            {
                BtStarNoise.Checked = false;
                int val = TrBarNoiseController.Value;
                PicBox2Editedmg.Image = ImageControl.VerticalScratches(val, -range).ToBitmap();
            }
        }

        private void BtSaveImg_Click(object sender, EventArgs e)
        {
            ImageControl.SaveImage();
        }

        private void BtCalcRMSError_Click(object sender, EventArgs e)
        {
            try
            { 
            TxtBoxRMSError.Text = ImageControl.RMSE();
            }
            catch
            {
            }
        }
    }
}
