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
            if (StartButton.Text == "Start")
            {
                NetworkManager.Initialize();
                LogTextBox.AppendText("Server started!" + "\n");
                LogTextBox.AppendText("Waiting for connections.." + "\n\n");

                NumberOfPlayersGroup.Enabled = false;
                PortGroup.Enabled = false;

                StartButton.Text = "Disconnect";
            }
            else
            {
                NetworkManager.Deintialize();

                LogTextBox.AppendText("Server closed!" + "\n\n");
                NumberOfPlayersGroup.Enabled = true;
                PortGroup.Enabled = true;

                StartButton.Text = "Start";
            }
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
