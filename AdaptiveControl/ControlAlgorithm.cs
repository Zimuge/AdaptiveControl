using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaptiveControl
{
    /*******************abstract class****************/
   abstract class ControlAlgorithm
   {

     /********************************************
                      interface
     *********************************************/
       public abstract double controller(double r,double y);// to get control value
       public abstract void initChart();//initialize the chart(dataChart,ParaChart)
       public abstract void initTable();//initialize the Table(dataTable,ParaTable)

       public abstract void drawData();//drawing the data chart
       public abstract void drawParameters();// drawing the control parameters

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
            if (oldMax < dTime + T)
            {
                paraChart.ChartAreas[0].AxisX.Maximum = oldMax + 100;
                dataChart.ChartAreas[0].AxisX.Maximum = oldMax + 100;

                paraChart.ChartAreas[0].AxisX.Minimum = oldMax - T;
                dataChart.ChartAreas[0].AxisX.Minimum = oldMax - T;
            }
            spantime = dTime;
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


       /********************************************
                           varible
       *********************************************/
       public System.Windows.Forms.DataVisualization.Charting.Chart dataChart; // data chart 
       public System.Windows.Forms.DataVisualization.Charting.Chart paraChart;// parameters chart
       public System.Windows.Forms.DataGridView dataTable;// data talbe 
       public System.Windows.Forms.DataGridView paraTable;// parameters table 
       public DateTime bgTime;// the time of begin
       public double T;// control period
       public double y;// output value
       public double r;// setting value
       public double spantime;
       public double error;
       public double overshoot;
   }


}
