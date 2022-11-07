
namespace GameOfLife
{
    partial class GameOfLife
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.gameArea = new System.Windows.Forms.PictureBox();
            this.numCSize = new System.Windows.Forms.NumericUpDown();
            this.cellSize = new System.Windows.Forms.Label();
            this.resetBtn = new System.Windows.Forms.Button();
            this.startBtn = new System.Windows.Forms.Button();
            this.advanceBtn = new System.Windows.Forms.Button();
            this.clearBtn = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gameArea)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCSize)).BeginInit();
            this.SuspendLayout();
            // 
            // gameArea
            // 
            this.gameArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gameArea.BackColor = System.Drawing.Color.Black;
            this.gameArea.Location = new System.Drawing.Point(12, 12);
            this.gameArea.Name = "gameArea";
            this.gameArea.Size = new System.Drawing.Size(920, 528);
            this.gameArea.TabIndex = 0;
            this.gameArea.TabStop = false;
            this.gameArea.MouseClick += new System.Windows.Forms.MouseEventHandler(this.gameArea_MouseClick);
            // 
            // numCSize
            // 
            this.numCSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.numCSize.BackColor = System.Drawing.Color.LightSlateGray;
            this.numCSize.ForeColor = System.Drawing.SystemColors.Window;
            this.numCSize.Increment = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numCSize.Location = new System.Drawing.Point(71, 546);
            this.numCSize.Maximum = new decimal(new int[] {
            25,
            0,
            0,
            0});
            this.numCSize.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numCSize.Name = "numCSize";
            this.numCSize.ReadOnly = true;
            this.numCSize.Size = new System.Drawing.Size(65, 23);
            this.numCSize.TabIndex = 1;
            this.numCSize.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // cellSize
            // 
            this.cellSize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cellSize.AutoSize = true;
            this.cellSize.BackColor = System.Drawing.Color.White;
            this.cellSize.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cellSize.Location = new System.Drawing.Point(12, 548);
            this.cellSize.Name = "cellSize";
            this.cellSize.Size = new System.Drawing.Size(53, 15);
            this.cellSize.TabIndex = 2;
            this.cellSize.Text = "Cell Size:";
            // 
            // resetBtn
            // 
            this.resetBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.resetBtn.BackColor = System.Drawing.Color.LightSlateGray;
            this.resetBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.resetBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.resetBtn.Location = new System.Drawing.Point(154, 546);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(70, 23);
            this.resetBtn.TabIndex = 3;
            this.resetBtn.Text = "Random";
            this.resetBtn.UseVisualStyleBackColor = false;
            this.resetBtn.Click += new System.EventHandler(this.ResetBtn_Click);
            // 
            // startBtn
            // 
            this.startBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startBtn.BackColor = System.Drawing.Color.LightSlateGray;
            this.startBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.startBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.startBtn.Location = new System.Drawing.Point(862, 548);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(70, 23);
            this.startBtn.TabIndex = 4;
            this.startBtn.Text = "Start";
            this.startBtn.UseVisualStyleBackColor = false;
            this.startBtn.Click += new System.EventHandler(this.StartBtn_Click);
            // 
            // advanceBtn
            // 
            this.advanceBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.advanceBtn.BackColor = System.Drawing.Color.LightSlateGray;
            this.advanceBtn.Cursor = System.Windows.Forms.Cursors.Hand;
            this.advanceBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.advanceBtn.Location = new System.Drawing.Point(786, 548);
            this.advanceBtn.Name = "advanceBtn";
            this.advanceBtn.Size = new System.Drawing.Size(70, 23);
            this.advanceBtn.TabIndex = 5;
            this.advanceBtn.Text = "Advance";
            this.advanceBtn.UseVisualStyleBackColor = false;
            this.advanceBtn.Click += new System.EventHandler(this.AdvanceBtn_Click);
            // 
            // clearBtn
            // 
            this.clearBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.clearBtn.BackColor = System.Drawing.Color.Gray;
            this.clearBtn.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.clearBtn.Location = new System.Drawing.Point(230, 546);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(75, 23);
            this.clearBtn.TabIndex = 6;
            this.clearBtn.Text = "Clear";
            this.clearBtn.UseVisualStyleBackColor = false;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // GameOfLife
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(944, 581);
            this.Controls.Add(this.clearBtn);
            this.Controls.Add(this.advanceBtn);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.resetBtn);
            this.Controls.Add(this.cellSize);
            this.Controls.Add(this.numCSize);
            this.Controls.Add(this.gameArea);
            this.MinimumSize = new System.Drawing.Size(960, 620);
            this.Name = "GameOfLife";
            this.Text = "Game of Life";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GameOfLife_FormClosing);
            this.Load += new System.EventHandler(this.Load_GameOfLife);
            ((System.ComponentModel.ISupportInitialize)(this.gameArea)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox gameArea;
        private System.Windows.Forms.NumericUpDown numCSize;
        private System.Windows.Forms.Label cellSize;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.Button advanceBtn;
        private System.Windows.Forms.Button clearBtn;
    }
}

