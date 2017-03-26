using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Counter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            if (this.WindowState != FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x112)
            {
                switch ((int)m.WParam)
                {
                    //禁止双击标题栏关闭窗体
                    case 0xF063:
                    case 0xF093:
                        m.WParam = IntPtr.Zero;
                        break;
                    //禁止拖拽标题栏还原窗体
                    case 0xF012:
                    case 0xF010:
                        m.WParam = IntPtr.Zero;
                        break;
                    //禁止双击标题栏
                    case 0xf122:
                        m.WParam = IntPtr.Zero;
                        break;
                    //禁止关闭按钮
                    /*  case 0xF060:
                          m.WParam = IntPtr.Zero;
                          break;*/
                    //禁止最大化按钮
                    case 0xf020:
                        m.WParam = IntPtr.Zero;
                        break;
                    //禁止最小化按钮
                    case 0xf030:
                        m.WParam = IntPtr.Zero;
                        break;
                        //禁止还原按钮
                        /*   case 0xf120:
                               m.WParam = IntPtr.Zero;
                               break;*/
                }
            }
            base.WndProc(ref m);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RB_Nor.Checked = true;
            ColumnChart.Series[0].IsVisibleInLegend = false;
            ColumnChart.Series[0].LabelForeColor = Color.Red;
            ColumnChart.Series[0].Points.AddXY(1, 0);
            ColumnChart.ChartAreas[0].AxisX.Interval = 1;
            ColumnChart.Series[0].Points.AddXY(2, 0);
            ColumnChart.Series[0].Points.AddXY(3, 0);
            ColumnChart.Series[0].Points.AddXY(4, 0);
            ColumnChart.Series[0].Points.AddXY(5, 0);
            ColumnChart.Series[0].Points.AddXY(6, 0);
            ColumnChart.Series[0].Points[0].Color = Color.Green;
            ColumnChart.Series[0].Points[1].Color = Color.Green;
            ColumnChart.Series[0].Points[2].Color = Color.Green;
            ColumnChart.Series[0].Points[3].Color = Color.Green;
            ColumnChart.Series[0].Points[4].Color = Color.Green;
            ColumnChart.Series[0].Points[5].Color = Color.Green;
            ColumnChart.ChartAreas[0].AxisX.CustomLabels.Add(0, 2, "第一展服室");
            ColumnChart.ChartAreas[0].AxisX.CustomLabels.Add(1, 3, "第二展服室");
            ColumnChart.ChartAreas[0].AxisX.CustomLabels.Add(2, 4, "第三展服室");
            ColumnChart.ChartAreas[0].AxisX.CustomLabels.Add(3, 5, "第四展服室");
            ColumnChart.ChartAreas[0].AxisX.CustomLabels.Add(4, 6, "静海营销服务部");
            ColumnChart.ChartAreas[0].AxisX.CustomLabels.Add(5, 7, "蓟县营销服务部");
            startService();
        }

        private void startService()
        {
            Listener lis = new Listener(18888);
            lis.GetMessage += Lis_GetMessage;
            lis.StartListen();
        }

        private void Lis_GetMessage(string msg)
        {
            try
            {
                string[] msgs = msg.Split(Encoding.UTF8.GetChars(Encoding.UTF8.GetBytes("-"), 0, 1));
                if (msgs.Length == 3)
                {
                    int gid = Int32.Parse(msgs[0]);
                    double num = 0;
                    double curNum = ColumnChart.Series[0].Points[gid].YValues[0];
                    if (msgs[2].Equals("add"))
                    {
                        num = curNum + Int32.Parse(msgs[1]);
                    }
                    else
                    {
                        num = curNum - Int32.Parse(msgs[1]);
                    }
                    setViewByData(gid, num);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        private void setViewByData(int gid, double num)
        {
            double tar = 0;
            if (num > ColumnChart.Series[0].Points[gid].YValues[0])
            {
                tar = num - ColumnChart.Series[0].Points[gid].YValues[0];
                    for (int i=1;i<= tar;i++) {
                    ColumnChart.Invoke(new EventHandler(delegate {
                        ColumnChart.Series[0].Points[gid].YValues = new Double[] { ColumnChart.Series[0].Points[gid].YValues[0]+1 };
                        int cur = (int)ColumnChart.Series[0].Points[gid].YValues[0];
                        if (cur >= 0 && cur < 10)
                        {
                            ColumnChart.Series[0].Points[gid].Color = Color.Green;
                        }
                        else if (cur >= 10 && cur < 50)
                        {
                            ColumnChart.Series[0].Points[gid].Color = Color.Blue;
                        }
                        else if (cur >= 50 && cur < 80)
                        {
                            ColumnChart.Series[0].Points[gid].Color = Color.Orange;
                        }
                        else if (cur >= 80 && cur < 100)
                        {
                            ColumnChart.Series[0].Points[gid].Color = Color.Red;
                        }
                        Thread.Sleep(10);
                        RefreshTheChart();
                    }));

                }
                if (num < ColumnChart.Series[0].Points[gid].YValues[0])
                {
                    tar = ColumnChart.Series[0].Points[gid].YValues[0]-num;
                    for (int i = 1; i <= tar; i++)
                    {
                        ColumnChart.Invoke(new EventHandler(delegate
                        {
                            ColumnChart.Series[0].Points[gid].YValues = new Double[] { ColumnChart.Series[0].Points[gid].YValues[0] - 1 };
                            int cur = (int)ColumnChart.Series[0].Points[gid].YValues[0];
                            if (cur >= 0 && cur < 10)
                            {
                                ColumnChart.Series[0].Points[gid].Color = Color.Green;
                            }
                            else if (cur >= 10 && cur < 50)
                            {
                                ColumnChart.Series[0].Points[gid].Color = Color.Blue;
                            }
                            else if (cur >= 50 && cur < 80)
                            {
                                ColumnChart.Series[0].Points[gid].Color = Color.Orange;
                            }
                            else if (cur >= 80 && cur < 100)
                            {
                                ColumnChart.Series[0].Points[gid].Color = Color.Red;
                            }
                        }));
                    }

                }

               

            }
       
        }

        private void RefreshTheChart()
        {
            if (RB_Nor.Checked == true)
            {
                RB_Nor.PerformClick();
            }
            else
            {
                RB_3D.PerformClick();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_Nor.Checked == true)
            {
                ColumnChart.ChartAreas[0].Area3DStyle.Enable3D = false;
            }
        }

        private void RB_3D_CheckedChanged(object sender, EventArgs e)
        {
            if (RB_3D.Checked == true)
            {
                ColumnChart.ChartAreas[0].Area3DStyle.Enable3D = true;
            }
        }
    }
}
