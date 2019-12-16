namespace ImageCrusher
{
    partial class Form1
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
            this.TrBarNoiseController = new System.Windows.Forms.TrackBar();
            this.PicBox2Editedmg = new System.Windows.Forms.PictureBox();
            this.PicBox1OrgImg = new System.Windows.Forms.PictureBox();
            this.BtLoadImg = new System.Windows.Forms.Button();
            this.BtSaveImg = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LblRangeNoise = new System.Windows.Forms.Label();
            this.TrBarRangeNoise = new System.Windows.Forms.TrackBar();
            this.BtScratches = new System.Windows.Forms.RadioButton();
            this.BtStarNoise = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.BtCalcRMSError = new System.Windows.Forms.Button();
            this.TxtBoxRMSError = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.TrBarNoiseController)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox2Editedmg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox1OrgImg)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrBarRangeNoise)).BeginInit();
            this.SuspendLayout();
            // 
            // TrBarNoiseController
            // 
            this.TrBarNoiseController.LargeChange = 1;
            this.TrBarNoiseController.Location = new System.Drawing.Point(291, 669);
            this.TrBarNoiseController.Maximum = 11;
            this.TrBarNoiseController.Name = "TrBarNoiseController";
            this.TrBarNoiseController.Size = new System.Drawing.Size(146, 56);
            this.TrBarNoiseController.TabIndex = 0;
            this.TrBarNoiseController.ValueChanged += new System.EventHandler(this.TrBarNoiseController_ValueChanged);
            // 
            // PicBox2Editedmg
            // 
            this.PicBox2Editedmg.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PicBox2Editedmg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicBox2Editedmg.Location = new System.Drawing.Point(691, 12);
            this.PicBox2Editedmg.Name = "PicBox2Editedmg";
            this.PicBox2Editedmg.Size = new System.Drawing.Size(641, 532);
            this.PicBox2Editedmg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBox2Editedmg.TabIndex = 1;
            this.PicBox2Editedmg.TabStop = false;
            // 
            // PicBox1OrgImg
            // 
            this.PicBox1OrgImg.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.PicBox1OrgImg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.PicBox1OrgImg.Location = new System.Drawing.Point(12, 12);
            this.PicBox1OrgImg.Name = "PicBox1OrgImg";
            this.PicBox1OrgImg.Size = new System.Drawing.Size(656, 532);
            this.PicBox1OrgImg.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBox1OrgImg.TabIndex = 2;
            this.PicBox1OrgImg.TabStop = false;
            // 
            // BtLoadImg
            // 
            this.BtLoadImg.Location = new System.Drawing.Point(65, 566);
            this.BtLoadImg.Name = "BtLoadImg";
            this.BtLoadImg.Size = new System.Drawing.Size(108, 30);
            this.BtLoadImg.TabIndex = 3;
            this.BtLoadImg.Text = "Load Image";
            this.BtLoadImg.UseVisualStyleBackColor = true;
            this.BtLoadImg.Click += new System.EventHandler(this.BtLoadImg_Click);
            // 
            // BtSaveImg
            // 
            this.BtSaveImg.Location = new System.Drawing.Point(745, 565);
            this.BtSaveImg.Name = "BtSaveImg";
            this.BtSaveImg.Size = new System.Drawing.Size(135, 31);
            this.BtSaveImg.TabIndex = 4;
            this.BtSaveImg.Text = "Save Image";
            this.BtSaveImg.UseVisualStyleBackColor = true;
            this.BtSaveImg.Click += new System.EventHandler(this.BtSaveImg_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(270, 649);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(206, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Make some noise and no undo ";
            // 
            // LblRangeNoise
            // 
            this.LblRangeNoise.AutoSize = true;
            this.LblRangeNoise.Location = new System.Drawing.Point(507, 649);
            this.LblRangeNoise.Name = "LblRangeNoise";
            this.LblRangeNoise.Size = new System.Drawing.Size(199, 17);
            this.LblRangeNoise.TabIndex = 7;
            this.LblRangeNoise.Text = "How big should be noise signs";
            // 
            // TrBarRangeNoise
            // 
            this.TrBarRangeNoise.LargeChange = 1;
            this.TrBarRangeNoise.Location = new System.Drawing.Point(529, 669);
            this.TrBarRangeNoise.Maximum = 100;
            this.TrBarRangeNoise.Name = "TrBarRangeNoise";
            this.TrBarRangeNoise.Size = new System.Drawing.Size(146, 56);
            this.TrBarRangeNoise.TabIndex = 6;
            // 
            // BtScratches
            // 
            this.BtScratches.AutoSize = true;
            this.BtScratches.Location = new System.Drawing.Point(529, 566);
            this.BtScratches.Name = "BtScratches";
            this.BtScratches.Size = new System.Drawing.Size(135, 21);
            this.BtScratches.TabIndex = 8;
            this.BtScratches.TabStop = true;
            this.BtScratches.Text = "Diagonal scratch";
            this.BtScratches.UseVisualStyleBackColor = true;
            this.BtScratches.CheckedChanged += new System.EventHandler(this.BtScratches_CheckedChanged);
            // 
            // BtStarNoise
            // 
            this.BtStarNoise.AutoSize = true;
            this.BtStarNoise.Location = new System.Drawing.Point(529, 593);
            this.BtStarNoise.Name = "BtStarNoise";
            this.BtStarNoise.Size = new System.Drawing.Size(118, 21);
            this.BtStarNoise.TabIndex = 9;
            this.BtStarNoise.TabStop = true;
            this.BtStarNoise.Text = "Star like noise";
            this.BtStarNoise.UseVisualStyleBackColor = true;
            this.BtStarNoise.CheckedChanged += new System.EventHandler(this.BtStarNoise_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.8F);
            this.label2.Location = new System.Drawing.Point(286, 572);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 25);
            this.label2.TabIndex = 10;
            this.label2.Text = "Choose noise type";
            // 
            // BtCalcRMSError
            // 
            this.BtCalcRMSError.Location = new System.Drawing.Point(745, 642);
            this.BtCalcRMSError.Name = "BtCalcRMSError";
            this.BtCalcRMSError.Size = new System.Drawing.Size(163, 31);
            this.BtCalcRMSError.TabIndex = 11;
            this.BtCalcRMSError.Text = "Calculate RMS Error";
            this.BtCalcRMSError.UseVisualStyleBackColor = true;
            this.BtCalcRMSError.Click += new System.EventHandler(this.BtCalcRMSError_Click);
            // 
            // TxtBoxRMSError
            // 
            this.TxtBoxRMSError.Location = new System.Drawing.Point(951, 646);
            this.TxtBoxRMSError.Name = "TxtBoxRMSError";
            this.TxtBoxRMSError.Size = new System.Drawing.Size(107, 22);
            this.TxtBoxRMSError.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.ClientSize = new System.Drawing.Size(1344, 740);
            this.Controls.Add(this.TxtBoxRMSError);
            this.Controls.Add(this.BtCalcRMSError);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BtStarNoise);
            this.Controls.Add(this.BtScratches);
            this.Controls.Add(this.LblRangeNoise);
            this.Controls.Add(this.TrBarRangeNoise);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtSaveImg);
            this.Controls.Add(this.BtLoadImg);
            this.Controls.Add(this.PicBox1OrgImg);
            this.Controls.Add(this.PicBox2Editedmg);
            this.Controls.Add(this.TrBarNoiseController);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.TrBarNoiseController)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox2Editedmg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBox1OrgImg)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrBarRangeNoise)).EndInit();
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
        private System.Windows.Forms.RadioButton BtStarNoise;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtCalcRMSError;
        private System.Windows.Forms.TextBox TxtBoxRMSError;
    }
}

