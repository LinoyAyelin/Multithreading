using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace RedGreenBlue_ButtonLabel_Transport_UserControl
{
    public delegate void myDelegateDelete(UserControl1 uc, int i);
    public delegate void myDelegateInsert(UserControl1 ucto, int indexto, UserControl1 ucfrom, int indexfrom, int to_counter);

    public partial class Form1 : Form
    {
        private UserControl1[] UCFrom = new UserControl1[2];
        private UserControl1[] arrUC_To = new UserControl1[3];
        private Color[] arrColors = { Color.Red, Color.Green, Color.Blue };
        private int[] counter_To = new int[3] { 0, 0, 0 };       // מערך שסופר באיזה מיקום הוספתי לכל קונטרול
        private int[] totalCounter = new int[3] { 0, 0, 0 };     // מערך שסופר כמה פקדים הוספתי בכל קונטרול
        private AutoResetEvent[] autoReset = new AutoResetEvent[3];
        Thread[] arrThreads = new Thread[3];
        private int[] Counter = new int[3] { 2, 2, 2 };         // מערך למיקום הפקדים בכל קונטרול

        private int width_FromTo = 1000, maxCounter_FromTo = 31;

        public Form1()
        {
            InitializeComponent();
            this.Width = 1025;

            UCFrom[0] = new UserControl1(width_FromTo, maxCounter_FromTo, "Full");

            UCFrom[0].Location = new Point(2, 70);
            this.Controls.Add(UCFrom[0]);

            UCFrom[1] = new UserControl1(width_FromTo, maxCounter_FromTo, "Full");
            UCFrom[1].Location = new Point(2, 104);
            this.Controls.Add(UCFrom[1]);

            for (int i = 0; i < 3; i++)
            {
                arrUC_To[i] = new UserControl1(width_FromTo, maxCounter_FromTo, "Empty");
                arrUC_To[i].Location = new Point(2, 220 + 57 * i);
                this.Controls.Add(arrUC_To[i]);
            }
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 3; i++)
            {
                arrThreads[i] = new Thread(ToDestination);

                if (i == 0)
                    autoReset[i] = new AutoResetEvent(true);
                else
                    autoReset[i] = new AutoResetEvent(false);

                arrThreads[i].Start(i);
            }

            /*
            Thread t1 = new Thread(ToDestination);
            Thread t2 = new Thread(ToDestination);
            Thread t3 = new Thread(ToDestination);
            t1.Start(0);
            t2.Start(1);
            t3.Start(2);
            */
        }

        
        private void ToDestination(object o)
        {
            int n = (int)o;
            autoReset[n].WaitOne();

            for (int y = 1; y < 3; y++)
            {
                for (int x = 0; x < UCFrom.Length; x++)
                {
                    for (int i = 0; i < UCFrom[x].arrControls.Length; i++)
                    {
                        if (UCFrom[x].arrControls[i] == null)
                            continue;
                            
                        int temp = int.Parse(UCFrom[x].arrControls[i].Text);
                        if (temp == y)
                        {
                            if (n == 0 && UCFrom[x].arrControls[i].BackColor.R > 0 && UCFrom[x].arrControls[i].BackColor.B == 0 && UCFrom[x].arrControls[i].BackColor.G == 0 ||
                                     n == 1 && UCFrom[x].arrControls[i].BackColor.G > 0 && UCFrom[x].arrControls[i].BackColor.R == 0 && UCFrom[x].arrControls[i].BackColor.B == 0 ||
                                     n == 2 && UCFrom[x].arrControls[i].BackColor.B > 0 && UCFrom[x].arrControls[i].BackColor.R == 0 && UCFrom[x].arrControls[i].BackColor.G == 0)
                            {
                                {
                                    this.Invoke(new myDelegateInsert(Insert), arrUC_To[n], counter_To[n], UCFrom[x], i, Counter[n]);
                                    this.Invoke(new myDelegateDelete(Delete), UCFrom[x], i);
                                    counter_To[n]++;
                                    totalCounter[n]++;
                                    Counter[n] += 32;
                                    Thread.Sleep(500);
                                    if (totalCounter[n] == 5)
                                    {
                                        setNextActive(n);
                                        totalCounter[n] = 0;
                                        autoReset[n].WaitOne();
                                    }
                                }
                            }
                        }
                    }
                }
            }
                setNextActive(n);
        }

        public void Delete(UserControl1 uc, int i)
        {
            uc.Controls.Remove(uc.arrControls[i]);    //מחיקה מהיוזר קונטרול
            uc.arrControls[i] = null;                  // מחיקה ממערך הקונטרולים
        }

        public void Insert(UserControl1 ucto, int indexto, UserControl1 ucfrom, int indexfrom, int to_counter)
        {
            ucto.Controls.Remove(ucto.arrControls[indexto]);
            ucto.arrControls[indexto] = ucfrom.arrControls[indexfrom];
            ucto.Controls.Add(ucfrom.arrControls[indexfrom]);
            ucto.arrControls[indexto].Location = new Point(to_counter, 3);
        }

        private void setNextActive(int indexTypes)
        {
            var a =(indexTypes + 1) % 3;
            if (arrThreads[a].IsAlive)
                autoReset[(indexTypes + 1) % 3].Set();
            else
            {
                for (int i = 0; i < 3; i++)
                {
                    if (i != indexTypes && arrThreads[i].IsAlive)
                    {
                          autoReset[i].Set();
                          return;
                    }
                }
                autoReset[indexTypes].Set();
            }
        }
    }

}





