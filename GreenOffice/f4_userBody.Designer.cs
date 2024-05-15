namespace GreenOffice
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
            this.timeoutLabel = new System.Windows.Forms.Label();
            this.timeoutButton = new System.Windows.Forms.Button();
            this.calendarLabel = new System.Windows.Forms.Label();
            this.calendarButton = new System.Windows.Forms.Button();
            this.timerStartButton = new System.Windows.Forms.Button();
            this.viewUserTextbox = new System.Windows.Forms.TextBox();
            this.endTimerButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.timerButton = new System.Windows.Forms.Button();
            this.Panel = new System.Windows.Forms.Panel();
            this.headerGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerGroupBox
            // 
            this.headerGroupBox.Controls.Add(this.timeoutLabel);
            this.headerGroupBox.Controls.Add(this.timeoutButton);
            this.headerGroupBox.Controls.Add(this.calendarLabel);
            this.headerGroupBox.Controls.Add(this.calendarButton);
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
            // timeoutLabel
            // 
            this.timeoutLabel.AutoSize = true;
            this.timeoutLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timeoutLabel.Location = new System.Drawing.Point(316, 75);
            this.timeoutLabel.Name = "timeoutLabel";
            this.timeoutLabel.Size = new System.Drawing.Size(121, 15);
            this.timeoutLabel.TabIndex = 2;
            this.timeoutLabel.Text = "Zarejestrój dni wolne";
            // 
            // timeoutButton
            // 
            this.timeoutButton.BackgroundImage = global::GreenOffice.Properties.Resources.google_docs;
            this.timeoutButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.timeoutButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.timeoutButton.FlatAppearance.BorderSize = 0;
            this.timeoutButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.timeoutButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            this.timeoutButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.timeoutButton.Location = new System.Drawing.Point(338, 12);
            this.timeoutButton.Name = "timeoutButton";
            this.timeoutButton.Size = new System.Drawing.Size(60, 60);
            this.timeoutButton.TabIndex = 8;
            this.timeoutButton.UseVisualStyleBackColor = true;
            // 
            // calendarLabel
            // 
            this.calendarLabel.AutoSize = true;
            this.calendarLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calendarLabel.Location = new System.Drawing.Point(168, 75);
            this.calendarLabel.Name = "calendarLabel";
            this.calendarLabel.Size = new System.Drawing.Size(123, 15);
            this.calendarLabel.TabIndex = 2;
            this.calendarLabel.Text = "Personalny terminarz";
            // 
            // calendarButton
            // 
            this.calendarButton.BackgroundImage = global::GreenOffice.Properties.Resources.calendar;
            this.calendarButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.calendarButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.calendarButton.FlatAppearance.BorderSize = 0;
            this.calendarButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.calendarButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            this.calendarButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.calendarButton.Location = new System.Drawing.Point(189, 12);
            this.calendarButton.Name = "calendarButton";
            this.calendarButton.Size = new System.Drawing.Size(60, 60);
            this.calendarButton.TabIndex = 7;
            this.calendarButton.UseVisualStyleBackColor = true;
            // 
            // timerStartButton
            // 
            this.timerStartButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.timerStartButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.timerStartButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            this.timerStartButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.timerStartButton.Location = new System.Drawing.Point(1000, 41);
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
            this.viewUserTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.viewUserTextbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.viewUserTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.viewUserTextbox.Location = new System.Drawing.Point(976, 12);
            this.viewUserTextbox.Multiline = true;
            this.viewUserTextbox.Name = "viewUserTextbox";
            this.viewUserTextbox.ReadOnly = true;
            this.viewUserTextbox.Size = new System.Drawing.Size(202, 23);
            this.viewUserTextbox.TabIndex = 4;
            this.viewUserTextbox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // endTimerButton
            // 
            this.endTimerButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.endTimerButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.endTimerButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            this.endTimerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.endTimerButton.Location = new System.Drawing.Point(1000, 70);
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
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(131, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Ewidencja czasu pracy";
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
            this.timerButton.Location = new System.Drawing.Point(37, 12);
            this.timerButton.Name = "timerButton";
            this.timerButton.Size = new System.Drawing.Size(60, 60);
            this.timerButton.TabIndex = 0;
            this.timerButton.UseVisualStyleBackColor = true;
            this.timerButton.Click += new System.EventHandler(this.timerButton_Click);
            // 
            // Panel
            // 
            this.Panel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Panel.Location = new System.Drawing.Point(12, 103);
            this.Panel.Name = "Panel";
            this.Panel.Size = new System.Drawing.Size(1160, 566);
            this.Panel.TabIndex = 2;
            // 
            // f4_userBody
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1184, 681);
            this.Controls.Add(this.Panel);
            this.Controls.Add(this.headerGroupBox);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
        private System.Windows.Forms.Label calendarLabel;
        private System.Windows.Forms.Button calendarButton;
        private System.Windows.Forms.Button timeoutButton;
        private System.Windows.Forms.Label timeoutLabel;
        private System.Windows.Forms.Panel Panel;
    }
}