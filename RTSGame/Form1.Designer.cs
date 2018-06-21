namespace RTSGame
{
    partial class GameScreen
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
            this.upMidDivideLabel = new System.Windows.Forms.Label();
            this.midBottomDivideLabel = new System.Windows.Forms.Label();
            this.GameTimer = new System.Windows.Forms.Timer(this.components);
            this.p1ResourceLabel = new System.Windows.Forms.Label();
            this.gameFieldLabel = new System.Windows.Forms.Label();
            this.p2ResourceLabel = new System.Windows.Forms.Label();
            this.p2HealthLabel = new System.Windows.Forms.Label();
            this.p1HealthLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // upMidDivideLabel
            // 
            this.upMidDivideLabel.Location = new System.Drawing.Point(22, 110);
            this.upMidDivideLabel.Name = "upMidDivideLabel";
            this.upMidDivideLabel.Size = new System.Drawing.Size(563, 55);
            this.upMidDivideLabel.TabIndex = 1;
            // 
            // midBottomDivideLabel
            // 
            this.midBottomDivideLabel.Location = new System.Drawing.Point(25, 243);
            this.midBottomDivideLabel.Name = "midBottomDivideLabel";
            this.midBottomDivideLabel.Size = new System.Drawing.Size(560, 54);
            this.midBottomDivideLabel.TabIndex = 2;
            // 
            // GameTimer
            // 
            this.GameTimer.Enabled = true;
            this.GameTimer.Interval = 20;
            this.GameTimer.Tick += new System.EventHandler(this.GameTimer_Tick);
            // 
            // p1ResourceLabel
            // 
            this.p1ResourceLabel.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.p1ResourceLabel.Font = new System.Drawing.Font("OCR A Std", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p1ResourceLabel.ForeColor = System.Drawing.Color.Maroon;
            this.p1ResourceLabel.Location = new System.Drawing.Point(12, 9);
            this.p1ResourceLabel.Name = "p1ResourceLabel";
            this.p1ResourceLabel.Size = new System.Drawing.Size(170, 23);
            this.p1ResourceLabel.TabIndex = 3;
            this.p1ResourceLabel.Text = "Resources: ";
            // 
            // gameFieldLabel
            // 
            this.gameFieldLabel.BackColor = System.Drawing.Color.DarkGray;
            this.gameFieldLabel.Location = new System.Drawing.Point(25, 32);
            this.gameFieldLabel.Name = "gameFieldLabel";
            this.gameFieldLabel.Size = new System.Drawing.Size(541, 336);
            this.gameFieldLabel.TabIndex = 0;
            this.gameFieldLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.gameField_Paint);
            // 
            // p2ResourceLabel
            // 
            this.p2ResourceLabel.Font = new System.Drawing.Font("OCR A Std", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p2ResourceLabel.ForeColor = System.Drawing.Color.Navy;
            this.p2ResourceLabel.Location = new System.Drawing.Point(437, 9);
            this.p2ResourceLabel.Name = "p2ResourceLabel";
            this.p2ResourceLabel.Size = new System.Drawing.Size(165, 23);
            this.p2ResourceLabel.TabIndex = 4;
            this.p2ResourceLabel.Text = "Resources: ";
            // 
            // p2HealthLabel
            // 
            this.p2HealthLabel.BackColor = System.Drawing.Color.DimGray;
            this.p2HealthLabel.Font = new System.Drawing.Font("OCR A Std", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p2HealthLabel.ForeColor = System.Drawing.Color.Navy;
            this.p2HealthLabel.Location = new System.Drawing.Point(473, 383);
            this.p2HealthLabel.Name = "p2HealthLabel";
            this.p2HealthLabel.Size = new System.Drawing.Size(129, 23);
            this.p2HealthLabel.TabIndex = 5;
            this.p2HealthLabel.Text = "Health: ";
            // 
            // p1HealthLabel
            // 
            this.p1HealthLabel.Font = new System.Drawing.Font("OCR A Std", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.p1HealthLabel.ForeColor = System.Drawing.Color.Maroon;
            this.p1HealthLabel.Location = new System.Drawing.Point(12, 383);
            this.p1HealthLabel.Name = "p1HealthLabel";
            this.p1HealthLabel.Size = new System.Drawing.Size(144, 23);
            this.p1HealthLabel.TabIndex = 6;
            this.p1HealthLabel.Text = "Health: ";
            // 
            // GameScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(614, 405);
            this.Controls.Add(this.p1HealthLabel);
            this.Controls.Add(this.p2HealthLabel);
            this.Controls.Add(this.p2ResourceLabel);
            this.Controls.Add(this.p1ResourceLabel);
            this.Controls.Add(this.midBottomDivideLabel);
            this.Controls.Add(this.upMidDivideLabel);
            this.Controls.Add(this.gameFieldLabel);
            this.Name = "GameScreen";
            this.Text = "RTS";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label upMidDivideLabel;
        private System.Windows.Forms.Label midBottomDivideLabel;
        private System.Windows.Forms.Timer GameTimer;
        private System.Windows.Forms.Label p1ResourceLabel;
        private System.Windows.Forms.Label gameFieldLabel;
        private System.Windows.Forms.Label p2ResourceLabel;
        private System.Windows.Forms.Label p2HealthLabel;
        private System.Windows.Forms.Label p1HealthLabel;
    }
}

