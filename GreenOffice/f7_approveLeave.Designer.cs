namespace GreenOffice
{
    partial class f7_approveLeave
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f7_approveLeave));
            this.deleteLabel = new System.Windows.Forms.Label();
            this.backgroundPanel = new System.Windows.Forms.Panel();
            this.doNotApproveButton = new System.Windows.Forms.Button();
            this.ApproveButton = new System.Windows.Forms.Button();
            this.backgroundPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // deleteLabel
            // 
            this.deleteLabel.AutoSize = true;
            this.deleteLabel.BackColor = System.Drawing.Color.White;
            this.deleteLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.deleteLabel.Location = new System.Drawing.Point(38, 9);
            this.deleteLabel.Name = "deleteLabel";
            this.deleteLabel.Size = new System.Drawing.Size(230, 24);
            this.deleteLabel.TabIndex = 2;
            this.deleteLabel.Text = "Zatwierdzić nieobecność?";
            // 
            // backgroundPanel
            // 
            this.backgroundPanel.BackgroundImage = global::GreenOffice.Properties.Resources.logoNoCap;
            this.backgroundPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.backgroundPanel.Controls.Add(this.doNotApproveButton);
            this.backgroundPanel.Controls.Add(this.ApproveButton);
            this.backgroundPanel.Location = new System.Drawing.Point(5, 20);
            this.backgroundPanel.Name = "backgroundPanel";
            this.backgroundPanel.Size = new System.Drawing.Size(300, 135);
            this.backgroundPanel.TabIndex = 3;
            // 
            // doNotApproveButton
            // 
            this.doNotApproveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.doNotApproveButton.FlatAppearance.BorderSize = 0;
            this.doNotApproveButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.doNotApproveButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            this.doNotApproveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.doNotApproveButton.Location = new System.Drawing.Point(10, 61);
            this.doNotApproveButton.Name = "doNotApproveButton";
            this.doNotApproveButton.Size = new System.Drawing.Size(81, 39);
            this.doNotApproveButton.TabIndex = 3;
            this.doNotApproveButton.Text = "Nie zatwierdzaj";
            this.doNotApproveButton.UseVisualStyleBackColor = true;
            this.doNotApproveButton.Click += new System.EventHandler(this.doNotApproveButton_Click);
            // 
            // ApproveButton
            // 
            this.ApproveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.ApproveButton.FlatAppearance.BorderSize = 0;
            this.ApproveButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.ApproveButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            this.ApproveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ApproveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ApproveButton.Location = new System.Drawing.Point(206, 61);
            this.ApproveButton.Name = "ApproveButton";
            this.ApproveButton.Size = new System.Drawing.Size(94, 39);
            this.ApproveButton.TabIndex = 2;
            this.ApproveButton.Text = "Zatwierdź";
            this.ApproveButton.UseVisualStyleBackColor = true;
            this.ApproveButton.Click += new System.EventHandler(this.ApproveButton_Click);
            // 
            // f7_approveLeave
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(306, 160);
            this.Controls.Add(this.deleteLabel);
            this.Controls.Add(this.backgroundPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "f7_approveLeave";
            this.Text = "Zatwierdzić nieobecność?";
            this.backgroundPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label deleteLabel;
        private System.Windows.Forms.Panel backgroundPanel;
        private System.Windows.Forms.Button doNotApproveButton;
        private System.Windows.Forms.Button ApproveButton;
    }
}