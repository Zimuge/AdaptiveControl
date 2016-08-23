using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace AdaptiveControl
{
    class FuzzyPid:ControlAlgorithm
    {
        int[,] Fuzzy_Table = new int[,]{{0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,-6,-6,-6,-6,-6,-6,-5,-5,-3,-3,-2,-2,0},
        {0,-6,-6,-6,-6,-6,-6,-5,-5,-3,-3,-2,-2,0},
        {0,-5,-5,-5,-5,-5,-5,-3,-3,-2,-2,-1,-1,1},
        {0,-5,-5,-5,-5,-5,-5,-3,-3,-2,-2,-1,-1,1},
        {0,-4,-4,-4,-4,-3,-3,-2,-2,0,0,0,0,2},
        {0,-4,-4,-4,-4,-3,-3,-2,-2,0,0,0,0,2},
        {0,-3,-3,-2,-2,0,0,0,0,0,0,2,2,3},
        {0,-3,-3,-2,-2,0,0,0,0,0,0,2,2,3},
        {0,-2,-2,0,0,0,0,2,2,3,3,4,4,4},
        {0,-2,-2,0,0,0,0,2,2,3,3,4,4,4},
        {0,-1,-1,1,1,2,2,3,3,5,5,5,5,5},
        {0,-1,-1,1,1,2,2,3,3,5,5,5,5,5},
        {0,0,0,2,2,3,3,5,5,6,6,6,6,6}
        };
        //KP模糊控制规则表
        int[,] Fuzzy_TableKp = new int[,]{{0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0, -6, -6,-5, -5,  -4, -4, -3, -3, -2, -2, 0,  0,  1},
        {0, -6, -6, -5, -5, -4, -4, -3, -3, -2, -2, 0,  0,  1},
        {0, -5,-5,  -5, -5, -4, -4, -2, -2, -1, -1, 0,  0,  1},
        {0, -5, -5, -5, -5, -4, -4, -2, -2, -1, -1, 0,  0,  1},
        {0, -4, -4, -4, -4, -3, -3, -1, -1, 1,  1,  1,  1,  2},
        {0, -4, -4, -4, -4, -3, -3, -1, -1, 1,  1,  1,  1,  2},
        {0, -3, -3, -2, -2, -1, -1, -1, -1, 1,  1,  2,  2,  3},
        {0, -3, -3, -2, -2, -1, -1, -1, -1, 1,  1,  2,  2,  3},
        {0, -3, -3, -2, -2, -1, -1, 0,  0,  2,  2,  3,  3,  4},
        {0, -3, -3,-2,  -2, -1, -1, 0,  0,  2,  2,  3,  3,  4},
        {0, -1, -1, 1,  1,  1,  1,  2,  2,  3,  3,  4,  4,  4},
        {0, -1, -1, 1,  1,  1,  1,  2,  2,  3,  3,  4,  4,  4},
        {0, -1, -1, 1,  1,  1,  1,  2,  2,  4,  4,  4,  4,  5}};
        //Ki模糊控制规则表
        int[,] Fuzzy_TableKi = new int[,]{{0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0, 6,  6,  5,  5,  4,  4,  3,  3,  2,  2,  1,  1,  0 },
        {0, 6,  6,  5,  5,  4,  4,  3,  3,  2,  2,  1,  1,  0 },
        {0, 5,  5,  4,  4,  4,  4,  2,  2,  1,  1,  0,  0,  -1},
        {0, 5,  5,  4,  4,  4,  4,  2,  2,  1,  1,  0,  0,  -1},
        {0, 5,  5,  4,  4,  2,  2,  1,  1,  -1, -1, -1, -1, -2},
        {0, 5,  5,  4,  4,  2,  2,  1,  1,  -1, -1, -1, -1, -2},
        {0, 4,  4,  3,  3,  1,  1,  0,  0,  -1, -1, -3, -3, -4},
        {0, 4,  4,  3,  3,  1,  1,  0,  0,  -1, -1, -3, -3, -4},
        {0, 2,  2,  1,  1,  1,  1,  -1, -1, -2, -2, -4, -4, -5},
        {0, 2,  2,  1,  1,  1,  1,  -1, -1, -2, -2, -4, -4, -5},
        {0, 2,  2,  1,  1,  -1, -1, -2, -2, -4, -4, -4, -4, -5},
        {0, 2,  2,  1,  1,  -1, -1, -2, -2, -4, -4, -4, -4, -5},
        {0, 0,  0,  -1, -1, -2, -2, -3, -3, -4, -4, -5, -5, -6}};
        //Kd模糊控制规则表
        int[,] Fuzzy_TableKd = new int[,]{{0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0, 0,  0,  1,  1,  4,  4,  5,  5,  4,  4,  2,  2,  1 },
        {0, 0,  0,  1,  1,  4,  4,  5,  5,  4,  4,  2,  2,  1 },
        {0, 0,  0,  2,  2,  4,  4,  4,  4,  4,  4,  2,  2,  1 },
        {0, 0,  0,  2,  2,  4,  4,  4,  4,  4,  4,  2,  2,  1 },
        {0, 0,  0,  2,  2,  4,  4,  4,  4,  3,  3,  2,  2,  1 },
        {0, 0,  0,  2,  2,  4,  4,  4,  4,  3,  3,  2,  2,  1 },
        {0, 1,  1,  2,  2,  2,  2,  2,  2,  2,  2,  1,  1,  1 },
        {0, 1,  1,  2,  2,  2,  2,  2,  2,  2,  2,  1,  1,  1 },
        {0, -1, -1, -1, -1, 0,  0,  0,  0,  0,  0,  -1, -1, -1},
        {0, -1, -1, -1, -1, 0,  0,  0,  0,  0,  0,  -1, -1, -1},
        {0, -2, -2, -1, -1, -1, -1, -2, -2, -2, -2, -2, -2, -3},
        {0, -2, -2, -1, -1, -1, -1, -2, -2, -2, -2, -2, -2, -3},
        {0, -2, -2, -2, -2, -1, -1, -3, -3, -3, -3, -3, -3, -4}};
        double ek, ek_1, ek_2;
        int E, EC, U;
        double Ke, eh, el, Kec, eck, ech, ecl, Ku, uh, ul, kph, kpl, kih, kil, kki, kdh, kdl, kkd, kkp;
        double Kp, Ti, Td, T, I, Ki, Kd;
        double PIDU;
     

        public FuzzyPid(double period,double setValue,
            System.Windows.Forms.DataVisualization.Charting.Chart paraChart,
            System.Windows.Forms.DataVisualization.Charting.Chart controlChart,
            System.Windows.Forms.DataGridView paraGridView,
            System.Windows.Forms.DataGridView dataGridView)
        {

            ek = 0; ek_1 = 0; ek_2 = 0;
            eh = 40; el = -40;
            Ke = 6.0 / (eh - el);
            uh = 100; ul = 0;
            Ku = (uh - ul) * 1.0 / 12;
            ech = 10; ecl = -10;
            Kec = 6.0 / (ech - ecl);
            Kp = 1.8;
            Ti = 5;
            Td = 0.8;
            T = 0.58;
            PIDU = 0;
            Kp = 0;
            Ki = 0;
            Kd = 0;
            I = 0;
            kph = 1.2; kpl = 0;
            kkp = (kph - kpl) * 1.0 / 12;
            kih = 0.14; kil = 0;
            kki = (kih - kil) * 1.0 / 12;
            kdh = 1.55; kdl = 0;
            kkd = (kdh - kdl) * 1.0 / 12;

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
            ek_2 = ek_1;
            ek_1 = ek;
            ek = r - y;
            eck = ek - ek_1;
            //计算E
            E = (int)(Ke * (ek - (eh + el) / 2));
            if (E > 6)
                E = 6;
            else if (E < -6)
                E = -6;
            //计算EC
            EC = (int)(Kec * (eck - (ech + ecl) / 2));
            if (EC > 6)
                EC = 6;
            else if (EC < -6)
                EC = -6;
            double detU;
            double kp0 = 0;
            double kd0 = 0;
            double ki0 = 0.0;
            //查表，PID自适应
            Kp = kp0 + Fuzzy_TableKp[E + 7, EC + 7] * kkp + (kph + kpl) / 2;
            Ki = ki0 + Fuzzy_TableKi[E + 7, EC + 7] * kki + (kih + kil) / 2;
            Kd = kd0 + Fuzzy_TableKd[E + 7, EC + 7] * kkd + (kdh + kdl) / 2;
            I += ek;
            double u = 0;
            //计算控制量U
            u = Kp * ek + Ki * I + Kd * (ek - ek_1);
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
            for (int i = 0; i < 3; i++) // add the paraChart Series
            {
                paraChart.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series()); // add series
                paraChart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
                    // set the series type,pie or bar and etc
                paraChart.Series[i].BorderWidth = 2; // set the line width
            }

            paraChart.Series[0].LegendText = "Kp";
            paraChart.Series[1].LegendText = "Ti";
            paraChart.Series[2].LegendText = "Td";

            paraChart.ChartAreas[0].AxisX.Maximum = 60;
            paraChart.ChartAreas[0].AxisX.Minimum = 0;

            paraChart.ChartAreas[0].AxisY.Maximum = 5;
            paraChart.ChartAreas[0].AxisY.Minimum = -5;

            paraChart.Series[0].Points.Add(0, 0);
        }

       
          

        public override void initParaTable()
        {
            paraTable.Columns.Clear();
            paraTable.Rows.Clear();

            for (int i = 0; i < 3; i++) // initialize the paraTable
            {
                base.paraTable.Columns.Add(new DataGridViewTextBoxColumn());
                base.paraTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            base.paraTable.Columns[0].HeaderText = "比例系数/Kp";
            base.paraTable.Columns[1].HeaderText = "积分系数/Ki";
            base.paraTable.Columns[2].HeaderText = "微分系数/Kd";


        }

        
        public override void drawParameters()
        {
            setParaChartAxisY(Math.Round(Kp, 4));
            paraChart.Series[0].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(Kp, 4)));// draw Kp

            setParaChartAxisY(Math.Round(Ki, 4));
            paraChart.Series[1].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(Ki, 4)));// draw Ti

            setParaChartAxisY(Math.Round(Kd, 4));
            paraChart.Series[2].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(Kd, 4)));// draw Td
        }

  
        public override void showParameters()
        {
            paraTable.Rows[0].Cells[0].Value = Math.Round(Kp, 4);// show Kp
            paraTable.Rows[0].Cells[1].Value = Math.Round(Ki, 4);// show Ti
            paraTable.Rows[0].Cells[2].Value = Math.Round(Kd, 4);// show Td
        }
    }
}
