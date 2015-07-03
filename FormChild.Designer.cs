namespace Paint
{
    partial class FormChild
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
            this.components = new System.ComponentModel.Container();
            this.sprayGunTimer = new System.Windows.Forms.Timer(this.components);
            this.alphaTextBox = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // sprayGunTimer
            // 
            this.sprayGunTimer.Tick += new System.EventHandler(this.OnTimer);
            // 
            // alphaTextBox
            // 
            this.alphaTextBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.alphaTextBox.Location = new System.Drawing.Point(12, 12);
            this.alphaTextBox.Multiline = true;
            this.alphaTextBox.Name = "alphaTextBox";
            this.alphaTextBox.Size = new System.Drawing.Size(100, 21);
            this.alphaTextBox.TabIndex = 0;
            this.alphaTextBox.Visible = false;
            this.alphaTextBox.WordWrap = false;
            this.alphaTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TextBox_Key_Down);
            this.alphaTextBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TextBox_Mouse_Down);
            this.alphaTextBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.TextBox_Pre_Key_Down);
            // 
            // FormChild
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(400, 300);
            this.ControlBox = false;
            this.Controls.Add(this.alphaTextBox);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormChild";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "绘图区";
            this.ResizeEnd += new System.EventHandler(this.OnSizeChanged);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.OnPaint);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Mouse_Move);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer sprayGunTimer;
        public System.Windows.Forms.TextBox alphaTextBox;
    }
}