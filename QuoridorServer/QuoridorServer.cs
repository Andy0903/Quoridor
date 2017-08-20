using System;
using System.Windows.Forms;

namespace QuoridorServer
{
    public partial class QuoridorServerForm : Form
    {
        public QuoridorServerForm()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            if (StartButton.Text == "Start")
            {
                RunServer();
            }
            else
            {
                StopServer();
            }
        }

        private void RunServer()
        {
            NetworkManager.Initialize(PortTextBox);
            LogTextBox.AppendText("Server started!" + "\n");
            LogTextBox.AppendText("Waiting for connections.." + "\n\n");

            NumberOfPlayersGroup.Enabled = false;
            PortGroup.Enabled = false;

            StartButton.Text = "Disconnect";
        }

        private void StopServer()
        {
            NetworkManager.Deintialize();

            LogTextBox.AppendText("Server closed!" + "\n\n");
            NumberOfPlayersGroup.Enabled = true;
            PortGroup.Enabled = true;

            StartButton.Text = "Start";
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (StartButton.Text == "Disconnect")
            {
                NetworkManager.Update(LogTextBox);
                PlayerManager.Update(LogTextBox);
            }
        }
    }
}
