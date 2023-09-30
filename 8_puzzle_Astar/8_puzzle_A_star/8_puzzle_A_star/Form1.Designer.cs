namespace _8_15_puzzle_A_star
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
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.radioBtnHeuristic1 = new System.Windows.Forms.RadioButton();
            this.radioBtnHeuristic2 = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.cbbSize = new System.Windows.Forms.ComboBox();
            this.btnDung = new System.Windows.Forms.Button();
            this.btnChoiMoi = new System.Windows.Forms.Button();
            this.btnGiai = new System.Windows.Forms.Button();
            this.lbSoLanDiChuyen = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cbbTocDo = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Interval = 2000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightCyan;
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1285, 750);
            this.panel1.TabIndex = 12;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.OldLace;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.radioBtnHeuristic1);
            this.panel2.Controls.Add(this.radioBtnHeuristic2);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.cbbSize);
            this.panel2.Controls.Add(this.btnDung);
            this.panel2.Controls.Add(this.btnChoiMoi);
            this.panel2.Controls.Add(this.btnGiai);
            this.panel2.Controls.Add(this.lbSoLanDiChuyen);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.cbbTocDo);
            this.panel2.Location = new System.Drawing.Point(13, 44);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(411, 652);
            this.panel2.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label2.Location = new System.Drawing.Point(56, 547);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 36);
            this.label2.TabIndex = 24;
            this.label2.Text = "Bước: ";
            // 
            // radioBtnHeuristic1
            // 
            this.radioBtnHeuristic1.AutoSize = true;
            this.radioBtnHeuristic1.Checked = true;
            this.radioBtnHeuristic1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnHeuristic1.Location = new System.Drawing.Point(46, 455);
            this.radioBtnHeuristic1.Name = "radioBtnHeuristic1";
            this.radioBtnHeuristic1.Size = new System.Drawing.Size(124, 29);
            this.radioBtnHeuristic1.TabIndex = 22;
            this.radioBtnHeuristic1.TabStop = true;
            this.radioBtnHeuristic1.Text = "Heuristic 1";
            this.radioBtnHeuristic1.UseVisualStyleBackColor = true;
            this.radioBtnHeuristic1.CheckedChanged += new System.EventHandler(this.radioBtnHeuristic1_CheckedChanged);
            // 
            // radioBtnHeuristic2
            // 
            this.radioBtnHeuristic2.AutoSize = true;
            this.radioBtnHeuristic2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioBtnHeuristic2.Location = new System.Drawing.Point(233, 455);
            this.radioBtnHeuristic2.Name = "radioBtnHeuristic2";
            this.radioBtnHeuristic2.Size = new System.Drawing.Size(124, 29);
            this.radioBtnHeuristic2.TabIndex = 23;
            this.radioBtnHeuristic2.TabStop = true;
            this.radioBtnHeuristic2.Text = "Heuristic 2";
            this.radioBtnHeuristic2.UseVisualStyleBackColor = true;
            this.radioBtnHeuristic2.CheckedChanged += new System.EventHandler(this.radioBtnHeuristic2_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label3.Location = new System.Drawing.Point(100, 310);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 20);
            this.label3.TabIndex = 20;
            this.label3.Text = "Size:";
            // 
            // cbbSize
            // 
            this.cbbSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbSize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbSize.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbSize.FormattingEnabled = true;
            this.cbbSize.Items.AddRange(new object[] {
            "3",
            "4"});
            this.cbbSize.Location = new System.Drawing.Point(200, 306);
            this.cbbSize.Name = "cbbSize";
            this.cbbSize.Size = new System.Drawing.Size(121, 28);
            this.cbbSize.TabIndex = 17;
            this.cbbSize.SelectedIndexChanged += new System.EventHandler(this.cbbSize_SelectedIndexChanged);
            // 
            // btnDung
            // 
            this.btnDung.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnDung.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDung.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnDung.ForeColor = System.Drawing.Color.White;
            this.btnDung.Location = new System.Drawing.Point(138, 207);
            this.btnDung.Margin = new System.Windows.Forms.Padding(4);
            this.btnDung.Name = "btnDung";
            this.btnDung.Size = new System.Drawing.Size(123, 49);
            this.btnDung.TabIndex = 17;
            this.btnDung.Text = "Tạm dừng";
            this.btnDung.UseVisualStyleBackColor = false;
            this.btnDung.Click += new System.EventHandler(this.btDung_Click);
            // 
            // btnChoiMoi
            // 
            this.btnChoiMoi.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnChoiMoi.FlatAppearance.BorderColor = System.Drawing.Color.Green;
            this.btnChoiMoi.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChoiMoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnChoiMoi.ForeColor = System.Drawing.Color.White;
            this.btnChoiMoi.Location = new System.Drawing.Point(138, 46);
            this.btnChoiMoi.Margin = new System.Windows.Forms.Padding(4);
            this.btnChoiMoi.Name = "btnChoiMoi";
            this.btnChoiMoi.Size = new System.Drawing.Size(123, 49);
            this.btnChoiMoi.TabIndex = 18;
            this.btnChoiMoi.Text = "Trộn";
            this.btnChoiMoi.UseVisualStyleBackColor = false;
            this.btnChoiMoi.Click += new System.EventHandler(this.ChoiMoi);
            // 
            // btnGiai
            // 
            this.btnGiai.BackColor = System.Drawing.Color.DodgerBlue;
            this.btnGiai.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGiai.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnGiai.ForeColor = System.Drawing.Color.White;
            this.btnGiai.Location = new System.Drawing.Point(138, 128);
            this.btnGiai.Margin = new System.Windows.Forms.Padding(4);
            this.btnGiai.Name = "btnGiai";
            this.btnGiai.Size = new System.Drawing.Size(123, 49);
            this.btnGiai.TabIndex = 19;
            this.btnGiai.Text = "Giải";
            this.btnGiai.UseVisualStyleBackColor = false;
            this.btnGiai.Click += new System.EventHandler(this.btnGiai_Click);
            // 
            // lbSoLanDiChuyen
            // 
            this.lbSoLanDiChuyen.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lbSoLanDiChuyen.Location = new System.Drawing.Point(155, 526);
            this.lbSoLanDiChuyen.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSoLanDiChuyen.Name = "lbSoLanDiChuyen";
            this.lbSoLanDiChuyen.Size = new System.Drawing.Size(241, 75);
            this.lbSoLanDiChuyen.TabIndex = 16;
            this.lbSoLanDiChuyen.Text = "0";
            this.lbSoLanDiChuyen.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.label1.Location = new System.Drawing.Point(92, 375);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 20);
            this.label1.TabIndex = 14;
            this.label1.Text = "Tốc độ: ";
            // 
            // cbbTocDo
            // 
            this.cbbTocDo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbTocDo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cbbTocDo.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbbTocDo.ForeColor = System.Drawing.Color.Black;
            this.cbbTocDo.FormattingEnabled = true;
            this.cbbTocDo.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5"});
            this.cbbTocDo.Location = new System.Drawing.Point(200, 375);
            this.cbbTocDo.Margin = new System.Windows.Forms.Padding(4);
            this.cbbTocDo.Name = "cbbTocDo";
            this.cbbTocDo.Size = new System.Drawing.Size(121, 28);
            this.cbbTocDo.TabIndex = 13;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.OldLace;
            this.panel3.Location = new System.Drawing.Point(431, 44);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(766, 652);
            this.panel3.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1285, 750);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Trò chơi xếp số";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnDung;
        private System.Windows.Forms.Button btnChoiMoi;
        private System.Windows.Forms.Button btnGiai;
        private System.Windows.Forms.Label lbSoLanDiChuyen;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbbTocDo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox cbbSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton radioBtnHeuristic1;
        private System.Windows.Forms.RadioButton radioBtnHeuristic2;
        private System.Windows.Forms.Label label2;
    }
}

