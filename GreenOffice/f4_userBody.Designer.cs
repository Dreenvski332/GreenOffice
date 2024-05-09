﻿namespace GreenOffice
{
    partial class f4_userBody
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f4_userBody));
            this.headerGroupBox = new System.Windows.Forms.GroupBox();
            this.timerStartButton = new System.Windows.Forms.Button();
            this.viewUserTextbox = new System.Windows.Forms.TextBox();
            this.endTimerButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timerButton = new System.Windows.Forms.Button();
            this.headerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerGroupBox
            // 
            this.headerGroupBox.Controls.Add(this.timerStartButton);
            this.headerGroupBox.Controls.Add(this.viewUserTextbox);
            this.headerGroupBox.Controls.Add(this.endTimerButton);
            this.headerGroupBox.Controls.Add(this.label1);
            this.headerGroupBox.Controls.Add(this.timerButton);
            this.headerGroupBox.Location = new System.Drawing.Point(0, 0);
            this.headerGroupBox.Name = "headerGroupBox";
            this.headerGroupBox.Size = new System.Drawing.Size(1184, 97);
            this.headerGroupBox.TabIndex = 1;
            this.headerGroupBox.TabStop = false;
            // 
            // timerStartButton
            // 
            this.timerStartButton.Location = new System.Drawing.Point(1017, 38);
            this.timerStartButton.Name = "timerStartButton";
            this.timerStartButton.Size = new System.Drawing.Size(155, 23);
            this.timerStartButton.TabIndex = 6;
            this.timerStartButton.Text = "zacznij czasomierz";
            this.timerStartButton.UseVisualStyleBackColor = true;
            this.timerStartButton.Click += new System.EventHandler(this.timerStartButton_Click);
            // 
            // viewUserTextbox
            // 
            this.viewUserTextbox.BackColor = System.Drawing.Color.White;
            this.viewUserTextbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.viewUserTextbox.Location = new System.Drawing.Point(1000, 12);
            this.viewUserTextbox.Name = "viewUserTextbox";
            this.viewUserTextbox.ReadOnly = true;
            this.viewUserTextbox.Size = new System.Drawing.Size(178, 20);
            this.viewUserTextbox.TabIndex = 4;
            // 
            // endTimerButton
            // 
            this.endTimerButton.Location = new System.Drawing.Point(1017, 67);
            this.endTimerButton.Name = "endTimerButton";
            this.endTimerButton.Size = new System.Drawing.Size(155, 23);
            this.endTimerButton.TabIndex = 3;
            this.endTimerButton.Text = "zakończ czasomierz";
            this.endTimerButton.UseVisualStyleBackColor = true;
            this.endTimerButton.Click += new System.EventHandler(this.endTimerButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Cursor = System.Windows.Forms.Cursors.Default;
            this.label1.Location = new System.Drawing.Point(12, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Czasomierz";
            // 
            // timerButton
            // 
            this.timerButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("timerButton.BackgroundImage")));
            this.timerButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.timerButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.timerButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.timerButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.timerButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            this.timerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.timerButton.Location = new System.Drawing.Point(12, 12);
            this.timerButton.Name = "timerButton";
            this.timerButton.Size = new System.Drawing.Size(60, 60);
            this.timerButton.TabIndex = 0;
            this.timerButton.UseVisualStyleBackColor = true;
            this.timerButton.Click += new System.EventHandler(this.timerButton_Click);
            // 
            // f4_userBody
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1184, 681);
            this.Controls.Add(this.headerGroupBox);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "f4_userBody";
            this.Text = "GREEN Office: Panel użytkownika";
            this.headerGroupBox.ResumeLayout(false);
            this.headerGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox headerGroupBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button timerButton;
        private System.Windows.Forms.Button endTimerButton;
        private System.Windows.Forms.Button timerStartButton;
        private System.Windows.Forms.TextBox viewUserTextbox;
    }
}