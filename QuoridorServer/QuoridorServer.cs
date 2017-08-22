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
                NetworkManager.Update(LogTextBox, PlayerLabel);
                PlayerManager.Update(LogTextBox);
            }
        }

        private void RadioButton2Players_CheckedChanged(object sender, EventArgs e)
        {
            if(RadioButton2Players.Checked)
            {
                PlayerManager.GameModePlayers = PlayerManager.NumberOfPlayersGameMode.TwoPlayers;
            }
        }

        private void RadioButton4Player_CheckedChanged(object sender, EventArgs e)
        {
            if (RadioButton4Player.Checked)
            {
                PlayerManager.GameModePlayers = PlayerManager.NumberOfPlayersGameMode.FourPlayers;
            }
        }
    }
}
