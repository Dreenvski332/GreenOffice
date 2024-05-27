namespace GreenOffice
{
    partial class f6_deleteEvent
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f6_deleteEvent));
            this.deleteLabel = new System.Windows.Forms.Label();
            this.backgroundPanel = new System.Windows.Forms.Panel();
            this.keepButton = new System.Windows.Forms.Button();
            this.deleteButton = new System.Windows.Forms.Button();
            this.backgroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // deleteLabel
            // 
            this.deleteLabel.AutoSize = true;
            this.deleteLabel.BackColor = System.Drawing.Color.White;
            this.deleteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteLabel.Location = new System.Drawing.Point(-2, 9);
            this.deleteLabel.Name = "deleteLabel";
            this.deleteLabel.Size = new System.Drawing.Size(304, 24);
            this.deleteLabel.TabIndex = 0;
            this.deleteLabel.Text = "Czy chcesz usunąć to wydarzenie?";
            // 
            // backgroundPanel
            // 
            this.backgroundPanel.BackgroundImage = global::GreenOffice.Properties.Resources.logoNoCap;
            this.backgroundPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.backgroundPanel.Controls.Add(this.keepButton);
            this.backgroundPanel.Controls.Add(this.deleteButton);
            this.backgroundPanel.Location = new System.Drawing.Point(2, 23);
            this.backgroundPanel.Name = "backgroundPanel";
            this.backgroundPanel.Size = new System.Drawing.Size(300, 135);
            this.backgroundPanel.TabIndex = 1;
            // 
            // keepButton
            // 
            this.keepButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.keepButton.FlatAppearance.BorderSize = 0;
            this.keepButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.keepButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            this.keepButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.keepButton.Location = new System.Drawing.Point(10, 61);
            this.keepButton.Name = "keepButton";
            this.keepButton.Size = new System.Drawing.Size(81, 39);
            this.keepButton.TabIndex = 3;
            this.keepButton.Text = "Anuluj Operację";
            this.keepButton.UseVisualStyleBackColor = true;
            this.keepButton.Click += new System.EventHandler(this.keepButton_Click);
            // 
            // deleteButton
            // 
            this.deleteButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.deleteButton.FlatAppearance.BorderSize = 0;
            this.deleteButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.deleteButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            this.deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.deleteButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteButton.Location = new System.Drawing.Point(206, 61);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(94, 39);
            this.deleteButton.TabIndex = 2;
            this.deleteButton.Text = "Usuń Wydarzenie";
            this.deleteButton.UseVisualStyleBackColor = true;
            this.deleteButton.Click += new System.EventHandler(this.deleteButton_Click);
            // 
            // f6_deleteEvent
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(306, 160);
            this.Controls.Add(this.deleteLabel);
            this.Controls.Add(this.backgroundPanel);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "f6_deleteEvent";
            this.Text = "Usunąć wydarzenie?";
            this.backgroundPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label deleteLabel;
        private System.Windows.Forms.Panel backgroundPanel;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button keepButton;
    }
}