namespace GreenOffice
{
    partial class DayUserControl
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
            this.dayLabel = new System.Windows.Forms.Label();
            this.codeViewUserLabel = new System.Windows.Forms.Label();
            this.displayEventTextbox = new System.Windows.Forms.TextBox();
            this.eventDescriptionTooltip = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // dayLabel
            // 
            this.dayLabel.AutoSize = true;
            this.dayLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dayLabel.Location = new System.Drawing.Point(3, 3);
            this.dayLabel.Name = "dayLabel";
            this.dayLabel.Size = new System.Drawing.Size(17, 18);
            this.dayLabel.TabIndex = 0;
            this.dayLabel.Text = "1";
            this.dayLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // codeViewUserLabel
            // 
            this.codeViewUserLabel.AutoSize = true;
            this.codeViewUserLabel.ForeColor = System.Drawing.Color.Honeydew;
            this.codeViewUserLabel.Location = new System.Drawing.Point(35, 59);
            this.codeViewUserLabel.Name = "codeViewUserLabel";
            this.codeViewUserLabel.Size = new System.Drawing.Size(35, 13);
            this.codeViewUserLabel.TabIndex = 2;
            this.codeViewUserLabel.Text = "label1";
            // 
            // displayEventTextbox
            // 
            this.displayEventTextbox.BackColor = System.Drawing.Color.Honeydew;
            this.displayEventTextbox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.displayEventTextbox.Cursor = System.Windows.Forms.Cursors.Default;
            this.displayEventTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.displayEventTextbox.Location = new System.Drawing.Point(26, 20);
            this.displayEventTextbox.Multiline = true;
            this.displayEventTextbox.Name = "displayEventTextbox";
            this.displayEventTextbox.ReadOnly = true;
            this.displayEventTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.displayEventTextbox.Size = new System.Drawing.Size(85, 47);
            this.displayEventTextbox.TabIndex = 3;
            // 
            // eventDescriptionTooltip
            // 
            this.eventDescriptionTooltip.AutomaticDelay = 250;
            this.eventDescriptionTooltip.AutoPopDelay = 10000;
            this.eventDescriptionTooltip.BackColor = System.Drawing.Color.MintCream;
            this.eventDescriptionTooltip.InitialDelay = 250;
            this.eventDescriptionTooltip.ReshowDelay = 50;
            // 
            // DayUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Honeydew;
            this.Controls.Add(this.displayEventTextbox);
            this.Controls.Add(this.codeViewUserLabel);
            this.Controls.Add(this.dayLabel);
            this.Name = "DayUserControl";
            this.Size = new System.Drawing.Size(114, 70);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label dayLabel;
        private System.Windows.Forms.Label codeViewUserLabel;
        private System.Windows.Forms.TextBox displayEventTextbox;
        private System.Windows.Forms.ToolTip eventDescriptionTooltip;
    }
}
