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
    class PidController : ControlAlgorithm
    {
        /********************************************
                       varible
        *********************************************/
        int i = 0;
       // double lastD = 0;
        double Kp = 1.2, Ti = 80, Td = 10;
        double Error_K;
        double Error_K_1;
        double Error_K_2;
        double ControlU = 0;



        /********************************************
                       member function
        *********************************************/
        public PidController(double T, double SetValue,
            System.Windows.Forms.DataVisualization.Charting.Chart paraChart,
            System.Windows.Forms.DataVisualization.Charting.Chart controlChart,
            System.Windows.Forms.DataGridView  paraGridView,
            System.Windows.Forms.DataGridView dataGridView)
        {
            base.T = T;
            base.r = SetValue;
            Error_K = 0;
            Error_K_1 = 0;
            Error_K_2 = 0;
            Kp = 1.2; Ti = 80; Td = 10;
            base.paraChart = paraChart;
            dataChart = controlChart;
            paraTable = paraGridView;
            dataTable = dataGridView;
            bgTime = DateTime.Now;
            spantime = 0;
            error = 0;
            overshoot = 0;

            initChart();
            initTable();
            
        }
        public double getControlValue()
        {
            double u = ControlU;
            double detU;
            double y = base.y;
            double SetValue = base.r;
            Error_K_2 = Error_K_1;
            Error_K_1 = Error_K;
            Error_K = SetValue - y;
            //普通PID
            if (Ti == 0)
            {
                detU = Kp * ((Error_K - Error_K_1) + Td / base.T * (Error_K - 2 * Error_K_1 + Error_K_2));
            }
            else
            {
                detU = Kp * ((Error_K - Error_K_1) + base.T / Ti * Error_K + Td / base.T * (Error_K - 2 * Error_K_1 + Error_K_2));
            }

            u += detU;
            return u;
        }

        //
        // according to the para to change the Aixs range
        //
        private void setParaChartAxisY(double para)
        {
            double oldMax = paraChart.ChartAreas[0].AxisY.Maximum;
            double oldMin = paraChart.ChartAreas[0].AxisY.Minimum;
            if (oldMax <= para)
            {
                paraChart.ChartAreas[0].AxisY.Maximum = oldMax + 50;
            }
            if (oldMin >= para)
            {
                paraChart.ChartAreas[0].AxisY.Maximum = oldMin - 50;
            }

        }
       

        private void setDataChartAxisY(double data)
        {
            double oldMax = dataChart.ChartAreas[0].AxisY.Maximum;
            double oldMin = dataChart.ChartAreas[0].AxisY.Minimum;
            if (oldMax <= data)
            {
                dataChart.ChartAreas[0].AxisY.Maximum = oldMax + 50;
            }
            if (oldMin >= data)
            {
                dataChart.ChartAreas[0].AxisY.Maximum = oldMin - 50;
            }

        }

        /********************************************
                    interface
        *********************************************/

        //
        // get the control value
        //
        public override double controller(double r, double y)
        {
            base.r = r;
            base.y = y;
            ControlU = getControlValue();
            return ControlU;
        }


        //
        // initiliaze the chart
        //
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
            for (i = 0; i < 3; i++)// add the paraChart Series
            {
                paraChart.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series());// add series
                paraChart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;// set the series type,pie or bar and etc
                paraChart.Series[i].BorderWidth = 1;// set the line width
            }

            paraChart.Series[0].LegendText = "Kp";
            paraChart.Series[1].LegendText = "Ti";
            paraChart.Series[2].LegendText = "Td";

            //
            // add the dataChart Series
            //
            for (i = 0; i < 3; i++)
            {
                dataChart.Series.Add(new System.Windows.Forms.DataVisualization.Charting.Series());// add series
                dataChart.Series[i].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;// set the series type,pie or bar and etc
                dataChart.Series[i].BorderWidth = 1;// set the line width
            }

            dataChart.Series[0].LegendText = "setValue/r";
            dataChart.Series[1].LegendText = "outputValue/y";
            dataChart.Series[2].LegendText = "controlValue/u";


            paraChart.ChartAreas[0].AxisX.Maximum = 100;
            paraChart.ChartAreas[0].AxisX.Minimum = 0;
            paraChart.ChartAreas[0].AxisY.Maximum = 100;
            paraChart.ChartAreas[0].AxisY.Minimum = -50;

            dataChart.ChartAreas[0].AxisX.Maximum = 100;
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

            for (i = 0; i < 3; i++)// initialize the paraTable
            {
                base.paraTable.Columns.Add(new DataGridViewTextBoxColumn());
                base.paraTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            base.paraTable.Columns[0].HeaderText = "增益/Kp";
            base.paraTable.Columns[1].HeaderText = "积分时间/Ti";
            base.paraTable.Columns[2].HeaderText = "微分时间/Td";

            for (i = 0; i < 5; i++)// initialize the dataTable
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

        //
        //draw data in the dataChart
        //
        public override void drawData()
        {
            //
            // draw setValue
            //
            setDataChartAxisY(Math.Round(r, 4));
            dataChart.Series[0].Points.Add(Math.Round(spantime - T, 4), Math.Round(r,4));

            //
            // draw output value
            //
            setDataChartAxisY(Math.Round(y, 4));
            dataChart.Series[1].Points.Add(Math.Round(spantime - T, 4), Math.Round(y, 4));

            //
            // draw control value
            //
            setDataChartAxisY(Math.Round(ControlU, 4));
            dataChart.Series[2].Points.Add(Math.Round(spantime - T, 4), Math.Round(ControlU, 4));
         }

        //
        //draw parameters in the paraChart
        //
        public override void drawParameters()
        {
            setParaChartAxisY(Math.Round(Kp,4));
            paraChart.Series[0].Points.Add(Math.Round(spantime - T, 4), Math.Round(Kp, 4));// draw Kp

            setParaChartAxisY(Math.Round(Ti, 4));
            paraChart.Series[1].Points.Add(Math.Round(spantime - T, 4), Math.Round(Ti, 4));// draw Ti

            setParaChartAxisY(Math.Round(Td, 4));
            paraChart.Series[2].Points.Add(Math.Round(spantime - T, 4), Math.Round(Td, 4));// draw Td
        }

        //
        //show data in the dataTable
        //
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

        //
        //show parameters in the paraTable
        //
        public override void showParameters()
        {
            paraTable.Rows[0].Cells[0].Value = Math.Round(Kp, 4);// show Kp
            paraTable.Rows[0].Cells[1].Value = Math.Round(Ti, 4);// show Ti
            paraTable.Rows[0].Cells[2].Value = Math.Round(Td, 4);// show Td
        }
    }
    
}
