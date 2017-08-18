using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Lidgren.Network;

namespace QuoridorServer
{
    public partial class QuoridorServerForm : Form
    {
        public QuoridorServerForm()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e) //TODO Sluta hårdkoda.
        {
            NetworkManager.Initialize();
            LogTextBox.AppendText("Server started!" + "\r\n");
            LogTextBox.AppendText("Waiting for connections.." + "\r\n\r\n");

            StartButton.Enabled = false;
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (StartButton.Enabled == false)
            {
                NetworkManager.Update(LogTextBox);
                //Update player
            }
        }
    }
}
