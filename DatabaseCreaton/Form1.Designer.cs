namespace DatabaseCreaton
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
            this.clearDebug = new System.Windows.Forms.Button();
            this.w_base_butt = new System.Windows.Forms.Button();
            this.w_ext_butt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // clearDebug
            // 
            this.clearDebug.Location = new System.Drawing.Point(176, 124);
            this.clearDebug.Name = "clearDebug";
            this.clearDebug.Size = new System.Drawing.Size(118, 31);
            this.clearDebug.TabIndex = 5;
            this.clearDebug.Text = "Очистить Debug";
            this.clearDebug.UseVisualStyleBackColor = true;
            this.clearDebug.Click += new System.EventHandler(this.clearDebug_Click);
            // 
            // w_base_butt
            // 
            this.w_base_butt.Location = new System.Drawing.Point(25, 25);
            this.w_base_butt.Name = "w_base_butt";
            this.w_base_butt.Size = new System.Drawing.Size(118, 93);
            this.w_base_butt.TabIndex = 6;
            this.w_base_butt.Text = "w_base";
            this.w_base_butt.UseVisualStyleBackColor = true;
            this.w_base_butt.Click += new System.EventHandler(this.w_base_butt_Click);
            // 
            // w_ext_butt
            // 
            this.w_ext_butt.Location = new System.Drawing.Point(176, 25);
            this.w_ext_butt.Name = "w_ext_butt";
            this.w_ext_butt.Size = new System.Drawing.Size(118, 93);
            this.w_ext_butt.TabIndex = 7;
            this.w_ext_butt.Text = "w_ext";
            this.w_ext_butt.UseVisualStyleBackColor = true;
            this.w_ext_butt.Click += new System.EventHandler(this.w_ext_butt_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(320, 158);
            this.Controls.Add(this.w_ext_butt);
            this.Controls.Add(this.w_base_butt);
            this.Controls.Add(this.clearDebug);
            this.Name = "Form1";
            this.Text = "DatabaseCreaton";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button clearDebug;
        private System.Windows.Forms.Button w_base_butt;
        private System.Windows.Forms.Button w_ext_butt;
    }
}

