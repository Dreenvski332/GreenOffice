namespace GreenOffice
{
    partial class AdminCalendarUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.idLabel = new System.Windows.Forms.Label();
            this.displayLeaveTextbox = new System.Windows.Forms.TextBox();
            this.codeViewUserLabel = new System.Windows.Forms.Label();
            this.dayLabel = new System.Windows.Forms.Label();
            this.leaveDescriptionTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.BackColor = System.Drawing.Color.Transparent;
            this.idLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.idLabel.ForeColor = System.Drawing.Color.Honeydew;
            this.idLabel.Location = new System.Drawing.Point(46, 1);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(35, 13);
            this.idLabel.TabIndex = 8;
            this.idLabel.Text = "label1";
            // 
            // displayLeaveTextbox
            // 
            this.displayLeaveTextbox.BackColor = System.Drawing.Color.Honeydew;
            this.displayLeaveTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.displayLeaveTextbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.displayLeaveTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayLeaveTextbox.Location = new System.Drawing.Point(26, 18);
            this.displayLeaveTextbox.Multiline = true;
            this.displayLeaveTextbox.Name = "displayLeaveTextbox";
            this.displayLeaveTextbox.ReadOnly = true;
            this.displayLeaveTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.displayLeaveTextbox.Size = new System.Drawing.Size(85, 47);
            this.displayLeaveTextbox.TabIndex = 7;
            this.displayLeaveTextbox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.displayLeaveTextbox_MouseDown);
            this.displayLeaveTextbox.MouseEnter += new System.EventHandler(this.displayLeaveTextbox_MouseEnter);
            this.displayLeaveTextbox.MouseLeave += new System.EventHandler(this.displayLeaveTextbox_MouseLeave);
            // 
            // codeViewUserLabel
            // 
            this.codeViewUserLabel.AutoSize = true;
            this.codeViewUserLabel.ForeColor = System.Drawing.Color.Honeydew;
            this.codeViewUserLabel.Location = new System.Drawing.Point(35, 57);
            this.codeViewUserLabel.Name = "codeViewUserLabel";
            this.codeViewUserLabel.Size = new System.Drawing.Size(35, 13);
            this.codeViewUserLabel.TabIndex = 6;
            this.codeViewUserLabel.Text = "label1";
            // 
            // dayLabel
            // 
            this.dayLabel.AutoSize = true;
            this.dayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dayLabel.Location = new System.Drawing.Point(3, 1);
            this.dayLabel.Name = "dayLabel";
            this.dayLabel.Size = new System.Drawing.Size(17, 18);
            this.dayLabel.TabIndex = 5;
            this.dayLabel.Text = "1";
            this.dayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // AdminCalendarUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.Controls.Add(this.idLabel);
            this.Controls.Add(this.displayLeaveTextbox);
            this.Controls.Add(this.codeViewUserLabel);
            this.Controls.Add(this.dayLabel);
            this.Name = "AdminCalendarUserControl";
            this.Size = new System.Drawing.Size(114, 70);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.TextBox displayLeaveTextbox;
        private System.Windows.Forms.Label codeViewUserLabel;
        private System.Windows.Forms.Label dayLabel;
        private System.Windows.Forms.ToolTip leaveDescriptionTooltip;
    }
}
