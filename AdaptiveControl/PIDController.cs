using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Diagnostics;

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
        //double ControlU = 0;
        //double outputU = 0;



        /********************************************
                       member function
        *********************************************/
        public PidController(double period, double setValue,
            System.Windows.Forms.DataVisualization.Charting.Chart paraChart,
            System.Windows.Forms.DataVisualization.Charting.Chart controlChart,
            System.Windows.Forms.DataGridView  paraGridView,
            System.Windows.Forms.DataGridView dataGridView)
        {
            
            Error_K = 0;
            Error_K_1 = 0;
            Error_K_2 = 0;
            Kp = 1.2; Ti = 80; Td = 10;

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
            double u = controlU;
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
     
       

       

        /********************************************
                    interface
        *********************************************/

        //
        // get the control value
        //
        

        //
        // initiliaze the chart
        //
        public override void initParaChart()
        {

            //
            // clear the chart series
            //
            paraChart.Series.Clear();
           

            //
            // add the paraChart Series
            //
            for (i = 0; i < 3; i++) // add the paraChart Series
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

            paraChart.ChartAreas[0].AxisY.Maximum = 100;
            paraChart.ChartAreas[0].AxisY.Minimum = -20;

            paraChart.Series[0].Points.Add(0, 0);
        }
        //
        // add the dataChart Series
        //
       

        public override void initParaTable()
        {
            paraTable.Columns.Clear();
            paraTable.Rows.Clear();

            for (i = 0; i < 3; i++) // initialize the paraTable
            {
                base.paraTable.Columns.Add(new DataGridViewTextBoxColumn());
                base.paraTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            base.paraTable.Columns[0].HeaderText = "增益/Kp";
            base.paraTable.Columns[1].HeaderText = "积分时间/Ti";
            base.paraTable.Columns[2].HeaderText = "微分时间/Td";
        }

        
        //
        //draw data in the dataChart
        //
        
        //
        //draw parameters in the paraChart
        //
        public override void drawParameters()
        {
            
            setParaChartAxisY(Math.Round(Kp,4));
            paraChart.Series[0].Points.Add(new DataPoint(Math.Round(spantime, 4), Math.Round(Kp, 4) ));// draw Kp

            setParaChartAxisY(Math.Round(Ti, 4));
            paraChart.Series[1].Points.Add(new DataPoint(Math.Round(spantime, 4),Math.Round(Ti, 4)) );// draw Ti

            setParaChartAxisY(Math.Round(Td, 4));
            paraChart.Series[2].Points.Add(new DataPoint(Math.Round(spantime, 4),Math.Round(Td, 4)));// draw Td
        }

        //
        //show data in the dataTable
        //
        
        
        //
        //show parameters in the paraTable
        //
        public override void showParameters()
        {
            paraTable.Rows[0].Cells[0].Value = Math.Round(Kp, 4);// show Kp
            paraTable.Rows[0].Cells[1].Value = Math.Round(Ti, 4);// show Ti
            paraTable.Rows[0].Cells[2].Value = Math.Round(Td, 4);// show Td
        }

        //
        // when the start button is cliked ,this function is called
        //
      

        //
        //timer interrupt function
        

        
    }

    
}
