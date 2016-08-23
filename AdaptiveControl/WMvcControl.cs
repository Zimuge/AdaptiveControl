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
    class WMvcControl:ControlAlgorithm
    {
        private double[] phie = new double[10];
        private double[] uk = new double[10];
        private double[] yk = new double[10];
        private double[] tte1 = new double[10];
        private double[] tte2 = new double[10];
        private double[][] arrListP = new double[10][];
        private double[] arrListK = new double[10];
        //参数
        private double[][] tte = new double[500][];
        private double u, lamda, Pw, R, Q, yek;
        private int sp;// ng, np, nr,,  nq,


        public WMvcControl(double period, double setValue,
            System.Windows.Forms.DataVisualization.Charting.Chart paraChart,
            System.Windows.Forms.DataVisualization.Charting.Chart controlChart,
            System.Windows.Forms.DataGridView paraGridView,
            System.Windows.Forms.DataGridView dataGridView)
        {
            int indexI, indexJ;
            lamda = 0.98;
            sp = 2;
            for (indexI = 0; indexI < 10; indexI++)
            {
                yk[indexI] = 0;
                uk[indexI] = 0;
            }
            for (indexI = 0; indexI < 10; indexI++)
            {
                tte1[indexI] = 0.001;
                tte2[indexI] = 0.001;
            }
            for (indexI = 0; indexI < 500; indexI++)
            {
                tte[indexI] = new double[6];
            }
            for (indexI = 0; indexI < 6; indexI++)
            {
                tte[0][indexI] = 0;
                tte[1][indexI] = 0;
            }
            for (indexI = 0; indexI < 10; indexI++)
            {
                arrListP[indexI] = new double[10];
                for (indexJ = 0; indexJ < 10; indexJ++)
                {
                    if (indexI == indexJ)
                        arrListP[indexI][indexJ] = 10e6;
                    else
                        arrListP[indexI][indexJ] = 0;
                }
            }

            for (indexI = 0; indexI < 10; indexI++)
            {
                phie[indexI] = 0;
            }
            Pw = 1.2; R = 1.2; Q = 0.05;  //ng = 1;nf = 2;np = 0; nr = 0;  nq = 0;

            yek = 0;


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

            initParaChart();
            initDataChart();


            initParaTable();
            initDataTable();


        }

        public override double getControlValue()
        {
           //double y = y_real
            //更新控制器参数
            phie[0] = yk[1]; phie[1] = yk[2]; phie[2] = uk[1];
            phie[3] = uk[2]; phie[4] = uk[3]; phie[6] = -yek;

            //递推计算数组K
            double[] arrTT = new double[6];
            int indexI, indexJ, indexK;
            for (indexI = 0; indexI < 6; indexI++)
            {
                arrTT[indexI] = 0;
                for (indexJ = 0; indexJ < 6; indexJ++)
                {
                    arrTT[indexI] += 1.0 * arrListP[indexI][indexJ] * phie[indexJ];
                }
                arrListK[indexI] = arrTT[indexI];
            }
            double iTemp = 0;
            for (indexI = 0; indexI < 6; indexI++)
            {
                iTemp += arrTT[indexI] * phie[indexI];
            }
            iTemp += lamda;
            for (indexI = 0; indexI < 6; indexI++)
            {
                arrListK[indexI] /= iTemp;
            }
            //递推估计控制器参数
            iTemp = 0;
            for (indexI = 0; indexI < 6; indexI++)
            {
                iTemp += phie[indexI] * tte[sp - 1][indexI];
            }
            for (indexI = 0; indexI < 6; indexI++)
            {
                tte[sp][indexI] = tte[sp - 1][indexI] + arrListK[indexI] * (y - iTemp);
            }
            //递推计算数组P
            double[][] arrayTT = new double[6][];
            for (indexI = 0; indexI < 6; indexI++)
            {
                arrayTT[indexI] = new double[6];
                for (indexJ = 0; indexJ < 6; indexJ++)
                {
                    if (indexI == indexJ)
                        arrayTT[indexI][indexJ] = 1 - arrListK[indexI] * phie[indexJ];
                    else
                        arrayTT[indexI][indexJ] = -arrListK[indexI] * phie[indexJ];
                }
            }
            //控制器参数计算
            double[][] arrTTP = new double[6][];
            for (indexI = 0; indexI < 6; indexI++)
            {
                arrTTP[indexI] = new double[6];
                for (indexJ = 0; indexJ < 6; indexJ++)
                {
                    double temp = 0;
                    for (indexK = 0; indexK < 6; indexK++)
                    {
                        temp += arrayTT[indexI][indexK] * arrListP[indexK][indexJ];
                    }
                    arrTTP[indexI][indexJ] = temp;
                }
            }
            for (indexI = 0; indexI < 6; indexI++)
            {
                for (indexJ = 0; indexJ < 6; indexJ++)
                {
                    arrListP[indexI][indexJ] = arrTTP[indexI][indexJ] * 1.0 / lamda;
                }
            }
            double ye = 0;
            for (indexI = 0; indexI < 6; indexI++)
            {
                ye += phie[indexI] * tte[sp - 2][indexI];
            }
            double[] arrGE = new double[2];
            arrGE[0] = tte[sp][0]; arrGE[1] = tte[sp][1];
            double[] arrFE = new double[3];
            arrFE[0] = tte[sp][2]; arrFE[1] = tte[sp][3]; arrFE[2] = tte[sp][4];
            double[] arrCE = new double[2];
            arrCE[0] = 1; arrCE[1] = tte[sp][5];
            if (arrFE[0] < 0.1)
                arrFE[0] = 0.1;
            double[] arrayCQ = new double[2];
            double[] arrayFP = new double[3];
            double[] arrayCR = new double[2];
            double[] GP = new double[2];
            arrayCQ[0] = arrCE[0] * Q; arrayCQ[1] = arrCE[1] * Q;
            arrayFP[0] = arrFE[0] * Pw; arrayFP[1] = arrFE[1] * Pw; arrayFP[2] = arrFE[2] * Pw;
            arrayCR[0] = arrCE[0] * R; arrayCR[1] = arrCE[1] * R; GP[0] = arrGE[0] * Pw; GP[1] = arrGE[1] * Pw;
            //计算控制量
            u = (-Q * arrayCQ[1] * uk[0] / arrFE[0] - arrayFP[1] * uk[0] - arrayFP[2] * uk[1] + arrayCR[0] * r + arrayCR[1] * r - GP[0] * y - GP[1] * yk[0]) / (Q * Q / arrFE[0] + arrFE[0]);
            //对控制量进行限幅
            if (u > 100)
                u = 100;
            else if (u < 0)
                u = 0;
            //更新参数
            phie[0] = -y; phie[1] = -yk[0]; phie[2] = u;
            phie[3] = uk[0]; phie[4] = uk[1];
            for (indexI = 3; indexI > 0; indexI--)
            {
                uk[indexI] = uk[indexI - 1];
            }
            uk[0] = u;
            yk[2] = yk[1];
            yk[1] = yk[0];
            yk[0] = y;
            sp++;
            return u;
        }



        public override void initParaChart()
        {
            //
            // clear the chart series
            //
            paraChart.Series.Clear();


            //
            // add the paraChart Series
            //
            for (int i = 0; i < 6; i++) // add the paraChart Series
            {
                paraChart.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series()); // add series
                paraChart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    // set the series type,pie or bar and etc
                paraChart.Series[i].BorderWidth = 2; // set the line width
            }

            paraChart.Series[0].LegendText = "g0";
            paraChart.Series[1].LegendText = "g1";
            paraChart.Series[2].LegendText = "f0";
            paraChart.Series[3].LegendText = "f1";
            paraChart.Series[4].LegendText = "f2";
            paraChart.Series[5].LegendText = "c1";

            paraChart.ChartAreas[0].AxisX.Maximum = 60; //para_x
            paraChart.ChartAreas[0].AxisX.Minimum = 0;

            paraChart.ChartAreas[0].AxisY.Maximum = 2; //para_y
            paraChart.ChartAreas[0].AxisY.Minimum = -2;

            paraChart.Series[0].Points.Add(0, 0);
        }


       
        public override void drawParameters()
        {
            setParaChartAxisY(Math.Round(tte[sp - 1][0], 4));
            paraChart.Series[0].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(tte[sp - 1][0], 4)));// draw g0

            setParaChartAxisY(Math.Round(tte[sp - 1][1], 4));
            paraChart.Series[1].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(tte[sp - 1][1], 4)));// draw g1

            setParaChartAxisY(Math.Round(tte[sp - 1][2], 4));
            paraChart.Series[2].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(tte[sp - 1][2], 4)));// draw f0

            setParaChartAxisY(Math.Round(tte[sp - 1][3], 4));
            paraChart.Series[3].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(tte[sp - 1][3], 4)));// draw f1

            setParaChartAxisY(Math.Round(tte[sp - 1][4], 4));
            paraChart.Series[4].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(tte[sp - 1][4], 4)));// draw f2

            setParaChartAxisY(Math.Round(tte[sp - 1][5], 4));
            paraChart.Series[5].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(tte[sp - 1][5], 4)));// draw c1

        }

        public override void showParameters()
        {
            paraTable.Rows[0].Cells[0].Value = Math.Round(tte[sp - 1][0], 4);// show g0
            paraTable.Rows[0].Cells[1].Value = Math.Round(tte[sp - 1][1], 4);// show g1
            paraTable.Rows[0].Cells[2].Value = Math.Round(tte[sp - 1][2], 4);// show f0
            paraTable.Rows[0].Cells[3].Value = Math.Round(tte[sp - 1][3], 4);// show f1
            paraTable.Rows[0].Cells[4].Value = Math.Round(tte[sp - 1][4], 4);// show f2
            paraTable.Rows[0].Cells[5].Value = Math.Round(tte[sp - 1][5], 4);// show c1
        }

        public override void initParaTable()
        {
            paraTable.Columns.Clear();
            paraTable.Rows.Clear();



            for (int i = 0; i < 6; i++) // initialize the paraTable
            {
                base.paraTable.Columns.Add(new DataGridViewTextBoxColumn());
                base.paraTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            paraTable.Columns[0].HeaderText = "f0";
            paraTable.Columns[1].HeaderText = "g1";
            paraTable.Columns[2].HeaderText = "f0";
            paraTable.Columns[3].HeaderText = "f1";
            paraTable.Columns[4].HeaderText = "f2";
            paraTable.Columns[5].HeaderText = "c1";
        }
    }
}
