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
            this.cars_butt = new System.Windows.Forms.Button();
            this.drivers_butt = new System.Windows.Forms.Button();
            this.store_butt = new System.Windows.Forms.Button();
            this.store_info_butt = new System.Windows.Forms.Button();
            this.debug = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cars_butt
            // 
            this.cars_butt.Location = new System.Drawing.Point(13, 13);
            this.cars_butt.Name = "cars_butt";
            this.cars_butt.Size = new System.Drawing.Size(118, 96);
            this.cars_butt.TabIndex = 0;
            this.cars_butt.Text = "cars";
            this.cars_butt.UseVisualStyleBackColor = true;
            this.cars_butt.Click += new System.EventHandler(this.cars_butt_Click);
            // 
            // drivers_butt
            // 
            this.drivers_butt.Location = new System.Drawing.Point(149, 13);
            this.drivers_butt.Margin = new System.Windows.Forms.Padding(15);
            this.drivers_butt.Name = "drivers_butt";
            this.drivers_butt.Size = new System.Drawing.Size(118, 96);
            this.drivers_butt.TabIndex = 1;
            this.drivers_butt.Text = "drivers";
            this.drivers_butt.UseVisualStyleBackColor = true;
            this.drivers_butt.Click += new System.EventHandler(this.drivers_butt_Click);
            // 
            // store_butt
            // 
            this.store_butt.Location = new System.Drawing.Point(285, 13);
            this.store_butt.Name = "store_butt";
            this.store_butt.Size = new System.Drawing.Size(118, 96);
            this.store_butt.TabIndex = 2;
            this.store_butt.Text = "store";
            this.store_butt.UseVisualStyleBackColor = true;
            this.store_butt.Click += new System.EventHandler(this.store_butt_Click);
            // 
            // store_info_butt
            // 
            this.store_info_butt.Location = new System.Drawing.Point(421, 12);
            this.store_info_butt.Margin = new System.Windows.Forms.Padding(15);
            this.store_info_butt.Name = "store_info_butt";
            this.store_info_butt.Size = new System.Drawing.Size(118, 96);
            this.store_info_butt.TabIndex = 3;
            this.store_info_butt.Text = "store_info";
            this.store_info_butt.UseVisualStyleBackColor = true;
            this.store_info_butt.Click += new System.EventHandler(this.store_info_butt_Click);
            // 
            // debug
            // 
            this.debug.Location = new System.Drawing.Point(557, 12);
            this.debug.Name = "debug";
            this.debug.Size = new System.Drawing.Size(118, 96);
            this.debug.TabIndex = 4;
            this.debug.Text = "debug";
            this.debug.UseVisualStyleBackColor = true;
            this.debug.Click += new System.EventHandler(this.debug_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(702, 134);
            this.Controls.Add(this.debug);
            this.Controls.Add(this.store_info_butt);
            this.Controls.Add(this.store_butt);
            this.Controls.Add(this.drivers_butt);
            this.Controls.Add(this.cars_butt);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cars_butt;
        private System.Windows.Forms.Button drivers_butt;
        private System.Windows.Forms.Button store_butt;
        private System.Windows.Forms.Button store_info_butt;
        private System.Windows.Forms.Button debug;
    }
}

