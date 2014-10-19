namespace AoCHacker
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
                UnhookWindowsHookEx(_hookID);
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
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.SpeedTextBox = new System.Windows.Forms.TextBox();
            this.GetSpeedButton = new System.Windows.Forms.Button();
            this.MaxRunButton = new System.Windows.Forms.Button();
            this.NormalRunButton = new System.Windows.Forms.Button();
            this.maxStealthButton = new System.Windows.Forms.Button();
            this.zAxisDown = new System.Windows.Forms.Button();
            this.zAxisPlusButton = new System.Windows.Forms.Button();
            this.noFallDMGCheck = new System.Windows.Forms.CheckBox();
            this.GetPixelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.AutoSize = true;
            this.SpeedLabel.Location = new System.Drawing.Point(12, 14);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(38, 13);
            this.SpeedLabel.TabIndex = 0;
            this.SpeedLabel.Text = "Speed";
            // 
            // SpeedTextBox
            // 
            this.SpeedTextBox.Location = new System.Drawing.Point(56, 7);
            this.SpeedTextBox.Name = "SpeedTextBox";
            this.SpeedTextBox.Size = new System.Drawing.Size(101, 20);
            this.SpeedTextBox.TabIndex = 1;
            // 
            // GetSpeedButton
            // 
            this.GetSpeedButton.Location = new System.Drawing.Point(163, 4);
            this.GetSpeedButton.Name = "GetSpeedButton";
            this.GetSpeedButton.Size = new System.Drawing.Size(75, 23);
            this.GetSpeedButton.TabIndex = 2;
            this.GetSpeedButton.Text = "Get Speed";
            this.GetSpeedButton.UseVisualStyleBackColor = true;
            this.GetSpeedButton.Click += new System.EventHandler(this.GetSpeedButton_Click);
            // 
            // MaxRunButton
            // 
            this.MaxRunButton.Location = new System.Drawing.Point(4, 44);
            this.MaxRunButton.Name = "MaxRunButton";
            this.MaxRunButton.Size = new System.Drawing.Size(75, 23);
            this.MaxRunButton.TabIndex = 3;
            this.MaxRunButton.Text = "Max Run";
            this.MaxRunButton.UseVisualStyleBackColor = true;
            this.MaxRunButton.Click += new System.EventHandler(this.MaxRunButton_Click);
            // 
            // NormalRunButton
            // 
            this.NormalRunButton.Location = new System.Drawing.Point(166, 44);
            this.NormalRunButton.Name = "NormalRunButton";
            this.NormalRunButton.Size = new System.Drawing.Size(75, 23);
            this.NormalRunButton.TabIndex = 4;
            this.NormalRunButton.Text = "Normal Run";
            this.NormalRunButton.UseVisualStyleBackColor = true;
            this.NormalRunButton.Click += new System.EventHandler(this.NormalRunButton_Click);
            // 
            // maxStealthButton
            // 
            this.maxStealthButton.Location = new System.Drawing.Point(85, 44);
            this.maxStealthButton.Name = "maxStealthButton";
            this.maxStealthButton.Size = new System.Drawing.Size(75, 23);
            this.maxStealthButton.TabIndex = 5;
            this.maxStealthButton.Text = "Max Stealth";
            this.maxStealthButton.UseVisualStyleBackColor = true;
            this.maxStealthButton.Click += new System.EventHandler(this.maxStealthButton_Click);
            // 
            // zAxisDown
            // 
            this.zAxisDown.Location = new System.Drawing.Point(41, 84);
            this.zAxisDown.Name = "zAxisDown";
            this.zAxisDown.Size = new System.Drawing.Size(30, 23);
            this.zAxisDown.TabIndex = 7;
            this.zAxisDown.Text = "Z -";
            this.zAxisDown.UseVisualStyleBackColor = true;
            this.zAxisDown.Click += new System.EventHandler(this.zAxisDown_Click);
            // 
            // zAxisPlusButton
            // 
            this.zAxisPlusButton.Location = new System.Drawing.Point(4, 84);
            this.zAxisPlusButton.Name = "zAxisPlusButton";
            this.zAxisPlusButton.Size = new System.Drawing.Size(31, 23);
            this.zAxisPlusButton.TabIndex = 10;
            this.zAxisPlusButton.Text = "Z +";
            this.zAxisPlusButton.UseVisualStyleBackColor = true;
            this.zAxisPlusButton.Click += new System.EventHandler(this.zAxisPlusButton_Click);
            // 
            // noFallDMGCheck
            // 
            this.noFallDMGCheck.AutoSize = true;
            this.noFallDMGCheck.Location = new System.Drawing.Point(85, 90);
            this.noFallDMGCheck.Name = "noFallDMGCheck";
            this.noFallDMGCheck.Size = new System.Drawing.Size(87, 17);
            this.noFallDMGCheck.TabIndex = 11;
            this.noFallDMGCheck.Text = "No Fall DMG";
            this.noFallDMGCheck.UseVisualStyleBackColor = true;
            this.noFallDMGCheck.CheckedChanged += new System.EventHandler(this.noFallDMGCheck_CheckedChanged);
            // 
            // GetPixelButton
            // 
            this.GetPixelButton.Location = new System.Drawing.Point(166, 84);
            this.GetPixelButton.Name = "GetPixelButton";
            this.GetPixelButton.Size = new System.Drawing.Size(75, 23);
            this.GetPixelButton.TabIndex = 12;
            this.GetPixelButton.Text = "Get Pixel";
            this.GetPixelButton.UseVisualStyleBackColor = true;
            this.GetPixelButton.Click += new System.EventHandler(this.GetPixelButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(252, 115);
            this.Controls.Add(this.GetPixelButton);
            this.Controls.Add(this.noFallDMGCheck);
            this.Controls.Add(this.zAxisPlusButton);
            this.Controls.Add(this.zAxisDown);
            this.Controls.Add(this.maxStealthButton);
            this.Controls.Add(this.NormalRunButton);
            this.Controls.Add(this.MaxRunButton);
            this.Controls.Add(this.GetSpeedButton);
            this.Controls.Add(this.SpeedTextBox);
            this.Controls.Add(this.SpeedLabel);
            this.Name = "Form1";
            this.Text = "AoCHacker";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label SpeedLabel;
        private System.Windows.Forms.TextBox SpeedTextBox;
        private System.Windows.Forms.Button GetSpeedButton;
        private System.Windows.Forms.Button MaxRunButton;
        private System.Windows.Forms.Button NormalRunButton;
        private System.Windows.Forms.Button maxStealthButton;
        private System.Windows.Forms.Button zAxisDown;
        private System.Windows.Forms.Button zAxisPlusButton;
        private System.Windows.Forms.Button GetPixelButton;
        private System.Windows.Forms.CheckBox noFallDMGCheck;
    }
}

