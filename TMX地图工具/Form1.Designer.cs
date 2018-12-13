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
            this.湖泊trackBar1 = new System.Windows.Forms.TrackBar();
            this.湖泊label2 = new System.Windows.Forms.Label();
            this.山体label3 = new System.Windows.Forms.Label();
            this.山体trackBar2 = new System.Windows.Forms.TrackBar();
            this.树林label4 = new System.Windows.Forms.Label();
            this.树林trackBar3 = new System.Windows.Forms.TrackBar();
            this.湖泊装饰label5 = new System.Windows.Forms.Label();
            this.湖泊装饰trackBar4 = new System.Windows.Forms.TrackBar();
            this.陆地装饰label2 = new System.Windows.Forms.Label();
            this.陆地trackBar1 = new System.Windows.Forms.TrackBar();
            this.Area1_ID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Area2_ID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Area_1_2_ID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox_terrain = new System.Windows.Forms.CheckBox();
            this.btn_resource = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.湖泊trackBar1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.山体trackBar2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.树林trackBar3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.湖泊装饰trackBar4)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.陆地trackBar1)).BeginInit();
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
            this.progressBar1.Location = new System.Drawing.Point(56, 430);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(645, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // btn_level
            // 
            this.btn_level.Location = new System.Drawing.Point(353, 213);
            this.btn_level.Name = "btn_level";
            this.btn_level.Size = new System.Drawing.Size(103, 23);
            this.btn_level.TabIndex = 5;
            this.btn_level.Text = "随机土地等级";
            this.btn_level.UseVisualStyleBackColor = true;
            this.btn_level.Click += new System.EventHandler(this.btn_level_Click);
            // 
            // btn_segment
            // 
            this.btn_segment.Location = new System.Drawing.Point(353, 162);
            this.btn_segment.Name = "btn_segment";
            this.btn_segment.Size = new System.Drawing.Size(106, 23);
            this.btn_segment.TabIndex = 6;
            this.btn_segment.Text = "地图区域分块";
            this.btn_segment.UseVisualStyleBackColor = true;
            this.btn_segment.Click += new System.EventHandler(this.btn_segment_Click);
            // 
            // btn_export_area
            // 
            this.btn_export_area.Location = new System.Drawing.Point(574, 162);
            this.btn_export_area.Name = "btn_export_area";
            this.btn_export_area.Size = new System.Drawing.Size(106, 23);
            this.btn_export_area.TabIndex = 7;
            this.btn_export_area.Text = "导出城市区域";
            this.btn_export_area.UseVisualStyleBackColor = true;
            this.btn_export_area.Click += new System.EventHandler(this.btn_export_area_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(56, 87);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(109, 33);
            this.button1.TabIndex = 8;
            this.button1.Text = "地图随机";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.地图整体随机);
            // 
            // 湖泊trackBar1
            // 
            this.湖泊trackBar1.Location = new System.Drawing.Point(32, 140);
            this.湖泊trackBar1.Maximum = 100;
            this.湖泊trackBar1.Name = "湖泊trackBar1";
            this.湖泊trackBar1.Size = new System.Drawing.Size(194, 45);
            this.湖泊trackBar1.TabIndex = 9;
            this.湖泊trackBar1.Value = 15;
            this.湖泊trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // 湖泊label2
            // 
            this.湖泊label2.AutoSize = true;
            this.湖泊label2.Location = new System.Drawing.Point(232, 140);
            this.湖泊label2.Name = "湖泊label2";
            this.湖泊label2.Size = new System.Drawing.Size(47, 12);
            this.湖泊label2.TabIndex = 10;
            this.湖泊label2.Text = "湖泊10%";
            // 
            // 山体label3
            // 
            this.山体label3.AutoSize = true;
            this.山体label3.Location = new System.Drawing.Point(232, 191);
            this.山体label3.Name = "山体label3";
            this.山体label3.Size = new System.Drawing.Size(47, 12);
            this.山体label3.TabIndex = 12;
            this.山体label3.Text = "山体25%";
            // 
            // 山体trackBar2
            // 
            this.山体trackBar2.Location = new System.Drawing.Point(32, 191);
            this.山体trackBar2.Maximum = 100;
            this.山体trackBar2.Name = "山体trackBar2";
            this.山体trackBar2.Size = new System.Drawing.Size(194, 45);
            this.山体trackBar2.TabIndex = 11;
            this.山体trackBar2.Value = 15;
            this.山体trackBar2.Scroll += new System.EventHandler(this.trackBar2_Scroll);
            // 
            // 树林label4
            // 
            this.树林label4.AutoSize = true;
            this.树林label4.Location = new System.Drawing.Point(232, 242);
            this.树林label4.Name = "树林label4";
            this.树林label4.Size = new System.Drawing.Size(47, 12);
            this.树林label4.TabIndex = 14;
            this.树林label4.Text = "树林35%";
            // 
            // 树林trackBar3
            // 
            this.树林trackBar3.Location = new System.Drawing.Point(32, 242);
            this.树林trackBar3.Maximum = 100;
            this.树林trackBar3.Name = "树林trackBar3";
            this.树林trackBar3.Size = new System.Drawing.Size(194, 45);
            this.树林trackBar3.TabIndex = 13;
            this.树林trackBar3.Value = 25;
            this.树林trackBar3.Scroll += new System.EventHandler(this.trackBar3_Scroll);
            // 
            // 湖泊装饰label5
            // 
            this.湖泊装饰label5.AutoSize = true;
            this.湖泊装饰label5.Location = new System.Drawing.Point(232, 293);
            this.湖泊装饰label5.Name = "湖泊装饰label5";
            this.湖泊装饰label5.Size = new System.Drawing.Size(65, 12);
            this.湖泊装饰label5.TabIndex = 16;
            this.湖泊装饰label5.Text = "湖泊装饰5%";
            // 
            // 湖泊装饰trackBar4
            // 
            this.湖泊装饰trackBar4.Location = new System.Drawing.Point(32, 293);
            this.湖泊装饰trackBar4.Maximum = 30;
            this.湖泊装饰trackBar4.Name = "湖泊装饰trackBar4";
            this.湖泊装饰trackBar4.Size = new System.Drawing.Size(194, 45);
            this.湖泊装饰trackBar4.TabIndex = 15;
            this.湖泊装饰trackBar4.Value = 1;
            this.湖泊装饰trackBar4.Scroll += new System.EventHandler(this.trackBar4_Scroll);
            // 
            // 陆地装饰label2
            // 
            this.陆地装饰label2.AutoSize = true;
            this.陆地装饰label2.Location = new System.Drawing.Point(232, 344);
            this.陆地装饰label2.Name = "陆地装饰label2";
            this.陆地装饰label2.Size = new System.Drawing.Size(65, 12);
            this.陆地装饰label2.TabIndex = 18;
            this.陆地装饰label2.Text = "陆地装饰5%";
            // 
            // 陆地trackBar1
            // 
            this.陆地trackBar1.Location = new System.Drawing.Point(32, 344);
            this.陆地trackBar1.Maximum = 30;
            this.陆地trackBar1.Name = "陆地trackBar1";
            this.陆地trackBar1.Size = new System.Drawing.Size(194, 45);
            this.陆地trackBar1.TabIndex = 17;
            this.陆地trackBar1.Value = 5;
            this.陆地trackBar1.Scroll += new System.EventHandler(this.陆地trackBar1_Scroll);
            // 
            // Area1_ID
            // 
            this.Area1_ID.Location = new System.Drawing.Point(393, 99);
            this.Area1_ID.Name = "Area1_ID";
            this.Area1_ID.Size = new System.Drawing.Size(36, 21);
            this.Area1_ID.TabIndex = 20;
            this.Area1_ID.Text = "102";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(334, 102);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 19;
            this.label2.Text = "Area1_ID:";
            // 
            // Area2_ID
            // 
            this.Area2_ID.Location = new System.Drawing.Point(521, 99);
            this.Area2_ID.Name = "Area2_ID";
            this.Area2_ID.Size = new System.Drawing.Size(36, 21);
            this.Area2_ID.TabIndex = 22;
            this.Area2_ID.Text = "103";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(462, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 21;
            this.label3.Text = "Area2_ID:";
            // 
            // Area_1_2_ID
            // 
            this.Area_1_2_ID.Location = new System.Drawing.Point(676, 99);
            this.Area_1_2_ID.Name = "Area_1_2_ID";
            this.Area_1_2_ID.Size = new System.Drawing.Size(36, 21);
            this.Area_1_2_ID.TabIndex = 24;
            this.Area_1_2_ID.Text = "100";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(593, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 23;
            this.label4.Text = "Area_1-2_ID:";
            // 
            // checkBox_terrain
            // 
            this.checkBox_terrain.AutoSize = true;
            this.checkBox_terrain.Checked = true;
            this.checkBox_terrain.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_terrain.Location = new System.Drawing.Point(201, 96);
            this.checkBox_terrain.Name = "checkBox_terrain";
            this.checkBox_terrain.Size = new System.Drawing.Size(48, 16);
            this.checkBox_terrain.TabIndex = 25;
            this.checkBox_terrain.Text = "地型";
            this.checkBox_terrain.UseVisualStyleBackColor = true;
            // 
            // btn_resource
            // 
            this.btn_resource.Location = new System.Drawing.Point(353, 264);
            this.btn_resource.Name = "btn_resource";
            this.btn_resource.Size = new System.Drawing.Size(103, 23);
            this.btn_resource.TabIndex = 26;
            this.btn_resource.Text = "随机土地资源";
            this.btn_resource.UseVisualStyleBackColor = true;
            this.btn_resource.Click += new System.EventHandler(this.btn_resource_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 562);
            this.Controls.Add(this.btn_resource);
            this.Controls.Add(this.checkBox_terrain);
            this.Controls.Add(this.Area_1_2_ID);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Area2_ID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Area1_ID);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.陆地装饰label2);
            this.Controls.Add(this.陆地trackBar1);
            this.Controls.Add(this.湖泊装饰label5);
            this.Controls.Add(this.湖泊装饰trackBar4);
            this.Controls.Add(this.树林label4);
            this.Controls.Add(this.树林trackBar3);
            this.Controls.Add(this.山体label3);
            this.Controls.Add(this.山体trackBar2);
            this.Controls.Add(this.湖泊label2);
            this.Controls.Add(this.湖泊trackBar1);
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
            ((System.ComponentModel.ISupportInitialize)(this.湖泊trackBar1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.山体trackBar2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.树林trackBar3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.湖泊装饰trackBar4)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.陆地trackBar1)).EndInit();
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
        private System.Windows.Forms.TrackBar 湖泊trackBar1;
        private System.Windows.Forms.Label 湖泊label2;
        private System.Windows.Forms.Label 山体label3;
        private System.Windows.Forms.TrackBar 山体trackBar2;
        private System.Windows.Forms.Label 树林label4;
        private System.Windows.Forms.TrackBar 树林trackBar3;
        private System.Windows.Forms.Label 湖泊装饰label5;
        private System.Windows.Forms.TrackBar 湖泊装饰trackBar4;
        private System.Windows.Forms.Label 陆地装饰label2;
        private System.Windows.Forms.TrackBar 陆地trackBar1;
        private System.Windows.Forms.TextBox Area1_ID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Area2_ID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Area_1_2_ID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox checkBox_terrain;
        private System.Windows.Forms.Button btn_resource;
    }
}

