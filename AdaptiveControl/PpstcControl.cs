using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;


namespace AdaptiveControl
{
    class PpstcControl:ControlAlgorithm
    {
        private double[] arrayA = new double[3];
        private double[] arrayB = new double[3];
        private double[] arrayC = new double[3];
     //   private int na;
      //  private int nb;
        private double[] arrUK = new double[6];
        private double[] yk = new double[3];
        private double[] arrTTA = new double[7];
        private double[] arrTTAE1 = new double[7];
        private double[] arrTAE = new double[7];
        private double[][] arrayP = new double[7][];
        private double[] arrPHI = new double[7];
        private double[] arrPHI1 = new double[7];
        private double[] arrayK = new double[7];
        private double lamda;
       // private double xie;
        private double[] xiek = new double[2];
        private double[] Am = new double[4];
        private double[] u = new double[400];
       // private int d;
        private double A0;
        private double[] arrAA = new double[3];
        //private int naa;
       // private int nfg;
        private double[] arrUF = new double[4];
        private double[] arrYF = new double[4];

        private double controlU;
        private double outputU;

        public PpstcControl(double period, double setValue,
            System.Windows.Forms.DataVisualization.Charting.Chart paraChart,
            System.Windows.Forms.DataVisualization.Charting.Chart controlChart,
            System.Windows.Forms.DataGridView paraGridView,
            System.Windows.Forms.DataGridView dataGridView)
        {
            //na = 2;
            //nb = 2;
            int i, j;
            lamda = 0.98;
            //int L=100;
            for (i = 0; i < 3; i++)
            {
                arrayA[i] = 0;
                arrayB[i] = 0;
                yk[i] = 0;
            }
            for (i = 0; i < 6; i++)
            {
                arrUK[i] = 0;
                arrTAE[i] = 0;
            }
            for (i = 0; i < 6; i++)
            {
                arrayP[i] = new double[6];
                for (j = 0; j < 6; j++)
                {
                    if (i == j)
                        arrayP[i][j] = 10e5;
                    else
                        arrayP[i][j] = 0;
                }
            }
            for (i = 0; i < 6; i++)
            {
                arrTAE[i] = 0.001;
                arrTTAE1[i] = 0.001;
            }
            //xie = 0;
            for (i = 0; i < 5; i++)
            {
                arrPHI[i] = 0;
                arrPHI1[i] = 0;
            }
            //d = 2;

            Am[0] = 1;
            Am[1] = -0.0000373;
            Am[2] = 1.365 * 1e-9;

            A0 = 1;
            arrAA[0] = Am[0]; arrAA[1] = Am[1]; arrAA[2] = Am[2];
            //naa = 2;
           // nfg = 2;
            for (i = 0; i < 4; i++)
            {
                arrUF[i] = 0;
                arrYF[i] = 0;
            }


            base.paraChart = paraChart;
            dataChart = controlChart;
            paraTable = paraGridView;
            dataTable = dataGridView;

            base.T = period;
            base.r = setValue;

            bgTime = DateTime.Now;
            spantime = 0;
            error = 0;
            overshoot = 0;

            initChart();
            initTable();
        }
        public double getControlValue()
        {
            int i, j, k;
           // double y = yNow;
            arrUF[1] = -arrAA[1] * arrUF[2] - arrAA[2] * arrUF[3] + arrUK[1];
            arrYF[1] = -arrAA[1] * arrYF[2] - arrAA[2] * arrYF[3] + yk[1];

            //更新控制器参数
            arrPHI[0] = arrUF[1]; arrPHI[1] = arrUF[2]; arrPHI[2] = arrUF[3];
            arrPHI[3] = arrYF[1]; arrPHI[4] = arrYF[2];
            //递推计算数组K
            double[] arrTTP = new double[5];
            for (i = 0; i < 5; i++)
            {
                arrTTP[i] = 0;
                for (j = 0; j < 5; j++)
                {
                    arrTTP[i] += 1.0 * arrayP[i][j] * arrPHI[j];
                }
                arrayK[i] = arrTTP[i];

            }
            double iTemp = 0;
            for (i = 0; i < 5; i++)
            {
                iTemp += arrTTP[i] * arrPHI[i];
            }
            iTemp += lamda;
            for (i = 0; i < 5; i++)
            {
                arrayK[i] /= iTemp;
            }
            //递推估计控制器参数
            iTemp = 0;
            for (i = 0; i < 5; i++)
            {
                iTemp += arrPHI[i] * arrTTAE1[i];
            }
            for (i = 0; i < 5; i++)
            {
                arrTAE[i] = arrTTAE1[i] + arrayK[i] * (y - iTemp);
            }
            //递推计算数组P
            double[][] arrayTT = new double[5][];
            for (i = 0; i < 5; i++)
            {
                arrayTT[i] = new double[5];
                for (j = 0; j < 5; j++)
                {
                    if (i == j)
                        arrayTT[i][j] = 1 - arrayK[i] * arrPHI[j];
                    else
                        arrayTT[i][j] = -arrayK[i] * arrPHI[j];
                }
            }
            double[][] arrayTempP = new double[5][];
            for (i = 0; i < 5; i++)
            {
                arrayTempP[i] = new double[5];
                for (j = 0; j < 5; j++)
                {
                    double temp = 0;
                    for (k = 0; k < 5; k++)
                    {
                        temp += arrayTT[i][k] * arrayP[k][j];
                    }
                    arrayTempP[i][j] = temp;
                }
            }
            for (i = 0; i < 5; i++)
            {
                for (j = 0; j < 5; j++)
                {
                    arrayP[i][j] = arrayTempP[i][j] / lamda;
                }
            }
            //控制器参数计算
            double be0 = arrTAE[0];
            double[] arrTTAE = new double[5];
            for (i = 0; i < 5; i++)
            {
                arrTTAE[i] = arrTAE[i] / be0;
            }
            double[] arrFF = new double[3] { arrTTAE[0], arrTTAE[1], arrTTAE[2] };
            double[] arrGG = new double[2] { arrTTAE[3], arrTTAE[4] };
            double dBm = (Am[0] + Am[1] + Am[2]) / be0;
            double R = dBm * A0;
            //计算控制量
            double uu = (-arrFF[1] * arrUK[0] - arrFF[2] * arrUK[1] + R * r - arrGG[0] * y - arrGG[1] * yk[0]) / arrFF[0];
            //对控制量进行限幅
            if (uu > 96.5)
                uu = 96.5;
            if (uu < 0)
                uu = 0;
            //更新参数
            for (i = 0; i < 5; i++)
            {
                arrTTAE1[i] = arrTAE[i];
            }
            arrUK[3] = arrUK[2]; arrUK[2] = arrUK[1];
            arrUK[1] = arrUK[0]; arrUK[0] = uu;
            yk[2] = yk[1]; yk[1] = yk[0]; yk[0] = y;
            for (i = 3; i > 0; i--)
            {
                arrUF[i] = arrUF[i - 1];
                arrYF[i] = arrYF[i - 1];
            }
            return uu;
        }

