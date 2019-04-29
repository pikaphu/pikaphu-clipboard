using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PikaPhuClipboard
{
    public partial class Form1 : Form
    {
        public static bool isEnable = true;

        public static Form1 thisForm;

        public Form1()
        {
            InitializeComponent();

            thisForm = this;

            ToggleEnable(isEnable);

            this.WindowState = FormWindowState.Normal;            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ToggleEnable(!isEnable);
        }

        void ToggleEnable(bool status)
        {
            isEnable = status;

            if (isEnable)
            {
                lbl_status.Text = "Enable"; lbl_status.ForeColor = Color.Green;
            }
            else
            {
                lbl_status.Text = "Disable"; lbl_status.ForeColor = Color.Red;
            }
        }

        public void UpdateUI()
        {
            if (!isEnable)
            {
                return;
            }

            string temp  = Clipboard.GetText();

            //Console.WriteLine(temp);      
        }

        public void ClearClip()
        {
            if (!isEnable)
            {
                return;
            }

            string temp = Clipboard.GetText();

            Task.Delay(500).ContinueWith(
                t => Invoke((Action)(() => { Clipboard.Clear(); }))
            );


            //Console.WriteLine("Clear: " + temp);
        }

        void NotifyIcon_Hide()
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = true;
                //notifyIcon1.ShowBalloonTip(1000);
                Hide();
            }
        }

        void NotifyIcon_Show()
        {            
            if (this.WindowState == FormWindowState.Minimized)
            {
                notifyIcon1.Visible = false;                
                Show();
            }
            this.WindowState = FormWindowState.Normal;            
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            NotifyIcon_Hide();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            NotifyIcon_Show();
        }

        private void Form1_Activated(object sender, EventArgs e)
        {
            
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            NotifyIcon_Show();
        }

        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            // First Hide
            //System.Threading.Thread.Sleep(500);
            this.WindowState = FormWindowState.Minimized;
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void contextMenuStrip1_MouseClick(object sender, MouseEventArgs e)
        {
            NotifyIcon_Show();
        }
    }
}
