namespace TMX地图工具 {
    partial class Form1 {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose( bool disposing ) {
            if ( disposing && ( components != null ) ) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.label1 = new System.Windows.Forms.Label();
            this.tb_path = new System.Windows.Forms.TextBox();
            this.btn_brow = new System.Windows.Forms.Button();
            this.btn_export_excel = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btn_level = new System.Windows.Forms.Button();
            this.btn_segment = new System.Windows.Forms.Button();
            this.btn_export_area = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "地图路径";
            // 
            // tb_path
            // 
            this.tb_path.Location = new System.Drawing.Point(113, 36);
            this.tb_path.Name = "tb_path";
            this.tb_path.Size = new System.Drawing.Size(507, 21);
            this.tb_path.TabIndex = 1;
            this.tb_path.Text = "G:\\ZLF\\Unity_Projects\\SanGuo\\MapEditor\\Map\\WorldMap.tmx";
            // 
            // btn_brow
            // 
            this.btn_brow.Location = new System.Drawing.Point(626, 34);
            this.btn_brow.Name = "btn_brow";
            this.btn_brow.Size = new System.Drawing.Size(75, 23);
            this.btn_brow.TabIndex = 2;
            this.btn_brow.Text = "浏览";
            this.btn_brow.UseVisualStyleBackColor = true;
            this.btn_brow.Click += new System.EventHandler(this.btn_brow_Click);
            // 
            // btn_export_excel
            // 
            this.btn_export_excel.Location = new System.Drawing.Point(669, 530);
            this.btn_export_excel.Name = "btn_export_excel";
            this.btn_export_excel.Size = new System.Drawing.Size(103, 23);
            this.btn_export_excel.TabIndex = 3;
            this.btn_export_excel.Text = "导出地块等级";
            this.btn_export_excel.UseVisualStyleBackColor = true;
            this.btn_export_excel.Visible = false;
            this.btn_export_excel.Click += new System.EventHandler(this.btn_export_excel_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(56, 329);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(645, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // btn_level
            // 
            this.btn_level.Location = new System.Drawing.Point(669, 501);
            this.btn_level.Name = "btn_level";
            this.btn_level.Size = new System.Drawing.Size(103, 23);
            this.btn_level.TabIndex = 5;
            this.btn_level.Text = "预生成地块等级";
            this.btn_level.UseVisualStyleBackColor = true;
            this.btn_level.Visible = false;
            this.btn_level.Click += new System.EventHandler(this.btn_level_Click);
            // 
            // btn_segment
            // 
            this.btn_segment.Location = new System.Drawing.Point(485, 140);
            this.btn_segment.Name = "btn_segment";
            this.btn_segment.Size = new System.Drawing.Size(106, 23);
            this.btn_segment.TabIndex = 6;
            this.btn_segment.Text = "地图区域分块";
            this.btn_segment.UseVisualStyleBackColor = true;
            this.btn_segment.Click += new System.EventHandler(this.btn_segment_Click);
            // 
            // btn_export_area
            // 
            this.btn_export_area.Location = new System.Drawing.Point(485, 271);
            this.btn_export_area.Name = "btn_export_area";
            this.btn_export_area.Size = new System.Drawing.Size(106, 23);
            this.btn_export_area.TabIndex = 7;
            this.btn_export_area.Text = "导出城市区域";
            this.btn_export_area.UseVisualStyleBackColor = true;
            this.btn_export_area.Click += new System.EventHandler(this.btn_export_area_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(113, 140);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 33);
            this.button1.TabIndex = 8;
            this.button1.Text = "地图随机";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.地图整体随机);
            // 
            // trackBar1
            // 
            this.trackBar1.Location = new System.Drawing.Point(56, 179);
            this.trackBar1.Maximum = 100;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(194, 45);
            this.trackBar1.TabIndex = 9;
            this.trackBar1.Value = 10;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 179);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 10;
            this.label2.Text = "湖泊10%";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trackBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_export_area);
            this.Controls.Add(this.btn_segment);
            this.Controls.Add(this.btn_level);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.btn_export_excel);
            this.Controls.Add(this.btn_brow);
            this.Controls.Add(this.tb_path);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "地图工具";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tb_path;
        private System.Windows.Forms.Button btn_brow;
        private System.Windows.Forms.Button btn_export_excel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button btn_level;
        private System.Windows.Forms.Button btn_segment;
        private System.Windows.Forms.Button btn_export_area;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label2;
    }
}