        public override double controller()
        {
            controlU = getControlValue();
            outputU = controlU;
            if (outputU >= 100)
            {
                outputU = 100;
            }

            if (outputU <= 0)
            {
                outputU = 0;
            }
            return outputU;
        }

        public override void initChart()
        {
            //
            // clear the chart series
            //
            paraChart.Series.Clear();
            dataChart.Series.Clear();

            //
            // add the paraChart Series
            //
            for (int i = 0; i < 5; i++)// add the paraChart Series
            {
                paraChart.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series());// add series
                paraChart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;// set the series type,pie or bar and etc
                paraChart.Series[i].BorderWidth = 2;// set the line width
            }

            paraChart.Series[0].LegendText = "f0";
            paraChart.Series[1].LegendText = "f1";
            paraChart.Series[2].LegendText = "f2";
            paraChart.Series[3].LegendText = "g0";
            paraChart.Series[4].LegendText = "b1";

            //
            // add the dataChart Series
            //
            for (int i = 0; i < 3; i++)
            {
                dataChart.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series());// add series
                dataChart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;// set the series type,pie or bar and etc
                dataChart.Series[i].BorderWidth = 2;// set the line width
            }

            dataChart.Series[0].LegendText = "setValue/r";
            dataChart.Series[1].LegendText = "outputValue/y";
            dataChart.Series[2].LegendText = "controlValue/u";


            paraChart.ChartAreas[0].AxisX.Maximum = 60;//para_x
            paraChart.ChartAreas[0].AxisX.Minimum = 0;

            paraChart.ChartAreas[0].AxisY.Maximum = 5;//para_y
            paraChart.ChartAreas[0].AxisY.Minimum = -5;

