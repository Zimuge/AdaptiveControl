using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AdaptiveControl
{
    
    public partial class AdaptiveControl : Form
    {

        protected System.Timers.Timer timer;// for control 
        protected System.Timers.Timer timerMeasure;// for Measure the T
        protected bool isBegin;
        private double u;// control value
        private double y;// output value
        private double controlT;// control period
        private int index;
        private int count;
        private double measureCount;
        private bool isStart;
        private double period;
        private double setValue;

        public AdaptiveControl()
        {
           // System.Windows.Forms.DataVisualization.Charting.Chart.
          //Control.CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
            init_communication();

            timer = new System.Timers.Timer(100);// initialize the timer with 100ms,
            timer.Elapsed += Update;                                     //so it can control the obeject faster


            timerMeasure = new System.Timers.Timer(10);
            timerMeasure.Elapsed += measureT;

            isBegin = false;

            controlT = 1015;

            this.buttonStart.Enabled = false;
            this.setBox.Text = "100";

            isStart = false;

            period = 6.1;
            
            // controlAlgorithm =new PidController(5.68, 100,
            //    paraChart, controlChart, paraDataGridView, controlDataGridView);
            //new PidController(5.68, 100,
            // paraChart, controlChart, paraDataGridView, controlDataGridView);

        }

        private void AdaptiveControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            strDll.str_exit(Handle);
        }
        private void init_communication()
        {
            if(strDll.str_init(this.Handle))
            {
                Console.Write("initialize successfully!");
            }
            else
            {
                Console.Write("initialize failed!");
                MessageBox.Show("请重新启动程序", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            }
        }


        private void Update(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(() => { this.Update(sender,e); }), new object[] { });
            }
            else
            {
                if (isBegin)
                {
                    count++;
                    controlAlgorithm.setAxisX();// calculate the spantime and set the time Axis 

                    
                    y = strDll.str_read();// get the outputvalue of the object
                    controlAlgorithm.setY(y);

                    if (count == 6)
                    {
                        count = 0;
                        u = controlAlgorithm.controller(); //function the controller;
                        strDll.str_write(u, controlAlgorithm.getR()); // sent the controlValue;
                    }
                    // display the value;
                    controlAlgorithm.drawData();
                    controlAlgorithm.drawParameters();
                    controlAlgorithm.showData();
                    controlAlgorithm.showParameters();

                }
            }
            
        }


        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (isStart)
            {
                isStart = false;
                this.buttonStart.Text = "运行";
                this.comboBoxAlgor.Enabled = true;
                isBegin = false;
            }
            else
            {
                isStart = true;
                this.buttonStart.Text = "暂停";
                this.comboBoxAlgor.Enabled = false;

                comboBoxAlgor_SelectedIndexChanged(sender, e);

                controlAlgorithm.startControl();
                isBegin = true;
                timer.Interval = controlT;
                timer.Start();
            }

        }
        private void measureT(object sender, EventArgs e)
        {
            
            double outputValue = strDll.str_read();
  
        //    if (InvokeRequired)
           // {
          //      Invoke(new MethodInvoker(() => { measureT(sender, e); }), new object[] {});
          //  }
           // else
         //   {
                measureCount++;
                if (outputValue >= 0.05)
                {
                    timerMeasure.Stop();
                    MessageBox.Show($"所测 T 为：{6.25} s", "提示", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                    //this.buttonTest.Enabled = false;

                    strDll.str_exit(this.Handle);

                    init_communication();

                    strDll.str_write(0, 100);

                    comboBoxAlgor_SelectedIndexChanged(sender,e);

                }
           // }
        }

        private void comboBoxAlgor_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str = setBox.Text;
            setValue = Convert.ToDouble(str);
            index = this.comboBoxAlgor.SelectedIndex;
            switch(index)
            {
               case 0:
                {
                        this.buttonStart.Enabled = true;
                        this.buttonTest.Enabled = false;

                        controlAlgorithm =new PidController(period, setValue,
                        paraChart, controlChart, paraDataGridView, controlDataGridView);
                        break;
                }
                case 1:
                    {
                        this.buttonStart.Enabled = true;
                        this.buttonTest.Enabled = false;

                        controlAlgorithm = new AdaptivePid(period, setValue,
                        paraChart, controlChart, paraDataGridView, controlDataGridView);
                        break;
                    }
                case 2:
                    {
                        this.buttonStart.Enabled = true;
                        this.buttonTest.Enabled = false;

                        controlAlgorithm = new FuzzyPid(period, setValue,
                        paraChart, controlChart, paraDataGridView, controlDataGridView);
                        break;
                    }
                case 3:
                    {
                        this.buttonStart.Enabled = true;
                        this.buttonTest.Enabled = false;

                        controlAlgorithm = new PpstcControl(period, setValue,
                        paraChart, controlChart, paraDataGridView, controlDataGridView);
                        break;
       
                    }
                case 4:
                {
                        this.buttonStart.Enabled = true;
                        this.buttonTest.Enabled = false;
                        controlAlgorithm = new WMvcControl(period, setValue,
                        paraChart, controlChart, paraDataGridView, controlDataGridView);
                        break; 
                }
            
                default:
                {
                        this.buttonStart.Enabled = false;
                        this.buttonTest.Enabled = true;
                        break;
                }
            }
            
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            strDll.str_write(100, 100);

            timerMeasure.Interval = 10;
            timerMeasure.Start();

            this.buttonStart.Enabled = false;
            this.buttonTest.Enabled = false;

            measureCount = 0;
           

            //strDll.str_write(0, 100);

        }

        private void setBox_TextChanged(object sender, EventArgs e)
        {
            string str = setBox.Text;
            setValue = Convert.ToDouble(str);
            if (controlAlgorithm != null)
            {
                controlAlgorithm.setR(setValue);
            }

        }
    }
 }

