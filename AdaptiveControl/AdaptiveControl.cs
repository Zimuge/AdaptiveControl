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

        protected System.Timers.Timer timer;
        protected bool isBegin;
        private double u;// control value
        private double y;// output value
        private double controlT;// control period
        private int index;

        public AdaptiveControl()
        {
           // System.Windows.Forms.DataVisualization.Charting.Chart.
          //Control.CheckForIllegalCrossThreadCalls = false;

            InitializeComponent();
            init_communication();

            timer = new System.Timers.Timer(100);// initialize the timer with 100ms,
                                                 //so it can control the obeject faster
            timer.Elapsed += Update;

            isBegin = false;

            controlT = 5670;

            this.buttonStart.Enabled = false;
            this.setBox.Text = "100";

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

        private void buttonStart_Click(object sender, EventArgs e)
        {
            controlAlgorithm.startControl();

            isBegin = true;
            timer.Interval =  controlT;
            timer.Start();
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

                    controlAlgorithm.setAxisX();// calculate the spantime and set the time Axis 

                    y = strDll.str_read();// get the outputvalue of the object
                    controlAlgorithm.setY(y);

                    u = controlAlgorithm.controller();//function the controller;

                    strDll.str_write(u, controlAlgorithm.getR());// sent the controlValue;

                    // display the value;
                    controlAlgorithm.drawData();
                    controlAlgorithm.drawParameters();
                    controlAlgorithm.showData();
                    controlAlgorithm.showParameters();

                }
            }
            
        }

        private void comboBoxAlgor_SelectedIndexChanged(object sender, EventArgs e)
        {
            index = this.comboBoxAlgor.SelectedIndex;
            switch(index)
            {
               case 0:
                {
                        this.buttonStart.Enabled = true;
                        controlAlgorithm =new PidController(5.68, 100,
                        paraChart, controlChart, paraDataGridView, controlDataGridView);
                        break;
                }
                case 1:
                    {
                        this.buttonStart.Enabled = true;
                        controlAlgorithm = new AdaptivePid(5.68, 100,
                        paraChart, controlChart, paraDataGridView, controlDataGridView);
                        break;
                    }
                case 2:
                    {
                        this.buttonStart.Enabled = true;
                        controlAlgorithm = new FuzzyPid(5.68, 100,
                        paraChart, controlChart, paraDataGridView, controlDataGridView);
                        break;
                    }
            
                default:
                {
                        this.buttonStart.Enabled = false;
                        break;
                }
            }
            
        }
    }
 }