            dataChart.ChartAreas[0].AxisX.Maximum = 60;//data_x
            dataChart.ChartAreas[0].AxisX.Minimum = 0;

            dataChart.ChartAreas[0].AxisY.Maximum = 100;//para_y
            dataChart.ChartAreas[0].AxisY.Minimum = 0;

            paraChart.Series[0].Points.Add(0, 0);
            dataChart.Series[0].Points.Add(0, 0);
        }

        public override void initTable()
        {
            paraTable.Columns.Clear();
            paraTable.Rows.Clear();

            dataTable.Columns.Clear();
            dataTable.Rows.Clear();

            for (int i = 0; i < 5; i++)// initialize the paraTable
            {
                base.paraTable.Columns.Add(new DataGridViewTextBoxColumn());
                base.paraTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            paraTable.Columns[0].HeaderText = "f0";
            paraTable.Columns[1].HeaderText = "f1";
            paraTable.Columns[2].HeaderText = "f2";
            paraTable.Columns[3].HeaderText = "g0";
            paraTable.Columns[4].HeaderText = "g1";

            for (int i = 0; i < 5; i++)// initialize the dataTable
            {
                base.dataTable.Columns.Add(new DataGridViewTextBoxColumn());
                base.dataTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dataTable.Columns[0].HeaderText = "控制周期/T";
            base.dataTable.Columns[1].HeaderText = "控制量/u";
            base.dataTable.Columns[2].HeaderText = "输出量/y";
            base.dataTable.Columns[3].HeaderText = "误差/e";
            dataTable.Columns[4].HeaderText = "超调量/σ";
        }

        public override void drawData()
        {
            setDataChartAxisY(Math.Round(r, 4));
            dataChart.Series[0].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(r, 4)));

            //
            // draw output value
            //
            setDataChartAxisY(Math.Round(y, 4));
            dataChart.Series[1].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(y, 4)));

            //
            // draw control value
            //
            setDataChartAxisY(Math.Round(outputU, 4));
            dataChart.Series[2].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(outputU, 4)));
        }

        public override void drawParameters()
        {
            setParaChartAxisY(Math.Round(arrTAE[0], 4));
            paraChart.Series[0].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(arrTAE[0], 4)));// draw f0

            setParaChartAxisY(Math.Round(arrTAE[1], 4));
            paraChart.Series[1].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(arrTAE[1], 4)));// draw f1

            setParaChartAxisY(Math.Round(arrTAE[2], 4));
            paraChart.Series[2].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(arrTAE[2], 4)));// draw f2

            setParaChartAxisY(Math.Round(arrTAE[3], 4));
            paraChart.Series[3].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(arrTAE[3], 4)));// draw g0

            setParaChartAxisY(Math.Round(arrTAE[4], 4));
            paraChart.Series[4].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(arrTAE[4], 4)));// draw g1
        }

        public override void showData()
        {
            dataTable.Rows[0].Cells[0].Value = Math.Round(T, 4);// show control period
            dataTable.Rows[0].Cells[1].Value = Math.Round(outputU, 4);// show ControlU
            dataTable.Rows[0].Cells[2].Value = Math.Round(y, 4);// show output value

            error = System.Math.Abs(r - y) / (r);// calculate error
            string strError = (Math.Round(error, 4) * 100).ToString() + "%";

            dataTable.Rows[0].Cells[3].Value = strError;// show error in percentage

            if ((y - r) > 0)// calculate the overshoot
            {
                if (overshoot < error)
                {
                    overshoot = error;
                }
            }

            string strOver = (Math.Round(overshoot, 4) * 100).ToString() + "%";

            dataTable.Rows[0].Cells[4].Value = strOver;// show overshoot
        }

        public override void showParameters()
        {
            paraTable.Rows[0].Cells[0].Value = Math.Round(arrTAE[0], 4);// show f0
            paraTable.Rows[0].Cells[1].Value = Math.Round(arrTAE[1], 4);// show f1
            paraTable.Rows[0].Cells[2].Value = Math.Round(arrTAE[2], 4);// show f2
            paraTable.Rows[0].Cells[3].Value = Math.Round(arrTAE[3], 4);// show g0
            paraTable.Rows[0].Cells[4].Value = Math.Round(arrTAE[4], 4);// show g1
        }
    }
}
