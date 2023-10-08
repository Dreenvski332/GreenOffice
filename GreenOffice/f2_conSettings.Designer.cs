namespace GreenOffice
{
    partial class f2_conSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(f2_conSettings));
            this.portTextbox = new System.Windows.Forms.TextBox();
            this.usernameTextbox = new System.Windows.Forms.TextBox();
            this.passwordTextbox = new System.Windows.Forms.TextBox();
            this.sourceTextbox = new System.Windows.Forms.TextBox();
            this.databaseTextbox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.saveButton = new System.Windows.Forms.Button();
            this.creditsButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // portTextbox
            // 
            this.portTextbox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.portTextbox, "portTextbox");
            this.portTextbox.Name = "portTextbox";
            // 
            // usernameTextbox
            // 
            this.usernameTextbox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.usernameTextbox, "usernameTextbox");
            this.usernameTextbox.Name = "usernameTextbox";
            // 
            // passwordTextbox
            // 
            this.passwordTextbox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.passwordTextbox, "passwordTextbox");
            this.passwordTextbox.Name = "passwordTextbox";
            // 
            // sourceTextbox
            // 
            this.sourceTextbox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.sourceTextbox, "sourceTextbox");
            this.sourceTextbox.Name = "sourceTextbox";
            // 
            // databaseTextbox
            // 
            this.databaseTextbox.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.databaseTextbox, "databaseTextbox");
            this.databaseTextbox.Name = "databaseTextbox";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // saveButton
            // 
            this.saveButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.saveButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.saveButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.saveButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            resources.ApplyResources(this.saveButton, "saveButton");
            this.saveButton.Name = "saveButton";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // creditsButton
            // 
            this.creditsButton.Cursor = System.Windows.Forms.Cursors.Hand;
            this.creditsButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.creditsButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.ForestGreen;
            this.creditsButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.DarkSeaGreen;
            resources.ApplyResources(this.creditsButton, "creditsButton");
            this.creditsButton.Name = "creditsButton";
            this.creditsButton.UseVisualStyleBackColor = true;
            this.creditsButton.Click += new System.EventHandler(this.creditsButton_Click);
            // 
            // f2_conSettings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.creditsButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.databaseTextbox);
            this.Controls.Add(this.sourceTextbox);
            this.Controls.Add(this.passwordTextbox);
            this.Controls.Add(this.usernameTextbox);
            this.Controls.Add(this.portTextbox);
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "f2_conSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox portTextbox;
        private System.Windows.Forms.TextBox usernameTextbox;
        private System.Windows.Forms.TextBox passwordTextbox;
        private System.Windows.Forms.TextBox sourceTextbox;
        private System.Windows.Forms.TextBox databaseTextbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button creditsButton;
    }
}