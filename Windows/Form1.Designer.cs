namespace ImageCrusher
{
    partial class MainWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.TrBarNoiseController = new System.Windows.Forms.TrackBar();
            this.PicBox2Editedmg = new System.Windows.Forms.PictureBox();
            this.PicBox1OrgImg = new System.Windows.Forms.PictureBox();
            this.BtLoadImg = new System.Windows.Forms.Button();
            this.BtSaveImg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LblRangeNoise = new System.Windows.Forms.Label();
            this.TrBarRangeNoise = new System.Windows.Forms.TrackBar();
            this.BtScratches = new System.Windows.Forms.RadioButton();
            this.BtSaltNPepperNoise = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.BtCalcRMSerror = new System.Windows.Forms.Button();
            this.TxtBoxRMSerror = new System.Windows.Forms.TextBox();
            this.BtCalcPSNRerror = new System.Windows.Forms.Button();
            this.PicBox3InPainted = new System.Windows.Forms.PictureBox();
            this.BtInpaintNavierStokes = new System.Windows.Forms.Button();
            this.BtLoadMask = new System.Windows.Forms.Button();
            this.BtInpaintTelea = new System.Windows.Forms.Button();
            this.BtSquare = new System.Windows.Forms.RadioButton();
            this.BtInpaintNans = new System.Windows.Forms.Button();
            this.BtSaveMask = new System.Windows.Forms.Button();
            this.BtRGB_Mask = new System.Windows.Forms.Button();
            this.BtSaveRGbMask = new System.Windows.Forms.Button();
            this.BtInpaintFSRBest = new System.Windows.Forms.Button();
            this.TxtBoxCalcPSNR = new System.Windows.Forms.TextBox();
            this.TxtBoxCalcGMSD = new System.Windows.Forms.TextBox();
            this.BtCalcGMSD = new System.Windows.Forms.Button();
            this.TxtBoxCalcSSIM = new System.Windows.Forms.TextBox();
            this.BtCalcSSIM = new System.Windows.Forms.Button();
            this.BtInpaintFSRFast = new System.Windows.Forms.Button();
            this.BtInpaintShiftMap = new System.Windows.Forms.Button();
            this.BtAutomate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TrBarNoiseController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox2Editedmg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox1OrgImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrBarRangeNoise)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox3InPainted)).BeginInit();
            this.SuspendLayout();
            // 
            // TrBarNoiseController
            // 
            this.TrBarNoiseController.LargeChange = 1;
            this.TrBarNoiseController.Location = new System.Drawing.Point(163, 696);
            this.TrBarNoiseController.Maximum = 11;
            this.TrBarNoiseController.Name = "TrBarNoiseController";
            this.TrBarNoiseController.Size = new System.Drawing.Size(185, 56);
            this.TrBarNoiseController.TabIndex = 0;
            this.TrBarNoiseController.ValueChanged += new System.EventHandler(this.TrBarNoiseController_ValueChanged);
            // 
            // PicBox2Editedmg
            // 
            this.PicBox2Editedmg.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PicBox2Editedmg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicBox2Editedmg.Location = new System.Drawing.Point(650, 12);
            this.PicBox2Editedmg.Name = "PicBox2Editedmg";
            this.PicBox2Editedmg.Size = new System.Drawing.Size(597, 532);
            this.PicBox2Editedmg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBox2Editedmg.TabIndex = 1;
            this.PicBox2Editedmg.TabStop = false;
            // 
            // PicBox1OrgImg
            // 
            this.PicBox1OrgImg.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PicBox1OrgImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicBox1OrgImg.Location = new System.Drawing.Point(12, 12);
            this.PicBox1OrgImg.Name = "PicBox1OrgImg";
            this.PicBox1OrgImg.Size = new System.Drawing.Size(608, 532);
            this.PicBox1OrgImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBox1OrgImg.TabIndex = 2;
            this.PicBox1OrgImg.TabStop = false;
            // 
            // BtLoadImg
            // 
            this.BtLoadImg.Location = new System.Drawing.Point(12, 582);
            this.BtLoadImg.Name = "BtLoadImg";
            this.BtLoadImg.Size = new System.Drawing.Size(108, 30);
            this.BtLoadImg.TabIndex = 3;
            this.BtLoadImg.Text = "Load Image";
            this.BtLoadImg.UseVisualStyleBackColor = true;
            this.BtLoadImg.Click += new System.EventHandler(this.BtLoadImg_Click);
            // 
            // BtSaveImg
            // 
            this.BtSaveImg.Location = new System.Drawing.Point(1531, 710);
            this.BtSaveImg.Name = "BtSaveImg";
            this.BtSaveImg.Size = new System.Drawing.Size(163, 31);
            this.BtSaveImg.TabIndex = 4;
            this.BtSaveImg.Text = "Save Inpainted Image";
            this.BtSaveImg.UseVisualStyleBackColor = true;
            this.BtSaveImg.Click += new System.EventHandler(this.BtSaveImg_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(184, 676);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Pick noise amount";
            // 
            // LblRangeNoise
            // 
            this.LblRangeNoise.AutoSize = true;
            this.LblRangeNoise.Location = new System.Drawing.Point(401, 676);
            this.LblRangeNoise.Name = "LblRangeNoise";
            this.LblRangeNoise.Size = new System.Drawing.Size(154, 17);
            this.LblRangeNoise.TabIndex = 7;
            this.LblRangeNoise.Text = "Pick size of noise signs";
            // 
            // TrBarRangeNoise
            // 
            this.TrBarRangeNoise.LargeChange = 1;
            this.TrBarRangeNoise.Location = new System.Drawing.Point(386, 696);
            this.TrBarRangeNoise.Maximum = 100;
            this.TrBarRangeNoise.Name = "TrBarRangeNoise";
            this.TrBarRangeNoise.Size = new System.Drawing.Size(169, 56);
            this.TrBarRangeNoise.TabIndex = 6;
            // 
            // BtScratches
            // 
            this.BtScratches.AutoSize = true;
            this.BtScratches.Location = new System.Drawing.Point(354, 587);
            this.BtScratches.Name = "BtScratches";
            this.BtScratches.Size = new System.Drawing.Size(135, 21);
            this.BtScratches.TabIndex = 8;
            this.BtScratches.TabStop = true;
            this.BtScratches.Text = "Diagonal scratch";
            this.BtScratches.UseVisualStyleBackColor = true;
            this.BtScratches.CheckedChanged += new System.EventHandler(this.BtScratches_CheckedChanged);
            // 
            // BtSaltNPepperNoise
            // 
            this.BtSaltNPepperNoise.AutoSize = true;
            this.BtSaltNPepperNoise.Location = new System.Drawing.Point(354, 614);
            this.BtSaltNPepperNoise.Name = "BtSaltNPepperNoise";
            this.BtSaltNPepperNoise.Size = new System.Drawing.Size(134, 21);
            this.BtSaltNPepperNoise.TabIndex = 9;
            this.BtSaltNPepperNoise.TabStop = true;
            this.BtSaltNPepperNoise.Text = "Salt and pepper ";
            this.BtSaltNPepperNoise.UseVisualStyleBackColor = true;
            this.BtSaltNPepperNoise.CheckedChanged += new System.EventHandler(this.BtSaltNPepperNoise_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.8F);
            this.label2.Location = new System.Drawing.Point(158, 596);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Choose noise type";
            // 
            // BtCalcRMSerror
            // 
            this.BtCalcRMSerror.Location = new System.Drawing.Point(650, 656);
            this.BtCalcRMSerror.Name = "BtCalcRMSerror";
            this.BtCalcRMSerror.Size = new System.Drawing.Size(163, 30);
            this.BtCalcRMSerror.TabIndex = 11;
            this.BtCalcRMSerror.Text = "Calculate RMS Error";
            this.BtCalcRMSerror.UseVisualStyleBackColor = true;
            this.BtCalcRMSerror.Click += new System.EventHandler(this.BtCalcRMSError_Click);
            // 
            // TxtBoxRMSerror
            // 
            this.TxtBoxRMSerror.Location = new System.Drawing.Point(830, 660);
            this.TxtBoxRMSerror.Name = "TxtBoxRMSerror";
            this.TxtBoxRMSerror.Size = new System.Drawing.Size(90, 22);
            this.TxtBoxRMSerror.TabIndex = 12;
            // 
            // BtCalcPSNRerror
            // 
            this.BtCalcPSNRerror.Location = new System.Drawing.Point(650, 692);
            this.BtCalcPSNRerror.Name = "BtCalcPSNRerror";
            this.BtCalcPSNRerror.Size = new System.Drawing.Size(163, 30);
            this.BtCalcPSNRerror.TabIndex = 13;
            this.BtCalcPSNRerror.Text = "Calculate PSNR";
            this.BtCalcPSNRerror.UseVisualStyleBackColor = true;
            this.BtCalcPSNRerror.Click += new System.EventHandler(this.BtCalcPSNRerror_Click);
            // 
            // PicBox3InPainted
            // 
            this.PicBox3InPainted.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PicBox3InPainted.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicBox3InPainted.Location = new System.Drawing.Point(1278, 12);
            this.PicBox3InPainted.Name = "PicBox3InPainted";
            this.PicBox3InPainted.Size = new System.Drawing.Size(597, 532);
            this.PicBox3InPainted.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PicBox3InPainted.TabIndex = 17;
            this.PicBox3InPainted.TabStop = false;
            // 
            // BtInpaintNavierStokes
            // 
            this.BtInpaintNavierStokes.Location = new System.Drawing.Point(1383, 571);
            this.BtInpaintNavierStokes.Name = "BtInpaintNavierStokes";
            this.BtInpaintNavierStokes.Size = new System.Drawing.Size(152, 31);
            this.BtInpaintNavierStokes.TabIndex = 18;
            this.BtInpaintNavierStokes.Text = "Inpaint NavierStokes";
            this.BtInpaintNavierStokes.UseVisualStyleBackColor = true;
            this.BtInpaintNavierStokes.Click += new System.EventHandler(this.BtInpaintNavierStokes_Click);
            // 
            // BtLoadMask
            // 
            this.BtLoadMask.Location = new System.Drawing.Point(650, 576);
            this.BtLoadMask.Name = "BtLoadMask";
            this.BtLoadMask.Size = new System.Drawing.Size(108, 30);
            this.BtLoadMask.TabIndex = 19;
            this.BtLoadMask.Text = "Load Mask";
            this.BtLoadMask.UseVisualStyleBackColor = true;
            this.BtLoadMask.Click += new System.EventHandler(this.BtLoadMask_Click);
            // 
            // BtInpaintTelea
            // 
            this.BtInpaintTelea.Location = new System.Drawing.Point(1550, 641);
            this.BtInpaintTelea.Name = "BtInpaintTelea";
            this.BtInpaintTelea.Size = new System.Drawing.Size(123, 31);
            this.BtInpaintTelea.TabIndex = 20;
            this.BtInpaintTelea.Text = "Inpaint Telea";
            this.BtInpaintTelea.UseVisualStyleBackColor = true;
            this.BtInpaintTelea.Click += new System.EventHandler(this.BtInpaintTelea_Click);
            // 
            // BtSquare
            // 
            this.BtSquare.AutoSize = true;
            this.BtSquare.Location = new System.Drawing.Point(495, 587);
            this.BtSquare.Name = "BtSquare";
            this.BtSquare.Size = new System.Drawing.Size(75, 21);
            this.BtSquare.TabIndex = 21;
            this.BtSquare.TabStop = true;
            this.BtSquare.Text = "Square";
            this.BtSquare.UseVisualStyleBackColor = true;
            this.BtSquare.CheckedChanged += new System.EventHandler(this.BtSquare_CheckedChanged);
            // 
            // BtInpaintNans
            // 
            this.BtInpaintNans.Location = new System.Drawing.Point(1550, 571);
            this.BtInpaintNans.Name = "BtInpaintNans";
            this.BtInpaintNans.Size = new System.Drawing.Size(123, 31);
            this.BtInpaintNans.TabIndex = 22;
            this.BtInpaintNans.Text = "Inpaint NaNs";
            this.BtInpaintNans.UseVisualStyleBackColor = true;
            this.BtInpaintNans.Click += new System.EventHandler(this.BtInpaintNans_Click);
            // 
            // BtSaveMask
            // 
            this.BtSaveMask.Location = new System.Drawing.Point(1139, 572);
            this.BtSaveMask.Name = "BtSaveMask";
            this.BtSaveMask.Size = new System.Drawing.Size(108, 30);
            this.BtSaveMask.TabIndex = 23;
            this.BtSaveMask.Text = "Save Mask";
            this.BtSaveMask.UseVisualStyleBackColor = true;
            this.BtSaveMask.Click += new System.EventHandler(this.BtSaveMask_Click);
            // 
            // BtRGB_Mask
            // 
            this.BtRGB_Mask.BackColor = System.Drawing.Color.LightCoral;
            this.BtRGB_Mask.Location = new System.Drawing.Point(650, 612);
            this.BtRGB_Mask.Name = "BtRGB_Mask";
            this.BtRGB_Mask.Size = new System.Drawing.Size(197, 30);
            this.BtRGB_Mask.TabIndex = 24;
            this.BtRGB_Mask.Text = "Load RBG Image As Mask";
            this.BtRGB_Mask.UseVisualStyleBackColor = false;
            this.BtRGB_Mask.Click += new System.EventHandler(this.BtRGB_Mask_Click);
            // 
            // BtSaveRGbMask
            // 
            this.BtSaveRGbMask.BackColor = System.Drawing.Color.LightCoral;
            this.BtSaveRGbMask.Location = new System.Drawing.Point(1124, 612);
            this.BtSaveRGbMask.Name = "BtSaveRGbMask";
            this.BtSaveRGbMask.Size = new System.Drawing.Size(123, 30);
            this.BtSaveRGbMask.TabIndex = 25;
            this.BtSaveRGbMask.Text = "Save RBG Mask";
            this.BtSaveRGbMask.UseVisualStyleBackColor = false;
            this.BtSaveRGbMask.Click += new System.EventHandler(this.BtSaveRGbMask_Click);
            // 
            // BtInpaintFSRBest
            // 
            this.BtInpaintFSRBest.Location = new System.Drawing.Point(1691, 571);
            this.BtInpaintFSRBest.Name = "BtInpaintFSRBest";
            this.BtInpaintFSRBest.Size = new System.Drawing.Size(139, 31);
            this.BtInpaintFSRBest.TabIndex = 26;
            this.BtInpaintFSRBest.Text = "Inpaint FSR";
            this.BtInpaintFSRBest.UseVisualStyleBackColor = true;
            this.BtInpaintFSRBest.Click += new System.EventHandler(this.BtInpaintFSR_Click);
            // 
            // TxtBoxCalcPSNR
            // 
            this.TxtBoxCalcPSNR.Location = new System.Drawing.Point(830, 696);
            this.TxtBoxCalcPSNR.Name = "TxtBoxCalcPSNR";
            this.TxtBoxCalcPSNR.Size = new System.Drawing.Size(90, 22);
            this.TxtBoxCalcPSNR.TabIndex = 27;
            // 
            // TxtBoxCalcGMSD
            // 
            this.TxtBoxCalcGMSD.Location = new System.Drawing.Point(1157, 696);
            this.TxtBoxCalcGMSD.Name = "TxtBoxCalcGMSD";
            this.TxtBoxCalcGMSD.Size = new System.Drawing.Size(90, 22);
            this.TxtBoxCalcGMSD.TabIndex = 31;
            // 
            // BtCalcGMSD
            // 
            this.BtCalcGMSD.Location = new System.Drawing.Point(973, 692);
            this.BtCalcGMSD.Name = "BtCalcGMSD";
            this.BtCalcGMSD.Size = new System.Drawing.Size(163, 30);
            this.BtCalcGMSD.TabIndex = 30;
            this.BtCalcGMSD.Text = "Calculate GMSD";
            this.BtCalcGMSD.UseVisualStyleBackColor = true;
            this.BtCalcGMSD.Click += new System.EventHandler(this.BtCalcGMSD_Click);
            // 
            // TxtBoxCalcSSIM
            // 
            this.TxtBoxCalcSSIM.Location = new System.Drawing.Point(1157, 660);
            this.TxtBoxCalcSSIM.Name = "TxtBoxCalcSSIM";
            this.TxtBoxCalcSSIM.Size = new System.Drawing.Size(90, 22);
            this.TxtBoxCalcSSIM.TabIndex = 29;
            // 
            // BtCalcSSIM
            // 
            this.BtCalcSSIM.Location = new System.Drawing.Point(973, 656);
            this.BtCalcSSIM.Name = "BtCalcSSIM";
            this.BtCalcSSIM.Size = new System.Drawing.Size(163, 30);
            this.BtCalcSSIM.TabIndex = 28;
            this.BtCalcSSIM.Text = "Calculate SSIM";
            this.BtCalcSSIM.UseVisualStyleBackColor = true;
            this.BtCalcSSIM.Click += new System.EventHandler(this.BtCalcSSIM_Click);
            // 
            // BtInpaintFSRFast
            // 
            this.BtInpaintFSRFast.Location = new System.Drawing.Point(1691, 641);
            this.BtInpaintFSRFast.Name = "BtInpaintFSRFast";
            this.BtInpaintFSRFast.Size = new System.Drawing.Size(139, 31);
            this.BtInpaintFSRFast.TabIndex = 32;
            this.BtInpaintFSRFast.Text = "Inpaint FSR Fast";
            this.BtInpaintFSRFast.UseVisualStyleBackColor = true;
            this.BtInpaintFSRFast.Click += new System.EventHandler(this.BtInpaintFSRFast_Click);
            // 
            // BtInpaintShiftMap
            // 
            this.BtInpaintShiftMap.Location = new System.Drawing.Point(1383, 641);
            this.BtInpaintShiftMap.Name = "BtInpaintShiftMap";
            this.BtInpaintShiftMap.Size = new System.Drawing.Size(152, 31);
            this.BtInpaintShiftMap.TabIndex = 33;
            this.BtInpaintShiftMap.Text = "Inpaint ShiftMap";
            this.BtInpaintShiftMap.UseVisualStyleBackColor = true;
            this.BtInpaintShiftMap.Click += new System.EventHandler(this.BtInpaintShiftMap_Click);
            // 
            // BtAutomate
            // 
            this.BtAutomate.BackColor = System.Drawing.Color.LightGreen;
            this.BtAutomate.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.BtAutomate.Location = new System.Drawing.Point(12, 676);
            this.BtAutomate.Name = "BtAutomate";
            this.BtAutomate.Size = new System.Drawing.Size(108, 64);
            this.BtAutomate.TabIndex = 36;
            this.BtAutomate.Text = "Automate results";
            this.BtAutomate.UseVisualStyleBackColor = false;
            this.BtAutomate.Click += new System.EventHandler(this.BtAutomate_Click);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.CausesValidation = false;
            this.ClientSize = new System.Drawing.Size(1888, 753);
            this.Controls.Add(this.BtAutomate);
            this.Controls.Add(this.BtInpaintShiftMap);
            this.Controls.Add(this.BtInpaintFSRFast);
            this.Controls.Add(this.TxtBoxCalcGMSD);
            this.Controls.Add(this.BtCalcGMSD);
            this.Controls.Add(this.TxtBoxCalcSSIM);
            this.Controls.Add(this.BtCalcSSIM);
            this.Controls.Add(this.TxtBoxCalcPSNR);
            this.Controls.Add(this.BtInpaintFSRBest);
            this.Controls.Add(this.BtSaveRGbMask);
            this.Controls.Add(this.BtRGB_Mask);
            this.Controls.Add(this.BtSaveMask);
            this.Controls.Add(this.BtInpaintNans);
            this.Controls.Add(this.BtSquare);
            this.Controls.Add(this.BtInpaintTelea);
            this.Controls.Add(this.BtLoadMask);
            this.Controls.Add(this.BtInpaintNavierStokes);
            this.Controls.Add(this.PicBox3InPainted);
            this.Controls.Add(this.BtCalcPSNRerror);
            this.Controls.Add(this.TxtBoxRMSerror);
            this.Controls.Add(this.BtCalcRMSerror);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtSaltNPepperNoise);
            this.Controls.Add(this.BtScratches);
            this.Controls.Add(this.LblRangeNoise);
            this.Controls.Add(this.TrBarRangeNoise);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtSaveImg);
            this.Controls.Add(this.BtLoadImg);
            this.Controls.Add(this.PicBox1OrgImg);
            this.Controls.Add(this.PicBox2Editedmg);
            this.Controls.Add(this.TrBarNoiseController);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1906, 800);
            this.MinimumSize = new System.Drawing.Size(1900, 750);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Inpainting Comparer";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            ((System.ComponentModel.ISupportInitialize)(this.TrBarNoiseController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox2Editedmg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox1OrgImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrBarRangeNoise)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox3InPainted)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar TrBarNoiseController;
        private System.Windows.Forms.PictureBox PicBox2Editedmg;
        private System.Windows.Forms.PictureBox PicBox1OrgImg;
        private System.Windows.Forms.Button BtLoadImg;
        private System.Windows.Forms.Button BtSaveImg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LblRangeNoise;
        private System.Windows.Forms.TrackBar TrBarRangeNoise;
        private System.Windows.Forms.RadioButton BtScratches;
        private System.Windows.Forms.RadioButton BtSaltNPepperNoise;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtCalcRMSerror;
        private System.Windows.Forms.TextBox TxtBoxRMSerror;
        private System.Windows.Forms.Button BtCalcPSNRerror;
        private System.Windows.Forms.PictureBox PicBox3InPainted;
        private System.Windows.Forms.Button BtInpaintNavierStokes;
        private System.Windows.Forms.Button BtLoadMask;
        private System.Windows.Forms.Button BtInpaintTelea;
        private System.Windows.Forms.RadioButton BtSquare;
        private System.Windows.Forms.Button BtInpaintNans;
        private System.Windows.Forms.Button BtSaveMask;
        private System.Windows.Forms.Button BtRGB_Mask;
        private System.Windows.Forms.Button BtSaveRGbMask;
        private System.Windows.Forms.Button BtInpaintFSRBest;
        private System.Windows.Forms.TextBox TxtBoxCalcPSNR;
        private System.Windows.Forms.TextBox TxtBoxCalcGMSD;
        private System.Windows.Forms.Button BtCalcGMSD;
        private System.Windows.Forms.TextBox TxtBoxCalcSSIM;
        private System.Windows.Forms.Button BtCalcSSIM;
        private System.Windows.Forms.Button BtInpaintFSRFast;
        private System.Windows.Forms.Button BtInpaintShiftMap;
        private System.Windows.Forms.Button BtAutomate;
    }
}

