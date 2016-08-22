using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace AdaptiveControl
{
    class AdaptivePid : ControlAlgorithm
    {

        public AdaptivePid(double period, double setValue,
            System.Windows.Forms.DataVisualization.Charting.Chart paraChart,
            System.Windows.Forms.DataVisualization.Charting.Chart controlChart,
            System.Windows.Forms.DataGridView paraGridView,
            System.Windows.Forms.DataGridView dataGridView)
        {

          //  na = 2;//A阶次
            //nb = 2;//B阶次
            int i, j;
            lamda = 0.99;
            int L=100;
            for (i = 0; i < 3; i++)
            {
                arrayA[i] = 0;
                arrayB[i] = 0;
                yk[i] = 0;
            }
            for (i = 0; i < 6; i++)
            {
                uk[i] = 0;
                tte[i] = 0;
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
                tte[i] = 0.001;
                tte1[i] = 0.001;
            }
            for (i = 0; i < L; i++)
            {
                u[i] = 0;
            }
            //st = 1;
            xie = 0;
            for (i = 0; i < 5; i++)
            {
                arrPhi[i] = 0;
                arrPhi1[i] = 0;
            }
            d = 2;
            Am[0] = 1;
            Am[1] = -0.0000373;
            Am[2] = 1.365 * 1e-9;

            ///
            ///
            //
          

            base.paraChart = paraChart;
            dataChart = controlChart;
            paraTable = paraGridView;
            dataTable = dataGridView;

            
            ///
            r = setValue;
            T = period;
            bgTime = DateTime.Now;
            spantime = 0;
            error = 0;
            overshoot = 0;


            ///
            initChart();
            initTable();
        }


        public double getControlValue()
        {
            int i, j, k;
            //变量参数初始化
            int na = 2;
            int nb = 1;
            //int nc = 1;
           // int nam = 2;
            int nm = na + nb + 1;
            int nf1 = nb + d + 2 - (na + 1) + 1;
            //int ng = 2;
            //更新输出y
           // double y = base.y;
            //更新控制器参数
            arrPhi[0] = -yk[0];
            arrPhi[1] = -yk[1];
            arrPhi[2] = uk[1];
            arrPhi[3] = uk[2];
            double[] arrTe = new double[4];
            //递推计算数组K
            for (i = 0; i < 4; i++)
            {
                arrTe[i] = 0;
                for (j = 0; j < 4; j++)
                {
                    arrTe[i] += 1.0 * arrayP[i][j] * arrPhi[j];
                }
                arrayK[i] = arrTe[i];
            }
            double ITP = 0;
            for (i = 0; i < 4; i++)
            {
                ITP += arrTe[i] * arrPhi[i];
            }
            ITP += lamda;
            for (i = 0; i < 4; i++)
            {
                arrayK[i] /= ITP;
            }
            //递推估计控制器参数
            ITP = 0;
            for (i = 0; i < 4; i++)
            {
                ITP += arrPhi[i] * tte1[i];
            }
            for (i = 0; i < 4; i++)
            {
                tte[i] = tte1[i] + arrayK[i] * (y - ITP);
            }
            //递推计算数组P
            double[][] arrayTT = new double[4][];
            for (i = 0; i < 4; i++)
            {
                arrayTT[i] = new double[5];
                for (j = 0; j < 4; j++)
                {
                    if (i == j)
                        arrayTT[i][j] = 1 - arrayK[i] * arrPhi[j];
                    else
                        arrayTT[i][j] = -arrayK[i] * arrPhi[j];
                }
            }
            double[][] arrTPP = new double[5][];
            for (i = 0; i < 4; i++)
            {
                arrTPP[i] = new double[4];
                for (j = 0; j < 4; j++)
                {
                    double temp = 0;
                    for (k = 0; k < 4; k++)
                    {
                        temp += arrayTT[i][k] * arrayP[k][j];
                    }
                    arrTPP[i][j] = temp;
                }
            }
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    arrayP[i][j] = arrTPP[i][j] / lamda;
                }
            }
            //控制器参数计算
            xie = y;
            for (i = 0; i < 4; i++)
            {
                xie -= arrPhi[i] * tte[i];
            }
            double[] arrAA = new double[3], arrBB = new double[3], arrCC = new double[2];
            arrAA[0] = 1; arrCC[0] = 1; arrAA[1] = tte[0]; arrAA[2] = tte[1];
            arrBB[0] = tte[2]; arrBB[1] = tte[3]; arrCC[1] = tte[4];

            //卷积计算
            double[] arrConAA = new double[5];
            double[] arrConTT = new double[6] { 1, -1, 0, 0, 0, 0 };
            arrConAA[0] = arrAA[0] * arrConTT[0]; arrConAA[1] = arrAA[0] * arrConTT[1] + arrAA[1] * arrConTT[0]; arrConAA[2] = arrAA[0] * arrConTT[2] + arrAA[1] * arrConTT[1] + arrAA[2] * arrConTT[0];
            arrConAA[3] = arrAA[0] * arrConTT[3] + arrAA[1] * arrConTT[2] + arrAA[2] * arrConTT[1]; arrConAA[4] = arrAA[0] * arrConTT[4] + arrAA[1] * arrConTT[3] + arrAA[2] * arrConTT[2];

            double[] dB = new double[4] { 0, 0, arrBB[0], arrBB[1] };
            //int nt = 3;
            double[] arrTT1 = new double[3];
            arrTT1[0] = Am[0]; arrTT1[1] = Am[1]; arrTT1[2] = Am[2];
            double[] arrTTT = new double[6] { arrTT1[0], arrTT1[1], arrTT1[2], 0, 0, 0 };
            double[,] arrMatrix = new double[6, 6];
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    arrMatrix[i + j, j] = arrConAA[i];
                }
            }
            for (i = 0; i < 4; i++)
            {
                for (j = 0; j < 3; j++)
                {
                    arrMatrix[i + j, j + 3] = dB[i];
                }
            }
            double[,] arrReverse = ReverseMatrix(arrMatrix, 6);

            double[] arrayTempL = new double[6];
            for (i = 0; i < 6; i++)
            {
                arrayTempL[i] = 0;
                for (j = 0; j < 6; j++)
                {
                    arrayTempL[i] += arrReverse[i, j] * arrTTT[j];
                }
            }
            double[] F1 = new double[3] { arrayTempL[0], arrayTempL[1], arrayTempL[2] };
            double[] G = new double[3] { arrayTempL[3], arrayTempL[4], arrayTempL[5] };

            double[] F = new double[4];
            F[0] = F1[0] * arrConTT[0];
            F[1] = F1[0] * arrConTT[1] + F1[1] * arrConTT[0];
            F[2] = F1[0] * arrConTT[2] + F1[1] * arrConTT[1] + F1[2] * arrConTT[0];
            F[3] = F1[0] * arrConTT[3] + F1[1] * arrConTT[2] + F1[2] * arrConTT[1];
            double R = G[0] + G[1] + G[2];
            //计算控制量
            double uu = (-F[1] * uk[0] - F[2] * uk[1] - F[3] * uk[2] + R * base.r - G[0] * base.y - G[1] * yk[0] - G[2] * yk[1]) / F[0];
            //对控制量进行限幅
            if (uu > 100)
                uu = 100;
            if (uu < 0)
                uu = 0;
            //更新参数
            for (i = 0; i < 4; i++)
            {
                tte1[i] = tte[i];
            }
            uk[3] = uk[2]; uk[2] = uk[1]; uk[1] = uk[0]; uk[0] = uu;
            yk[2] = yk[1]; yk[1] = yk[0]; yk[0] = y;
            return uu;
        }

        public static double MatrixValue(double[,] MatrixList, int Level)  //求得|A| 如果为0 说明不可逆
        {
            //计算行列式的方法
            //   a1 a2 a3
            //  b1 b2 b3
            //  c1 c2 c3
            // 结果为 a1·b2·c3+b1·c2·a3+c1·a2·b3-a3·b2·c1-b3·c2·a1-c3·a2·b1(注意对角线就容易记住了）
            double[,] dMatrix = new double[Level, Level];   //定义二维数组，行列数相同
            for (int i = 0; i < Level; i++)
                for (int j = 0; j < Level; j++)
                    dMatrix[i, j] = MatrixList[i, j];     //将参数的值，付给定义的数组

            double c, x;
            int k = 1;
            for (int i = 0, j = 0; i < Level && j < Level; i++, j++)
            {
                if (dMatrix[i, j] == 0)   //判断对角线上的数据是否为0
                {
                    int m = i;
                    for (; dMatrix[m, j] == 0; m++) ;  //如果对角线上数据为0，从该数据开始依次往后判断是否为0
                    if (m == Level)                      //当该行从对角线开始数据都为0 的时候 返回0
                        return 0;
                    else
                    {
                        // Row change between i-row and m-row
                        for (int n = j; n < Level; n++)
                        {
                            c = dMatrix[i, n];
                            dMatrix[i, n] = dMatrix[m, n];
                            dMatrix[m, n] = c;
                        }
                        // Change value pre-value
                        k *= (-1);
                    }
                }
                // Set 0 to the current column in the rows after current row
                for (int s = Level - 1; s > i; s--)
                {
                    x = dMatrix[s, j];
                    for (int t = j; t < Level; t++)
                        dMatrix[s, t] -= dMatrix[i, t] * (x / dMatrix[i, j]);
                }
            }
            double sn = 1;
            for (int i = 0; i < Level; i++)
            {
                if (dMatrix[i, i] != 0)
                    sn *= dMatrix[i, i];
                else
                    return 0;
            }
            return k * sn;
        }


        public static double[,] ReverseMatrix(double[,] dMatrix, int Level)
        {
            double dMatrixValue = MatrixValue(dMatrix, Level);
            if (dMatrixValue == 0) return null;       //A为该矩阵 若|A| =0 则该矩阵不可逆 返回空

            double[,] dReverseMatrix = new double[Level, 2 * Level];
            double x, c;
            // Init Reverse matrix
            for (int i = 0; i < Level; i++)     //创建一个矩阵（A|I） 以对其进行初等变换 求得其矩阵的逆
            {
                for (int j = 0; j < 2 * Level; j++)
                {
                    if (j < Level)
                        dReverseMatrix[i, j] = dMatrix[i, j];   //该 （A|I）矩阵前 Level列为矩阵A  后面为数据全部为0
                    else
                        dReverseMatrix[i, j] = 0;
                }
                dReverseMatrix[i, Level + i] = 1;
                //将Level+1行开始的Level阶 矩阵装换为单位矩阵 （起初的时候该矩阵都为0 现在在把对角线位置装换为1 ）
                //参考http://www.shuxuecheng.com/gaosuzk/content/lljx/wzja/12/12-6.htm
            }

            for (int i = 0, j = 0; i < Level && j < Level; i++, j++)
            {
                if (dReverseMatrix[i, j] == 0)   //判断一行对角线 是否为0
                {
                    int m = i;
                    for (; dMatrix[m, j] == 0; m++) ;
                    if (m == Level)
                        return null;  //某行对角线为0的时候 判断该行该数据所在的列在该数据后 是否为0 都为0 的话不可逆 返回空值
                    else
                    {
                        // Add i-row with m-row
                        for (int n = j; n < 2 * Level; n++)   //如果对角线为0 则该i行加上m行 m行为（初等变换要求对角线为1，0-->1先加上某行，下面在变1）
                            dReverseMatrix[i, n] += dReverseMatrix[m, n];
                    }
                }
                //  此时数据： 第二行加上第一行为第一行的数据
                //    1   1   3      1    1    0
                //    1   0   1      0    1    0
                //    4   2   1      0    0    1
                //
                // Format the i-row with "1" start
                x = dReverseMatrix[i, j];
                if (x != 1)                  //如果对角线元素不为1  执行以下
                {
                    for (int n = j; n < 2 * Level; n++)
                        if (dReverseMatrix[i, n] != 0)
                            dReverseMatrix[i, n] /= x;   //相除  使i行第一个数字为1
                }
                // Set 0 to the current column in the rows after current row
                for (int s = Level - 1; s > i; s--)         //该对角线数据为1 时，这一列其他数据 要转换为0
                {
                    x = dReverseMatrix[s, j];
                    // 第一次时
                    //    1      1   3      1    1    0
                    //    1      0   1      0    1    0
                    //   4(x)   2   1      0    0    1
                    //
                    for (int t = j; t < 2 * Level; t++)
                        dReverseMatrix[s, t] -= (dReverseMatrix[i, t] * x);
                    //第一个轮回   用第一行*4 减去第三行 为第三行的数据  依次类推
                    //     1      1   3      1    1    0
                    //    1      0   1      0    1    0
                    //    0(x)   -2  -11    -4   -4   1

                }
            }
            // Format the first matrix into unit-matrix
            for (int i = Level - 2; i >= 0; i--)
            //处理第一行二列的数据 思路如上 就是把除了对角线外的元素转换为0 
            {
                for (int j = i + 1; j < Level; j++)
                    if (dReverseMatrix[i, j] != 0)
                    {
                        c = dReverseMatrix[i, j];
                        for (int n = j; n < 2 * Level; n++)
                            dReverseMatrix[i, n] -= (c * dReverseMatrix[j, n]);
                    }
            }
            double[,] dReturn = new double[Level, Level];
            for (int i = 0; i < Level; i++)
                for (int j = 0; j < Level; j++)
                    dReturn[i, j] = dReverseMatrix[i, j + Level];
            //就是把Level阶的矩阵提取出来（减去原先为单位矩阵的部分）
            return dReturn;
        }


 /*       private void setDataChartAxisY(double data)
        {
            //SetTextCall
   
            double oldMax = dataChart.ChartAreas[0].AxisY.Maximum;
            double oldMin = dataChart.ChartAreas[0].AxisY.Minimum;
            if (oldMax <= data)
            {
                dataChart.ChartAreas[0].AxisY.Maximum = (Math.Round(data) + 10);
            }
            if (oldMin >= data)
            {
                dataChart.ChartAreas[0].AxisY.Minimum = (Math.Round(data) - 10);
            }

        }


        private void setParaChartAxisY(double para)
        {
            double oldMax = paraChart.ChartAreas[0].AxisY.Maximum;
            double oldMin = paraChart.ChartAreas[0].AxisY.Minimum;
            if (oldMax <= para)
            {
                paraChart.ChartAreas[0].AxisY.Maximum = Math.Round(para) + 10;
            }
            if (oldMin >= para)
            {
                paraChart.ChartAreas[0].AxisY.Minimum = Math.Round(para) - 10;
            }


        }

    */
        /// <summary>
        /// interface
        /// </summary>
        /// <returns></returns>
        public override double controller()
        {
            outputU = ControlU = getControlValue();
            if (outputU > 100)
            {
                outputU = 100;
            }
            if (outputU < 0)
            {
                outputU = 0;
            }

            return outputU;
        }

        public override void drawData()
        {
            //
            // draw setValue
            //
            setDataChartAxisY(Math.Round(r, 4));
            dataChart.Series[0].Points.Add(new DataPoint(Math.Round(spantime, 4),Math.Round(r, 4)));

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
            setParaChartAxisY(Math.Round(tte[0], 4));
            paraChart.Series[0].Points.Add(new DataPoint(Math.Round(spantime, 4),Math.Round(tte[0], 4)));// draw a0

            setParaChartAxisY(Math.Round(tte[1], 4));
            paraChart.Series[1].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(tte[1], 4)));// draw a1

            setParaChartAxisY(Math.Round(tte[2], 4));
            paraChart.Series[2].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(tte[2], 4)));// draw b0

            setParaChartAxisY(Math.Round(tte[3], 4));
            paraChart.Series[3].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(tte[3], 4)));// draw b1
        }

        public override void initChart()
        {
            paraChart.Series.Clear();
            dataChart.Series.Clear();

            //
            // add the paraChart Series
            //
            for (int i = 0; i < 4; i++)// add the paraChart Series
            {
                paraChart.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series());// add series
                paraChart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;// set the series type,pie or bar and etc
                paraChart.Series[i].BorderWidth = 2;// set the line width
            }

            

            //设置图例
            paraChart.Series[0].LegendText = "a0";
            paraChart.Series[1].LegendText = "a1";
            paraChart.Series[2].LegendText = "b1";
            paraChart.Series[3].LegendText = "b2";
          //  paraChart.Legends[0].Position.Height = 20;

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


            paraChart.ChartAreas[0].AxisX.Maximum = 60;
            paraChart.ChartAreas[0].AxisX.Minimum = 0;

            paraChart.ChartAreas[0].AxisY.Maximum = 5;
            paraChart.ChartAreas[0].AxisY.Minimum = -5;

            dataChart.ChartAreas[0].AxisX.Maximum = 60;
            dataChart.ChartAreas[0].AxisX.Minimum = 0;

            dataChart.ChartAreas[0].AxisY.Maximum = 100;
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


        
            for (int i = 0; i < 4; i++)
            {
                paraTable.Columns.Add(new DataGridViewTextBoxColumn());    
                paraTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }
            paraTable.Columns[0].HeaderText = "a0";
            paraTable.Columns[1].HeaderText = "a1";
            paraTable.Columns[2].HeaderText = "b1";
            paraTable.Columns[3].HeaderText = "b2";

            
         

            for (int i = 0; i < 5; i++)// initialize the dataTable
            {
                base.dataTable.Columns.Add(new DataGridViewTextBoxColumn());
                base.dataTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            dataTable.Columns[0].HeaderText = "对象延时/T";
            base.dataTable.Columns[1].HeaderText = "控制量/u";
            base.dataTable.Columns[2].HeaderText = "输出量/y";
            base.dataTable.Columns[3].HeaderText = "误差/e";
            dataTable.Columns[4].HeaderText = "超调量/σ";
        }

        public override void showData()
        {
            dataTable.Rows[0].Cells[0].Value = Math.Round(T, 4);// show control period
            dataTable.Rows[0].Cells[1].Value = Math.Round(ControlU, 4);// show ControlU
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
            paraTable.Rows[0].Cells[0].Value = Math.Round(tte[0], 4);// show a0
            paraTable.Rows[0].Cells[1].Value = Math.Round(tte[1], 4);// show a1
            paraTable.Rows[0].Cells[2].Value = Math.Round(tte[2], 4);// show b0
            paraTable.Rows[0].Cells[3].Value = Math.Round(tte[3], 4);// show b1
        }

       

        /// <summary>
        /// member 
        /// </summary>
        /// 
      //  private int i;
        private double[] arrayA = new double[3];
        private double[] arrayB = new double[3];
        private double[] arrayC = new double[3];
        //int d;//对象参数
    //    private int na;//A阶次
      //  private int nb;//B阶次
        private double[] uk = new double[6];
        private double[] yk = new double[3];
        private double[] tta = new double[7];
        private double[] tte1 = new double[7];
        private double[] tte = new double[7];
        private double[][] arrayP = new double[7][];
        private double[] arrPhi = new double[7];
        private double[] arrPhi1 = new double[7];
        private double[] arrayK = new double[7];
        private double lamda;
        private double xie;
      //  private int st;
        private double[] Am = new double[4];
        private double[] u = new double[400];
       // private int L;
        private int d;
       // private double A0;
        private double[] AA = new double[3];
      //  private int naa;
       // private int nfg;
        private double[] ufk = new double[4];
        private double[] yfk = new double[4];
        private double ControlU; // the control value the algorithm
        private double outputU;
        
    }
}
