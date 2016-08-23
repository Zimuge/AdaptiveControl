using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;

namespace AdaptiveControl
{
    /*******************abstract class****************/
   abstract class ControlAlgorithm
   {

     /********************************************
                      interface
     *********************************************/
       

       
        public abstract void initParaChart();
       

        public abstract void initParaTable();

        //public abstract void drawData();//drawing the data chart
        public abstract void drawParameters();// drawing the control parameters

       public abstract double getControlValue();//calculate the control value

       //public abstract void showData();//show the data in dataTable

       public abstract void showParameters();//show the control parameters in paraTable





        /********************************************
                         memberfuction
       *********************************************/


       public void initDataTable() //initialize the Table(dataTable,ParaTable)
       {
       

                dataTable.Columns.Clear();
                dataTable.Rows.Clear();

                for (int i = 0; i < 5; i++)// initialize the dataTable
                {
                    dataTable.Columns.Add(new DataGridViewTextBoxColumn());
                    dataTable.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }

                dataTable.Columns[0].HeaderText = "对象延时/T";
                 dataTable.Columns[1].HeaderText = "控制量/u";
                dataTable.Columns[2].HeaderText = "输出量/y";
                dataTable.Columns[3].HeaderText = "误差/e";
                dataTable.Columns[4].HeaderText = "超调量/σ";

        }

       public void initDataChart() //initialize the chart(dataChart,ParaChart)
        {
            dataChart.Series.Clear();

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

            dataChart.ChartAreas[0].AxisX.Maximum = 60;
            dataChart.ChartAreas[0].AxisX.Minimum = 0;

            dataChart.ChartAreas[0].AxisY.Maximum = 100;
            dataChart.ChartAreas[0].AxisY.Minimum = 0;


            dataChart.Series[0].Points.Add(0, 0);
        }

        //
        // according to the time to set the AxisX 
        //
        public  void setAxisX()
        {
            double dTime;

            TimeSpan sp = DateTime.Now.Subtract(bgTime);
            dTime = sp.TotalSeconds;    //HFY::以double类型表示的
           

            //获取paraChartX轴的最大值，若当前时间差超出最大值则重新设定X轴的最大值和最小值
            double oldMax = paraChart.ChartAreas[0].AxisX.Maximum;
            double oldMin = paraChart.ChartAreas[0].AxisX.Minimum;
            if (oldMax < dTime)
            {
                paraChart.ChartAreas[0].AxisX.Maximum = Math.Round(oldMax + 2 * T);
                dataChart.ChartAreas[0].AxisX.Maximum = Math.Round(oldMax + 2 * T);

                paraChart.ChartAreas[0].AxisX.Minimum = (oldMin + 2 * T);// + 10);
                dataChart.ChartAreas[0].AxisX.Minimum = (oldMin + 2 * T);//+10 );
            }

            Debug.WriteLine($"{oldMax} \n {oldMin} \n {dTime} \n ");
            spantime = dTime;
        }


        protected void setDataChartAxisY(double data)
        {
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

        protected void setParaChartAxisY(double para)
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

        public void drawData()
        {
            //
            // dranw setValue
            //
            Debug.WriteLine($"{r}  #{y}  #{outputU} \n spantime{spantime} ");


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

        public void showData()
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


        //
        // getting the control period 
        //
        public double getT()
        {
            return T;
        }


        //
        // getting the output value
        //
        public double getY()
        {
           
            return y;
        }


        //
        // getting the set value
        //
        public double getR()
        {
            return r;
        }


        //
        // setting the output value
        //
        public void setY(double y)
        {
            this.y = y;
        }


        //
        // setting the set value
        //
        public void setR(double r)
        {
            this.r = r;
        }

        public void setSpanTime(DateTime nowTime)
        {
            //spantime = 

        }

        public void startControl()// when the start button is clicked,start the control period
        {
            bgTime = DateTime.Now;
        }

        public double controller()
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

       


        /********************************************
                            varible
        *********************************************/
        public System.Windows.Forms.DataVisualization.Charting.Chart dataChart; // data chart 
       public System.Windows.Forms.DataVisualization.Charting.Chart paraChart;// parameters chart
       public System.Windows.Forms.DataGridView dataTable;// data talbe 
       public System.Windows.Forms.DataGridView paraTable;// parameters table 
       protected DateTime bgTime;// the time of begin
       protected double T;// the parameter of the object
       protected double y;// output value
       protected double r;// setting value
       protected double spantime;
       protected double error;
       protected double overshoot;
       protected double controlU;// the control value calculated by the algorithm
       protected double outputU;// the output control value
   }
    

}
