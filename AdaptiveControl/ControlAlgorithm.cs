using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

namespace AdaptiveControl
{
    /*******************abstract class****************/
   abstract class ControlAlgorithm
   {

     /********************************************
                      interface
     *********************************************/
       public abstract double controller();// to get control value
       public abstract void initChart();//initialize the chart(dataChart,ParaChart)
       public abstract void initTable();//initialize the Table(dataTable,ParaTable)

       public abstract void drawData();//drawing the data chart
       public abstract void drawParameters();// drawing the control parameters
       public void startControl()// when the start button is clicked,start the control period
       {
            bgTime = DateTime.Now;
       }
        public abstract void showData();//show the data in dataTable

       public abstract void showParameters();//show the control parameters in paraTable





        /********************************************
                         memberfuction
       *********************************************/

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
   }
    

}
