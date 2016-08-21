namespace AdaptiveControl
{
    partial class AdaptiveControl
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series4 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series5 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Series series6 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdaptiveControl));
            this.appName = new System.Windows.Forms.Label();
            this.myInfo = new System.Windows.Forms.Label();
            this.controlChartTitle = new System.Windows.Forms.Label();
            this.paraChartTitle = new System.Windows.Forms.Label();
            this.groupBoxConrolPanel = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.buttonStop = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.setBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxAlgor = new System.Windows.Forms.ComboBox();
            this.controlChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.paraChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxConrolPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.controlChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paraChart)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // appName
            // 
            this.appName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.appName.AutoSize = true;
            this.appName.Font = new System.Drawing.Font("新宋体", 42F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.appName.Location = new System.Drawing.Point(301, 9);
            this.appName.Name = "appName";
            this.appName.Size = new System.Drawing.Size(423, 56);
            this.appName.TabIndex = 2;
            this.appName.Text = "自适应仿真软件";
            // 
            // myInfo
            // 
            this.myInfo.AutoSize = true;
            this.myInfo.Cursor = System.Windows.Forms.Cursors.No;
            this.myInfo.ImageAlign = System.Drawing.ContentAlignment.BottomLeft;
            this.myInfo.Location = new System.Drawing.Point(912, 83);
            this.myInfo.Name = "myInfo";
            this.myInfo.Size = new System.Drawing.Size(128, 16);
            this.myInfo.TabIndex = 3;
            this.myInfo.Text = "陶鹏     151371";
            // 
            // controlChartTitle
            // 
            this.controlChartTitle.AutoSize = true;
            this.controlChartTitle.Location = new System.Drawing.Point(12, 65);
            this.controlChartTitle.Name = "controlChartTitle";
            this.controlChartTitle.Size = new System.Drawing.Size(72, 16);
            this.controlChartTitle.TabIndex = 6;
            this.controlChartTitle.Text = "控制曲线";
            // 
            // paraChartTitle
            // 
            this.paraChartTitle.AutoSize = true;
            this.paraChartTitle.Location = new System.Drawing.Point(12, 407);
            this.paraChartTitle.Name = "paraChartTitle";
            this.paraChartTitle.Size = new System.Drawing.Size(72, 16);
            this.paraChartTitle.TabIndex = 7;
            this.paraChartTitle.Text = "参数曲线";
            // 
            // groupBoxConrolPanel
            // 
            this.groupBoxConrolPanel.Controls.Add(this.label7);
            this.groupBoxConrolPanel.Controls.Add(this.buttonStop);
            this.groupBoxConrolPanel.Controls.Add(this.label6);
            this.groupBoxConrolPanel.Controls.Add(this.buttonStart);
            this.groupBoxConrolPanel.Controls.Add(this.setBox);
            this.groupBoxConrolPanel.Controls.Add(this.label5);
            this.groupBoxConrolPanel.Controls.Add(this.comboBoxAlgor);
            this.groupBoxConrolPanel.Location = new System.Drawing.Point(771, 102);
            this.groupBoxConrolPanel.Name = "groupBoxConrolPanel";
            this.groupBoxConrolPanel.Size = new System.Drawing.Size(269, 282);
            this.groupBoxConrolPanel.TabIndex = 10;
            this.groupBoxConrolPanel.TabStop = false;
            this.groupBoxConrolPanel.Text = "控制面板";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 196);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(56, 16);
            this.label7.TabIndex = 5;
            this.label7.Text = "操作：";
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(80, 239);
            this.buttonStop.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(77, 28);
            this.buttonStop.TabIndex = 0;
            this.buttonStop.Text = "暂停";
            this.buttonStop.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 16);
            this.label6.TabIndex = 4;
            this.label6.Text = "温度设定：";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(175, 239);
            this.buttonStart.Margin = new System.Windows.Forms.Padding(4);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(77, 28);
            this.buttonStart.TabIndex = 0;
            this.buttonStart.Text = "运行";
            this.buttonStart.UseVisualStyleBackColor = true;
            // 
            // setBox
            // 
            this.setBox.Location = new System.Drawing.Point(80, 68);
            this.setBox.Name = "setBox";
            this.setBox.Size = new System.Drawing.Size(172, 26);
            this.setBox.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 122);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(88, 16);
            this.label5.TabIndex = 2;
            this.label5.Text = "算法选择：";
            // 
            // comboBoxAlgor
            // 
            this.comboBoxAlgor.BackColor = System.Drawing.Color.White;
            this.comboBoxAlgor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxAlgor.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.comboBoxAlgor.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxAlgor.ForeColor = System.Drawing.SystemColors.ControlText;
            this.comboBoxAlgor.FormattingEnabled = true;
            this.comboBoxAlgor.Items.AddRange(new object[] {
            "PID控制",
            "模糊控制PID控制",
            "加权最小方差控制",
            "极点配置自校正控制",
            "自校正PID控制"});
            this.comboBoxAlgor.Location = new System.Drawing.Point(80, 161);
            this.comboBoxAlgor.Margin = new System.Windows.Forms.Padding(4);
            this.comboBoxAlgor.Name = "comboBoxAlgor";
            this.comboBoxAlgor.Size = new System.Drawing.Size(172, 24);
            this.comboBoxAlgor.TabIndex = 1;
            // 
            // controlChart
            // 
            chartArea1.Name = "ChartArea1";
            this.controlChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.controlChart.Legends.Add(legend1);
            this.controlChart.Location = new System.Drawing.Point(15, 84);
            this.controlChart.Name = "controlChart";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series1.Legend = "Legend1";
            series1.Name = "setValue/r";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series2.Legend = "Legend1";
            series2.Name = "controlValue/u";
            series3.ChartArea = "ChartArea1";
            series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series3.Legend = "Legend1";
            series3.Name = "outputValue/y";
            this.controlChart.Series.Add(series1);
            this.controlChart.Series.Add(series2);
            this.controlChart.Series.Add(series3);
            this.controlChart.Size = new System.Drawing.Size(696, 300);
            this.controlChart.TabIndex = 11;
            this.controlChart.Text = "chart1";
            // 
            // paraChart
            // 
            chartArea2.Name = "ChartArea1";
            this.paraChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend1";
            this.paraChart.Legends.Add(legend2);
            this.paraChart.Location = new System.Drawing.Point(12, 426);
            this.paraChart.Name = "paraChart";
            this.paraChart.Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Berry;
            series4.ChartArea = "ChartArea1";
            series4.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series4.Legend = "Legend1";
            series4.Name = "Kp";
            series5.ChartArea = "ChartArea1";
            series5.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series5.Legend = "Legend1";
            series5.Name = "Ki";
            series6.ChartArea = "ChartArea1";
            series6.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            series6.Legend = "Legend1";
            series6.Name = "Kd";
            this.paraChart.Series.Add(series4);
            this.paraChart.Series.Add(series5);
            this.paraChart.Series.Add(series6);
            this.paraChart.Size = new System.Drawing.Size(586, 300);
            this.paraChart.TabIndex = 12;
            this.paraChart.Text = "chart1";
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(604, 468);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(479, 93);
            this.dataGridView1.TabIndex = 13;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(604, 642);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(479, 84);
            this.dataGridView2.TabIndex = 14;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(601, 426);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "表1 控制数据";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(601, 602);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "表2 实时参数";
            // 
            // AdaptiveControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1095, 733);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.paraChart);
            this.Controls.Add(this.controlChart);
            this.Controls.Add(this.groupBoxConrolPanel);
            this.Controls.Add(this.paraChartTitle);
            this.Controls.Add(this.controlChartTitle);
            this.Controls.Add(this.myInfo);
            this.Controls.Add(this.appName);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "AdaptiveControl";
            this.RightToLeftLayout = true;
            this.Text = "陶鹏，学号：151371";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AdaptiveControl_FormClosing);
            this.groupBoxConrolPanel.ResumeLayout(false);
            this.groupBoxConrolPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.controlChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paraChart)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label appName;
        private System.Windows.Forms.Label myInfo;
        private System.Windows.Forms.Label controlChartTitle;
        private System.Windows.Forms.Label paraChartTitle;
        private System.Windows.Forms.GroupBox groupBoxConrolPanel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.TextBox setBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxAlgor;
        private System.Windows.Forms.DataVisualization.Charting.Chart controlChart;
        private System.Windows.Forms.DataVisualization.Charting.Chart paraChart;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

