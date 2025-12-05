namespace WinformPlay
{
    partial class frmMain
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
            this.components = new System.ComponentModel.Container();
            this.mPanelYUV = new System.Windows.Forms.Panel();
            this.mButtonPlay = new System.Windows.Forms.Button();
            this.mButtoonPause = new System.Windows.Forms.Button();
            this.mButtonStop = new System.Windows.Forms.Button();
            this.openMediaFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.mButtonCLI = new System.Windows.Forms.Button();
            this.mButtonTestScreen = new System.Windows.Forms.Button();
            this.mScrollBar = new System.Windows.Forms.HScrollBar();
            this.mTimer = new System.Windows.Forms.Timer(this.components);
            this.mLabelPos = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // mPanelYUV
            // 
            this.mPanelYUV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mPanelYUV.BackColor = System.Drawing.Color.ForestGreen;
            this.mPanelYUV.Location = new System.Drawing.Point(12, 12);
            this.mPanelYUV.Name = "mPanelYUV";
            this.mPanelYUV.Size = new System.Drawing.Size(1049, 409);
            this.mPanelYUV.TabIndex = 0;
            // 
            // mButtonPlay
            // 
            this.mButtonPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonPlay.Location = new System.Drawing.Point(824, 433);
            this.mButtonPlay.Name = "mButtonPlay";
            this.mButtonPlay.Size = new System.Drawing.Size(75, 23);
            this.mButtonPlay.TabIndex = 1;
            this.mButtonPlay.Text = "PLAY";
            this.mButtonPlay.UseVisualStyleBackColor = true;
            this.mButtonPlay.Click += new System.EventHandler(this.mButtonPlay_Click);
            // 
            // mButtoonPause
            // 
            this.mButtoonPause.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtoonPause.Location = new System.Drawing.Point(905, 433);
            this.mButtoonPause.Name = "mButtoonPause";
            this.mButtoonPause.Size = new System.Drawing.Size(75, 23);
            this.mButtoonPause.TabIndex = 2;
            this.mButtoonPause.Text = "PAUSE";
            this.mButtoonPause.UseVisualStyleBackColor = true;
            this.mButtoonPause.Click += new System.EventHandler(this.mButtoonPause_Click);
            // 
            // mButtonStop
            // 
            this.mButtonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.mButtonStop.Location = new System.Drawing.Point(986, 433);
            this.mButtonStop.Name = "mButtonStop";
            this.mButtonStop.Size = new System.Drawing.Size(75, 23);
            this.mButtonStop.TabIndex = 3;
            this.mButtonStop.Text = "STOP";
            this.mButtonStop.UseVisualStyleBackColor = true;
            this.mButtonStop.Click += new System.EventHandler(this.mButtonStop_Click);
            // 
            // mButtonCLI
            // 
            this.mButtonCLI.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mButtonCLI.Location = new System.Drawing.Point(128, 433);
            this.mButtonCLI.Name = "mButtonCLI";
            this.mButtonCLI.Size = new System.Drawing.Size(75, 23);
            this.mButtonCLI.TabIndex = 4;
            this.mButtonCLI.Text = "Run CLI";
            this.mButtonCLI.UseVisualStyleBackColor = true;
            this.mButtonCLI.Click += new System.EventHandler(this.mButtonCLI_Click);
            // 
            // mButtonTestScreen
            // 
            this.mButtonTestScreen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mButtonTestScreen.Location = new System.Drawing.Point(12, 433);
            this.mButtonTestScreen.Name = "mButtonTestScreen";
            this.mButtonTestScreen.Size = new System.Drawing.Size(110, 23);
            this.mButtonTestScreen.TabIndex = 5;
            this.mButtonTestScreen.Text = "TEST Screen";
            this.mButtonTestScreen.UseVisualStyleBackColor = true;
            this.mButtonTestScreen.Click += new System.EventHandler(this.mButtonTestScreen_Click);
            // 
            // mScrollBar
            // 
            this.mScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mScrollBar.Location = new System.Drawing.Point(229, 435);
            this.mScrollBar.Maximum = 1000;
            this.mScrollBar.Name = "mScrollBar";
            this.mScrollBar.Size = new System.Drawing.Size(565, 10);
            this.mScrollBar.TabIndex = 6;
            this.mScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.mScrollBar_Scroll);
            // 
            // mTimer
            // 
            this.mTimer.Interval = 1000;
            this.mTimer.Tick += new System.EventHandler(this.mTimer_Tick);
            // 
            // mLabelPos
            // 
            this.mLabelPos.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.mLabelPos.AutoSize = true;
            this.mLabelPos.Location = new System.Drawing.Point(226, 446);
            this.mLabelPos.Name = "mLabelPos";
            this.mLabelPos.Size = new System.Drawing.Size(0, 13);
            this.mLabelPos.TabIndex = 7;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1073, 468);
            this.Controls.Add(this.mLabelPos);
            this.Controls.Add(this.mScrollBar);
            this.Controls.Add(this.mButtonTestScreen);
            this.Controls.Add(this.mButtonCLI);
            this.Controls.Add(this.mButtonStop);
            this.Controls.Add(this.mButtoonPause);
            this.Controls.Add(this.mButtonPlay);
            this.Controls.Add(this.mPanelYUV);
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Winform FFPlay";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMain_FormClosed);
            this.ResizeEnd += new System.EventHandler(this.frmMain_ResizeEnd);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel mPanelYUV;
        private System.Windows.Forms.Button mButtonPlay;
        private System.Windows.Forms.Button mButtoonPause;
        private System.Windows.Forms.Button mButtonStop;
        private System.Windows.Forms.OpenFileDialog openMediaFileDialog;
        private System.Windows.Forms.Button mButtonCLI;
        private System.Windows.Forms.Button mButtonTestScreen;
        private System.Windows.Forms.HScrollBar mScrollBar;
        private System.Windows.Forms.Timer mTimer;
        private System.Windows.Forms.Label mLabelPos;
    }
}

